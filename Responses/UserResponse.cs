namespace DenaAPI.Responses
{
    public class UserResponse : BaseResponse
    {
        public string? Phone { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
