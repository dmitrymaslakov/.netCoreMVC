using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewsGatheringService.Core.Abstract;
using NewsGatheringService.Data.ContextDb;
using NewsGatheringService.Data.Entities;
using NewsGatheringService.Domain.Models;
using NewsGatheringServiceMVC.Models;

namespace NewsGatheringServiceMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;
        const string errorMessage = "Username or password is incorrect";

        public AccountController(IUnitOfWork unitOfWork, ILogger<AccountController> logger, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = _unitOfWork.UserRepository?
                        .FindBy(u => u.Login.Equals(model.Login), u => u.UserRoles)
                        .FirstOrDefault();

                    if (user == null)
                    {
                        /*user = new User
                        {
                            Id = Guid.NewGuid(),
                            Login = model.Login,
                        };

                        var passwordToBytes = Encoding.UTF8.GetBytes(model.Password);
                        var enText = Convert.ToBase64String(passwordToBytes);

                        user.PasswordHash = enText;

                        Role userRole = _unitOfWork.RoleRepository.FindBy(r => r.Name == "user").FirstOrDefault();
                        if (userRole != null)
                            user.UserRoles =
                                user.UserRoles
                                .Append(new UserRole
                                { Id = Guid.NewGuid(), RoleId = userRole.Id, UserId = user.Id, Date = DateTime.Now }).ToList();

                        await _unitOfWork.UserRepository.AddAsync(user);

                        await _unitOfWork.SaveChangesAsync();*/
                        await _userService.RegisterUser(model);
                        await Authenticate(user);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                        ModelState.AddModelError("", errorMessage);
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }

        }

        [HttpGet]
        public async Task<IActionResult> Welcome()
        {
            try
            {
                if (_unitOfWork.RoleRepository == null || _unitOfWork.RoleRepository.FindBy(r => r.Name.Equals("admin")).Count() == 0)
                    await InitializeAdminUserAsync();

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        [HttpGet]
        public IActionResult Login()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = _unitOfWork.UserRepository
                        .FindBy(u => u.Login.Equals(model.Login), u => u.UserRoles)
                        .FirstOrDefault();

                    if (user != null)
                    {
                        await Authenticate(user);

                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        private async Task InitializeAdminUserAsync()
        {
            const string adminRoleName = "admin";
            const string userRoleName = "user";

            string adminLogin = "adminLogin";
            string adminPassword = "123456";


            Role adminRole = new Role { Id = Guid.NewGuid(), Name = adminRoleName };
            Role userRole = new Role { Id = Guid.NewGuid(), Name = userRoleName };
            User adminUser = new User
            {
                Id = Guid.NewGuid(),
                Login = adminLogin,
                PasswordHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(adminPassword))
            };

            adminUser.UserRoles = adminUser.UserRoles
            .Append(new UserRole
            { Id = Guid.NewGuid(), RoleId = adminRole.Id, UserId = adminUser.Id, Date = DateTime.Now }).ToList();


            await _unitOfWork.RoleRepository.AddRangeAsync(new List<Role> { adminRole, userRole });

            await _unitOfWork.UserRepository.AddAsync(adminUser);

            await _unitOfWork.SaveChangesAsync();
        }
        private async Task Authenticate(User user)
        {
            var role = _unitOfWork.UserRoleRepository
                .FindBy(ur => ur.UserId.CompareTo(user.Id) == 0, ur => ur.Role)
                .FirstOrDefault()?
                .Role;

            var login = user.Login;
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Name)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));

        }
        private void SetCookieToken(string token)
        {
            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7),
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }


    }
}
