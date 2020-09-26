using System.ComponentModel.DataAnnotations;

namespace DocumentGenAPI.Models
{
    public class ForgotPasswordModel
    {
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    }
}