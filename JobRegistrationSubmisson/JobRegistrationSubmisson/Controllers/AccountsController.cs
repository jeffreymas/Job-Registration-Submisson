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


        public AccountsController(MyContext myContext, IConfiguration config)
        {
            _context = myContext;
            _configuration = config;
        }

        [Authorize]
        // GET api/values
        [HttpGet]
        public async Task<List<AccountVM>> GetAll()
        {
            List<AccountVM> list = new List<AccountVM>();
            var getData = await _context.AccRoles.Include("Account").Include("Roles").ToListAsync();
            if (getData.Count == 0)
            {
                return null;
            }
            foreach (var item in getData)
            {
                var user = new AccountVM()
                {
                    Id = item.Accounts.Id,
                    UserName = item.Accounts.UserName,
                    Email = item.Accounts.Email,
                    Password = item.Accounts.PasswordHash,
                    Phone = item.Accounts.PhoneNumber,
                    RoleName = item.Roles.Name,
                    VerifyCode = item.Accounts.SecurityStamp,
                };
                list.Add(user);
            }
            return list;
        }

        [Authorize]
        [HttpGet("{id}")]
        public AccountVM GetID(string id)
        {
            var getData = _context.AccRoles.Include("Accounts").Include("Role").SingleOrDefault(x => x.UserId == id);
            if (getData == null || getData.Roles == null || getData.Accounts == null)
            {
                return null;
            }
            var user = new AccountVM()
            {
                Id = getData.Accounts.Id,
                UserName = getData.Accounts.UserName,
                Email = getData.Accounts.Email,
                Password = getData.Accounts.PasswordHash,
                Phone = getData.Accounts.PhoneNumber,
                RoleID = getData.Roles.Id,
                RoleName = getData.Roles.Name
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
                          + "Please verifty Code for this Apps : \n"
                          + code
                          + "\n\nThank You";

                MailMessage mm = new MailMessage("donotreply@domain.com", accountVM.Email, "Create Email", fill);
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                client.Send(mm);

                var user = new Accounts
                {
                    UserName = accountVM.UserName,
                    Email = accountVM.Email,
                    SecurityStamp = code,
                    PasswordHash = Bcrypt.HashPassword(accountVM.Password),
                    PhoneNumber = accountVM.Phone,
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0
                };
                _context.Accounts.Add(user);
                var accRole = new AccRoles
                {
                    UserId = user.Id,
                    RoleId = "2"
                };
                _context.AccRoles.Add(accRole);
                _context.SaveChanges();
                return Ok("Successfully Created");
            }
            return BadRequest("Not Successfully");
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, AccountVM accountVM)
        {
            if (ModelState.IsValid)
            {
                var getData = _context.AccRoles.Include("Role").Include("User").SingleOrDefault(x => x.UserId == id);
                //var getId = _context.Users.SingleOrDefault(x => x.Id == id);
                getData.Accounts.UserName = accountVM.UserName;
                getData.Accounts.Email = accountVM.Email;
                getData.Accounts.PhoneNumber = accountVM.Phone;
                if (!Bcrypt.Verify(accountVM.Password, getData.Accounts.PasswordHash))
                {
                    getData.Accounts.PasswordHash = Bcrypt.HashPassword(accountVM.Password);
                }
                getData.RoleId = accountVM.RoleID;

                _context.AccRoles.Update(getData);
                _context.SaveChanges();
                return Ok("Successfully Updated");
            }
            return BadRequest("Not Successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var getId = _context.Accounts.Find(id);
            _context.Accounts.Remove(getId);
            _context.SaveChanges();
            return Ok(new { msg = "Successfully Delete" });
        }


        [HttpPost]
        [Route("Register")]
        public IActionResult Register(AccountVM accountVM)
        {
            if (ModelState.IsValid)
            {
                return Create(accountVM);
            }
            return BadRequest("Data Not Valid");
        }

        [HttpPost]
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
                    return BadRequest("Password must filled");
                }
                else if (!Bcrypt.Verify(accountVM.Password, getUserRole.Accounts.PasswordHash))
                {
                    return BadRequest("Password is Wrong");
                }
                else
                {
                    if (getUserRole != null)
                    {
                        var user = new AccountVM()
                        {
                            Id = getUserRole.Accounts.Id,
                            UserName = getUserRole.Accounts.UserName,
                            Email = getUserRole.Accounts.Email,
                            Password = getUserRole.Accounts.PasswordHash,
                            Phone = getUserRole.Accounts.PhoneNumber,
                            RoleID = getUserRole.Roles.Id,
                            RoleName = getUserRole.Roles.Name,
                            VerifyCode = getUserRole.Accounts.SecurityStamp,
                        };
                        return Ok(GetJWT(user));
                    }
                    return BadRequest("Invalid credentials");
                }
            }
            return BadRequest("Data Not Valid");
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
                    return BadRequest("Your Code is Wrong");
                }
                else
                {
                    getUserRole.Accounts.SecurityStamp = null;
                    _context.SaveChanges();
                    var user = new AccountVM()
                    {
                        Id = getUserRole.Accounts.Id,
                        UserName = getUserRole.Accounts.UserName,
                        Email = getUserRole.Accounts.Email,
                        Password = getUserRole.Accounts.PasswordHash,
                        Phone = getUserRole.Accounts.PhoneNumber,
                        RoleID = getUserRole.Roles.Id,
                        RoleName = getUserRole.Roles.Name,
                        VerifyCode = getUserRole.Accounts.SecurityStamp,
                    };
                    return StatusCode(200, GetJWT(user));
                }
            }
            return BadRequest("Data Not Valid");
        }

        private string GetJWT(AccountVM accountVM)
        {
            var claims = new List<Claim> {
                            new Claim("Id", accountVM.Id),
                            new Claim("UserName", accountVM.UserName),
                            new Claim("Email", accountVM.Email),
                            new Claim("RoleName", accountVM.RoleName),
                            new Claim("VerifyCode", accountVM.VerifyCode == null ? "" : accountVM.VerifyCode),
                        };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                            _configuration["Jwt:Issuer"],
                            _configuration["Jwt:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(1),
                            signingCredentials: signIn
                        );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}