using Microsoft.AspNetCore.Mvc;
using DenaAPI.DTO;
using DenaAPI.Interfaces;
using ClosedXML.Excel;

namespace DenaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FactorsController : BaseApiController
    {
        private readonly IFactorService factorService;

        public FactorsController([FromForm] IFactorService factorService)
        {
            this.factorService = factorService;
        }

        // GET: api/Factors
        [HttpGet]
        [Route("FactorsList")]
        public async Task<IActionResult> GetFactors()
        {
            var getFacResponse = await factorService.GetFacsAsync();

            if (!getFacResponse.Success)
            {
                return BadRequest(getFacResponse);
            }
            return Ok(getFacResponse);
        }

        // GET: api/Factors/5
        [HttpGet]
        [Route("ViewFactorById")]
        public async Task<IActionResult> GetFactor(int id)
        {
            var getFacResponse = await factorService.GetFacAsync(id);

            if (!getFacResponse.Success)
            {
                return BadRequest(getFacResponse);
            }
            return Ok(getFacResponse);
        }

        // PUT: api/Factors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("UpdateFactor")]
        public async Task<IActionResult> PutFactor(int id, [FromForm] FactorRequest factorRequest)
        {
            var getFacResponse = await factorService.UpdateFacAsync(id, factorRequest);

            if (!getFacResponse.Success)
            {
                return BadRequest(getFacResponse);
            }
            return Ok(getFacResponse);
        }

        [HttpPut]
        [Route("UpdatePostalDetail")]
        public async Task<IActionResult> PutPostDetails(int id, [FromForm] PostDetailRequest postDetailRequest, bool pid = true)
        {
            var getFacResponse = await factorService.UpdatePostAsync(id, postDetailRequest);

            if (!getFacResponse.Success)
            {
                return BadRequest(getFacResponse);
            }
            return Ok(getFacResponse);
        }

        // POST: api/Factors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("CreateFactor")]
        public async Task<IActionResult> PostFactor([FromForm] FactorRequest factorRequest, [FromForm] PostDetailRequest postDetailRequest)
        {
            var getFacResponse = await factorService.CreateFacAsync(factorRequest, postDetailRequest);

            if (!getFacResponse.Success)
            {
                return BadRequest(getFacResponse);
            }
            return Ok(getFacResponse);
        }

        // DELETE: api/Factors/5
        [HttpDelete]
        [Route("DeleteFactor")]
        public async Task<IActionResult> DeleteFactor(int id)
        {
            var getFacResponse = await factorService.DeleteFacAsync(id);

            if (!getFacResponse.Success)
            {
                return BadRequest(getFacResponse);
            }
            return Ok(getFacResponse);
        }

        [HttpGet]
        [Route("ExportFactorExcel")]
        public async Task<IActionResult> ExportToExcel()
        {
            var getFacResponse = await factorService.ExportExcelAsync();

            if (!getFacResponse.Success) return BadRequest(getFacResponse);


            //using ClosedXML.Excel;  
            using XLWorkbook wb = new();
            wb.Worksheets.Add(getFacResponse.ExportData);
            using MemoryStream stream = new();
            wb.SaveAs(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Factor-{DateTime.Now}.xlsx");
        }
    }
}
