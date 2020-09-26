using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DocumentGenAPI.Entities
{
    public class UserAditionalData
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Name must be inserted")]
        [MaxLength(50,ErrorMessage= "The field must contain {1} characters max")]
        public string Name { get; set; }
        [Required(ErrorMessage = "LastName must be inserted")]
        [MaxLength(50,ErrorMessage= "The field must contain {1} characters max")]
        public string LastName { get; set; }
        public string Profilepic { get; set; }
    }
}