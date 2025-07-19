using System.ComponentModel.DataAnnotations;

namespace UniBill.Validators
{
    public class ValidateBillDateAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var curr = DateTime.UtcNow.ToShortDateString();
            var date = (DateTime)value!;
            if (date.ToShortDateString() == DateTime.Now.ToShortDateString())
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Past or Future date not allowed. Only Current Date is Allowed.");
            }
        }
    }
}