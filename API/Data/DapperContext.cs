using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Linq;

namespace API.Data
{
    public class DapperContext
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

        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);

        public string ConnectionString { get { return _configuration.GetConnectionString("DefaultConnection"); }}
    }
}
