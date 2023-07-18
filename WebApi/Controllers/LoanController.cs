using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Repository;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1")]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _loanService;
        public LoanController(ILoanService loanService)
        {
            _loanService = loanService;
        }
        [HttpPost("v{version:apiVersion}/searchLoans")]
        public async Task<IEnumerable<LoanVM>> SearchLoanDetails([FromBody] LoanVMFilters LoanVMFilters)
        {
            return await _loanService.SearchLoan(LoanVMFilters);
        }
        [HttpGet("v{version:apiVersion}/LoanDetail")]
        public async Task<LoanVM> LoanDetail(string loanNumber)
        {
            return await _loanService.LoanDetails(loanNumber);
        }
        //[Authorize("Admin")]
        [HttpPost("v{version:apiVersion}/AddLoan")]
        public async Task<IActionResult> AddLoan([FromBody] LoanVM model)
        {
            try
            {
                var data = await _loanService.AddLoan(model);
                if (data.IsSuccess)
                {
                    return Ok(data);
                }
                return BadRequest(data);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpPut("v{version:apiVersion}/UpdateLoan")]
        public async Task<IActionResult> UpdateLoan([FromBody] LoanVM model)
        {
            var data = await _loanService.UpdateLoan(model);
            return Ok(data);
        }
    }
}
