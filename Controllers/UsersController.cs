using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DocumentGenAPI.Context;
using DocumentGenAPI.Helpers;
using DocumentGenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace DocumentGenAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(ActionFilters))]
    public class UsersController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogManager logmanager;

    
        public UsersController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogManager _logmanager)
        {
            this.context = context;
            this.userManager = userManager;
            logmanager = _logmanager;
        }
        [HttpPost]
        [Route("UserRoleAssign")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "admin")]
        public async Task<ActionResult> UserRoleAssign(UserRoleDTO userRoleDTO)
        {
            var usuario = await userManager.FindByIdAsync(userRoleDTO.UserId);
            await userManager.AddClaimAsync(usuario, new Claim(ClaimTypes.Role, userRoleDTO.RoleName));
            await userManager.AddToRoleAsync(usuario, userRoleDTO.RoleName);

            logmanager.CreateLog("Informacion",$" Role : {userRoleDTO.RoleName} was assigned to User : {userRoleDTO.UserId}");

            return Ok();
        }
        [HttpDelete]
        [Route("RemoveUserRole")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "admin")]
        public async Task<ActionResult> RemoveUserRole(UserRoleDTO userRoleDTO)
        {
            var usuario = await userManager.FindByIdAsync(userRoleDTO.UserId);
            await userManager.RemoveClaimAsync(usuario, new Claim(ClaimTypes.Role, userRoleDTO.RoleName));
            await userManager.RemoveFromRoleAsync(usuario, userRoleDTO.RoleName);

            logmanager.CreateLog("Informacion",$" Role : {userRoleDTO.RoleName} was removed to User : {userRoleDTO.UserId}");
        
            return Ok();
        }
    }
}