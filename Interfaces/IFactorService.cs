using DenaAPI.DTO;
using DenaAPI.Responses;

namespace DenaAPI.Interfaces
{
    public interface IFactorService
    {
        Task<FactorResponse> CreateFacAsync(FactorRequest factorRequest, PostDetailRequest postDetailRequest);
        Task<FactorResponse> GetFacsAsync();
        Task<FactorResponse> ExportExcelAsync();
        Task<FactorResponse> GetFacAsync(int id);
        Task<FactorResponse> DeleteFacAsync(int id);
        Task<FactorResponse> UpdateFacAsync(int id, FactorRequest factorRequest);
        Task<FactorResponse> UpdatePostAsync(int id, PostDetailRequest postDetailRequest);
    }
}
