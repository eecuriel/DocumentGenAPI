using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyExpManAPI.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MyExpManAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MyExpManAPI.Entities;
using MyExpManAPI.Context;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using MyExpManAPI.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;

namespace MyExpManAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ActionFilters))]
    public class UserAccountController : CustomBaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogManager logmanager;
        private readonly ApplicationDbContext context;  
        private readonly ImageLocalStorage imagelocalstorage;
        private readonly IMapper mapper;
        private readonly IEmailSender _emailSender;
        private readonly string container = "userprofile";

        public UserAccountController( 
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            ILogManager _logmanager,
            ApplicationDbContext _context,
            ImageLocalStorage _imagelocalstorage,
            IEmailSender emailSender,
            IMapper _mapper
            ) :base(_context, _mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            logmanager = _logmanager;
            context = _context;
            imagelocalstorage =_imagelocalstorage;
            mapper =_mapper;
            _emailSender = emailSender;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo userInfo)
        {
            var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, isPersistent: false, lockoutOnFailure: false);
            var usuario = await _userManager.FindByEmailAsync(userInfo.Email);
            
            if (result.Succeeded )
            {
                if (usuario.EmailConfirmed == true ) {
                var roles = await _userManager.GetRolesAsync(usuario);
                logmanager.CreateLog("Informacion",$" User : {userInfo.Email} log in successfully!");
                return BuildToken(userInfo, roles,usuario.Id);
                }
                //var _token = await _userManager.GenerateUserTokenAsync(usuario, "MyCustomTokenProvider", "Test");
                return BadRequest("Email no confirmed");
            }
            else
            {
                //await _userManager.IsEmailConfirmedAsync(ApplicationUser usuario.Id);
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return BadRequest(ModelState);
            }
        }

        private UserToken BuildToken(UserInfo userInfo, IList<string> roles, string UserId)
        {

            var claims = new List<Claim>
            {
        new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
        //new Claim("miValor", "Lo que yo quiera"),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            foreach (var rol in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tiempo de expiración del token. En nuestro caso lo hacemos de una hora.
            var expiration = DateTime.UtcNow.AddMinutes(60);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds);
            
            var newtoken = new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                Id = UserId
            
            };

            return newtoken;
        }

        [HttpPost("CreateUser")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "admin,regularuser")]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] UserInfo userInfo)
        {
            var user = new ApplicationUser { UserName = userInfo.Email, Email = userInfo.Email };
        
            var result = await _userManager.CreateAsync(user, userInfo.Password);

            if (result.Succeeded)

            {
                    //Role assing
                var usuario = await _userManager.FindByIdAsync(user.Id);
                await _userManager.AddClaimAsync(usuario, new Claim(ClaimTypes.Role,"regularuser"));
                await _userManager.AddToRoleAsync(usuario, "regularuser");

                var token = BuildToken(userInfo, new List<string>(),user.Id);
                logmanager.CreateLog("Informacion",$" User : {userInfo.Email} has been created!");

               // return BuildToken(userInfo, new List<string>());
                var emailtoken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action(nameof(ConfirmEmail), "UserAccount", new { emailtoken, email = user.Email }, Request.Scheme);
                var message = new Message(new string[] { user.Email }, "Confirmation email link", confirmationLink);
                //_emailSender.SendEmail(message);
                return Ok(confirmationLink);

                //return RedirectToAction(nameof(SuccessRegistration));
            }
            else
            {
                return BadRequest(result.Errors);
            }

        }

        [HttpGet("{id}", Name = "GetProfile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "admin,regularuser")]
        public async Task<ActionResult<UserAditionalData>> Get(string Id)
        {
            var userGral = await context.UserGenerals.FirstOrDefaultAsync(x => x.Id == Id);

            if (userGral == null)
            {
                return NotFound();
            }

            return userGral;
            
        }

        [HttpPost("CreateProfileData")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "admin,regularuser")]
        public async Task<ActionResult> AddProfileData([FromForm] UserAditionalDTO usergeneral) {

            var entity = mapper.Map<UserAditionalData>(usergeneral);

            if (usergeneral.ProfilePic != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await usergeneral.ProfilePic.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extention = Path.GetExtension(usergeneral.ProfilePic.FileName);
                    entity.Profilepic = await imagelocalstorage.SaveFile(content, extention, container,
                    usergeneral.ProfilePic.ContentType);
                }
            }

            context.Add(entity);
            await context.SaveChangesAsync();
            var dto = mapper.Map<UserAditionalData>(entity);
            return new CreatedAtRouteResult("GetProfile", new { id = entity.Id }, dto);
        }


        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "admin,regularuser")]
        public async Task<ActionResult> Put(string id, [FromForm] UserAditionalDTO usergeneral)
        {
            var useradiontaldatadb = await context.UserGenerals.FirstOrDefaultAsync(x => x.Id == id);

            if (useradiontaldatadb == null) { return NotFound(); }

            useradiontaldatadb = mapper.Map(usergeneral, useradiontaldatadb);

            if (usergeneral.ProfilePic != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await usergeneral.ProfilePic.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extention = Path.GetExtension(usergeneral.ProfilePic.FileName);
                    useradiontaldatadb.Profilepic = await imagelocalstorage.UpdateFile(content, extention, container,
                        useradiontaldatadb.Profilepic,
                        usergeneral.ProfilePic.ContentType);
                }
            }

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "admin,regularuser")]
        public async Task<ActionResult> Patch(string Id, [FromBody] JsonPatchDocument<UserAdionalPatchDTO> patchDocument)
            {

                if (patchDocument == null)
            {
                return BadRequest();
            }

            var userGralLaDB = await context.UserGenerals.FirstOrDefaultAsync(x => x.Id == Id);

            if (userGralLaDB == null)
            {
                return NotFound();
            }

            var userDataDTO = mapper.Map<UserAditionalDTO>(userGralLaDB);

            patchDocument.ApplyTo(userDataDTO, ModelState);

            var isValid = TryValidateModel(userGralLaDB);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(userDataDTO, userGralLaDB);

            await context.SaveChangesAsync();

            return NoContent();

            //return await Patch<UserAditionalDTO, UserAdionalPatchDTO>(Id, patchDocument);
            }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles= "admin,regularuser")]
        public async Task<ActionResult<UserAditionalData>> Delete(string Id)
            {

            //var userDataId = await context.UserGenerals.Select(x => x.Id).FirstOrDefaultAsync(x => x == Id);
            var userDataId = await context.UserGenerals.FirstOrDefaultAsync(x => x.Id == Id);
            var _user = await _userManager.FindByIdAsync(Id);

            var result = await _userManager.DeleteAsync(_user);

            if (userDataId !=null){
            context.Remove(userDataId);
            await context.SaveChangesAsync();
            }
            return Ok("User was deleted!");
            }


        [HttpPost("ForgotPassword")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<ForgotPasswordModel>> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(forgotPasswordModel);
            }
        
            var user = await _userManager.FindByEmailAsync(forgotPasswordModel.Email);
            if (user == null)
            {
                return BadRequest("User Not Found");
            }
            
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callback = Url.Action(nameof(ResetPassword), "UserAccount", new { token, email = user.Email }, Request.Scheme);

            var message = new Message(new string[] { "katiroth13@gmail.com" }, "Reset password token", callback);

            var userResponse = new UserConfirmation {
                    token = token,
                    Email= user.Email
            };
            //await _emailSender.SendEmailAsync(message);

            return Ok(userResponse);
        }

        [HttpPost("ResetPassword")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<ResetPasswordModel>> ResetPassword([FromBody] ResetPasswordModel resetPasswordModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(resetPasswordModel);
            }
            //resetPasswordModel.Token = token;
            //resetPasswordModel.Email = email;
            var user = await _userManager.FindByEmailAsync(resetPasswordModel.Email);
            if (user == null)
            {
            return BadRequest("Invalid User!");
            }
            var resetPassResult = await _userManager.ResetPasswordAsync(user,resetPasswordModel.Token,resetPasswordModel.Password);
            if(!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
                return Ok("Password was changed!");
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string emailtoken, string email)
            {
                    var user = await _userManager.FindByEmailAsync(email);
                    
                    if (user == null)
                    return BadRequest("Error");
                    var result = await _userManager.ConfirmEmailAsync(user, emailtoken);
                    return Ok(result.Succeeded ? nameof(ConfirmEmail) : "Error");
            }

        [HttpGet("SuccessRegistration")]
        public IActionResult SuccessRegistration()
        {
            return Ok("Registration Successful");
        }



    }

    }
