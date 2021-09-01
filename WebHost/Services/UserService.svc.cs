namespace WebHost.Services
{
    using ActiveDirectory.Access;
    using AutoMapper;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using WebHost.Ioc;
    using WebHost.Repository.Repositories;
    using WebHost.Repository.RepositoryContracts;
    using WebHost.Security;
    using WebHost.Utility;
    using System.DirectoryServices.AccountManagement;
    using System.Configuration;

    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService()
        {
            this._userRepository = IocManager.RegisterType<UserRepository, IUserRepository>();
            this._mapper = IocManager.RegisterMapper();
        }


        public DomainModels.User Register(DomainModels.AuthModel authModel)
        {
            try
            {
                if (authModel == null)
                {
                    throw new ArgumentNullException($"{authModel} can't be empty.");
                }

                this._userRepository.FindIfUserExists(authModel.Name);
                RegisterValidator.IsValidUsername(authModel.Name);
                RegisterValidator.IsValidPassword(authModel.Password);

                var newUser = this._mapper.Map<DomainModels.AuthModel, Data.User>(authModel);

                var newRegisteredUser = this._userRepository.RegisterUser(newUser);

                var user = this._userRepository.GetUserById(newRegisteredUser.UserId);

                var mappedUser = this._mapper.Map<Data.User, DomainModels.User>(user);

                WebOperationContext ctx = WebOperationContext.Current;
                ctx.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;

                return mappedUser;
            }
            catch (Exception ex)
            {
                throw new WebFaultException<string>($"{ex.Message}", HttpStatusCode.BadRequest);
            }
        }


        public string Login(DomainModels.AuthModel authModel)
        {
            try
            {
                var dataUser = this._mapper.Map<DomainModels.AuthModel, Data.User>(authModel);

                var user = this._userRepository.FindIfUserExists(dataUser.Name, dataUser.Password);

                var mappedUser = this._mapper.Map<Data.User, DomainModels.User>(user);

                var jwtGenerator = new JwtGenerator();
                string secureToken = jwtGenerator.GetJwt(mappedUser);

                var jwt = new Data.Token
                {
                    TokenId = Guid.NewGuid(),
                    UserToken = secureToken
                };

                this._userRepository.SaveJwtToken(jwt);

                return jwt.UserToken;
            }
            catch (Exception ex)
            {
                throw new WebFaultException<string>($"{ex.Message}", HttpStatusCode.NotFound);
            }
            //try
            //{
            //    if (!UserAuthentication.IsAuthenticatedUser(authModel.Name, authModel.Password))

            //    {
            //        throw new ArgumentException("User is not found");
            //    }
            //    else
            //    {
            //        var user = this._userRepository.FindIfUserExists(authModel.Name);

            //        if (user == null)
            //        {
            //            _userRepository.RegisterUser(new Data.User()
            //            {
            //                Name = authModel.Name
            //            });
            //            user = this._userRepository.FindIfUserExists(authModel.Name);
            //        }

            //        var mappedUser = this._mapper.Map<Data.User, DomainModels.User>(user);

            //        var jwtGenerator = new JwtGenerator();
            //        string secureToken = jwtGenerator.GetJwt(mappedUser);

            //        var jwt = new Data.Token
            //        {
            //            TokenId = Guid.NewGuid(),
            //            UserToken = secureToken
            //        };

            //        this._userRepository.SaveJwtToken(jwt);

            //        return jwt.UserToken;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new WebFaultException<string>($"{ex.Message}", HttpStatusCode.NotFound);
            //}
        }

        public DomainModels.CurrentUser GetCurrentUser()
        {
            var tokenRecogniser = new TokenRecogniser<Data.User>(this._userRepository);
            var userId = tokenRecogniser.DecodeToken();

            var user = this._userRepository.GetCurrentUser(userId, out string parkingSpaceNumber, out string bookedParkingSpaceForToday);
            var mappedUser = this._mapper.Map<Data.User, DomainModels.CurrentUser>(user);
            mappedUser.ParkingSpaceNumber = parkingSpaceNumber;
            mappedUser.BookedParkingSpaceForToday = bookedParkingSpaceForToday;
            return mappedUser;
        }

        public IEnumerable<DomainModels.User> GetAllUsers()
        {
            var users = this._userRepository.GetAllUsers();

            var mappedUsers = this._mapper.Map<IEnumerable<Data.User>, IEnumerable<DomainModels.User>>(users);

            return mappedUsers;
        }

        public DomainModels.Responses.GetUserInfoResponse GetCarInfo()
        {
            var tokenRecogniser = new TokenRecogniser<Data.User>(this._userRepository);
            var userId = tokenRecogniser.DecodeToken();

            var car = this._userRepository.GetCarInfo(userId);

            var mappedCar = this._mapper.Map<Data.Car, DomainModels.Responses.GetUserInfoResponse>(car);

            return mappedCar;
        }

        public DomainModels.Responses.SetUserInfoResponse SetCarInfo(DomainModels.Requests.SetUserInfoRequest carInfo)
        {
            var tokenRecogniser = new TokenRecogniser<Data.User>(this._userRepository);
            var userId = tokenRecogniser.DecodeToken();

            var carInfoToBeSet = this._mapper.Map<DomainModels.Requests.SetUserInfoRequest, Data.Car>(carInfo);
            carInfoToBeSet.UserId = userId;

            var car = this._userRepository.SetCarInfo(carInfoToBeSet);
            string phoneNumber = this._userRepository.SetPhoneNumber(carInfo.PhoneNumber, userId);

            var mappedCar = this._mapper.Map<Data.Car, DomainModels.Responses.SetUserInfoResponse>(car);
            mappedCar.PhoneNumber = phoneNumber;

            return mappedCar;
        }
    }
}
