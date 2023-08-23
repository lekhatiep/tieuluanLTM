using API.Data;
using API.Dto.Catalog;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.Catalog;
using API.Services.Firebase;
using API.Dto.Firebase;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Dapper;
using System.Data.SqlClient;
using API.Common.Paging;
using Microsoft.EntityFrameworkCore;

namespace API.Services.Catalog
{
    public class ModelRepository : IModelRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFirebaseService _firebaseService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DapperContext _dapperContext;

        public ModelRepository(
            AppDbContext appContext,
            IMapper mapper,
            IFirebaseService firebaseService,
            IHttpContextAccessor httpContextAccessor,
            DapperContext dapperContext
            )
        {
            _context = appContext;
            _mapper = mapper;
            _firebaseService = firebaseService;
            _httpContextAccessor = httpContextAccessor;
            _dapperContext = dapperContext;
        }

        public async Task<int> AddNewModel(CreateModelDto modelDto)
        {
            var rs = -1;
            var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userID = 0;

            if (identity != null)
            {
                userID = int.Parse(identity.FindFirst("id").Value);
            }

            try
            {

                var newModel = _mapper.Map<RoboModel>(modelDto);
                newModel.UserID = userID;
                newModel.IsDelete = false;
                newModel.CreatedDate = DateTime.Now;

                await _context.RoboModel.AddAsync(newModel);
                await _context.SaveChangesAsync();

                if (modelDto.File != null) {

                    if (modelDto.File.Length > 0 && newModel.ModelID > 0)
                    {
                        var uploadDto = new FileUploadDto()
                        {

                            File = modelDto.File
                        };

                        var strPath = await _firebaseService.UploadFileAsync(uploadDto);

                        if (!string.IsNullOrEmpty(strPath))
                        {
                            var uploadFile = new API.Entities.Catalog.UploadFile
                            {
                                UserID = userID,
                                ModelID = newModel.ModelID,
                                Path = strPath,
                                FileName = modelDto.File.FileName,
                                CreatedDate = DateTime.Now
                            };

                            await _context.UploadFile.AddAsync(uploadFile);
                            await _context.SaveChangesAsync();
                        }
                    }
                }             

                 rs = 1;

            }
            catch (Exception ex)
            {
                rs = -1;
                Console.WriteLine(ex.Message);
                throw;
            }

            return rs;
        }

        public async Task<int> DeleteModel(int id)
        {
            if (id > 0)
            {
                var model = await _context.RoboModel.Where(x => x.ModelID == id).FirstOrDefaultAsync();

                if (model != null)
                {
                    var uploadFile =  await _context.UploadFile.Where(x => x.ModelID == model.ModelID).FirstOrDefaultAsync();
                    if (uploadFile != null)
                    {
                        uploadFile.IsDelete = true;
                        await _context.SaveChangesAsync();
                    }

                    model.IsDelete = true;
                    await _context.SaveChangesAsync();
                }
                return 1;
            }


            return -1;
        }

        public async Task<PagedList<ListModelDto>> GetListModel(ModelRequestDto modelRequest)
        {
            var select = " rm.ModelID ModelID, rm.UserID, uf.Path ImgPath, rm.Name, rm.TypeName, rm.IsDelete, rm.CreatedDate ";
            var from = @" RoboModel rm left join UploadFile uf on rm.ModelID = uf.ModelID ";
            var where = " 1=1 AND ISNULL(uf.IsDelete,0) = 0 ";
            var oderBy = " rm.ModelID ";
            var parameters = new DynamicParameters();

            if (!String.IsNullOrEmpty(modelRequest.KeySearch) )
            {
                if (modelRequest.KeySearch == "ModelID")
                {
                    where += $" AND rm.ModelID LIKE '%{modelRequest.ValueSearch}%' ";
                }
                else if(modelRequest.KeySearch == "UserID")
                {
                    where += $" AND rm.UserID LIKE '%{modelRequest.ValueSearch}%' ";
                }
                else
                {
                    where += $" AND {modelRequest.KeySearch} LIKE '%{modelRequest.ValueSearch}%' ";
                }
                
            }

            if (!String.IsNullOrEmpty(modelRequest.SortBy))
            {
                if (modelRequest.SortBy.ToLower() == "modelid")
                {
                    oderBy = " rm.ModelID ";
                }
                else if (modelRequest.SortBy.ToLower() == "userid")
                {
                    oderBy = " rm.UserID ";
                }
                else if (modelRequest.SortBy.ToLower() == "createddate")
                {
                    oderBy = " rm.CreatedDate ";
                }
                else
                {
                    oderBy = $" {modelRequest.SortBy} ";
                }

            }
            if (modelRequest.Desc)
            {
                oderBy += " DESC ";
            }

            using (var conn = new SqlConnection(_dapperContext.ConnectionString))
            {
                var data = await conn.QueryPagingAsync<ListModelDto>(select, from, where, oderBy, parameters, modelRequest.PageNumber, modelRequest.PageSize);
                return data;
            }
        }

