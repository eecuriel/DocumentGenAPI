using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentGenAPI.Validators
{
    public class FilesSizeValidator: ValidationAttribute
    {
        private readonly int MBMaxSize;

        public FilesSizeValidator(int _MBMaxSize)
        {
            MBMaxSize = _MBMaxSize;
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

            if (formFile.Length > MBMaxSize * 1024 * 1024)
            {
                return new ValidationResult($"The file size must not be {MBMaxSize}mb");
            }

            return ValidationResult.Success;
        }
    }
}