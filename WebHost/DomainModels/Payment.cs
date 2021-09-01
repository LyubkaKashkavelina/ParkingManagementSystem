namespace WebHost.DomainModels
{
    using System;

    public class Payment
    {
        public Guid UserId { get; set; }
        public decimal PaidAmount { get; set; }
    }
}