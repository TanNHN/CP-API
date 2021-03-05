using API_Training3.App.Databases;
using API_Training3.Modules.Accounts.Entities;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Training3.Modules.Accounts.Validate
{
    public class SignUpEmailValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var mongo = validationContext.GetService<MongoDBContext>();
            string email = (string)value;
            if (!string.IsNullOrEmpty(email))
            {
                if (mongo.MongoDatabase.GetCollection<Account>("Account").AsQueryable<Account>().Any(f => f.Email.Equals(email)))
                {
                    return new ValidationResult("Email is already existed");
                }
            }
            return ValidationResult.Success;
        }
    }
}
