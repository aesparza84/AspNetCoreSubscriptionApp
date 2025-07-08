namespace Models
{
    public class Subscription
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required double Price { get; set; }   
        public required CategoryTypeEnum Category { get; set; }
        public required RenewTypeEnum RenewalType { get; set; }

        private DateTime renewalDate;
        public required DateTime RenewalDate 
        { 
            get {  return renewalDate; } 
            set
            {
                renewalDate = value.Date;

                switch (RenewalType)
                {
                    case RenewTypeEnum.Monthly: NextRenewalDate = renewalDate.AddMonths(1);
                        break;
                    case RenewTypeEnum.Weekly: NextRenewalDate = renewalDate.AddDays(7);
                        break;
                    case RenewTypeEnum.Annual: NextRenewalDate = renewalDate.AddYears(1);
                        break;
                }
            }
        }
        
        
        public DateTime NextRenewalDate {  get; private set; }
        public Subscription() { Id = Guid.NewGuid(); }
    }
}
