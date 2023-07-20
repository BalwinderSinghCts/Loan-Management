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

        [HttpGet("v{version:apiVersion}/customerlist")]
        public async Task<IActionResult> GetCustomers()
        {
            ResponseMessage<CustomerVM> response = new ResponseMessage<CustomerVM>();
            try
            {
                var datalist = await _customerService.CustomerList();
                if (datalist is not null && datalist.Count() > 0)
                {
                    response.IsSuccess = true;
                    response.ListData = datalist;
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

        [HttpGet("v{version:apiVersion}/customerdetail")]
        public async Task<IActionResult> GetCustomerDetail(string id)
        {
            ResponseMessage<CustomerVM> response = new ResponseMessage<CustomerVM>();
            try
            {
                var data = await _customerService.GetCustomerDetail(Convert.ToInt32(id));
                if (data is not null && data.Id > 0)
                {
                    response.IsSuccess = true;
                    response.Data = data;
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
