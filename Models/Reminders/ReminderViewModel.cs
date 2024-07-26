using System;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RingoMediaReminder.Models.Reminders
{
    public class ReminderViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime ReminderDateTime { get; set; }

        public List<string> Emails { get; set; } = new List<string>();
    }

    public class EmailListValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is List<string> emailList)
            {
                var emailAddressAttribute = new EmailAddressAttribute();
                foreach (var email in emailList)
                {
                    if (!emailAddressAttribute.IsValid(email))
                    {
                        return new ValidationResult("One or more email addresses are invalid.");
                    }
                }
                return ValidationResult.Success;
            }
            return new ValidationResult("Invalid email list.");
        }
    }

}