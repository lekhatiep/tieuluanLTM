using System;

namespace API.Interface
{
    public interface ICommonProp
    {
        public DateTime CreatedDate { get; set; }
        public bool IsDelete { get; set; }
    }
}
