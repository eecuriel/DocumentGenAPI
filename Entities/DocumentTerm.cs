using System.ComponentModel.DataAnnotations;

namespace DocumentGenAPI.Entities
{
    public class DocumentTerm
    {
        [Key]
        public int IdTerm { get; set; }
        public string IdDocument { get; set; }
        public string TermDescription { get; set; }
    }
}