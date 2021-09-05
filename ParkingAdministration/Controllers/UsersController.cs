using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ParkingAdministration.Data;
using ParkingAdministration.Models;
using ParkingAdministration.ViewModels;

namespace ParkingAdministration.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IConfiguration Configuration;
        public UsersController(ParkingManagementSystemContext context, IConfiguration configuration)
            : base(context)
        {

            Configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchString)
        {
            var userId = HttpContext.Request.Cookies["user_id"];

            if (userId != null || !String.IsNullOrEmpty(userId))
            {
                ViewData["FindEmployee"] = searchString;

                var query = from x in this._context.UserToParkingSpaces
                            .Include(u => u.ParkingSpace)
                            .Include(u => u.User)
                            select x;

                if (!String.IsNullOrEmpty(searchString))
                {
                    query = query.Where(x => x.User.Name.Contains(searchString) ||
                    x.User.PhoneNumber.Contains(searchString) ||
                    x.ParkingSpace.ParkingSpaceNumber.Contains(searchString));
                }

                var orderedQuery = query
                    .OrderBy(x => x.ParkingSpace.ParkingSpaceNumber)
                    .ThenBy(x => x.EndDate);

                return View(await orderedQuery.ToListAsync());
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public IActionResult Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userToParkingSpaceEntity = this.GetUserToParkingSpaceEntity(id);

            return View(userToParkingSpaceEntity);
        }

        public IActionResult Create()
        {
            ViewData["ParkingSpaceId"] = new SelectList(_context.ParkingSpaces.OrderBy(x => x.ParkingSpaceNumber), "ParkingSpaceId", "ParkingSpaceNumber");
            ViewData["UserId"] = getUsers(null);

            UserToParkingSpaceEntity userEntity = new UserToParkingSpaceEntity()
            {
                UserToParkingSpace = new UserToParkingSpace(),
                Vehicle = new Car()
            };

            return View(userEntity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserToParkingSpace,Vehicle,PhoneNumber")] UserToParkingSpaceEntity userToParkingSpaceEntity)
        {
            try
            {
                string userName = userToParkingSpaceEntity.UserToParkingSpace.User.Name;
                userToParkingSpaceEntity.UserToParkingSpace.User = null;

                ViewData["ParkingSpaceId"] = new SelectList(this._context.ParkingSpaces.OrderBy(x => x.ParkingSpaceNumber), "ParkingSpaceId", "ParkingSpaceNumber", userToParkingSpaceEntity.UserToParkingSpace.ParkingSpaceId);
                ViewData["UserId"] = getUsers(userName);

                this.ValidateCreateRequest(userToParkingSpaceEntity);
                if (ModelState.IsValid)
                {
                    var user = this._context.Users
                        .FirstOrDefault(u => u.Name.Equals(userName));

                    if (user == null)
                    {
                        user = createUser(userName);
                    }

                    var userToParkingSpace = this._context.UserToParkingSpaces
                        .FirstOrDefault(x => x.UserId == user.UserId && x.EndDate != null);

                    if (userToParkingSpace != null)
                    {
                        throw new ArgumentException("User already have assigned parking space.");
                    }

                    var car = this._context.Cars
                        .FirstOrDefault(x => x.UserId == user.UserId);

                    if (car == null &&
                        (userToParkingSpaceEntity.Vehicle.CarNumber == null ||
                        userToParkingSpaceEntity.PhoneNumber == null))
                    {
                        throw new ArgumentException("User doesn't have releted car. User should have entered the needed information about their car");
                        
                    }
                    //else if (car == null &&
                    //    userToParkingSpaceEntity.Vehicle.CarNumber != null ||
                    //    userToParkingSpaceEntity.PhoneNumber != null) 
                    //{
                    //    this._context.Cars.Add(new Car()
                    //    {
                    //        Make = userToParkingSpaceEntity.Vehicle.Make,
                    //        Model = userToParkingSpaceEntity.Vehicle.Model,
                    //        Color = userToParkingSpaceEntity.Vehicle.Color,
                    //        CarNumber = userToParkingSpaceEntity.Vehicle.CarNumber,
                    //        UserId = user.UserId,
                    //        CarId = Guid.NewGuid()
                    //    });
                    //    this._context.SaveChanges();
                    //}
                    //else
                    //{
                    //    car.Make = userToParkingSpaceEntity.Vehicle.Make;
                    //    car.Model = userToParkingSpaceEntity.Vehicle.Model;
                    //    car.Color = userToParkingSpaceEntity.Vehicle.Color;
                    //    if (userToParkingSpaceEntity.Vehicle.CarNumber != null)
                    //    {
                    //        car.CarNumber = userToParkingSpaceEntity.Vehicle.CarNumber;
                    //    }
                    //}

                    //if (userToParkingSpaceEntity.PhoneNumber != null)
                    //{
                    //    user.PhoneNumber = userToParkingSpaceEntity.PhoneNumber;
                    //}
                    userToParkingSpaceEntity.UserToParkingSpace.UserId = user.UserId;

                    userToParkingSpaceEntity.UserToParkingSpace.UserToParkingSpaceId = Guid.NewGuid();
                    this._context.UserToParkingSpaces.Add(userToParkingSpaceEntity.UserToParkingSpace);

                    await this._context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }

                return View(userToParkingSpaceEntity.UserToParkingSpace);
            }
            catch (Exception ex)
            {
                this.Notify(ex.Message, $"Failed to set parking space.", NotificationType.error);
                return View(userToParkingSpaceEntity);
            }
        }

        public IActionResult Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userToParkingSpaceEntity = this.GetUserToParkingSpaceEntity(id);
            ViewData["ParkingSpaceId"] = new SelectList(this._context.ParkingSpaces.OrderBy(x => x.ParkingSpaceNumber), "ParkingSpaceId", "ParkingSpaceNumber", userToParkingSpaceEntity.UserToParkingSpace.ParkingSpaceId);
            ViewData["UserId"] = getUsers(userToParkingSpaceEntity.UserToParkingSpace.User.Name);

            return View(userToParkingSpaceEntity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("PhoneNumber,UserToParkingSpace,Vehicle")] UserToParkingSpaceEntity userToParkingSpaceEntity)
        {
            try
            {
                string userName = userToParkingSpaceEntity.UserToParkingSpace.User.Name;
                userToParkingSpaceEntity.UserToParkingSpace.User = null;
                ViewData["ParkingSpaceId"] = new SelectList(_context.ParkingSpaces.OrderBy(x => x.ParkingSpaceNumber), "ParkingSpaceId", "ParkingSpaceNumber", userToParkingSpaceEntity.UserToParkingSpace.ParkingSpaceId);
                ViewData["UserId"] = getUsers(userName);

                var user = this._context.Users
                    .FirstOrDefault(u => u.Name.Equals(userName));

                this.ValidateEditRequest(userToParkingSpaceEntity, user);
                if (ModelState.IsValid)
                {
                    if (user == null)
                    {
                        user = createUser(userName);
                    }

                    user.PhoneNumber = userToParkingSpaceEntity.PhoneNumber;

                    userToParkingSpaceEntity.UserToParkingSpace.UserId = user.UserId;

                    this._context.Update(userToParkingSpaceEntity.UserToParkingSpace);


                    Car car = this._context.Cars
                        .FirstOrDefault(c => c.UserId == user.UserId);
                    if (car == null)
                    {
                        this._context.Cars.Add(new Car()
                        {
                            Make = userToParkingSpaceEntity.Vehicle.Make,
                            Model = userToParkingSpaceEntity.Vehicle.Model,
                            Color = userToParkingSpaceEntity.Vehicle.Color,
                            CarNumber = userToParkingSpaceEntity.Vehicle.CarNumber,
                            UserId = user.UserId,
                            CarId = Guid.NewGuid()
                        });
                        this._context.SaveChanges();
                    }
                    else
                    {
                        car.Make = userToParkingSpaceEntity.Vehicle.Make;
                        car.Model = userToParkingSpaceEntity.Vehicle.Model;
                        car.Color = userToParkingSpaceEntity.Vehicle.Color;
                        car.CarNumber = userToParkingSpaceEntity.Vehicle.CarNumber;
                    }

                    await this._context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                return View(userToParkingSpaceEntity);
            }
            catch (Exception ex)
            {
                if (!UserToParkingSpaceExists(userToParkingSpaceEntity.UserToParkingSpace.UserToParkingSpaceId))
                {
                    return NotFound();
                }

                this.Notify(ex.Message, $"Failed to set parking space.", NotificationType.error);
                return View(userToParkingSpaceEntity);
            }
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var userToParkingSpace = await _context.UserToParkingSpaces
            //    .Include(u => u.ParkingSpace)
            //    .Include(u => u.User)
            //    .FirstOrDefaultAsync(m => m.UserToParkingSpaceId == id);

            var userToParkingSpaceEntity = this.GetUserToParkingSpaceEntity(id);

            return View(userToParkingSpaceEntity);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var userToParkingSpace = await this._context.UserToParkingSpaces.FindAsync(id);
            var freeParkingSpaceId = _context.FreeParkingSpaces.Where(f => f.UserSpaceId == id);
            var ids = freeParkingSpaceId.Select(f => f.FreeParkingSpaceId).ToList();
            var freeParkingSpace =  _context.FreeParkingSpaces.Where(f => ids.Contains(f.FreeParkingSpaceId));
            var bookings = _context.Bookings.Where(b => ids.Contains(b.ParkingSpaceId));

            this._context.Bookings.RemoveRange(bookings);
            this._context.FreeParkingSpaces.RemoveRange(freeParkingSpace);
            this._context.UserToParkingSpaces.Remove(userToParkingSpace);
            await this._context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private UserToParkingSpaceEntity GetUserToParkingSpaceEntity(Guid id)
        {
            var userToParkingSpace = this._context.UserToParkingSpaces.Find(id);
            var user = this._context.Users
                .FirstOrDefault(u => u.UserId == userToParkingSpace.UserId);
            var vehicle = this._context.Cars
                .FirstOrDefault(c => c.UserId == userToParkingSpace.UserId);
            var parkingSpace = this._context.ParkingSpaces
                .FirstOrDefault(ps => ps.ParkingSpaceId == userToParkingSpace.ParkingSpaceId);

            userToParkingSpace.ParkingSpace = parkingSpace;

            UserToParkingSpaceEntity userToParkingSpaceEntity = new UserToParkingSpaceEntity()
            {
                UserToParkingSpace = userToParkingSpace,
                Vehicle = vehicle,
                PhoneNumber = user.PhoneNumber
            };

            return userToParkingSpaceEntity;
        }

        private bool UserToParkingSpaceExists(Guid id)
        {
            return this._context.UserToParkingSpaces.Any(e => e.UserToParkingSpaceId == id);
        }

        private bool VehicleExists(Guid id)
        {
            return this._context.Cars.Any(c => c.CarId == id);
        }

        private SelectList getUsers(string userName)
        {
            var users = this._context.Users.ToList();
            var useAD = Convert.ToBoolean(Configuration["UseActiveDirectory"]);
            if (useAD)
            {
                var adServer = Configuration["ActiveDirectoryServer"];
                var adUser = Configuration["ActiveDirectoryUserName"];
                var adPass = Configuration["ActiveDirectoryPass"];
                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, adServer, adUser, adPass))
                {
                    using (var searcher = new PrincipalSearcher(new UserPrincipal(pc) { Enabled = true }))
                    {
                        var usersFromAD = searcher.FindAll();
                        foreach (var item in usersFromAD)
                        {
                            if (!users.Any(u => u.Name.Equals(item.SamAccountName)))
                            {
                                users.Add(new Data.User()
                                {
                                    Name = item.SamAccountName,
                                    UserId = Guid.NewGuid(),
                                    CreationDate = null
                                });
                            }
                        }
                    }
                }

            }
            if (userName == null)
            {

                var result = new SelectList(users.OrderBy(x => x.Name), "Name", "Name");
                return result;
            }
            else
            {
                var result = new SelectList(users.OrderBy(x => x.Name), "Name", "Name", userName);
                return result;
            }
        }

        private Data.User createUser(string name)
        {
            this._context.Users.Add(new Data.User()
            {
                Name = name,
                IsAdmin = false,
                UserId = Guid.NewGuid(),
                CreationDate = DateTime.Today
            });
            this._context.SaveChanges();


            Data.User user = this._context.Users
                .FirstOrDefault(u => u.Name == name);

            return user;
        }
    }
}
