using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dto.Catalog
{
    public class ModelDto
    {
        public int ModelID { get; set; }
        public int UserID { get; set; }
        public int MediaID { get; set; }
        public string PathImage { get; set; }
        public string Name { get; set; }
        public string TypeName { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
