using Microsoft.AspNetCore.Mvc;
using DenaAPI.Interfaces;
using DenaAPI.DTO;

namespace DenaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : BaseApiController
    {
        private readonly ICategoryService categoryService;

        public CategoriesController([FromForm] ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }


        // GET: api/Categories
        [HttpGet]
        [Route("CategoriesList")]
        public async Task<IActionResult> GetCategory()
        {
            var getCatResponse = await categoryService.GetAllCatsAsync();

            if (!getCatResponse.Success)
            {
                return BadRequest(getCatResponse);
            }
            return Ok(getCatResponse);
        }

        [HttpGet]
        [Route("GetChildren")]
        public async Task<IActionResult> GetChildren(int parendId, bool isparent = true)
        {
            var getCatResponse = await categoryService.GetChildAsync(parendId);

            if (!getCatResponse.Success)
            {
                return BadRequest(getCatResponse);
            }
            return Ok(getCatResponse);
        }

        // GET: api/Categories/5
        [HttpGet]
        [Route("ViewCategoryById")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var getCatResponse = await categoryService.GetCatAsync(id);

            if (!getCatResponse.Success)
            {
                return BadRequest(getCatResponse);
            }
            return Ok(getCatResponse);
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("UpdateCategory")]
        public async Task<IActionResult> PutCategory(int id, CategoryRequest categoryRequest)
        {
            var getCatResponse = await categoryService.UpdateCatAsync(id, categoryRequest);

            if (!getCatResponse.Success)
            {
                return BadRequest(getCatResponse);
            }
            return Ok(getCatResponse);
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("CreateCategory")]
        public async Task<IActionResult> PostCategory(CategoryRequest categoryRequest)
        {
            var getCatResponse = await categoryService.CreateCatAsync(categoryRequest);

            if (!getCatResponse.Success)
            {
                return BadRequest(getCatResponse);
            }
            return Ok(getCatResponse);
        }

        // DELETE: api/Categories/5
        [HttpDelete]
        [Route("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var getCatResponse = await categoryService.DeleteCatAsync(id);

            if (!getCatResponse.Success)
            {
                return BadRequest(getCatResponse);
            }
            return Ok(getCatResponse);
        }


    }
}
