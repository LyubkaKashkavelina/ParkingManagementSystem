namespace WebHost.DomainModels.Responses
{
    using System;
    using System.Runtime.Serialization;

    public class GetAllBookingsByUserResponse : DateSerizalizer
    {
        private string dueAmount;

        [IgnoreDataMember]
        public string DueAmountString
        {
            get
            {
                return this.dueAmount;
            }
            set
            {
                this.dueAmount = value;
            }
        }

        [DataMember]
        public decimal DueAmount
        {
            get
            {
                var args = this.dueAmount.Split('.');
                var result = args[0] + ',' + args[1];
                return Convert.ToDecimal(result);
            }
            set
            {
                this.dueAmount = value.ToString();
            }
        }

        public Guid ParkingSpaceId { get; set; }

        public string ParkingSpaceNumber { get; set; }

        public Guid BookingId { get; set; }

        public int Days { get; set; }

        public decimal MonthlyFee { get; set; }
    }
}