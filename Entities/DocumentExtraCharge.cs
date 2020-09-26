using System.Runtime.InteropServices;
using System.Data.SqlTypes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentGenAPI.Entities
{
    public class DocumentExtraCharge
    {
        [Key]
        public int IdCharge { get; set; }
        public string IdDocument { get; set; }
        public int IdCurrency { get; set; }
        [Required(ErrorMessage = "Description must be inserted")]
        [MinLength(4,ErrorMessage= "The field must contain {1} characters min")]
        [MaxLength(50,ErrorMessage= "The field must contain {1} characters max")]
        public string ChargeDescription { get; set; }
    
        [Column(TypeName= "money")]
        public decimal ChargeAmount { get; set; }
    

    }
}