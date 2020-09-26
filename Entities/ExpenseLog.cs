using System;
using System.ComponentModel.DataAnnotations;

namespace MyExpManAPI.Entities
{
    public class ExpenseLog
    {
        [Key]
        public int EventID { get; set; }
        public string EventArgument { get; set; }
        public string EventContext { get; set; }
        public DateTime  EventDateGeneration { get; set; }
    }
}