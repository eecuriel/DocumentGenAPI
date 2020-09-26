using System;
using System.Security.AccessControl;
using System.ComponentModel.DataAnnotations;

namespace DocumentGenAPI.Entities
{
    public class Customer
    {
        [Key]
        public int IdCustomer { get; set; }
        public string IdUserOwner {get; set;}
        
        [Required(ErrorMessage = "First Name must be inserted")]
        [MinLength(4,ErrorMessage= "The field must contain {1} characters min")]
        [MaxLength(100,ErrorMessage= "The field must contain {1} characters max")]
        public string CustomerFirstName { get; set; }
        [Required(ErrorMessage = "Last Name must be inserted")]
        [MinLength(4,ErrorMessage= "The field must contain {1} characters min")]
        [MaxLength(100,ErrorMessage= "The field must contain {1} characters max")]
        public string CustomerLastName { get; set; }
    
        [MinLength(4,ErrorMessage= "The field must contain {1} characters min")]
        [MaxLength(100,ErrorMessage= "The field must contain {1} characters max")]
        public string ComercialName { get; set; }

        [Required(ErrorMessage = "Email must be inserted")]
        [EmailAddress]
        public string CustomerEmail { get; set; }
        [Phone]
        public string CustomerPhone1 { get; set; }
        [Phone]
        public string CustomerPhone2 { get; set; }
        [MinLength(4,ErrorMessage= "The field must contain {1} characters min")]
        [MaxLength(100,ErrorMessage= "The field must contain {1} characters max")]
        public string  CustomerAddress { get; set; }

        [MaxLength(100,ErrorMessage= "The field must contain {1} characters max")]
        public string CustomerCountry { get; set; }
        
        [MaxLength(100,ErrorMessage= "The field must contain {1} characters max")]
        public string CustomerCity { get; set; }
        
        [MaxLength(100,ErrorMessage= "The field must contain {1} characters max")]
        public string CustomerState  { get; set; }
        
        [MaxLength(10,ErrorMessage= "The field must contain {1} characters max")]
        public int CustomerPostalCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime  CustomerCreationDate { get; set; }


    }
}