namespace WebHost.Repository.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using WebHost.Data;
    using WebHost.Repository.RepositoryContracts;

    public class UserRepository : Repository<User>, IUserRepository
    {

        public UserRepository(ParkingManagementSystemEntities context)
            : base(context)
        {

        }

        private ParkingManagementSystemEntities ParkingManagementSystemContext
        {
            get { return Context as ParkingManagementSystemEntities; }
        }

        public Data.User FindIfUserExists(string name, string password)
        {
            var user = this.ParkingManagementSystemContext.Users.FirstOrDefault(x => x.Name == name);

            if (user == null)
            {
                throw new ArgumentException("User is not found");
            }

            SHA256 sha256Hash = SHA256.Create();

            //user.Password = "94e0f9bc7f5a5225bd141bad5adf9befcc112aef09b88f47a14e20b75a7bbec2";
            if (!VerifyPasswordHash(sha256Hash, password, user.Password))
            {
                throw new ArgumentException("Password is not correct!");
            }

            return user;
        }

        public bool FindIfUserExists(string name)
        {
            foreach (var user in this.ParkingManagementSystemContext.Users)
            {
                if (user.Name == name)
                {
                    throw new ArgumentException("Username is already taken.");
                }
            }

            return false;
        }

        public IEnumerable<Data.User> GetAllUsers()
            => this.ParkingManagementSystemContext.Users.ToList();

        public void AddParkingSpaceForUser(Guid userId, Guid parkingSpaceid)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<ParkingSpace> GetAllFreeParkingSpaces()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// RegisterUser method is responsible to create the user and store it in the database.
        /// Set the userId to be a new Guid, hash the password, adds the user to the database and saves the changes and finally returns the registered user.
        /// </summary>
        /// <param name="user">The user that have to be registered.</param>
        /// <returns></returns>
        /// 
        
        public User RegisterUser(User user)
        {
            user.UserId = Guid.NewGuid();
            user.CreationDate = DateTime.Now;

            using (SHA256 sha256Hash = SHA256.Create())
            {
                string passwordHash = GetHash(sha256Hash, user.Password);
                user.Password = passwordHash;
            }

            this.ParkingManagementSystemContext.Users.Add(user);
            this.ParkingManagementSystemContext.SaveChanges();

            return user;
        }
        

        /// <summary>
        /// GetHash method is responsible to create a hash for a given string value by a specific hash algorithm.
        /// </summary>
        /// <param name="hashAlgorithm">Here we can specify by which algorithm we want our password to be hashed.</param>
        /// <param name="input">The value to be hashed.</param>
        /// <returns></returns>
        private string GetHash(HashAlgorithm hashAlgorithm, string input)
        {
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            var sb = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }

            return sb.ToString();
        }

        private bool VerifyPasswordHash(HashAlgorithm hashAlgorithm, string input, string hash)
        {
            var hashOfInput = GetHash(hashAlgorithm, input);

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return comparer.Compare(hashOfInput, hash) == 0;
        }

        public User GetUserById(Guid userId)
        {
            return this.ParkingManagementSystemContext.Users
                .FirstOrDefault(x => x.UserId == userId);
        }

        public void SaveJwtToken(Data.Token jwt)
        {
            if (jwt == null)
            {
                throw new ArgumentNullException();
            }

            var token = this.ParkingManagementSystemContext.Tokens.FirstOrDefault(t => t.UserToken == jwt.UserToken);

            if (token == null)
            {
                this.ParkingManagementSystemContext.Tokens.Add(jwt);
                this.ParkingManagementSystemContext.SaveChanges();
            }
        }

        public Data.User GetCurrentUser(Guid userId, out string parkingSpaceNumber, out string bookedParkingSpaceForToday)
        {
            parkingSpaceNumber = this.ParkingManagementSystemContext.GetUserParkingSpaceNumber(userId).FirstOrDefault();
            var bookedParkingSpaceForTodayObj = this.ParkingManagementSystemContext.GetAllBookingsByUser(userId)
                .FirstOrDefault(x => x.StartDate.Date <= DateTime.Now.Date && x.EndDate.Date >= DateTime.Now.Date);

            if(bookedParkingSpaceForTodayObj != null)
            {
                bookedParkingSpaceForToday = bookedParkingSpaceForTodayObj.ParkingSpaceNumber;
            }
            else
            {
                bookedParkingSpaceForToday = null;
            }

            return this.ParkingManagementSystemContext.Users.FirstOrDefault(u => u.UserId == userId);
        }

        public Data.Car GetCarInfo(Guid userId)
        {
            return this.ParkingManagementSystemContext.Cars.FirstOrDefault(c => c.UserId == userId); 
        }

        public Data.Car SetCarInfo(Data.Car car)
        {
            var carDatabaseRecord = this.ParkingManagementSystemContext.Cars.FirstOrDefault(c => c.UserId == car.UserId);

            if(carDatabaseRecord == null)
            {
                car.CarId = Guid.NewGuid();
                this.ParkingManagementSystemContext.Cars.Add(car);
            }
            else
            {
                carDatabaseRecord.CarNumber = car.CarNumber;
                carDatabaseRecord.Color = car.Color;
                carDatabaseRecord.Make = car.Make;
                carDatabaseRecord.Model = car.Model;
            }
            this.ParkingManagementSystemContext.SaveChanges();

            return car;
        }

        public string SetPhoneNumber(string phoneNumber, Guid userId)
        {
            var currentUserDatabaseRecord = this.ParkingManagementSystemContext.Users.FirstOrDefault(u => u.UserId == userId);

            currentUserDatabaseRecord.PhoneNumber = phoneNumber;
            this.ParkingManagementSystemContext.SaveChanges();

            return phoneNumber;
        }
    }
}
