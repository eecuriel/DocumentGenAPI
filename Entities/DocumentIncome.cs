using System.Runtime.InteropServices;
using System.Data.SqlTypes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyExpManAPI.Entities
{
    public class DocumentIncome
    {
        
        [Key]
        public int IdIncomeList { get; set; }
        [Required(ErrorMessage = "Description must be inserted")]
        [MinLength(4,ErrorMessage= "The field must contain {1} characters min")]
        [MaxLength(50,ErrorMessage= "The field must contain {1} characters max")]
        public string IncomeDescription { get; set; }
        public int IdDocument { get; set; }
        [Column(TypeName= "money")]
        public decimal IncomeAmount { get; set; }
        public int IdCurrency { get; set; }

    }
}