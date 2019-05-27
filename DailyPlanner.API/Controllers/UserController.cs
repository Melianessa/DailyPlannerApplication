using System;
using System.Collections.Generic;
using DailyPlanner.DomainClasses;
using Microsoft.AspNetCore.Mvc;
using DailyPlanner.DomainClasses.Interfaces;
using DailyPlanner.DomainClasses.Models;
using Microsoft.AspNetCore.Authorization;

namespace DailyPlanner.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _iRepo;
        public UserController(IUserRepository repo)
        {
            _iRepo = repo;
        }
        [HttpGet]
        public IEnumerable<User> GetAll()
        {
            return _iRepo.GetAll();
        }
        
        [HttpGet]
        public IEnumerable<UserDTO> GetAllUsers()
        {
            return _iRepo.GetAllUsers();
        }
        
        [HttpGet("{id}")]
        public UserDTO GetUser(Guid id)
        {
            return _iRepo.GetUser(id);
        }

        
        [HttpPost]
        public Guid Post([FromBody] User user)
        {
            return _iRepo.Add(user);
        }

       [HttpPut("{id}")]
        public User Put([FromBody] User user)
        {
            return _iRepo.Update(user);
        }

        [HttpDelete("{id}")]
        public void Delete(User b)
        {
            _iRepo.Delete(b);
        }
    }
}
