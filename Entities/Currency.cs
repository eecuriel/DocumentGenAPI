using System.ComponentModel.DataAnnotations;

namespace MyExpManAPI.Entities
{
    public class Currency
    {
        [Key]
        public int IdCurrency { get; set; }
        [Required(ErrorMessage = "Description must be inserted")]
        [MinLength(3,ErrorMessage= "The field must contain {1} characters min")]
        [MaxLength(50,ErrorMessage= "The field must contain {1} characters max")]
        public string CurrencyDescription { get; set; }
    }
}