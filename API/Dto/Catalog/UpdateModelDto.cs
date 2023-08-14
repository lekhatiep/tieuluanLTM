using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dto.Catalog
{
    public class UpdateModelDto
    {
        public int ModelID { get; set; }
        public string Name { get; set; }
        public string TypeName { get; set; }
        public IFormFile File { get; set; }
    }
}
