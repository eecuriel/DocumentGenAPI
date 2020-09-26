using System.Runtime.ConstrainedExecution;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace DocumentGenAPI.Entities
{
    public class DocumentDetail
    {
        [Key]
        public int IdItem { get; set; }
        public string IdDocument { get; set; }
        public string ItemDescription { get; set; }
        public int ItemQty { get; set; }

        [Column(TypeName= "money")]
        public int IdCurrency { get; set; }
        public decimal ItemAmount { get; set; }
    }
}