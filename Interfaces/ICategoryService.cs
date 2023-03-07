using DenaAPI.DTO;
using DenaAPI.Responses;

namespace DenaAPI.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryResponse> DeleteCatAsync(int id);
        Task<CategoryResponse> GetCatAsync(int id);
        Task<CategoryResponse> GetAllCatsAsync();
        Task<CategoryResponse> UpdateCatAsync(int id,CategoryRequest categoryRequest);
        Task<CategoryResponse> CreateCatAsync(CategoryRequest categoryRequest);
        Task<CategoryResponse> GetChildAsync(int parentid);
    }
}
