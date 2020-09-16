﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobRegistrationSubmisson.Context;
using JobRegistrationSubmisson.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobRegistrationSubmisson.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly MyContext _context;
        public RolesController(MyContext myContext)
        {
            _context = myContext;
        }

        [HttpGet]
        public List<Role> GetAll()
        {
            List<Role> list = new List<Role>();
            foreach (var item in _context.Roles)
            {
                list.Add(item);
            }
            return list;
        }

        [HttpGet("{id}")]
        public Role GetID(int id)
        {
            return _context.Roles.Find(id);
        }

        [HttpPost]
        public IActionResult Create(Role role)
        {
            if (ModelState.IsValid)
            {
                var roles = new Role() { Name = role.Name };
                _context.Roles.AddAsync(roles);
                _context.SaveChanges();
                return Ok("Successfully Created");
                //return data;
            }
            return BadRequest("Create Failed!");
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, Role role)
        {
            if (ModelState.IsValid)
            {
                var getId = _context.Roles.Find(id);
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
                var getId = _context.Roles.Find(id);
                _context.Roles.Remove(getId);
                _context.SaveChanges();
            }
            return Ok("Successfully Delete");
        }
    }
}