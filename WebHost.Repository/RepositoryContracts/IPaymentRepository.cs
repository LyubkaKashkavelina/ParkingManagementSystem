namespace WebHost.Repository.RepositoryContracts
{
    using System;
    using System.Collections.Generic;
    using WebHost.Data;

    public interface IPaymentRepository : IRepository<Payment>
    {
        IEnumerable<Payment> GetAllPayments(Guid userId);
        Data.Payment Pay(Data.Payment payment);
    }
}
