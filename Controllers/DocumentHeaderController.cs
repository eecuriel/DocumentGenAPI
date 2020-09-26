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
using DocumentGenAPI.Context;
using DocumentGenAPI.Entities;
using DocumentGenAPI.Helpers;

namespace DocumentGenAPI.Controllers
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

        // GET api/DocumentHeader/Documentheaderlist
        [HttpGet("Documentheaderlist")]
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

          // GET api/DocumentHeader/dcoumentheaderbyid/00001
        [HttpGet("dcoumentheaderbyid/{id}",Name="OneDocumentHeader")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "admin,regularuser")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "user")]
        public async Task<ActionResult<DocumentHeader>> GetbyId(string id) 
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
            string _IdDocument = "";

            if (iddocument != 0) 
            { 
                iddocument = iddocument + 1;
                _IdDocument = string.Concat(Enumerable.Repeat("0", 5 - iddocument.ToString().Length));
            }

            _IdDocument = string.Concat(Enumerable.Repeat("0", 5 - iddocument.ToString().Length));

            var _documentheader = new DocumentHeader{
                IdDocument = _IdDocument,
                IdUser = documentheader.IdUser,
                DocumentDescription = documentheader.DocumentDescription,
                DocumentType = documentheader.DocumentType,
                Logopic = documentheader.Logopic,
                CreationDate = documentheader.CreationDate,
                ModificationDate = documentheader.ModificationDate
            };
            context.Add(_documentheader);
            await context.SaveChangesAsync();
            logmanager.CreateLog("Informacion",$"{_documentheader.IdDocument} has been created");
            return new CreatedAtRouteResult("OneDocumentHeader", new { id = documentheader.IdDocument}, documentheader);
        
        }

         // PUT api/DocumentHeader/UpdateExpenseHeader/1
        [HttpPut("UpdateDocumentHeader/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,  Roles= "admin,regularuser")]
        public async Task<ActionResult> Put(string id, [FromBody] DocumentHeader documentheader)
        {
            
            if(id !=  documentheader.IdDocument)
            {
                return BadRequest();
            }

            context.Entry(documentheader).State = EntityState.Modified;
            await context.SaveChangesAsync();
            logmanager.CreateLog("Informacion",$"{documentheader.IdDocument} has been updated");

            return new CreatedAtRouteResult("OneDocumentHeader", new { id = documentheader.IdDocument}, documentheader);
        }

        // DELETE api/DocumentHeader/DeleteDocumentHeader/00001
        [HttpDelete("DeleteDocumentHeader/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,  Roles= "admin,regularuser")]
        public async Task<ActionResult> Delete(string id)
        {
        
            var documentheader =  await context.DocumentHeaders.Select(x => x.IdDocument).FirstOrDefaultAsync(x => x == id);

            if (documentheader == default(string))
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