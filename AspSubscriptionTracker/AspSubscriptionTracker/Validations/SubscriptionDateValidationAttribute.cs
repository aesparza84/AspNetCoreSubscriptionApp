using System.ComponentModel.DataAnnotations;

namespace AspSubscriptionTracker.Validations
{
    public class SubscriptionDateValidationAttribute : ValidationAttribute
    {
        private int minimumYear = DateTime.Now.Year - 1; //
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult($"{validationContext.DisplayName} can not be null");

            DateTime current = (DateTime)value;

            if (current.Date > DateTime.Now)
            {
                return new ValidationResult($"{validationContext.DisplayName} can not be in the future");
            }

            if (current.Date.Year < minimumYear)
            {
                return new ValidationResult($"{validationContext.DisplayName} can not be before {minimumYear}");
            }


            return ValidationResult.Success;
        }
    }
}
