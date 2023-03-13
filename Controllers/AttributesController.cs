using Microsoft.AspNetCore.Mvc;
using DenaAPI.DTO;
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
        [Route("AttributesList")]
        public async Task<IActionResult> GetAttributes()
        {
            var getAttrResponse = await attributeService.GetAllAttrsAsync();

            if (!getAttrResponse.Success)
            {
                return BadRequest(getAttrResponse);
            }
            return Ok(getAttrResponse);
        }

        // GET: api/Attributes/5
        [HttpGet]
        [Route("ViewAttriuteById")]
        public async Task<IActionResult> GetAttribute(int id)
        {
            var getAttrResponse = await attributeService.GetAttrAsync(id);

            if (!getAttrResponse.Success)
            {
                return BadRequest(getAttrResponse);
            }
            return Ok(getAttrResponse);
        }

        // PUT: api/Attributes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("UpdateAttriute")]
        public async Task<IActionResult> PutAttribute([FromForm] int id, [FromForm] AttributeRequest attributeRequest)
        {
            var getAttrResponse = await attributeService.UpdateAttrAsync(id, attributeRequest);

            if (!getAttrResponse.Success)
            {
                return BadRequest(getAttrResponse);
            }
            return Ok(getAttrResponse);
        }

        // POST: api/Attributes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("CreateAttribute")]
        public async Task<IActionResult> PostAttribute([FromForm] AttributeRequest attributeRequest)
        {
            var getAttrResponse = await attributeService.CreateAttrAsync(attributeRequest);

            if (!getAttrResponse.Success)
            {
                return BadRequest(getAttrResponse);
            }
            return Ok(getAttrResponse);
        }

        // DELETE: api/Attributes/5
        [HttpDelete]
        [Route("DeleteAttribute")]
        public async Task<IActionResult> DeleteAttribute(int id)
        {
            var getAttrResponse = await attributeService.DeleteAttrAsync(id);

            if (!getAttrResponse.Success)
            {
                return BadRequest(getAttrResponse);
            }
            return Ok(getAttrResponse);
        }


    }
}
