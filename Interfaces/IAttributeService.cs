using DenaAPI.DTO;
using DenaAPI.Responses;

namespace DenaAPI.Interfaces
{
    public interface IAttributeService
    {
        Task<AttributeResponse> GetAllAttrsAsync();
        Task<AttributeResponse> GetAttrAsync(int attrId);
        Task<AttributeResponse> DeleteAttrAsync(int attrId);
        Task<AttributeResponse> CreateAttrAsync(AttributeRequest attributeRequest);
        Task<AttributeResponse> UpdateAttrAsync(AttributeRequest attributeRequest);
    }
}
