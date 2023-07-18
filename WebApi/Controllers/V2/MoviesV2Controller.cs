using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.DataBaseContext;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("2")]
    public class MoviesV2Controller : ControllerBase
    {
        private readonly DatabaseDbContext _databaseDbContext = null;
        public MoviesV2Controller(DatabaseDbContext databaseDbContext)
        {
            _databaseDbContext = databaseDbContext;
        }

        [Authorize]
        //[HttpGet("getAllData")]
        [HttpGet("v{version:apiVersion}/getAllData")]
        public IActionResult GetAll()
        {
            return Ok(_databaseDbContext.Movies);
        }
        [Authorize]
        // [HttpGet("getAllData")]
        [HttpGet("/v2/getAllData/{id}"), ApiVersion("2")]
        public IActionResult GetAll(int id)
        {
            return Ok(_databaseDbContext.Movies);
        }
        [Authorize]
        [HttpGet("v{version:apiVersion}/getMovieById/{id}")]
        public IActionResult Get(int id)
        {
            var data = _databaseDbContext.Movies.FirstOrDefault(x => x.Id == id);
            if (data == null)
            {
                return NotFound("Data not found for this Id");
            }
            return Ok(data);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Post(Movie movie)
        {
            _databaseDbContext.Movies.Add(movie);
            _databaseDbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("v{version:apiVersion}/{id}")]

        public IActionResult Put(int id, [FromBody] Movie movie)
        {
            var movieObj = _databaseDbContext.Movies.Find(id);
            if (movieObj == null)
            {
                return NotFound("Data not found for this Id");
            }
            movieObj.Name = movie.Name;
            movieObj.Language = movie.Language;
            _databaseDbContext.SaveChanges();
            return StatusCode(StatusCodes.Status200OK);
        }

        //[Authorize(Roles = "admin")]
        [HttpDelete("v{version:apiVersion}/deletemovie/{id}")]
        public IActionResult Delete(int id)
        {
            var movieObj = _databaseDbContext.Movies.Find(id);
            if (movieObj == null)
            {
                return NotFound("Data not found for this Id");
            }
            _databaseDbContext.Remove(movieObj);
            _databaseDbContext.SaveChanges();
            return StatusCode(StatusCodes.Status200OK);
        }
    }
}
