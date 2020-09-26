
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using DocumentGenAPI.Validators;

namespace DocumentGenAPI.Models
{
    public class UserAditionalDataDTO
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Name must be inserted")]
        [MinLength(4,ErrorMessage= "The field must contain {1} characters min")]
        [MaxLength(50,ErrorMessage= "The field must contain {1} characters max")]
        public string Name { get; set; }
        [Required(ErrorMessage = "LastName must be inserted")]
        [MinLength(4,ErrorMessage= "The field must contain {1} characters min")]
        [MaxLength(50,ErrorMessage= "The field must contain {1} characters max")]
        public string LastName { get; set; }
        [FilesSizeValidator(_MBMaxSize: 4)]
        [Filetypevaliator(filetype: FileGroup.Image)]
        public IFormFile Profilepic { get; set; }
    }
}