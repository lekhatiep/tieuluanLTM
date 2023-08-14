using System;

namespace API.Dto.Catalog
{
    public class ListModelDto
    {
        public int ModelID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public string TypeName { get; set; }
        public string ImgPath { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
