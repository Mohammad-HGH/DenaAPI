using DenaAPI.DTO;
using DenaAPI.Interfaces;
using DenaAPI.Models;
using DenaAPI.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;

namespace DenaAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly DenaDbContext denaDbContext;

        public ProductService(DenaDbContext denaDbContext)
        {
            this.denaDbContext = denaDbContext;
        }

        public async Task<ProductResponse> GetAllProdsAsync()
        {
            if (denaDbContext.Products == null)
            {
                return new ProductResponse
                {
                    Success = false,
                    Message = "Not found!",
                    ErrorCode = "404"
                };
            }
            return new ProductResponse
            {
                Success = true,
                Products = await denaDbContext.Products.ToListAsync()
            };

        }
        public async Task<ProductResponse> GetProdAsync(int ProdId)
        {
            if (denaDbContext.Products == null)
            {
                return new ProductResponse
                {
                    Success = false,
                    Message = "Not found!",
                    ErrorCode = "404"
                };
            }
            var product = await denaDbContext.Products.FindAsync(ProdId);

            if (product == null)
            {
                return new ProductResponse
                {
                    Success = false,
                    Message = "Not found!",
                    ErrorCode = "404"
                };
            }
            List<Models.Product> Product = new() { product };

            return new ProductResponse
            {
                Success = true,
                Products = Product
            };
        }
        public async Task<ProductResponse> DeleteProdAsync(int ProdId)
        {

            if (denaDbContext.Products == null)
            {
                return new ProductResponse
                {
                    Success = false,
                    Message = "Not found!",
                    ErrorCode = "404"
                };
            }
            var product = await denaDbContext.Products.FindAsync(ProdId);
            if (product == null)
            {
                return new ProductResponse
                {
                    Success = false,
                    Message = "Not found!",
                    ErrorCode = "404"
                };
            }

            denaDbContext.Products.Remove(product);
            await denaDbContext.SaveChangesAsync();

            return new ProductResponse
            {
                Success = true,
                Message = "Product deleted!",
            };
        }
        public async Task<ProductResponse> CreateProdAsync(ProductRequest productRequest, AttributeRequest attributeRequest, long price)
        {
            var existingProduct = await denaDbContext.Products.SingleOrDefaultAsync(
                product => product.CatId == productRequest.CatId
                && product.Name == productRequest.Name
                );
            var existingAttribute = await denaDbContext.Attributes.SingleOrDefaultAsync(
                    attribute => attribute.Type == attributeRequest.Type
                    && attribute.Size == attributeRequest.Size
                    && attribute.Color == attributeRequest.Color
                    && attribute.Brand == attributeRequest.Brand
                    );
            var product = new Product();
            var attribute = new Models.Attribute();
            if (existingProduct == null && existingAttribute == null)
            {


                product.Name = productRequest.Name;
                product.CatId = productRequest.CatId;

                await denaDbContext.AddAsync(product);
                await denaDbContext.SaveChangesAsync();

                attribute.Color = attributeRequest.Color;
                attribute.Brand = attributeRequest.Brand;
                attribute.Size = attributeRequest.Size;
                attribute.Type = attributeRequest.Type;
                await denaDbContext.AddAsync(attribute);
                await denaDbContext.SaveChangesAsync();
            }
            else if (existingProduct == null && existingAttribute != null)
            {
                product.Name = productRequest.Name;
                product.CatId = productRequest.CatId;
                await denaDbContext.AddAsync(product);
                await denaDbContext.SaveChangesAsync();

                attribute.Id = existingAttribute.Id;
            }
            else if (existingProduct != null && existingAttribute == null)
            {
                product.Id = existingProduct.Id;

                attribute.Color = attributeRequest.Color;
                attribute.Brand = attributeRequest.Brand;
                attribute.Size = attributeRequest.Size;
                attribute.Type = attributeRequest.Type;
                await denaDbContext.AddAsync(attribute);
                await denaDbContext.SaveChangesAsync();
            }
            else if (existingProduct != null && existingAttribute != null)
            {
                return new ProductResponse
                {
                    Success = false,
                    Message = "Attribute is Exist!",
                    ErrorCode = "500"
                };
            }
            var intermediate = new Intermediate
            {
                ProductId = product.Id,
                AttributeId = attribute.Id,
                Price = price,
            };
            await denaDbContext.AddAsync(intermediate);
            await denaDbContext.SaveChangesAsync();
            return new ProductResponse
            {
                Success = true,
                Message = "Attribute saved!"
            };
        }
        public async Task<ProductResponse> UpdateProdAsync(int id, ProductRequest productRequest)
        {
            var existingProduct = await denaDbContext.Products.FindAsync(id);

            if (existingProduct == null)
            {
                return new ProductResponse
                {
                    Success = false,
                    Message = "Attribute Not found!",
                    ErrorCode = "404"
                };
            }

            existingProduct.Name = productRequest.Name;
            existingProduct.CatId = productRequest.CatId;

            try
            {
                await denaDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(existingProduct.Id))
                {
                    return new ProductResponse
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

            return new ProductResponse
            {
                Success = true,
                Message = "Attribute updated!",
            };

        }


        private bool ProductExists(int id)
        {
            return (denaDbContext.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
