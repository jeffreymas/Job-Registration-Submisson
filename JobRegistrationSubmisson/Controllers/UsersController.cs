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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Bcrypt = BCrypt.Net.BCrypt;

namespace JobRegistrationSubmisson.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MyContext _context;

        AttrEmail attrEmail = new AttrEmail();
        RandomDigit randDig = new RandomDigit();
        SmtpClient client = new SmtpClient();
        public IConfiguration _configuration;

        public UsersController(MyContext myContext, IConfiguration config)
        {
            _context = myContext;
            _configuration = config;
        }

        //[Authorize]
        // GET api/values
        [HttpGet]
        public async Task<List<UserVM>> GetAll()
        {
            List<UserVM> list = new List<UserVM>();
            //var user = new UserVM();
            var getData = await _context.UserRole.Include("User").Include("Role").ToListAsync();
            if (getData.Count == 0)
            {
                return null;
            }
            foreach (var item in getData)
            {
                var user = new UserVM()
                {
                    Id = item.User.Id,
                    Username = item.User.UserName,
                    Email = item.User.Email,
                    Password = item.User.PasswordHash,
                    Phone = item.User.PhoneNumber,
                    RoleName = item.Role.Name,
                    //VerifyCode = item.User.SecurityStamp,
                };
                list.Add(user);
                //user.Id = item.User.Id;
                //user.Username = item.User.UserName;
                //user.Email = item.User.Email;
                //user.Password = item.User.PasswordHash;
                //user.Phone = item.User.PhoneNumber;
                //user.RoleName = item.Role.Name;
                //list.Add(user);
            }
            return list;
        }

        //[Authorize]
        [HttpGet("{id}")]
        public UserVM GetID(string id)
        {
            var getData = _context.UserRole.Include("User").Include("Role").SingleOrDefault(x => x.UserId == id);
            if (getData == null || getData.Role == null || getData.User == null)
            {
                return null;
            }
            var user = new UserVM()
            {
                Id = getData.User.Id,
                Username = getData.User.UserName,
                Email = getData.User.Email,
                Password = getData.User.PasswordHash,
                Phone = getData.User.PhoneNumber,
                RoleID = getData.Role.Id,
                RoleName = getData.Role.Name
            };
            return user;
        }

        [HttpPost]
        public IActionResult Create(UserVM userVM)
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
                var fill = "Hi " + userVM.Username + "\n\n"
                          + "Try this Code to Confirm: \n"
                          + code
                          + "\n\nThank You";

                MailMessage mm = new MailMessage("donotreply@domain.com", userVM.Email, "Create Email", fill);
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                client.Send(mm);

                userVM.RoleName = "JobSeeker";

                var JobS = new JobSeeker();
                var emp = new Employees();

                var role = _context.Roles.Where(r => r.Name == userVM.RoleName).FirstOrDefault();
                //var joblist = _context.Joblists.Where(Q => Q.Name == userVM.JoblistName).FirstOrDefault();

                var user = new User
                {
                    UserName = userVM.Username,
                    Email = userVM.Email,
                    EmailConfirmed = false,
                    PasswordHash = Bcrypt.HashPassword(userVM.Password),
                    PhoneNumber = userVM.Phone,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    SecurityStamp = code,
                };
                _context.Users.AddAsync(user);

                var roleuser = new UserRole
                {
                    UserId = user.Id,
                    RoleId = role.Id
                };
                _context.UserRole.AddAsync(roleuser);

                //roleuser.Role = role;
                //roleuser.User = user;
                if (userVM.RoleName == "HR")
                {
                    emp.EmpId = user.Id;
                    emp.CreateTime = DateTimeOffset.Now;
                    emp.IsDelete = false;
                    _context.Employees.AddAsync(emp);
                }
                else if (userVM.RoleName == "JobSeeker")
                {
                    JobS.JobSId = user.Id;
                    JobS.RegistDate = DateTimeOffset.Now;
                    JobS.Reject = false;
                    JobS.Approve = false;
                    //JobSList.JoblistId = userVM.Joblists;
                    _context.JobSeekers.AddAsync(JobS);

                }

                var JobSList = new JobSeekerList
                {
                    JobSeekerId = JobS.JobSId,
                    JoblistId = userVM.Joblists
                };
                //JobSList.Joblist = joblist;
                //JobSList.JobSeeker = JobS;
                _context.JobSeekerLists.AddAsync(JobSList);

                _context.SaveChanges();
                return Ok("Successfully Created");
            }
            return BadRequest("Not Successfully");
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, UserVM userVM)
        {
            if (ModelState.IsValid)
            {
                var getId = _context.Users.Find(id);
                var hasbcrypt = BCrypt.Net.BCrypt.HashPassword(userVM.Password, 12);
                getId.Id = userVM.Id;
                getId.UserName = userVM.Username;
                getId.Email = userVM.Email;
                getId.PasswordHash = hasbcrypt;
                getId.PhoneNumber = userVM.Phone;
                _context.SaveChanges();
                return Ok("Successfully Update");
            }
            return BadRequest("Not Successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var getId = _context.Users.Find(id);
            _context.Users.Remove(getId);
            _context.SaveChanges();
            return Ok(new { msg = "Successfully Delete" });
        }


        [HttpPost]
        [Route("Register")]
        public IActionResult Register(UserVM userVM)
        {
            if (ModelState.IsValid)
            {
                return Create(userVM);
            }
            return BadRequest("Data Not Valid");
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(UserVM userVM)
        {
            if (ModelState.IsValid)
            {
                var getUserRole = _context.UserRole.Include("User").Include("Role").SingleOrDefault(x => x.User.Email == userVM.Email);
                if (getUserRole == null)
                {
                    return NotFound();
                }
                else if (userVM.Password == null || userVM.Password.Equals(""))
                {
                    return BadRequest("Password must filled");
                }
                else if (!Bcrypt.Verify(userVM.Password, getUserRole.User.PasswordHash))
                {
                    return BadRequest("Password is Wrong");
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

                    //return StatusCode(200, new { 
                    //    Id = getUserRole.User.Id,
                    //    Username = getUserRole.User.UserName,
                    //    Email = getUserRole.User.Email,
                    //    RoleName = getUserRole.Role.Name,
                    //    VerifyCode = getUserRole.User.SecurityStamp,
                    //});
                    if (getUserRole != null)
                    {
                        var user = new UserVM()
                        {
                            Id = getUserRole.User.Id,
                            Username = getUserRole.User.UserName,
                            Email = getUserRole.User.Email,
                            Password = getUserRole.User.PasswordHash,
                            Phone = getUserRole.User.PhoneNumber,
                            RoleID = getUserRole.Role.Id,
                            RoleName = getUserRole.Role.Name,
                            VerifyCode = getUserRole.User.SecurityStamp,
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
        public IActionResult VerifyCode(UserVM userVM)
        {
            if (ModelState.IsValid)
            {
                var getUserRole = _context.UserRole.Include("User").Include("Role").SingleOrDefault(x => x.User.Email == userVM.Email);
                if (getUserRole == null)
                {
                    return NotFound();
                }
                else if (userVM.VerifyCode != getUserRole.User.SecurityStamp)
                {
                    return BadRequest("Your Code is Wrong");
                }
                else
                {
                    getUserRole.User.SecurityStamp = null;
                    _context.SaveChanges();
                    var user = new UserVM()
                    {
                        Id = getUserRole.User.Id,
                        Username = getUserRole.User.UserName,
                        Email = getUserRole.User.Email,
                        Password = getUserRole.User.PasswordHash,
                        Phone = getUserRole.User.PhoneNumber,
                        RoleID = getUserRole.Role.Id,
                        RoleName = getUserRole.Role.Name,
                        VerifyCode = getUserRole.User.SecurityStamp,
                    };
                    return StatusCode(200, GetJWT(user));
                }
            }
            return BadRequest("Data Not Valid");
        }

        private string GetJWT(UserVM userVM)
        {
            var claims = new List<Claim> {
                            new Claim("Id", userVM.Id),
                            new Claim("Username", userVM.Username),
                            new Claim("Email", userVM.Email),
                            new Claim("RoleName", userVM.RoleName),
                            new Claim("VerifyCode", userVM.VerifyCode == null ? "" : userVM.VerifyCode),
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