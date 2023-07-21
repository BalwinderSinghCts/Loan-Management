using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Repository;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1")]
    [EnableCors()]

    public class LoanController : ControllerBase
    {
        private readonly ILoanService _loanService;
        public LoanController(ILoanService loanService)
        {
            _loanService = loanService;
        }
        [HttpPost("v{version:apiVersion}/searchLoans")]
        public async Task<IActionResult> SearchLoanDetails([FromBody] LoanVMFilters LoanVMFilters)
        {
            try
            {
                var res = await _loanService.SearchLoan(LoanVMFilters);
                ResponseMessage<LoanVM> response = new ResponseMessage<LoanVM>();
                response.IsSuccess = true;
                response.ListData = res;
                return Ok(response);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _loanService.Dispose();
            }
        }
        [HttpGet("v{version:apiVersion}/LoanDetail")]
        public async Task<IActionResult> LoanDetail(string loanNumber)
        {
            try
            {
                var res = await _loanService.LoanDetails(loanNumber);
                ResponseMessage<LoanVM> response = new ResponseMessage<LoanVM>();
                response.IsSuccess = true;
                response.Data = res;
                return Ok(response);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _loanService.Dispose();
            }
        }
        //[Authorize(Roles = "Admin")]
        [HttpPost("v{version:apiVersion}/AddLoan")]
        public async Task<IActionResult> AddLoan([FromBody] LoanVM model)
        {
            ResponseMessage<LoanVM> response = new ResponseMessage<LoanVM>();
            try
            {
                var data = await _loanService.AddLoan(model);

                if (data is not null && data.LoanId > 0 && data.CustomerId > 0)
                {
                    response.IsSuccess = true;
                    response.Data = data;
                    response.Message = "Data Saved successfully";
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _loanService.Dispose();
            }
        }
        //[Authorize(Roles ="Admin")]
        [HttpPost("v{version:apiVersion}/UpdateLoan")]
        public async Task<IActionResult> UpdateLoan([FromBody] LoanVM model)
        {
            ResponseMessage<LoanVM> response = new ResponseMessage<LoanVM>();
            try
            {
                var data = await _loanService.UpdateLoan(model);
                if (data is not null && data.LoanId > 0)
                {
                    response.IsSuccess = true;
                    response.Data = data;
                    response.Message = "Data updated successfully";
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _loanService.Dispose();
            }

        }
    }
}
