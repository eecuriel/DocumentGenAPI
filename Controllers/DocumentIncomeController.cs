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
using MyExpManAPI.Helpers;

namespace MyExpManAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(ActionFilters))]
    public class DocumentIncomeController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly ILogManager logmanager;
        public DocumentIncomeController(ApplicationDbContext _context, IMapper _mapper,ILogManager _logmanager)
        {
            context =_context;
            mapper = _mapper;
            logmanager =_logmanager;
        }

        // GET api/DocumentIncome/expenseincomelist
        [HttpGet("expenseincomelist")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin,regularuser")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "user")]
        public  async Task<ActionResult<IEnumerable<DocumentIncome>>> Get() 
        {
            var documentincome  = await context.DocumentIncomes.ToListAsync();

            return  documentincome;
        } 

          // GET api/DocumentIncome/expenseincomebyid/1
        [HttpGet("expenseincomebyid/{id}",Name="OneExpenseIncome")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "admin,regularuser")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "user")]
        public async Task<ActionResult<DocumentIncome>> GetbyId(int id) 
        {
            var documentincome  = await context.DocumentIncomes.FirstOrDefaultAsync(x => x.IdIncomeList == id);

            if (documentincome == null)
            {
                return NotFound();
            }

            return  documentincome;
        }

         // POST api/DocumentIncome/CreateExpenseIncome
        [HttpPost("CreateExpenseIncome")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "admin,regularuser")]
        public async Task<ActionResult> Post([FromBody] DocumentIncome documentincome)
        {
            
            var _documentincome = new DocumentIncome{
                
                IncomeDescription = documentincome.IncomeDescription,
                IdCurrency = documentincome.IdCurrency,
                IdDocument = documentincome.IdDocument,
                IncomeAmount = documentincome.IncomeAmount
            };
            context.Add(_documentincome);
            await context.SaveChangesAsync();
            logmanager.CreateLog("Informacion",$"Income List  No.{_documentincome.IdIncomeList} has been created");

            return new CreatedAtRouteResult("OneExpenseIncome", new { id = documentincome.IdIncomeList}, documentincome);
        }

         // PUT api/DocumentIncome/UpdateExpenseIncome/1
        [HttpPut("UpdateExpenseIncome/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,  Roles= "admin,regularuser")]
        public async Task<ActionResult> Put(int id, [FromBody] DocumentIncome documentincome)
        {
            if(id !=  documentincome.IdIncomeList)
            {
                return BadRequest();
            }

            context.Entry(documentincome).State = EntityState.Modified;
            await context.SaveChangesAsync();

            logmanager.CreateLog("Informacion",$"Income List  No.{documentincome.IdIncomeList} has been updated");

            return new CreatedAtRouteResult("OneExpenseIncome", new { id = documentincome.IdIncomeList}, documentincome);
        }

        // DELETE api/DocumentIncome/DeleteExpenseIncome/1
        [HttpDelete("DeleteExpenseIncome/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,  Roles= "admin,regularuser")]
        public async Task<ActionResult> Delete(int id)
        {
        
            var documentincome =  await context.DocumentIncomes.Select(x => x.IdIncomeList).FirstOrDefaultAsync(x => x == id);

            if (documentincome == default(int))
            {
                return NotFound();
            }
            context.Remove(new DocumentIncome {IdIncomeList= documentincome});
            await context.SaveChangesAsync();

            logmanager.CreateLog("Informacion",$"Income List  No.{documentincome} has been deleted");
            
            return NoContent();
        }
    }
}