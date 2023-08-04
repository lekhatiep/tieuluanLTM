using System.ComponentModel.DataAnnotations;

namespace API.Dto.Auth
{
    public class CreateUserDto
    {
        [Required]
        public string Email { get; set; }

        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
