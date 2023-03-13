using DenaAPI.Models;
using System.Data;
namespace DenaAPI.Responses
{
    public class FactorResponse : BaseResponse
    {
        public string? Message { get; set; }
        public DataTable ExportData { get; set; }
        public List<Factor>? Factors { get; set; }
    }
}
