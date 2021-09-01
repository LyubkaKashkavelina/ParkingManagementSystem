using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ParkingAdministration.Data;
using ParkingAdministration.Models;

namespace ParkingAdministration.Controllers
{
    public class BaseController : Controller
    {
        internal readonly ParkingManagementSystemContext _context;

        public BaseController(ParkingManagementSystemContext context)
        {
            this._context = context;
        }

        public void Notify(string message, string title = "Sweet Alert Toastr Demo",
                                    NotificationType notificationType = NotificationType.success)
        {
            var msg = new
            {
                message = message,
                title = title,
                icon = notificationType.ToString(),
                type = notificationType.ToString(),
                provider = GetProvider()
            };

            TempData["Message"] = JsonConvert.SerializeObject(msg);
        }

        private string GetProvider()
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();

            var value = configuration["NotificationProvider"];

            return value;
        }

        internal void ValidateEditRequest(UserToParkingSpaceEntity userToParkingSpaceEntity, User user)
        {
            // Проверява дали има записи, различни от този със същото ид, които съдържат избраното парко място, 
            //които са още активни (End Date на записа е null)
            var containsParkingSpace = this._context.UserToParkingSpaces
                .Where(x => x.ParkingSpaceId == userToParkingSpaceEntity.UserToParkingSpace.ParkingSpaceId && x.EndDate == null &&
                x.UserToParkingSpaceId != userToParkingSpaceEntity.UserToParkingSpace.UserToParkingSpaceId).Any();

            // Проверява дали има записи, различни от този със същото ид, които съдържат избраният юзър, 
            //които са още активни (End Date на записа е null)
            var containsUser = this._context.UserToParkingSpaces
                .Where(x => x.UserId == user.UserId && x.EndDate == null &&
                x.UserToParkingSpaceId != userToParkingSpaceEntity.UserToParkingSpace.UserToParkingSpaceId).Any();

            if (containsParkingSpace)
            {
                throw new Exception("The parking space already has another owner");
            }

            if (containsUser)
            {
                throw new Exception("Employee already has a designated parking space.");
            }

            // Проверяваме дали има записи, различен от този със същото ид, 
            // в които участва избраното парко място и крайната дата не е null (т.е. записа е стар)
            // и дали крайната дата на записа е по-малка от началната дата на новия запис.
            var psValidateDates = this._context.UserToParkingSpaces
                .Where(x => x.ParkingSpaceId == userToParkingSpaceEntity.UserToParkingSpace.ParkingSpaceId && x.EndDate != null &&
                x.UserToParkingSpaceId != userToParkingSpaceEntity.UserToParkingSpace.UserToParkingSpaceId &&
                x.EndDate >= userToParkingSpaceEntity.UserToParkingSpace.StartDate)
                .Any();

            // Проверяваме дали има записи, различен от този със същото ид, 
            // в които участва избраното парко място и крайната дата не е null (т.е. записа е стар)
            // и дали крайната дата на записа е по-малка от началната дата на новия запис.
            var uValidateDates = this._context.UserToParkingSpaces
                .Where(x => x.UserId == userToParkingSpaceEntity.UserToParkingSpace.UserId && x.EndDate != null &&
                x.UserToParkingSpaceId != userToParkingSpaceEntity.UserToParkingSpace.UserToParkingSpaceId &&
                x.EndDate >= userToParkingSpaceEntity.UserToParkingSpace.StartDate)
                .Any();

            if (psValidateDates || uValidateDates)
            {
                throw new Exception("The parking space already has owner for the period.");
            }

            this.ValidateDates(userToParkingSpaceEntity.UserToParkingSpace);
        }

        internal void ValidateCreateRequest(UserToParkingSpaceEntity userToParkingSpaceEntity)
        {
            var containsParkingSpace = this._context.UserToParkingSpaces
                .Where(x => x.ParkingSpaceId == userToParkingSpaceEntity.UserToParkingSpace.ParkingSpaceId && x.EndDate == null)
                .Any();

            var containsUser = this._context.UserToParkingSpaces
                .Where(x => x.UserId == userToParkingSpaceEntity.UserToParkingSpace.UserId && x.EndDate == null)
                .Any();

            if (containsParkingSpace)
            {
                throw new Exception("The parking space already has another owner");
            }

            if (containsUser)
            {
                throw new Exception("Employee already has a designated parking space.");
            }

            var psValidateDates = this._context.UserToParkingSpaces
                .Where(x => x.ParkingSpaceId == userToParkingSpaceEntity.UserToParkingSpace.ParkingSpaceId && x.EndDate != null &&
                x.EndDate >= userToParkingSpaceEntity.UserToParkingSpace.StartDate)
                .Any();

            var uValidateDates = this._context.UserToParkingSpaces
                .Where(x => x.UserId == userToParkingSpaceEntity.UserToParkingSpace.UserId && x.EndDate != null &&
                x.EndDate >= userToParkingSpaceEntity.UserToParkingSpace.StartDate)
                .Any();

            if (psValidateDates || uValidateDates)
            {
                throw new Exception("The parking space already has owner for the period.");
            }

            this.ValidateDates(userToParkingSpaceEntity.UserToParkingSpace);
        }

        private void ValidateDates(UserToParkingSpace userToParkingSpace)
        {
            // Валидираме да не може да се променя крайната дата да е преди началната. (Общо и за create и edit)
            if (userToParkingSpace.EndDate <= userToParkingSpace.StartDate)
            {
                throw new Exception("End date can not be before the start date.");
            }
        }
    }
}
