using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JobRegistrationSubmisson.Context;
using JobRegistrationSubmisson.Models;
using JobRegistrationSubmisson.Services;
using JobRegistrationSubmisson.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Bcrypt = BCrypt.Net.BCrypt;

namespace JobRegistrationSubmisson.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly MyContext _context;
        public IConfiguration _configuration;
        AttrEmail attrEmail = new AttrEmail();
        RandomDigit randDig = new RandomDigit();
        SmtpClient client = new SmtpClient();

        public AccountsController(MyContext myContext, IConfiguration iconfiguration)
        {
            _context = myContext;
            _configuration = iconfiguration;
        }

        // GET api/values
        //[Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        //public async Task<List<User>> GetAll()
        public List<AccountVM> GetAll()
        {
            List<AccountVM> list = new List<AccountVM>();
            foreach (var item in _context.Accounts)
            {
                var rolee = _context.AccRoles.Where(ru => ru.Accounts.Id == item.Id).FirstOrDefault();
                var role = _context.Roles.Where(r => r.Id == rolee.RoleId).FirstOrDefault();
                AccountVM user = new AccountVM()
                {
                    Id = item.Id,
                    UserName = item.UserName,
                    Email = item.Email,
                    Password = item.PasswordHash,
                    Phone = item.PhoneNumber,
                    RoleName = role.Name
                };
                list.Add(user);
            }
            return list;
            //return await _context.Users.ToListAsync<User>();
        }

        [HttpGet("{id}")]
        public AccountVM GetID(string id)
        {

            //var rolee = _context.RoleUsers.Where(ru => ru.UserId == id).FirstOrDefault();
            var getId = _context.Accounts.Find(id);
            AccountVM user = new AccountVM()
            {
                Id = getId.Id,
                UserName = getId.UserName,
                Email = getId.Email,
                Password = getId.PasswordHash,
                Phone = getId.PhoneNumber,
                //RoleName = id.Name
            };
            return user;
        }

        [HttpPost]
        public IActionResult Create(AccountVM accountVM)
        {
            if (ModelState.IsValid)
            {
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(attrEmail.mail, attrEmail.pass);

                var code = randDig.GenerateRandom();
                var fill = "Hi " + accountVM.UserName + "\n\n"
                          + "Try this Code to Confirm: \n"
                          + code
                          + "\n\nThank You";

                MailMessage mm = new MailMessage("donotreply@domain.com", accountVM.Email, "Create Email", fill);
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                client.Send(mm);

                accountVM.RoleName = "Admin";
                var user = new Accounts();
                var accRoles = new AccRoles();
                var role = _context.Roles.Where(r => r.Name == accountVM.RoleName).FirstOrDefault();
                user.UserName = accountVM.UserName;
                user.Email = accountVM.Email;
                user.EmailConfirmed = false;
                user.PasswordHash = Bcrypt.HashPassword(accountVM.Password);
                user.PhoneNumber = accountVM.Phone;
                user.PhoneNumberConfirmed = false;
                user.TwoFactorEnabled = false;
                user.LockoutEnabled = false;
                user.AccessFailedCount = 0;
                user.SecurityStamp = code;
                accRoles.Roles = role;
                accRoles.Accounts = user;
       
                _context.AccRoles.AddAsync(accRoles);
                _context.Accounts.AddAsync(user);
                _context.SaveChanges();
                return Ok("Successfully Created");
            }
            return BadRequest("registration failed!");

        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, AccountVM accountVM)
        {
            if (ModelState.IsValid)
            {
                var getId = _context.Accounts.Find(id);
                var hasbcrypt = BCrypt.Net.BCrypt.HashPassword(accountVM.Password, 12);
                getId.Id = accountVM.Id;
                getId.UserName = accountVM.UserName;
                getId.Email = accountVM.Email;
                getId.PasswordHash = hasbcrypt;
                getId.PhoneNumber = accountVM.Phone;
                _context.SaveChanges();
                return Ok("Successfully Update");
            }
            return BadRequest("Update Failed!");

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (ModelState.IsValid)
            {
                var getUR = _context.AccRoles.Where(a => a.UserId == id).FirstOrDefault();
                var getId = _context.Accounts.Find(id);
                _context.AccRoles.Remove(getUR);
                _context.Accounts.Remove(getId);
                _context.SaveChanges();
                return Ok("Successfully Delete");
            }
            return BadRequest("Delete Failed!");

        }


        //[HttpPost]
        //[Route("Register")]
        //public IActionResult Register(UserVM userVM)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        this.Create(userVM);
        //        return Ok();
        //    }
        //    return BadRequest();
        //}

        [HttpPost]
        //[HttpGet("{}")]
        [Route("Login")]
        public IActionResult Login(AccountVM accountVM)
        {
            if (ModelState.IsValid)
            {
                var getUserRole = _context.AccRoles.Include("User").Include("Role").SingleOrDefault(x => x.Accounts.Email == accountVM.Email);
                if (getUserRole == null)
                {
                    return NotFound();
                }
                else if (accountVM.Password == null || accountVM.Password.Equals(""))
                {
                    return BadRequest(new { msg = "Password must filled" });
                }
                else if (!Bcrypt.Verify(accountVM.Password, getUserRole.Accounts.PasswordHash))
                {
                    return BadRequest(new { msg = "Password is Wrong" });
                }
                else
                {
                    //var user = new UserVM();
                    //user.Id = getUserRole.User.Id;
                    //user.UserName = getUserRole.User.UserName;
                    //user.Email = getUserRole.User.Email;
                    //user.Password = getUserRole.User.PasswordHash;
                    //user.Phone = getUserRole.User.PhoneNumber;
                    //user.RoleName = getUserRole.Role.Name;
                    if (getUserRole != null)
                    {
                        if (getUserRole.Accounts.SecurityStamp != null)
                        {
                            var claims = new List<Claim> {
                                new Claim("Id", getUserRole.Accounts.Id),
                                new Claim("UserName", getUserRole.Accounts.UserName),
                                new Claim("Email", getUserRole.Accounts.Email),
                                new Claim("RoleName", getUserRole.Roles.Name),
                                new Claim("VerifyCode", getUserRole.Accounts.SecurityStamp)
                            };
                            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);
                            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                        }
                        else
                        {
                            var claims = new List<Claim> {
                                new Claim("Id", getUserRole.Accounts.Id),
                                new Claim("UserName", getUserRole.Accounts.UserName),
                                new Claim("Email", getUserRole.Accounts.Email),
                                new Claim("RoleName", getUserRole.Roles.Name)
                            };
                            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);
                            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                        }
                    }
                    return BadRequest("Invalid credentials");
                }

            }
            return BadRequest("Login Failed");
        }

        [HttpPost]
        [Route("code")]
        public IActionResult VerifyCode(AccountVM accountVM)
        {
            if (ModelState.IsValid)
            {
                var getUserRole = _context.AccRoles.Include("User").Include("Role").SingleOrDefault(x => x.Accounts.Email == accountVM.Email);
                if (getUserRole == null)
                {
                    return NotFound();
                }
                else if (accountVM.VerifyCode != getUserRole.Accounts.SecurityStamp)
                {
                    return BadRequest(new { msg = "Your Code is Wrong" });
                }
                else
                {
                    //var user = new UserVM();
                    //user.Id = getUserRole.User.Id;
                    //user.Username = getUserRole.User.UserName;
                    //user.Email = getUserRole.User.Email;
                    //user.Password = getUserRole.User.PasswordHash;
                    //user.Phone = getUserRole.User.PhoneNumber;
                    //user.RoleName = getUserRole.Role.Name;
                    //return StatusCode(200, user);
                    return StatusCode(200, new
                    {
                        Id = getUserRole.Accounts.Id,
                        Username = getUserRole.Accounts.UserName,
                        Email = getUserRole.Accounts.Email,
                        RoleName = getUserRole.Roles.Name,
                        //Email = getUserRole.User.Email,
                        //Password = getUserRole.User.PasswordHash
                    });
                }
            }
            return BadRequest("Verify Code is Failed");
        }
    }
}