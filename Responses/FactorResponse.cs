using DenaAPI.Models;

namespace DenaAPI.Responses
{
    public class FactorResponse : BaseResponse
    {
        public string? Message { get; set; }
        public List<Factor>? Factors { get; set; }
    }
}
