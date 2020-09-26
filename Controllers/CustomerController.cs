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
    public class CustomerController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        public CustomerController(ApplicationDbContext _context, IMapper _mapper )
        {
            context =_context;
            mapper = _mapper;
        }


        // GET api/customer/customerlist
        [HttpGet("customerlist")]//full list
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin,regularuser")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "user")]
        public  async Task<ActionResult<IEnumerable<Customer>>> Get() 
        {
            var customer  = await context.Customers.ToListAsync();

            return  customer;
        } 

          // GET api/Customer/customerlist/1
        [HttpGet("customerlist/{idcustomer}/{IdUserOwner}",Name="OneCustomer")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "admin,regularuser")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "user")]
        public async Task<ActionResult<Customer>> GetbyId(int Idcustomer, string IdUserOwner) 
        {
            var customer  = await context.Customers.Where(x => x.IdCustomer == Idcustomer &&  x.IdUserOwner == IdUserOwner ).FirstOrDefaultAsync();

            if (customer == null)
            {
                return NotFound();
            }

            return  customer;
        }

         // POST api/Frequency/CreateCustomer
        [HttpPost("CreateCustomer")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "admin")]
        public async Task<ActionResult> Post([FromBody] Customer customer)
        {
            
            var _customer = new Customer {

                IdCustomer = customer.IdCustomer,
                IdUserOwner = customer.IdUserOwner,
                CustomerFirstName  = customer.CustomerFirstName,
                CustomerLastName = customer.CustomerLastName,
                ComercialName = customer.ComercialName,
                CustomerEmail = customer.CustomerEmail,
                CustomerPhone1 = customer.CustomerPhone1,
                CustomerPhone2 = customer.CustomerPhone2,
                CustomerAddress = customer.CustomerAddress,
                CustomerCountry = customer.CustomerCountry,
                CustomerCity = customer.CustomerCity,
                CustomerState = customer.CustomerState,
                CustomerPostalCode = customer.CustomerPostalCode,
                Latitude = customer.Latitude,
                Longitude = customer.Longitude,
                CustomerCreationDate = DateTime.UtcNow

            };
            context.Add(_customer);
            await context.SaveChangesAsync();

            return new CreatedAtRouteResult("OneCustomer", new { id = customer.IdCustomer }, customer);
        }

         // DELETE api/Frequency/DeleteFrequency
        [HttpDelete("DeleteCustomer/{idcustomer}/{IdUserOwner}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "admin")]
        public async Task<ActionResult> Delete(int Idcustomer, string IdUserOwner)
        {
        
            var customer =  await context.Customers.Where(x => x.IdCustomer == Idcustomer  && x.IdUserOwner == IdUserOwner).FirstOrDefaultAsync();
            
            context.Remove(customer);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}