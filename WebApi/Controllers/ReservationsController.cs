using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.DataBaseContext;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly DatabaseDbContext _databaseDbContext;

        public ReservationsController(DatabaseDbContext databaseDbContext)
        {
            _databaseDbContext = databaseDbContext;
        }
        

    }
}
