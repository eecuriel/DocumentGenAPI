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
    public class DocumentExtraChargeController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly ILogManager logmanager;
        public DocumentExtraChargeController(ApplicationDbContext _context, IMapper _mapper,ILogManager _logmanager)
        {
            context =_context;
            mapper = _mapper;
            logmanager =_logmanager;
        }

        // GET api/DocumentIncome/Chargelist
        [HttpGet("Chargelist")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin,regularuser")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "user")]
        public  async Task<ActionResult<IEnumerable<DocumentExtraCharge>>> Get() 
        {
            var documentincome  = await context.DocumentExtraCharges.ToListAsync();

            return  documentincome;
        } 

          // GET api/DocumentIncome/documentextrachargebyid/1
        [HttpGet("documentextrachargebyid/{id}",Name="OneExtraCharge")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "admin,regularuser")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "user")]
        public async Task<ActionResult<DocumentExtraCharge>> GetbyId(int id) 
        {
            var documentincome  = await context.DocumentExtraCharges.FirstOrDefaultAsync(x => x.IdCharge == id);

            if (documentincome == null)
            {
                return NotFound();
            }

            return  documentincome;
        }

         // POST api/DocumentIncome/CreateDocumentExtra
        [HttpPost("CreateDocumentExtra")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "admin,regularuser")]
        public async Task<ActionResult> Post([FromBody] DocumentExtraCharge documentextra)
        {
            
            var _documentextra = new DocumentExtraCharge{
                
                IdCharge = documentextra.IdCharge,
                IdDocument = documentextra.IdDocument,
                IdCurrency = documentextra.IdCurrency,
                ChargeDescription = documentextra.ChargeDescription,
                ChargeAmount = documentextra.ChargeAmount

            };
            context.Add(_documentextra);
            await context.SaveChangesAsync();
            logmanager.CreateLog("Informacion",$"Income List  No.{_documentextra.IdCharge} has been created");

            return new CreatedAtRouteResult("OneExtraCharge", new { id = documentextra.IdCharge}, documentextra);
        }

         // PUT api/DocumentIncome/UpdateExtraCharge/1
        [HttpPut("UpdateExtraCharge/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,  Roles= "admin,regularuser")]
        public async Task<ActionResult> Put(int id, [FromBody] DocumentExtraCharge documentextra)
        {
            if(id !=  documentextra.IdCharge)
            {
                return BadRequest();
            }

            context.Entry(documentextra).State = EntityState.Modified;
            await context.SaveChangesAsync();

            logmanager.CreateLog("Informacion",$"Income List  No.{documentextra.IdCharge} has been updated");

            return new CreatedAtRouteResult("OneExtraCharge", new { id = documentextra.IdCharge}, documentextra);
        }

        // DELETE api/DocumentIncome/DeleteExtraCharge/1
        [HttpDelete("DeleteExtraCharge/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,  Roles= "admin,regularuser")]
        public async Task<ActionResult> Delete(int id)
        {
        
            var documentextra =  await context.DocumentExtraCharges.Select(x => x.IdCharge).FirstOrDefaultAsync(x => x == id);

            if (documentextra == default(int))
            {
                return NotFound();
            }
            context.Remove(new DocumentExtraCharge {IdCharge= documentextra});
            await context.SaveChangesAsync();

            logmanager.CreateLog("Informacion",$"Income List  No.{documentextra} has been deleted");
            
            return NoContent();
        }
    }
}