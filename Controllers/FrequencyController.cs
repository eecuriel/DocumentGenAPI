using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyExpManAPI.Context;
using MyExpManAPI.Entities;

namespace MyExpManAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FrequencyController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        public FrequencyController(ApplicationDbContext _context, IMapper _mapper )
        {
            context =_context;
            mapper = _mapper;
        }


        // GET api/frequency/frequencylist
        [HttpGet("frequencylist")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin,regularuser")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "user")]
        public  async Task<ActionResult<IEnumerable<Frequency>>> Get() 
        {
            var frecuency  = await context.Frequencies.ToListAsync();

            return  frecuency;
        } 

          // GET api/Frequency/frequencylist/1
        [HttpGet("frequencylist/{id}",Name="OneFrecuency")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "admin,regularuser")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "user")]
        public async Task<ActionResult<Frequency>> GetbyId(int id) 
        {
            var frecuency  = await context.Frequencies.FirstOrDefaultAsync(x => x.IdFrenquency == id);

            if (frecuency == null)
            {
                return NotFound();
            }

            return  frecuency;
        }

         // POST api/Frequency/CreateFrecuency
        [HttpPost("CreateFrecuency")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "admin")]
        public async Task<ActionResult> Post([FromBody] Frequency frequency)
        {
            
            var _frequency = new Frequency {

                FrequencyDescription = frequency.FrequencyDescription
            };
            context.Add(_frequency);
            await context.SaveChangesAsync();

            return new CreatedAtRouteResult("OneFrecuency", new { id = frequency.IdFrenquency }, frequency);
        }

         // DELETE api/Frequency/DeleteFrequency
        [HttpDelete("DeleteFrequency/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "admin")]
        public async Task<ActionResult> Delete(int id)
        {
        
            var frequency =  await context.Frequencies.Select(x => x.IdFrenquency).FirstOrDefaultAsync(x => x == id);

            if (frequency == default(int))
            {
                return NotFound();
            }
            context.Remove(new Frequency {IdFrenquency= frequency});
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}