using System.ComponentModel.DataAnnotations;

namespace API.Entities.Auth
{
    public class RegistrationRole
    {
        [Key]
        public int UserID { get; set; }
        public int RoleID { get; set; }
    }
}