        public async Task<PagedList<ListModelDto>> GetListModelByUser(ModelRequestDto modelRequest)
        {
            var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userID = 0;

            if (identity != null)
            {
                userID = int.Parse(identity.FindFirst("id").Value);
            }

            var select = " rm.ModelID ModelID, rm.UserID, uf.Path ImgPath, rm.Name, rm.TypeName, rm.IsDelete, rm.CreatedDate ";
            var from = @" RoboModel rm left join UploadFile uf on rm.ModelID = uf.ModelID ";
            var where = $" 1=1 AND rm.UserID = {userID} AND rm.IsDelete = 0  AND ISNULL(uf.IsDelete,0) = 0 ";
            var oderBy = " rm.ModelID ";
            var parameters = new DynamicParameters();

            if (!String.IsNullOrEmpty(modelRequest.KeySearch) )
            {
                if (modelRequest.KeySearch == "ModelID")
                {
                    where += $" AND rm.ModelID LIKE '%{modelRequest.ValueSearch}%' ";
                }
                else if (modelRequest.KeySearch == "UserID")
                {
                    where += $" AND rm.UserID LIKE '%{modelRequest.ValueSearch}%' ";
                }
                else
                {
                    where += $" AND {modelRequest.KeySearch} LIKE '%{modelRequest.ValueSearch}%' ";
                }
            }

            if (!string.IsNullOrEmpty(modelRequest.SortBy))
            {
                if (modelRequest.SortBy.ToLower() == "modelid")
                {
                    oderBy = " rm.ModelID ";
                }
                else if (modelRequest.SortBy.ToLower() == "userid")
                {
                    oderBy = " rm.UserID ";
                }
                else if (modelRequest.SortBy.ToLower() == "createddate")
                {
                    oderBy = " rm.CreatedDate ";
                }
                else
                {
                    oderBy = $" {modelRequest.SortBy} ";
                }
            }          

            if (modelRequest.Desc)
            {
                oderBy += " DESC ";
            }
            parameters.Add("IsDelete", 0);

            using (var conn = new SqlConnection(_dapperContext.ConnectionString))
            {
                var data = await conn.QueryPagingAsync<ListModelDto>(select, from, where, oderBy, parameters, modelRequest.PageNumber, modelRequest.PageSize);
                
                return data;
            }
        }

        public async Task<ModelDto> GetModelById(int id)
        {
            if (id > 0)
            {
                var model =  _context.RoboModel;
                var uploadFile = _context.UploadFile;
                var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
                var userID = 0;

                if (identity != null)
                {
                    userID = int.Parse(identity.FindFirst("id").Value);
                }

                var query = from m in model
                           join uf in uploadFile on m.ModelID equals uf.ModelID into joinTable
                           from jt in joinTable.DefaultIfEmpty()
                           where m.ModelID == id && m.UserID == userID && m.IsDelete == false && jt.IsDelete == false
                           select new ModelDto
                           {
                               ModelID = m.ModelID,
                               UserID = m.UserID,
                               MediaID = jt.MediaID,
                               Name = m.Name,
                               TypeName = m.TypeName,
                               PathImage = jt.Path,
                               IsDelete = m.IsDelete,
                               CreatedDate = m.CreatedDate
                           };
                var data = await query.FirstOrDefaultAsync();

                if (data == null)
                {
                    return new ModelDto();
                }

                return data;
            }

            return new ModelDto();
        }

