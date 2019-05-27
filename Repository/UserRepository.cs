﻿using System;
using System.Collections.Generic;
using System.Linq;
using DailyPlanner.DomainClasses;
using DailyPlanner.DomainClasses.Interfaces;
using DailyPlanner.DomainClasses.Models;
using Microsoft.EntityFrameworkCore;

namespace DailyPlanner.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly PlannerDbContext _context;
        public UserRepository(PlannerDbContext context)
        {
            _context = context;
        }
        public IEnumerable<User> GetAll()
        {
            return _context.Users.Include(p => p.Events).ToList();
        }

        public User Get(Guid id)
        {
            var user = _context.Users.Include(e => e.Events).FirstOrDefault(u => u.Id == id);
            return user;
        }

        public Guid Add(User b)
        {
            b.Id = Guid.NewGuid();
            b.CreationDate = DateTime.UtcNow;
            b.IsActive = true;
            _context.Users.Add(b);
            _context.SaveChanges();
            return b.Id;
        }

        public User Update(User b)
        {
            var user = _context.Users.Find(b.Id);
            if (user != null)
            {
                user.FirstName = b.FirstName.Trim();
                user.LastName = b.LastName.Trim();
                user.DateOfBirth = b.DateOfBirth;
                user.Email = b.Email.Trim();
                user.Sex = b.Sex;
                user.Phone = b.Phone.Trim();
                user.Role = b.Role;
                user.IsActive = b.IsActive;
            }
            _context.SaveChanges();
            return b;
        }

        public void Delete(User b)
        {
            if (b != null)
            {
                _context.Users.Remove(b);
            }
            _context.SaveChanges();
        }

        public IEnumerable<UserDTO> GetAllUsers()
        {
            return _context.Users.Include(p => p.Events).Select(p => new UserDTO(p)).ToList();
        }
        public UserDTO GetUser(Guid id)
        {
            return _context.Users.Include(p => p.Events).Select(p => new UserDTO(p)).FirstOrDefault(p => p.Id == id);
        }
    }
}
