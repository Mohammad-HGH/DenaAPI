using DenaAPI.DTO;
using DenaAPI.Interfaces;
using DenaAPI.Models;
using DenaAPI.Responses;
using Microsoft.EntityFrameworkCore;
using System;

namespace DenaAPI.Services
{
    public class AttributeService : IAttributeService
    {
        private readonly DenaDbContext denaDbContext;

        public AttributeService(DenaDbContext denaDbContext)
        {
            this.denaDbContext = denaDbContext;
        }

        public async Task<AttributeResponse> GetAllAttrsAsync()
        {
            if (denaDbContext.Attributes == null)
            {
                return new AttributeResponse
                {
                    Success = false,
                    Message = "Not found!",
                    ErrorCode = "404"
                };
            }
            return new AttributeResponse
            {
                Success = true,
                Attributes = await denaDbContext.Attributes.ToListAsync()
            };

        }
        public async Task<AttributeResponse> GetAttrAsync(int attrId)
        {
            if (denaDbContext.Attributes == null)
            {
                return new AttributeResponse
                {
                    Success = false,
                    Message = "Not found!",
                    ErrorCode = "404"
                };
            }
            var attribute = await denaDbContext.Attributes.FindAsync(attrId);

            if (attribute == null)
            {
                return new AttributeResponse
                {
                    Success = false,
                    Message = "Not found!",
                    ErrorCode = "404"
                };
            }
            List<Models.Attribute> Attribute = new() { attribute };

            return new AttributeResponse
            {
                Success = true,
                Attributes = Attribute
            };
        }
        public async Task<AttributeResponse> DeleteAttrAsync(int attrId)
        {

            if (denaDbContext.Attributes == null)
            {
                return new AttributeResponse
                {
                    Success = false,
                    Message = "Not found!",
                    ErrorCode = "404"
                };
            }
            var attribute = await denaDbContext.Attributes.FindAsync(attrId);
            if (attribute == null)
            {
                return new AttributeResponse
                {
                    Success = false,
                    Message = "Not found!",
                    ErrorCode = "404"
                };
            }

            denaDbContext.Attributes.Remove(attribute);
            await denaDbContext.SaveChangesAsync();

            return new AttributeResponse
            {
                Success = true,
                Message = "Attribute deleted!",
            };
        }

        public async Task<AttributeResponse> CreateAttrAsync(AttributeRequest attributeRequest)
        {
            var Att = await denaDbContext.Attributes.SingleOrDefaultAsync(x => x.Type == attributeRequest.Type &&
                                                                              x.Size == attributeRequest.Size &&
                                                                              x.Color == attributeRequest.Color &&
                                                                              x.Brand == attributeRequest.Brand);
            if (Att == null)
            {
                Models.Attribute attribute1 = new()
                {
                    Brand = attributeRequest.Brand,
                    Color = attributeRequest.Color,
                    Size = attributeRequest.Size,
                    Type = attributeRequest.Type
                };
                denaDbContext.Attributes.Add(attribute1);
                await denaDbContext.SaveChangesAsync();
            }
            else
                return new AttributeResponse
                {
                    Success = false,
                    Message = "Attribute is Exist!",
                    ErrorCode = "500"
                }; ;
            return new AttributeResponse
            {
                Success = true,
                Message = "Attribute saved!"
            };
        }
        public async Task<AttributeResponse> UpdateAttrAsync(AttributeRequest attributeRequest)
        {
            var attribute1 = await denaDbContext.Attributes.FirstOrDefaultAsync(x => x.Id == attributeRequest.Id);

            if (attribute1 == null)
            {
                return new AttributeResponse
                {
                    Success = false,
                    Message = "Attribute Not found!",
                    ErrorCode = "404"
                };
            }

            attribute1.Brand = attributeRequest.Brand;
            attribute1.Color = attributeRequest.Color;
            attribute1.Size = attributeRequest.Size;
            attribute1.Type = attributeRequest.Type;

            try
            {
                await denaDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttributeExists(attributeRequest.Id))
                {
                    return new AttributeResponse
                    {
                        Success = false,
                        Message = "Attribute Not found!",
                        ErrorCode = "404"
                    };
                }
                else
                {
                    throw;
                }
            }

            return new AttributeResponse
            {
                Success = true,
                Message = "Attribute updated!",
            };

        }


        private bool AttributeExists(int id)
        {
            return (denaDbContext.Attributes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
