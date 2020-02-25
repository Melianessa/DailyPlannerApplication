﻿using DailyPlanner.API.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using DailyPlanner.DomainClasses.Enums;
using DailyPlanner.DomainClasses.Models;
using DailyPlanner.Repository;


namespace DailyPlanner.Test
{
    [TestClass]
    public class UserTests
    {
        User[] users = new[]
            {
                new User() { Id = Guid.NewGuid(), FirstName = "Alex", LastName = "Brown", DateOfBirth = new DateTime(1996, 08, 12), Email = "al@r.ua", Phone = "3333333", Role = RoleEnum.Admin, Sex = true },
                new User() { Id = Guid.NewGuid(), FirstName = "Karl", LastName = "Gop", DateOfBirth = new DateTime(1992, 01, 11), Email = "al3@r1.ua", Phone = "1111111", Role = RoleEnum.Client, Sex = false },
                new User() { Id = Guid.NewGuid(), FirstName = "Kate", LastName = "Lissa", DateOfBirth = new DateTime(1991, 10, 12), Email = "al1@r.ua", Phone = "2222222", Role = RoleEnum.Admin, Sex = false }
        }.OrderBy(p => p.FirstName).ToArray();

        private readonly UserRepository _userRepository;
        public static DbContextOptions<PlannerDbContext> DbContextOptions { get; }
        public static string ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=TestPlannerDB;Trusted_Connection=True;";
        static UserTests()
        {
            DbContextOptions = new DbContextOptionsBuilder<PlannerDbContext>()
                .UseSqlServer(ConnectionString)
                .Options;
        }
        public UserTests()
        {
            var context = new PlannerDbContext(DbContextOptions);
            _userRepository = new UserRepository(context);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Users.AddRange(users);
            context.SaveChanges();
        }
        [TestMethod]
        public void GetAllUsers_Test()
        {
            var controller = new UserController(_userRepository);
            //Act  
            var data = controller.GetAllUsers().OrderBy(p => p.FirstName).ToList();
            //Assert  
            Assert.AreEqual(data[0].FirstName, users[0].FirstName);
        }
        [TestMethod]
        public void AddUser_Test()
        {
            var controller = new UserController(_userRepository);
            User newUser = new User() { FirstName = "Mary", LastName = "Lane", DateOfBirth = new DateTime(1991, 08, 12), Email = "mary@r.ua", Phone = "7777777", Role = RoleEnum.Admin, Sex = false };
            //Act  
            var userId = controller.Post(newUser);
            var user = _userRepository.Get(userId);
            //Assert  
            Assert.AreEqual(user.LastName, newUser.LastName);
        }
        [TestMethod]
        public void GetUser_Test()
        {
            var controller = new UserController(_userRepository);
            //Act  
            var data = controller.GetUser(users[0].Id);
            //Assert  
            Assert.AreEqual(data.Email, users[0].Email);
        }
        [TestMethod]
        public void UpdateUser_Test()
        {
            var controller = new UserController(_userRepository);
            User newUser = new User() { LastName = "Snow" };
            var data = controller.GetUser(users[0].Id);
            //Act  
            var result = controller.Put(newUser);
            //Assert  
            Assert.AreEqual(result.LastName, newUser.LastName);
        }
        [TestMethod]
        public void DeleteUser_Test()
        {
            var controller = new UserController(_userRepository);
            var firstLength = users.Length;
            //Act
            controller.Delete(users[0]);
            var secondLength = controller.GetAllUsers().Count();
            //Assert
            Assert.AreNotEqual(firstLength, secondLength);
        }
    };
}
