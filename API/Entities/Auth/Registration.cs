using API.Interface;
using System;
using System.ComponentModel.DataAnnotations;

namespace API.Entities.Auth
{
    public class Registration : ICommonProp
    {
        [Key]
        public int UserID { get; set; }
        public  string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get ; set ; }
        public bool IsDelete { get; set ; }
    }
}
