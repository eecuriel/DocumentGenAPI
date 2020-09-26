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
    public class ConceptController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        public ConceptController(ApplicationDbContext _context, IMapper _mapper )
        {
            context =_context;
            mapper = _mapper;
        }


        // GET api/concept/conceptlist
        [HttpGet("conceptlist")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin,regularuser")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "user")]
        public  async Task<ActionResult<IEnumerable<Concept>>> Get() 
        {
            var concepts  = await context.Concepts.ToListAsync();

            return  concepts;
        } 

          // GET api/concept/conceptbyid/1
        [HttpGet("conceptbyid/{id}",Name="OneConcept")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "admin,regularuser")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "user")]
        public async Task<ActionResult<Concept>> GetbyId(int id) 
        {
            var concept  = await context.Concepts.FirstOrDefaultAsync(x => x.IdConcept == id);

            if (concept == null)
            {
                return NotFound();
            }

            return  concept;
        }

         // POST api/concept/CreateConcept
        [HttpPost("CreateConcept")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,  Roles= "admin,regularuser")]
        public async Task<ActionResult> Post([FromBody] Concept concept)
        {
            
            var _concept = new Concept{

                ConceptDescription = concept.ConceptDescription
            };
            context.Add(_concept);
            await context.SaveChangesAsync();

            return new CreatedAtRouteResult("OneConcept", new { id = concept.IdConcept }, concept);
        }

         // DELETE api/concept/DeleteConcept
        [HttpDelete("DeleteConcept/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "admin,regularuser")]
        public async Task<ActionResult> Delete(int id)
        {
        
            var concept =  await context.Concepts.Select(x => x.IdConcept).FirstOrDefaultAsync(x => x == id);

            if (concept == default(int))
            {
                return NotFound();
            }
            context.Remove(new Concept {IdConcept = concept});
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}