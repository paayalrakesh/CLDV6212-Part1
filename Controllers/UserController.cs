using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ABC_Retailers.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ABC_Retailers.Controllers
{
    public class UserController : Controller
    {
        private readonly TableClient _tableClient;

        public UserController(IConfiguration configuration)
        {
            string storageConnectionString = configuration.GetConnectionString("AzureStorage");
            _tableClient = new TableClient(storageConnectionString, "Users");
            _tableClient.CreateIfNotExists();
        }

        // GET: /Users
        public async Task<IActionResult> Index()
        {
            List<User> users = new List<User>();

            await foreach (User user in _tableClient.QueryAsync<User>())
            {
                users.Add(user);
            }

            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(string userName, string email)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(email))
            {
                TempData["Message"] = "Username and Email cannot be empty.";
                return RedirectToAction("Index");
            }

            User newUser = new User
            {
                PartitionKey = "UserPartition",
                RowKey = Guid.NewGuid().ToString(),
                UserName = userName,
                Email = email
            };

            try
            {
                await _tableClient.AddEntityAsync(newUser);
                TempData["Message"] = "User created successfully.";
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Error creating user: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Create()
        {

            return View();
        }


    }
}