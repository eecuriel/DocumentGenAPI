using Microsoft.AspNetCore.Http;
using DocumentGenAPI.Entities;
using DocumentGenAPI.Validators;

namespace DocumentGenAPI.Models
{
    public class UserAditionalDTO : UserAdionalPatchDTO
    {
        [FilesSizeValidator(_MBMaxSize: 4)]
        [Filetypevaliator(filetype: FileGroup.Image)]
        public IFormFile ProfilePic { get; set; }
    }
}