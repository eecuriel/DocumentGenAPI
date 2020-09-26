using System;
using System.ComponentModel.DataAnnotations;

namespace DocumentGenAPI.Entities
{
    public class DocumentHeader
    {
        [Key]
        public string IdDocument { get; set; }
        public string IdUser { get; set; }

        [Required(ErrorMessage = "Description must be inserted")]
        [MinLength(4,ErrorMessage= "The field must contain {1} characters min")]
        [MaxLength(50,ErrorMessage= "The field must contain {1} characters max")]
        public string DocumentDescription { get; set; }
        public int DocumentType { get; set; }
        public string Logopic { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }

    }
}