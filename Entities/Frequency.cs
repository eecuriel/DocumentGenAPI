using System.ComponentModel.DataAnnotations;

namespace MyExpManAPI.Entities
{
    public class Frequency
    {
        [Key]
        public int IdFrenquency { get; set; }
        [Required(ErrorMessage = "Description must be inserted")]
        [MinLength(4,ErrorMessage= "The field must contain {1} characters min")]
        [MaxLength(50,ErrorMessage= "The field must contain {1} characters max")]
        public string FrequencyDescription { get; set; }

    }
}