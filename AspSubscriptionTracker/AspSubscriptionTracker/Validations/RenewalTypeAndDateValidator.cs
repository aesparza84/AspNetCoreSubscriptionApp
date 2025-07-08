using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Models;

namespace AspSubscriptionTracker.Validations
{
    public class RenewalTypeAndDateValidator : ValidationAttribute
    {
        private string RenewPropertyName;
        public RenewalTypeAndDateValidator(string renewPropertyName)
        {
            this.RenewPropertyName = renewPropertyName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult($"{validationContext.DisplayName} can not be null");

            DateTime current = Convert.ToDateTime(value);

            if (current.Date > DateTime.Now)
            {
                return new ValidationResult($"{validationContext.DisplayName} can not be in the future");
            }

            //Reflection to read object meta-data
            //Grabs the property
            PropertyInfo? renewInfo = validationContext.ObjectType.GetProperty(RenewPropertyName);

                                //Reads the property's value
            RenewTypeEnum renewalType =  (RenewTypeEnum)renewInfo.GetValue(validationContext.ObjectInstance);


            switch (renewalType)
            {
                case RenewTypeEnum.Monthly:
                    if (current < (DateTime.Now.AddMonths(-1)))
                        return new ValidationResult($"{validationContext.DisplayName} can not be older than 1 month ago");

                    break;
                case RenewTypeEnum.Weekly:
                    if (current < (DateTime.Now.AddDays(-7)))
                        return new ValidationResult($"{validationContext.DisplayName} can not be older than 1 week ago");

                    break;
                case RenewTypeEnum.Annual:
                    if (current < (DateTime.Now.AddYears(-1)))
                        return new ValidationResult($"{validationContext.DisplayName} can not be older than 1 year ago");

                    break;
                default:
                    break;
            }

            return ValidationResult.Success;
        }
    }
}
