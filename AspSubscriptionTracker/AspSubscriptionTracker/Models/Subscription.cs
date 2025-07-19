using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AspSubscriptionTracker.Validations;
namespace Models
{
    public class Subscription
    {
        private const float maxPrice = 1000000;

        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} can not be null or empty.")]
        [Display(Name = "Subscription Name")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Name must be between 3-30 characters")]
        [RegularExpression("^[A-Za-z ]+$", ErrorMessage = "Name can only be letters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} can not be empty")]
        [Display(Name = "Subscription Price")]
        [Range(0, maxPrice, ErrorMessage = "Price must be between ${1} and ${2}")]
        public double Price { get; set; }

        [Required(ErrorMessage = "{0} can not be empty")]
        [Display(Name = "Sign Up Email")]
        [StringLength(maximumLength:30, ErrorMessage = "Name can not be longer than 30 characters")]
        [EmailAddress]
        public string Email {  get; set; }

        //[Required]
        public CategoryTypeEnum Category { get; set; }

        //[Required]
        public RenewTypeEnum RenewalType { get; set; }

        private DateTime purchaseDate;

        [Required]
        [Display(Name = "Purchase Date")]
        [RenewalTypeAndDateValidator(nameof(RenewalType))]
        public DateTime PurchaseDate
        { 
            get {  return purchaseDate; } 
            set
            {
                purchaseDate = value.Date;

                switch (RenewalType)
                {
                    case RenewTypeEnum.Monthly: NextRenewalDate = purchaseDate.AddMonths(1);
                        break;
                    case RenewTypeEnum.Weekly: NextRenewalDate = purchaseDate.AddDays(7);
                        break;
                    case RenewTypeEnum.Annual: NextRenewalDate = purchaseDate.AddYears(1);
                        break;
                }
            }
        }
       
        public DateTime NextRenewalDate {  get; set; }
        public override string ToString()
        {
            return $"Name: {Name}\n" +
                $"Price: {Price}\n" +
                $"Email: {Email}\n" +
                $"Catgory: {Category}\n" +
                $"Renew Type: {RenewalType}\n" +
                $"Renew Date: {PurchaseDate} \n";
        }

        public void SetNextRenewalDate()
        {
            switch (RenewalType)
            {
                case RenewTypeEnum.Monthly:
                    NextRenewalDate = purchaseDate.AddMonths(1);
                    break;
                case RenewTypeEnum.Weekly:
                    NextRenewalDate = purchaseDate.AddDays(7);
                    break;
                case RenewTypeEnum.Annual:
                    NextRenewalDate = purchaseDate.AddYears(1);
                    break;
            }
        }
    }
}
