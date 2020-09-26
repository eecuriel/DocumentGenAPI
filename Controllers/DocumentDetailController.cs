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

        // GET api/DocumentDetail/Documentdetail
        [HttpGet("Documentdetail")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin,regularuser")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "user")]
        public  async Task<ActionResult<IEnumerable<DocumentDetail>>> Get() 
        {
            var documentdetail  = await context.DocumentDetails.ToListAsync();

            return  documentdetail;
        } 

          // GET api/DocumentDetail/Documentdetailbyid/00001
        [HttpGet("Documentdetailbyid/{id}",Name="OneDocumentDetail")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "admin,regularuser")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "user")]
        public async Task<ActionResult<DocumentDetail>> GetbyId(string id) 
        {
            var documentdetail  = await context.DocumentDetails.FirstOrDefaultAsync(x => x.IdDocument == id);

            if (documentdetail == null)
            {
                return NotFound();
            }

            return  documentdetail;
        }

         // POST api/DocumentDetail/CreateDocumentDetail
        [HttpPost("CreateDocumentDetail")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "admin,regularuser")]
        public async Task<ActionResult> Post([FromBody] DocumentDetail documentdetail)
        {
            
            var _documentdetail = new DocumentDetail{
                
                IdItem = documentdetail.IdItem,
                IdDocument = documentdetail.IdDocument,
                ItemDescription = documentdetail.ItemDescription,
                ItemQty = documentdetail.ItemQty,
                IdCurrency = documentdetail.IdCurrency,
                ItemAmount = documentdetail.ItemAmount
            };
            context.Add(_documentdetail);
            await context.SaveChangesAsync();

            logmanager.CreateLog("Informacion",$"Item No.{_documentdetail.IdItem} has been created");

            return new CreatedAtRouteResult("OneDocumentDetail", new { id = documentdetail.IdItem}, documentdetail);
        }

         // PUT api/DocumentDetail/UpdateDocumentDetail/1
        [HttpPut("UpdateDocumentDetail/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,  Roles= "admin,regularuser")]
        public async Task<ActionResult> Put(int id, [FromBody] DocumentDetail documentdetail)
        {
            if(id !=  documentdetail.IdItem)
            {
                return BadRequest();
            }

            context.Entry(documentdetail).State = EntityState.Modified;
            await context.SaveChangesAsync();
            logmanager.CreateLog("Informacion",$"Item No. {documentdetail.IdItem} has been updated");

            return new CreatedAtRouteResult("OneDocumentDetail", new { id = documentdetail.IdItem}, documentdetail);
        }

        // DELETE api/DocumentDetail/DeleteDocumentDetail/1
        [HttpDelete("DeleteDocumentDetail/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,  Roles= "admin,regularuser")]
        public async Task<ActionResult> Delete(int id)
        {
        
            var documentdetail =  await context.DocumentDetails.Select(x => x.IdItem).FirstOrDefaultAsync(x => x == id);

            if (documentdetail == default(int))
            {
                return NotFound();
            }
            context.Remove(new DocumentDetail {IdItem= documentdetail});
            await context.SaveChangesAsync();
            logmanager.CreateLog("Informacion",$"Item No.{documentdetail} has been deleted");
            return NoContent();
        }
    }
}