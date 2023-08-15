using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Common.ReponseDto
{
    public class ResponsePagedList<T>
    {
        public int TotalRecord { get; set; }
        public List<T> Data { get; set; }

        public ResponsePagedList(List<T> data, int totalRecord = 0)
        {
            TotalRecord = totalRecord;
            Data = data;
        }
    }
}
