using System;
using System.ComponentModel.DataAnnotations;

namespace MyExpManAPI.Entities
{
    public class DocumentHeader
    {
        [Key]
        public int IdDocument { get; set; }
        [Required(ErrorMessage = "Description must be inserted")]
        [MinLength(4,ErrorMessage= "The field must contain {1} characters min")]
        [MaxLength(50,ErrorMessage= "The field must contain {1} characters max")]
        public string DocumentDescription { get; set; }
        public string IdUser { get; set; }
        public int IdIncomeList { get; set; }
        public int IdFrenquency { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }

    }
}