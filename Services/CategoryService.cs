using DenaAPI.DTO;
using DenaAPI.Interfaces;
using DenaAPI.Models;
using DenaAPI.Responses;
using Microsoft.EntityFrameworkCore;

namespace DenaAPI.Services
{
    public class CategoryService: ICategoryService
    {
        private readonly DenaDbContext denaDbContext;
        public CategoryService(DenaDbContext denaDbContext)
        {
            this.denaDbContext = denaDbContext;
        }
        public async Task<CategoryResponse> GetAllCatsAsync()
        {

            if (denaDbContext.Categories == null)
            {
                return new CategoryResponse
                {
                    Success = false,
                    Message = "Not found!",
                    ErrorCode = "404"
                };
            }
            return new CategoryResponse
            {
                Success = true,
                Categories = await denaDbContext.Categories.ToListAsync()
            };
        }
        public async Task<CategoryResponse> GetCatAsync(int id)
        {
            if (denaDbContext.Attributes == null)
            {
                return new CategoryResponse
                {
                    Success = false,
                    Message = "Not found!",
                    ErrorCode = "404"
                };
            }
            var category = await denaDbContext.Categories.FindAsync(id);

            if (category == null)
            {
                return new CategoryResponse
                {
                    Success = false,
                    Message = "Not found!",
                    ErrorCode = "404"
                };
            }
            List<Category> Category = new() { category };

            return new CategoryResponse
            {
                Success = true,
                Categories = Category
            };
        }
        public async Task<CategoryResponse> GetChildAsync(int parentid)
        {
            if (denaDbContext.Categories == null)
            {
                return new CategoryResponse
                {
                    Success = false,
                    Message = "Not found!",
                    ErrorCode = "404"
                };
            }
            var category = await denaDbContext.Categories.Where(c => c.ParentId == parentid).ToListAsync();
            
            return new CategoryResponse
            {
                Success = true,
                Categories = category
            };
        }
        public async Task<CategoryResponse> UpdateCatAsync(int id, CategoryRequest categoryRequest)
        {
            var category = new Category
            {
                Id = id,
                Name=categoryRequest.Name,
                ParentId=categoryRequest.ParentId
            };

            denaDbContext.Entry(category).State = EntityState.Modified;
            try { await denaDbContext.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return new CategoryResponse
                    {
                        Success = false,
                        Error = "Not found",
                        ErrorCode = "S02"
                    };
                }
                else throw;
            }

            return new CategoryResponse { Success = true, Message = "Category updated" };
        }
        public async Task<CategoryResponse> DeleteCatAsync(int id)
        {
            if (!CategoryExists(id))
            {
                return new CategoryResponse
                {
                    Success = false,
                    Message = "Not found!",
                    ErrorCode = "404"
                };
            }
            var category = await denaDbContext.Categories.FindAsync(id);
            if (category == null)
            {
                return new CategoryResponse
                {
                    Success = false,
                    Message = "Not found!",
                    ErrorCode = "404"
                };
            }

            denaDbContext.Categories.Remove(category);
            await denaDbContext.SaveChangesAsync();

            return new CategoryResponse
            {
                Success = true,
                Message = "Category deleted!",
            };
        }
        public async Task<CategoryResponse> CreateCatAsync(CategoryRequest categoryRequest)
        {
            var existingCategory = await denaDbContext.Categories.SingleOrDefaultAsync(category => category.Name == categoryRequest.Name);

            if (existingCategory != null)
            {
                return new CategoryResponse
                {
                    Success = false,
                    Error = "Category already exists with the same name",
                    ErrorCode = "500"
                };
            }
            var category= new Category
            {
                Name=categoryRequest.Name,
                ParentId=categoryRequest.ParentId
            };

            await denaDbContext.Categories.AddAsync(category);

            var saveResponse = await denaDbContext.SaveChangesAsync();

            if (saveResponse >= 0)
            {
                return new CategoryResponse { Success = true,Message="category createed!"};
            }

            return new CategoryResponse
            {
                Success = false,
                Error = "Unable to save the category",
                ErrorCode = "500"
            };
        }
        private bool CategoryExists(int id)
        {
            return (denaDbContext.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
