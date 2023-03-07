namespace DenaAPI.Responses
{
    public class CategoryResponse : BaseResponse
    {
        public List<Models.Category>? Categories { get; set; }
        public string Message { get; set; }
    }
}
