using System.ComponentModel.DataAnnotations;

namespace zwajapp.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }
        [StringLength(8,MinimumLength=4,ErrorMessage="Password doit contenir au minimuim 4 lettre et au max 8")]
        public string Password { get; set; }
    }
}