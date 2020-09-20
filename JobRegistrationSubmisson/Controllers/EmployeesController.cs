using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using JobRegistrationSubmisson.Context;
using JobRegistrationSubmisson.Models;
using JobRegistrationSubmisson.Services;
using JobRegistrationSubmisson.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace JobRegistrationSubmisson.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly MyContext _context;
        AttrEmail attrEmail = new AttrEmail();
        SmtpClient client = new SmtpClient();
        public IConfiguration _configuration;

        public EmployeesController(MyContext myContext, IConfiguration config)
        {
            _context = myContext;
            _configuration = config;
        }

        ///[Authorize]
        // GET api/values
        [HttpGet]
        public async Task<List<EmployeeVM>> GetAll()
        {
            List<EmployeeVM> list = new List<EmployeeVM>();
            //var user = new UserVM();
            //var getData = await _context.Employees.Include("User").Where(Q => Q.isDelete == false).ToListAsync();
            var getData = await _context.Employees.Include("User").ToListAsync();
            if (getData.Count == 0)
            {
                return null;
            }
            foreach (var item in getData)
            {
                var user = new EmployeeVM()
                {
                    EmpId = item.User.Id,
                    Username = item.User.UserName,
                    Email = item.User.Email,
                    Address = item.Address,
                    //Password = item.User.PasswordHash,
                    Phone = item.User.PhoneNumber,
                    //CreatedData = item.CreatedData,
                    //UpdatedData = item.UpdatedData
                    //RoleName = item.Role.Name,
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
        public EmployeeVM GetID(string id)
        {
            var getData = _context.Employees.Include("User").SingleOrDefault(x => x.EmpId == id);
            if (getData == null || getData.User == null)
            {
                return null;
            }
            var user = new EmployeeVM()
            {
                EmpId = getData.User.Id,
                Username = getData.User.UserName,
                Email = getData.User.Email,
                Address = getData.Address,
                //Password = item.User.PasswordHash,
                Phone = getData.User.PhoneNumber,
                //CreatedData = getData.CreatedData,
                //UpdatedData = getData.UpdatedData
            };
            return user;
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, UserVM userVM)
        {
            if (ModelState.IsValid)
            {
                var getId = _context.Employees.Include("User").SingleOrDefault(x => x.EmpId == id);
                //var getId = _context.Users.Find(id);
                var hasbcrypt = BCrypt.Net.BCrypt.HashPassword(userVM.Password, 12);
                getId.User.Id = userVM.Id;
                getId.User.UserName = userVM.Username;
                getId.User.Email = userVM.Email;
                getId.User.PasswordHash = hasbcrypt;
                getId.User.PhoneNumber = userVM.Phone;
                getId.UpdateTime = DateTimeOffset.Now;
                _context.SaveChanges();
                return Ok("Successfully Update");
            }
            return BadRequest("Not Successfully");
        }



        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (ModelState.IsValid)
            {
                var getData = _context.Employees.Include("User").SingleOrDefault(x => x.EmpId == id);
                if (getData == null)
                {
                    return BadRequest("Not Succsessfully");
                }
                getData.DeleteTime = DateTimeOffset.Now;
                getData.IsDelete = true;

                _context.Entry(getData).State = EntityState.Modified;
                _context.SaveChangesAsync();
                return Ok("Successfully Delete");
            }
            return BadRequest("Not Success");
        }

        [HttpPost]
        [Route("Approve")]
        public IActionResult Approve(UserVM userVM)
        {
            if (ModelState.IsValid)
            {
                var jobsid = _context.Users.Include("JobSeeker").Where(r => r.Id == userVM.Id).FirstOrDefault();
                //var user = new User();
                if (jobsid.Id == userVM.Id)
                {
                    userVM.Email = jobsid.Email;
                    userVM.Username = jobsid.JobSeeker.Name;

                }
                //var email = user.Email.Where(user.Id == userVM.Id);
                //userVM.Email = user.Email.Where(user.Id.Equals(userVM));
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(attrEmail.mail, attrEmail.pass);

                var fill = "Congratulation " + userVM.Username + "\n\n"
                          + "Your submission was approved! \n\n"
                          + "\nThank You";

                MailMessage mm = new MailMessage("donotreply@domain.com", userVM.Email, "Job Registration Submission", fill);
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                client.Send(mm);

                jobsid.JobSeeker.Approve = true;

                _context.SaveChanges();
                return Ok("Successfully Sent");
            }
            return BadRequest("Not Successfully");
        }

        [HttpPost]
        [Route("Reject")]
        public IActionResult Reject(UserVM userVM)
        {
            if (ModelState.IsValid)
            {
                var jobsid = _context.Users.Include("JobSeeker").Where(r => r.Id == userVM.Id).FirstOrDefault();
                //var user = new User();
                if (jobsid.Id == userVM.Id)
                {
                    userVM.Email = jobsid.Email;
                    userVM.Username = jobsid.JobSeeker.Name;

                }
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(attrEmail.mail, attrEmail.pass);

                var fill = "Dear " + userVM.Username + "\n\n"
                          + "We are sorry that your submission doesn't match what we are looking for, maybe you can try again next time or other position. Keep spirit and always have a nice days \n"
                          + "\n\nThank You";

                MailMessage mm = new MailMessage("donotreply@domain.com", userVM.Email, "Job Registration Submission", fill);
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                client.Send(mm);

                jobsid.JobSeeker.Reject = true;

                _context.SaveChanges();
                return Ok("Successfully Sent");
            }
            return BadRequest("Not Successfully");
        }
    }
}