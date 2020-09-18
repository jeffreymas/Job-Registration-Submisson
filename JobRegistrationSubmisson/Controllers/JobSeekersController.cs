using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobRegistrationSubmisson.Context;
using JobRegistrationSubmisson.Models;
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
            var getData = await _context.JobSeekers.Include("User").Where(Q => Q.Reject == false).ToListAsync();
            //var getData = await _context.jobSeekers.Include("User").ToListAsync();
            if (getData.Count == 0)
            {
                return null;
            }
            foreach (var item in getData)
            {
                var user = new JobSeekerVM()
                {
                    JobSId = item.User.Id,
                    Username = item.User.UserName,
                    Email = item.User.Email,
                    Address = item.Address,
                    //Password = item.User.PasswordHash,
                    Phone = item.User.PhoneNumber,
                    //JoblistName = item.Joblist.Name
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
                JobS.JoblistId = jobSeekerVM.JoblistId;
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
            var getData = _context.JobSeekers.Include("User").SingleOrDefault(x => x.JobSId == id);
            if (getData == null || getData.User == null)
            {
                return null;
            }
            var user = new JobSeekerVM()
            {
                JobSId = getData.User.Id,
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
        public IActionResult Update(string id, JobSeekerVM jobSeekerVM)
        {
            if (ModelState.IsValid)
            {
                var JobS = _context.JobSeekers.Include("User").SingleOrDefault(x => x.JobSId == id);
                //var getId = _context.Users.Find(id);
                //var hasbcrypt = BCrypt.Net.BCrypt.HashPassword(jobSeekerVM.Password, 12);
                JobS.User.Id = jobSeekerVM.JobSId;
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
                //JobS = jobSeekerVM.JoblistId;
                JobS.Reject = false;
                JobS.Approve = false;
                JobS.UpdateDate = DateTimeOffset.Now;

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