using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace MyExpManAPI.Entities
{
    public class DocumentDetail
    {
        [Key]
        public int IdTransaction { get; set; }
        public int IdDocument { get; set; }
        public int IdConcept { get; set; }

        [Column(TypeName= "money")]
        public decimal TransactionAmount { get; set; }
        public int IdCurrency { get; set; }
    }
}