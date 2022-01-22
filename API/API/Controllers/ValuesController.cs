using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

/*
 * 
 * This Controller is only for testing porpose
 * 
 */
namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        
        private readonly DataContext _dataContext;
        ///constructor 
        public ValuesController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        // GET: api/<ValuesController>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Values>> Get()
        {
            var value = await _dataContext.Values.ToListAsync();
            return Ok(value);
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Values>> Get(int id)
        {
            return await _dataContext.Values.SingleOrDefaultAsync(x => x.Id == id);
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
