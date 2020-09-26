using System.Threading;
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
    public class DocumentHeaderController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        
        private readonly ILogManager logmanager;
        public DocumentHeaderController(ApplicationDbContext _context, IMapper _mapper,ILogManager _logmanager)
        {
            context =_context;
            mapper = _mapper;
            logmanager = _logmanager;
        }

        // GET api/DocumentHeader/expenseheaderlist
        [HttpGet("expenseheaderlist")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin,regularuser")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "user")]
        public  async Task<ActionResult<IEnumerable<DocumentHeader>>> Get() 
        {
        
            //throw new NotImplementedException();
            var documentheader  = await context.DocumentHeaders.ToListAsync();
            //logger.LogInformation("Document List requested");
            //logManager.CreateLog("Information","Document List requested");
            
            return  documentheader;
            
        } 

          // GET api/DocumentHeader/expenseheaderbyid/1
        [HttpGet("expenseheaderbyid/{id}",Name="OneExpenseHeader")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "admin,regularuser")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "user")]
        public async Task<ActionResult<DocumentHeader>> GetbyId(int id) 
        {
            var documentheader  = await context.DocumentHeaders.FirstOrDefaultAsync(x => x.IdDocument == id);

            if (documentheader == null)
            {
                return NotFound();
            }

            return  documentheader;
        }

         // POST api/DocumentHeader/CreateExpenseHeader
        [HttpPost("CreateExpenseHeader")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "admin,regularuser")]
        public async Task<ActionResult> Post([FromBody] DocumentHeader documentheader)
        {
            
            var iddocument = await context.DocumentHeaders.CountAsync();

            if (iddocument != 0) 
            { 
                iddocument = iddocument + 1;
            }

            var _documentheader = new DocumentHeader{
                IdDocument = iddocument,
                DocumentDescription = documentheader.DocumentDescription,
                IdFrenquency = documentheader.IdFrenquency,
                IdIncomeList = documentheader.IdIncomeList,
                IdUser = documentheader.IdUser,
                CreationDate = documentheader.CreationDate,
                ModificationDate = documentheader.ModificationDate
            };
            context.Add(_documentheader);
            await context.SaveChangesAsync();
            logmanager.CreateLog("Informacion",$"{_documentheader.IdDocument} has been created");
            return new CreatedAtRouteResult("OneExpenseHeader", new { id = documentheader.IdDocument}, documentheader);
        
        }

         // PUT api/DocumentHeader/UpdateExpenseHeader/1
        [HttpPut("UpdateExpenseHeader/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,  Roles= "admin,regularuser")]
        public async Task<ActionResult> Put(int id, [FromBody] DocumentHeader documentheader)
        {
            
            if(id !=  documentheader.IdDocument)
            {
                return BadRequest();
            }

            context.Entry(documentheader).State = EntityState.Modified;
            await context.SaveChangesAsync();
            logmanager.CreateLog("Informacion",$"{documentheader.IdDocument} has been updated");

            return new CreatedAtRouteResult("OneExpenseHeader", new { id = documentheader.IdDocument}, documentheader);
        }

        // DELETE api/DocumentHeader/DeleteExpenseHeader/1
        [HttpDelete("DeleteExpenseHeader/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,  Roles= "admin,regularuser")]
        public async Task<ActionResult> Delete(int id)
        {
        
            var documentheader =  await context.DocumentHeaders.Select(x => x.IdDocument).FirstOrDefaultAsync(x => x == id);

            if (documentheader == default(int))
            {
                return NotFound();
            }
            context.Remove(new DocumentHeader {IdDocument= documentheader});
            await context.SaveChangesAsync();
            logmanager.CreateLog("Informacion",$"{documentheader} has been deleted");
            return NoContent();
        }
    }
}