using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace MyExpManAPI.Validators
{
    public class Filetypevaliator: ValidationAttribute
    {
        private readonly string[] validtypes;

        public Filetypevaliator(string[] _validtypes)
        {
            this.validtypes = _validtypes;
        }

        public Filetypevaliator(FileGroup filetype)
        {
            if (filetype == FileGroup.Image)
            {
                validtypes = new string[] { "image/jpeg", "image/png", "image/gif" };
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            IFormFile formFile = value as IFormFile;

            if (formFile == null)
            {
                return ValidationResult.Success;
            }

            if (!validtypes.Contains(formFile.ContentType))
            {
                return new ValidationResult($"File type must be the following: {string.Join(", ", validtypes)}");
            }

            return ValidationResult.Success;
        }
    }
}