namespace WebHost.Repository.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using WebHost.Data;
    using WebHost.Repository.RepositoryContracts;

    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        public PaymentRepository(ParkingManagementSystemEntities context)
            : base(context)
        { }

        private ParkingManagementSystemEntities ParkingManagementSystemContext
        {
            get { return Context as ParkingManagementSystemEntities; }
        }

        public IEnumerable<Payment> GetAllPayments(Guid userId)
            => this.ParkingManagementSystemContext.Payments
            .Where(payment => payment.UserId == userId);

        public Data.Payment Pay(Data.Payment payment)
        {
            DateTime currentDate = DateTime.Now;

            payment.PaymentId = Guid.NewGuid();
            payment.UserId = payment.UserId;
            payment.PaymentDate = currentDate;
            payment.PaidAmount = (decimal)payment.PaidAmount;     

            this.ParkingManagementSystemContext.Payments.Add(payment);
            this.ParkingManagementSystemContext.SaveChanges();

            return payment;
        }
    }
}
