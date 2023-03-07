using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DenaAPI.DTO;
using DenaAPI.Services;
using DenaAPI.Interfaces;

namespace DenaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttributesController : BaseApiController
    {
        private readonly IAttributeService attributeService;

        public AttributesController([FromForm] IAttributeService attributeService)
        {
            this.attributeService = attributeService;
        }

        // GET: api/Attributes
        [HttpGet]
        public async Task<IActionResult> GetAttributes()
        {
            var getAttrResponse = await attributeService.GetAllAttrsAsync();

            if (!getAttrResponse.Success)
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = "Not found",
                    ErrorCode = "S02"
                });
            }
            return Ok(getAttrResponse);
        }

        // GET: api/Attributes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAttribute([FromForm] int id)
        {
            var getAttrResponse = await attributeService.GetAttrAsync(id);

            if (!getAttrResponse.Success)
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = "Not found",
                    ErrorCode = "S02"
                });
            }
            return Ok(getAttrResponse);
        }

        // PUT: api/Attributes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutAttribute([FromForm] AttributeRequest attributeRequest)
        {
            var getAttrResponse = await attributeService.UpdateAttrAsync(attributeRequest);

            if (!getAttrResponse.Success)
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = "Error!",
                    ErrorCode = "500"
                });
            }
            return Ok(getAttrResponse);
        }

        // POST: api/Attributes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostAttribute([FromForm] AttributeRequest attributeRequest)
        {
            var getAttrResponse = await attributeService.CreateAttrAsync(attributeRequest);

            if (!getAttrResponse.Success)
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = "Attribute is Exist!",
                    ErrorCode = "500"
                });
            }
            return Ok(getAttrResponse);
        }

        // DELETE: api/Attributes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttribute(int id)
        {
            var getAttrResponse = await attributeService.DeleteAttrAsync(id);

            if (!getAttrResponse.Success)
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = "Not found",
                    ErrorCode = "S02"
                });
            }
            return Ok(getAttrResponse);
        }


    }
}
