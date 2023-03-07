using System.ComponentModel.DataAnnotations;

namespace DenaAPI.DTO
{
    public class SignupRequest
    {
        [Required]
        public string? Phone { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? ConfirmPassword { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }

    }
}
