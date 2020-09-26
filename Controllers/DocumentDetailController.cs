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
    public class DocumentDetailController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly ILogManager logmanager;
        public DocumentDetailController(ApplicationDbContext _context, IMapper _mapper,ILogManager _logmanager )
        {
            context =_context;
            mapper = _mapper;
            logmanager =_logmanager;

        }

        // GET api/DocumentDetail/expensedetail
        [HttpGet("expensedetail")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin,regularuser")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "user")]
        public  async Task<ActionResult<IEnumerable<DocumentDetail>>> Get() 
        {
            var documentdetail  = await context.DocumentDetails.ToListAsync();

            return  documentdetail;
        } 

          // GET api/DocumentDetail/expensedetailbyid/1
        [HttpGet("expensedetailbyid/{id}",Name="OneExpenseDetail")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "admin,regularuser")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "user")]
        public async Task<ActionResult<DocumentDetail>> GetbyId(int id) 
        {
            var documentdetail  = await context.DocumentDetails.FirstOrDefaultAsync(x => x.IdTransaction == id);

            if (documentdetail == null)
            {
                return NotFound();
            }

            return  documentdetail;
        }

         // POST api/DocumentDetail/CreateExpenseDetail
        [HttpPost("CreateExpenseDetail")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "admin,regularuser")]
        public async Task<ActionResult> Post([FromBody] DocumentDetail documentdetail)
        {
            
            var _documentdetail = new DocumentDetail{
                
                IdConcept = documentdetail.IdConcept,
                IdCurrency = documentdetail.IdCurrency,
                IdDocument = documentdetail.IdDocument,
                TransactionAmount = documentdetail.TransactionAmount
            };
            context.Add(_documentdetail);
            await context.SaveChangesAsync();

            logmanager.CreateLog("Informacion",$"Transaction No.{_documentdetail.IdTransaction} has been created");

            return new CreatedAtRouteResult("OneExpenseDetail", new { id = documentdetail.IdTransaction}, documentdetail);
        }

         // PUT api/DocumentDetail/UpdateExpenseDetail/1
        [HttpPut("UpdateExpenseDetail/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,  Roles= "admin,regularuser")]
        public async Task<ActionResult> Put(int id, [FromBody] DocumentDetail documentdetail)
        {
            if(id !=  documentdetail.IdTransaction)
            {
                return BadRequest();
            }

            context.Entry(documentdetail).State = EntityState.Modified;
            await context.SaveChangesAsync();
            logmanager.CreateLog("Informacion",$"Transactio No. {documentdetail.IdTransaction} has been updated");

            return new CreatedAtRouteResult("OneExpenseDetail", new { id = documentdetail.IdTransaction}, documentdetail);
        }

        // DELETE api/DocumentDetail/DeleteExpenseDetail/1
        [HttpDelete("DeleteExpenseDetail/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,  Roles= "admin,regularuser")]
        public async Task<ActionResult> Delete(int id)
        {
        
            var documentdetail =  await context.DocumentDetails.Select(x => x.IdTransaction).FirstOrDefaultAsync(x => x == id);

            if (documentdetail == default(int))
            {
                return NotFound();
            }
            context.Remove(new DocumentDetail {IdTransaction= documentdetail});
            await context.SaveChangesAsync();
            logmanager.CreateLog("Informacion",$"Transaction No.{documentdetail} has been deleted");
            return NoContent();
        }
    }
}