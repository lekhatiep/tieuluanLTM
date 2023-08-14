using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities.Catalog
{
    public class RoboModel
    {
        [Key]
        public int ModelID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public string TypeName { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
