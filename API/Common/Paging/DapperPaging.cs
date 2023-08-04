using Dapper;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace API.Common.Paging
{
    public  static class DapperPaging
    {
        const string SqlPagging =
            @"WITH ListPaged As
            ( SELECT    ROW_NUMBER() OVER ( ORDER BY {0}) AS RowNum, {1}
                      FROM      {2}
                      WHERE     {3}
            )
            SELECT * FROM ListPaged
            WHERE   RowNum > {4} 
                AND RowNum < {5}";


        const string sqlShowMore = @"SELECT TOP {0}  1 AS RowNum, {2}
                                FROM      {3}
                                WHERE     {4} AND {5}
                                ORDER BY    {1}";
        public static async Task<PagedList<T>> QueryPagingAsync<T>(this SqlConnection conn, string sqlSelect, string sqlFrom, string sqlWhere, string OrderBy, dynamic param = null, int pageindex = 1, int? pagesize = 10, string MaxId = "", bool useOver = false)
        {
            string querypaging = "";
            //int totalRecord;
            pagesize = pagesize ?? 0;
            if (MaxId != "")
            {
                //totalRecord = pagesize.Value * (pageindex + 2);
                querypaging = String.Format(sqlShowMore, pagesize, OrderBy, sqlSelect, sqlFrom, sqlWhere, MaxId);
            }
            else
            {
                querypaging = String.Format(" SET ARITHABORT ON ; SELECT COUNT(1) " + (useOver ? " OVER() " : "") + " FROM {0} WHERE {1}", sqlFrom, sqlWhere);
                //totalRecord = await conn.QueryAsync<int>(querypaging, (object)param).Result.FirstOrDefault();

                int from = pageindex * pagesize.Value;
                int to = (from + pagesize.Value + 1) <= 1 ? int.MaxValue : from + pagesize.Value + 1;
                if (pagesize == 0) to = int.MaxValue;

                querypaging = String.Format(SqlPagging, OrderBy, sqlSelect, sqlFrom, sqlWhere, from, to);
            }

            var data = await conn.QueryAsync<T>(querypaging, (object)param);
            return PagedList<T>.ToPagedList(data, pageindex, (pagesize > 0 ? pagesize.Value : int.MaxValue));
        }
    }
}
