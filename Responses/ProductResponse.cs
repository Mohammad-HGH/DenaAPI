using DenaAPI.Models;

namespace DenaAPI.Responses
{
    public class ProductResponse : BaseResponse
    {
        public List<Product>? Products { get; set; }
        public string? Message { get; set; }
    }
}
