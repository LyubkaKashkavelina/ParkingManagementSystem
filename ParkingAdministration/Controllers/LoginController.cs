using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;
using ActiveDirectory.Access;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ParkingAdministration.Data;
using ParkingAdministration.Models;
using ParkingAdministration.ViewModels;
using WebHost.Ioc;
using WebHost.Repository.Repositories;
using WebHost.Repository.RepositoryContracts;
using WebHost.Utility;

namespace ParkingAdministration.Controllers
{
    public class LoginController : BaseController
    {
        private readonly IConfiguration Configuration;
        //private readonly IUserRepository _userRepository;
        //private readonly IMapper _mapper;

        public LoginController(ParkingManagementSystemContext context, IConfiguration configuration)
            : base(context)
        {
            Configuration = configuration;
            //this._userRepository = IocManager.RegisterType<UserRepository, IUserRepository>();
            //this._mapper = IocManager.RegisterMapper();
        }

        public ActionResult Login()
        {
            return View("~/Views/Authentication/Login.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel loginModel)
        {
            //try
            //{
            //    if (loginModel == null)
            //    {
            //        throw new ArgumentNullException($"{loginModel} can't be empty.");
            //    }

            //    this._userRepository.FindIfUserExists(loginModel.Name);
            //    RegisterValidator.IsValidUsername(loginModel.Name);
            //    RegisterValidator.IsValidPassword(loginModel.Password);

            //    var newUser = this._mapper.Map<WebHost.DomainModels.AuthModel, Data.User>(loginModel);

            //    var newRegisteredUser = this._userRepository.RegisterUser(newUser);

            //    var user = this._userRepository.GetUserById(newRegisteredUser.UserId);

            //    var mappedUser = this._mapper.Map<Data.User, WebHost.DomainModels.User>(user);

            //    WebOperationContext ctx = WebOperationContext.Current;
            //    ctx.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;

            //    return mappedUser;
            //}
            //catch (Exception ex)
            //{
            //    throw new WebFaultException<string>($"{ex.Message}", HttpStatusCode.BadRequest);
            //}
            try
            {


                if (ModelState.IsValid)
                {
                    var useAD = Convert.ToBoolean(Configuration["UseActiveDirectory"]);
                    bool isValid = false;
                    if (useAD)
                    {
                        var adServer = Configuration["ActiveDirectoryServer"];
                        var adUser = Configuration["ActiveDirectoryUserName"];
                        var adPass = Configuration["ActiveDirectoryPass"];
                        using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, adServer, adUser, adPass))
                        {
                            // validate the credentials
                            isValid = pc.ValidateCredentials(loginModel.Name, loginModel.Password);
                        }
                    }
                    else
                    {
                        isValid = loginModel.Password == "1111" && !string.IsNullOrEmpty(loginModel.Name);
                    }
                    if (isValid)
                    {
                        var user = _context.Users
                            .FirstOrDefault(u => u.Name == loginModel.Name && u.IsAdmin);

                        if (user != null)
                        {
                            HttpContext.Response.Cookies.Append("user_id", user.UserId.ToString());
                            return View("../Home/Index");
                        }
                    }
                    if (!isValid)
                    {
                        throw new InvalidOperationException("Incorrect username or password. Please try again.");
                    }
                }
            }

            catch (Exception ex)
            {
                this.Notify(ex.Message, String.Empty, NotificationType.error);
            }

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("user_id");
            return RedirectToAction("Login", "Login");
        }
    }
}
