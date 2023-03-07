using DenaAPI.DTO;
using DenaAPI.Responses;

namespace DenaAPI.Interfaces
{
    public interface IProductService
    {
        Task<ProductResponse> GetAllProdsAsync();
        Task<ProductResponse> GetProdAsync(int ProdId);
        Task<ProductResponse> DeleteProdAsync(int ProdId);
        Task<ProductResponse> CreateProdAsync(ProductRequest prductRequest, AttributeRequest attributeRequest, long price);
        Task<ProductResponse> UpdateProdAsync(int id, ProductRequest prductRequest);
    }
}
