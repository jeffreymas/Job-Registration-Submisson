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
    public class JobSeekersController : ControllerBase
    {
        private readonly MyContext _context;

        public IConfiguration _configuration;

        AttrEmail attrEmail = new AttrEmail();
        SmtpClient client = new SmtpClient();

        public JobSeekersController(MyContext myContext, IConfiguration config)
        {
            _context = myContext;
            _configuration = config;
        }

        ///[Authorize]
        //GET api/values
        [HttpGet]
        public async Task<List<JobSeekerVM>> GetAll()
        {
            List<JobSeekerVM> list = new List<JobSeekerVM>();
            //var user = new UserVM();
            var getData = await _context.JobSeekerLists.Include("JobSeeker").Include("Joblist").Where(Q => Q.JobSeeker.Reject == false && Q.JobSeeker.Approve == false).ToListAsync();
            //var getuser = await _context.Users.Include("JobSeeker").Include("JobSeekerList").Include("Joblist").Where(Q => Q.JobSeeker.Reject == false).ToListAsync();

            //var conget = new List<string>().Concat(getData).concat(getuser)
            //var getjobS = .Include("User").ToListAsync();
            if (getData.Count == 0)
            {
                return null;
            }
            foreach (var item in getData)
            {
                var user = new JobSeekerVM()
                    {
                        JobSId = item.JobSeeker.JobSId,
                        //Username = item.JobSeeker.User.UserName,
                        //Email = item.JobSeeker.User.Email,
                        Name = item.JobSeeker.Name,
                        Address = item.JobSeeker.Address,
                        //Password = item.User.PasswordHash,
                        //Phone = item.JobSeeker.User.PhoneNumber,
                        JoblistName = item.Joblist.Name
                        //CreatedData = item.CreatedData,
                        //UpdatedData = item.UpdatedData
                        //RoleName = item.Role.Name,

                        //VerifyCode = item.User.SecurityStamp,

                    };
                    list.Add(user);

                //user.JobSId = item.JobSeeker.User.Id;
                //user.Username = item.JobSeeker.User.UserName;
                //user.Email = item.JobSeeker.User.Email;
                //user.Password = item.JobSeeker.User.PasswordHash;
                //user.Phone = item.JobSeeker.User.PhoneNumber;
                //user.JoblistName = item.Joblist.Name;
                //list.Add(user);
            }
            //foreach (var usr in getuser)
            //{
            //    var JobSU = new JobSeekerVM()
            //    {
            //        Username = usr.User.UserName
            //    };
            //    list.Add(JobSU);
            //}

            //int[] z = new List<string>().Concat().Concat(b).ToArray();

            return list;
        }


        [HttpGet]
        [Route("GetApprove")]
        public async Task<List<JobSeekerVM>> GetApprove()
        {
            List<JobSeekerVM> list = new List<JobSeekerVM>();
            var getData = await _context.JobSeekerLists.Include("JobSeeker").Include("Joblist").Where(Q => Q.JobSeeker.Approve == true).ToListAsync();

            if (getData.Count == 0)
            {
                return null;
            }
            foreach (var item in getData)
            {
                var user = new JobSeekerVM()
                {
                    JobSId = item.JobSeeker.JobSId,
                    Name = item.JobSeeker.Name,
                    Address = item.JobSeeker.Address,
                    JoblistName = item.Joblist.Name,
                    RegistDate = item.JobSeeker.RegistDate


                };
                list.Add(user);
            }
            return list;
        }

        //[Authorize]

        [HttpPost]
        public IActionResult Create(JobSeekerVM jobSeekerVM)
        {
            if (ModelState.IsValid)
            {
                
                var JobS = new JobSeeker();
                //JobS.JobSId = jobSeekerVM.JobSId;
                JobS.User.UserName = jobSeekerVM.Username;
                JobS.Gender = jobSeekerVM.Gender;
                JobS.Birth_Date = jobSeekerVM.Birth_Date;
                JobS.Address = jobSeekerVM.Address;
                JobS.Religion = jobSeekerVM.Religion;
                JobS.Marital_Status = jobSeekerVM.Marital_Status;
                JobS.Nationality = jobSeekerVM.Nationality;
                JobS.Last_Education = jobSeekerVM.Last_Education;
                JobS.GPA = jobSeekerVM.GPA;
                JobS.Technical_Skill = jobSeekerVM.Technical_Skill;
                JobS.Experience = jobSeekerVM.Experience;
                JobS.Achievement = jobSeekerVM.Achievement;
                //JobS.JoblistId = jobSeekerVM.JoblistId;
                JobS.RegistDate = DateTimeOffset.Now;
                JobS.Reject = false;
                JobS.Approve = false;
                _context.JobSeekers.AddAsync(JobS);

                _context.SaveChanges();
                return Ok("Successfully Created");
            }
            return BadRequest("Not Successfully");
        }



        [HttpGet("{id}")]
        public JobSeekerVM GetID(string id)
        {
            var getData = _context.JobSeekerLists.Include("JobSeeker").Include("Joblist").Where(Q => Q.JobSeeker.Reject == false).SingleOrDefault(x => x.JobSeekerId == id); ;
            if (getData == null)
            {
                return null;
            }
            var user = new JobSeekerVM()
            {
                JobSId = getData.JobSeeker.JobSId,
                Name = getData.JobSeeker.Name,
                Address = getData.JobSeeker.Address,
                Birth_Date = getData.JobSeeker.Birth_Date,
                Nationality = getData.JobSeeker.Nationality,
                Marital_Status = getData.JobSeeker.Marital_Status,
                Gender = getData.JobSeeker.Gender,
                Religion = getData.JobSeeker.Religion,
                Last_Education = getData.JobSeeker.Last_Education,
                GPA = getData.JobSeeker.GPA,
                Technical_Skill = getData.JobSeeker.Technical_Skill,
                Experience = getData.JobSeeker.Experience,
                Achievement = getData.JobSeeker.Achievement
                //JoblistName = getData.Joblist.Name

            };
            return user;
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, JobSeekerVM jobSeekerVM)
        {
            if (ModelState.IsValid)
            {
                //var getid = _context.JobSeekers.Include
                var JobS = _context.JobSeekers.Include("User").FirstOrDefault(x => x.JobSId == id);
                //var getId = _context.Users.Find(id);
                //var hasbcrypt = BCrypt.Net.BCrypt.HashPassword(jobSeekerVM.Password, 12);
                JobS.User.Id = jobSeekerVM.JobSId;
                JobS.Name = jobSeekerVM.Name;
                JobS.Gender = jobSeekerVM.Gender;
                JobS.Birth_Date = jobSeekerVM.Birth_Date;
                JobS.Address = jobSeekerVM.Address;
                JobS.Religion = jobSeekerVM.Religion;
                JobS.Marital_Status = jobSeekerVM.Marital_Status;
                JobS.Nationality = jobSeekerVM.Nationality;
                JobS.Last_Education = jobSeekerVM.Last_Education;
                JobS.GPA = jobSeekerVM.GPA;
                JobS.Technical_Skill = jobSeekerVM.Technical_Skill;
                JobS.Experience = jobSeekerVM.Experience;
                JobS.Achievement = jobSeekerVM.Achievement;
                //JobS = jobSeekerVM.JoblistId;
                JobS.Reject = false;
                JobS.Approve = false;
                JobS.UpdateDate = DateTimeOffset.Now;

                _context.SaveChanges();
                if (JobS.User.EmailConfirmed == false)
                {
                    client.Port = 587;
                    client.Host = "smtp.gmail.com";
                    client.EnableSsl = true;
                    client.Timeout = 10000;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(attrEmail.mail, attrEmail.pass);
                    var fill = "Hi you have a notification \n\n"
                              + "Check you application account \n"
                              + "\n\nThank You";

                    MailMessage mm = new MailMessage("donotreply@domain.com", "jepri.tugas@gmail.com", "Notification Jobseeker", fill);
                    mm.BodyEncoding = UTF8Encoding.UTF8;
                    mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                    client.Send(mm);
                    JobS.User.EmailConfirmed = true;
                    _context.SaveChanges();
                }

                return Ok("Successfully Update");
            }
            return BadRequest("Not Successfully");
        }



        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (ModelState.IsValid)
            {
                var getData = _context.JobSeekers.Include("User").SingleOrDefault(x => x.JobSId == id);
                if (getData == null)
                {
                    return BadRequest("Not Succsessfully");
                }
                getData.RejectDate = DateTimeOffset.Now;
                getData.Reject = true;

                _context.Entry(getData).State = EntityState.Modified;
                _context.SaveChangesAsync();
                return Ok("Successfully Delete");
            }
            return BadRequest("Not Success");
        }
    }
}