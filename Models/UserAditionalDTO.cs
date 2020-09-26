using Microsoft.AspNetCore.Http;
using MyExpManAPI.Entities;
using MyExpManAPI.Validators;

namespace MyExpManAPI.Models
{
    public class UserAditionalDTO : UserAdionalPatchDTO
    {
        [FilesSizeValidator(_MBMaxSize: 4)]
        [Filetypevaliator(filetype: FileGroup.Image)]
        public IFormFile ProfilePic { get; set; }
    }
}