        public async Task<ModelDto> UpdateModel(UpdateModelDto updateModelDto)
        {
            if (updateModelDto.ModelID > 0)
            {
                var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
                var userID = 0;

                if (identity != null)
                {
                    userID = int.Parse(identity.FindFirst("id").Value);
                }
                var model = await _context.RoboModel.FirstOrDefaultAsync(x => x.ModelID == updateModelDto.ModelID && x.UserID == userID);

                if (model == null)
                {
                    return new ModelDto();
                }

                _mapper.Map(updateModelDto, model);

                if (updateModelDto.File != null  && model.ModelID > 0)
                {
                    if (updateModelDto.File.Length < 0)
                    {
                        return new ModelDto();
                    }

                    var uploadDto = new FileUploadDto()
                    {

                        File = updateModelDto.File
                    };

                    var strPath = await _firebaseService.UploadFileAsync(uploadDto);

                    if (!string.IsNullOrEmpty(strPath))
                    {
                        var uploadFile = new API.Entities.Catalog.UploadFile
                        {
                            UserID = model.UserID,
                            ModelID = model.ModelID,
                            Path = strPath,
                            FileName = updateModelDto.File.FileName,
                            CreatedDate = DateTime.Now
                        };

                        await _context.UploadFile.AddAsync(uploadFile);
                        await _context.SaveChangesAsync();
                    }

                    //Delete old media
                    var media = _context.UploadFile.Where(x => x.ModelID == model.ModelID).FirstOrDefault();
                    if (media != null)
                    {
                        media.IsDelete = true;
                        await _context.SaveChangesAsync();
                    }

                    await _firebaseService.DeleteFileAsync(media.FileName);
                }

                await _context.SaveChangesAsync();

                var modelDto = await GetModelById(model.ModelID);
                return modelDto;
            }
            return new ModelDto();
        }

        public async Task<int> TotalRecord(ModelRequestDto modelRequest, bool isCountForUser)
        {
            var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userID = 0;

            if (identity != null)
            {
                userID = int.Parse(identity.FindFirst("id").Value);
            }

            var from = @" RoboModel rm left join UploadFile uf on rm.ModelID = uf.ModelID ";
            var where = $" 1=1 AND ISNULL(uf.IsDelete,0) = 0 ";

            if (isCountForUser)
            {
                where += $" AND rm.UserID = {userID} AND rm.IsDelete = 0 ";
            }

            var oderBy = " rm.ModelID ";
            var parameters = new DynamicParameters();

            if (!String.IsNullOrEmpty(modelRequest.KeySearch))
            {
                if (modelRequest.KeySearch == "ModelID")
                {
                    where += $" AND rm.ModelID LIKE '%{modelRequest.ValueSearch}%' ";
                }
                else if (modelRequest.KeySearch == "UserID")
                {
                    where += $" AND rm.UserID LIKE '%{modelRequest.ValueSearch}%' ";
                }
                else
                {
                    where += $" AND {modelRequest.KeySearch} LIKE '%{modelRequest.ValueSearch}%' ";
                }
            }

            if (!String.IsNullOrEmpty(modelRequest.SortBy))
            {
                if (modelRequest.SortBy == "ModelID")
                {
                    oderBy = " ModelID ";
                }
                else
                {
                    oderBy = $" {modelRequest.SortBy} ";
                }
            }
            if (modelRequest.Desc)
            {
                oderBy += " DESC ";
            }
            parameters.Add("IsDelete", 0);

            var query = $"SELECT COUNT(1) FROM  {from} WHERE {where}";

            using (var cn = _dapperContext.CreateConnection())
            {
                var rs = await cn.QueryFirstOrDefaultAsync<int>(query);
                return rs;
            }
        }
    }
}
