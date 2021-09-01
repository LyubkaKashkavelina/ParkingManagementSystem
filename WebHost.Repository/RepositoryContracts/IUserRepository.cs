namespace WebHost.Repository.RepositoryContracts
{
    using System;
    using System.Collections.Generic;
    using WebHost.Data;

    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<User> GetAllUsers();

        IEnumerable<ParkingSpace> GetAllFreeParkingSpaces();

        void AddParkingSpaceForUser(Guid userId, Guid parkingSpaceid);

        User RegisterUser(User user);

        User GetUserById(Guid userId);

        Data.User FindIfUserExists(string name, string password);

        bool FindIfUserExists(string name);

        void SaveJwtToken(Data.Token token);

        Data.User GetCurrentUser(Guid userId, out string parkingSpaceNumber, out string bookedParkingSpaceForToday);

        Data.Car GetCarInfo(Guid userId);

        Data.Car SetCarInfo(Data.Car car);

        string SetPhoneNumber(string phoneNumber, Guid userId);
    }
}