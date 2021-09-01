namespace WebHost.Services
{
    using AutoMapper;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.ServiceModel.Web;
    using WebHost.Ioc;
    using WebHost.Repository.Repositories;
    using WebHost.Repository.RepositoryContracts;
    using WebHost.Security;

    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly Guid userId;

        public PaymentService()
        {
            this._paymentRepository = IocManager.RegisterType<PaymentRepository, IPaymentRepository>();
            this._mapper = IocManager.RegisterMapper();

            var tokenRecogniser = new TokenRecogniser<Data.Payment>(this._paymentRepository);
            this.userId = tokenRecogniser.DecodeToken();
        }

        public IEnumerable<DomainModels.Payment> GetAllPayments()
        {
            var allPayments = this._paymentRepository.GetAllPayments(this.userId);

            var mappedPayments = this._mapper.Map<IEnumerable<Data.Payment>, IEnumerable<DomainModels.Payment>>(allPayments);

            return mappedPayments;
        }

        public DomainModels.Payment Pay(DomainModels.Payment payment)
        {
            if (payment == null)
            {
                throw new WebFaultException<string>("Payment is not valid.", HttpStatusCode.BadRequest);
            }

            var paymentToBeSet = this._mapper.Map<DomainModels.Payment, Data.Payment>(payment);

            var newPayment = this._paymentRepository.Pay(paymentToBeSet);

            return this._mapper.Map<Data.Payment, DomainModels.Payment>(newPayment);
        }
    }
}
