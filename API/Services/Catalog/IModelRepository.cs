﻿using API.Dto.Catalog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services.Catalog
{
    public interface IModelRepository
    {
        Task<int> AddNewModel(CreateModelDto modelDto);
        Task<int> DeleteModel(int id);
        Task<ModelDto> UpdateModel (UpdateModelDto model);
        Task<ModelDto> GetModelById(int id);
        Task<List<ListModelDto>> GetListModel(ModelRequestDto modelRequest);
        Task<List<ListModelDto>> GetListModelByUser(ModelRequestDto modelRequest);
    }
}
