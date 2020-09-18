using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobRegistrationSubmisson.Context;
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
    }
}