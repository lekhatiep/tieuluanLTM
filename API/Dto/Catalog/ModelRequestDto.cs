using API.Common.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dto.Catalog
{
    public class ModelRequestDto : PagingRequetBase
    {
        public string SortBy { get; set; }

        //Filter
        public string KeySearch { get; set; }
        //Values

        public string ValueSearch { get; set; }

        public bool Desc { get;  set; }
    }
}
