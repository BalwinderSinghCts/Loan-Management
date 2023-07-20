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

    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        
        //[Authorize(Roles = "Admin")]
        [HttpPost("v{version:apiVersion}/Addcustomer")]
        public async Task<IActionResult> AddCustomer([FromBody] CustomerVM model)
        {
            ResponseMessage<CustomerVM> response = new ResponseMessage<CustomerVM>();
            try
            {
                var data = await _customerService.AddCustomer(model);
                if (data is not null && data.Id > 0)
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
                _customerService.Dispose();
            }
        }
    }
}
