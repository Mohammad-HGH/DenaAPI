using DenaAPI.Models;

namespace DenaAPI.DTO
{
    public class UpdateRequest : User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
