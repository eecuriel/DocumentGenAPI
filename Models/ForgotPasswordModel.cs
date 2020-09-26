using System.ComponentModel.DataAnnotations;

namespace MyExpManAPI.Models
{
    public class ForgotPasswordModel
    {
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    }
}