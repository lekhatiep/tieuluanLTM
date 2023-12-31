﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities.Catalog
{
    public class UploadFile
    {
        [Key]
        public int MediaID { get; set; }

        public int UserID { get; set; }

        public int ModelID { get; set; }

        public string Path { get; set; }

        public string FileName { get; set; }

        public bool IsDelete { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
