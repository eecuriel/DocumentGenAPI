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
using DocumentGenAPI.Context;
using DocumentGenAPI.Entities;

namespace DocumentGenAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        public CurrencyController(ApplicationDbContext _context, IMapper _mapper )
        {
            context =_context;
            mapper = _mapper;
        }


        // GET api/currency/currencylist
        [HttpGet("currencylist")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin,regularuser")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "user")]
        public  async Task<ActionResult<IEnumerable<Currency>>> Get() 
        {
            var currency  = await context.Currencies.ToListAsync();

            return  currency;
        } 

          // GET api/currency/currencybyid/1
        [HttpGet("currencybyid/{id}",Name="OneCurrency")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "admin,regularuser")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "user")]
        public async Task<ActionResult<Currency>> GetbyId(int id) 
        {
            var currency  = await context.Currencies.FirstOrDefaultAsync(x => x.IdCurrency == id);

            if (currency == null)
            {
                return NotFound();
            }

            return  currency;
        }

         // POST api/currency/CreateCurrency
        [HttpPost("CreateCurrency")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "admin")]
        public async Task<ActionResult> Post([FromBody] Currency currency)
        {
            
            var _currency = new Currency{

                CurrencyDescription = currency.CurrencyDescription
            };
            context.Add(_currency);
            await context.SaveChangesAsync();

            return new CreatedAtRouteResult("OneCurrency", new { id = currency.IdCurrency }, currency);
        }

         // DELETE api/currency/DeleteCurrency
        [HttpDelete("DeleteCurrency/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "admin")]
        public async Task<ActionResult> Delete(int id)
        {
        
            var currency =  await context.Currencies.Select(x => x.IdCurrency).FirstOrDefaultAsync(x => x == id);

            if (currency == default(int))
            {
                return NotFound();
            }
            context.Remove(new Currency {IdCurrency= currency});
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}