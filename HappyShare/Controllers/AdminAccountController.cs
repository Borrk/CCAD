using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using HappyShare.Data;
using HappyShare.Models;
using Microsoft.Extensions.Logging;
using HappyShare.Services;
using Microsoft.EntityFrameworkCore;
using HappyShare.Models.AccountViewModels;

namespace HappyShare.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminAccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;

        public AdminAccountController(
            ApplicationDbContext context,
            IServiceProvider serviceProvider,
            IEmailSender emailSender,
            ILogger<AdminAccountController> logger)
        {
            _context = context;
            _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            _emailSender = emailSender;
            _logger = logger;
        }

        // GET: AdminAccount
        public IActionResult Index()
        {
            IEnumerable<ApplicationUser> members = ReturnAllMembers().Result;
            return View(members);
        }

        public async Task<IEnumerable<ApplicationUser>> ReturnAllMembers()
        {
            var users = await _userManager.GetUsersInRoleAsync("Member");
            return users;
        }

        public async Task<IActionResult> EnableDisable(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            IEnumerable<ApplicationUser> members = ReturnAllMembers().Result;
            ApplicationUser member = (ApplicationUser)members.Single(u => u.Id == id);
            if (member == null)
            {
                return NotFound();
            }
            member.Enabled = !member.Enabled;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            IEnumerable<ApplicationUser> members = ReturnAllMembers().Result;
            ApplicationUser user = (ApplicationUser)members.Single(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            // remove role
            {
                // Remove member role
                await _userManager.RemoveFromRoleAsync(user, "Member");

                _logger.LogInformation("User {0} has been removed.", user.UserName );

                await _emailSender.SendEmailAsync(user.Email, "You are removed by the administrator", "Please be kindly notified that you are removed from Qualtiy Souvenir system.");
            }

            // remove user
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            IEnumerable<ApplicationUser> members = ReturnAllMembers().Result;
            ApplicationUser user = (ApplicationUser)members.Single(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            ViewData["userId"] = user.Id;

            var vm = new AccountViewModelBase()
            {
                Email = user.Email,
                FirstName=user.FirstName,
                LastName=user.LastName,
                HomePhoneNumber=user.HomePhoneNumber,
                WorkPhoneNumber=user.WorkPhoneNumber,
                MobilePhoneNumber=user.MobilePhoneNumber,
                Address=user.Address
            };

            return View(vm);
        }

        // POST: AdminAccount/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( AccountViewModelBase model, string userId )
        {
             if (userId == null)
            {
                return NotFound();
            }
            IEnumerable<ApplicationUser> members = ReturnAllMembers().Result;
            ApplicationUser user = (ApplicationUser)members.Single(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }

            // check same email
            var checkUser = await _userManager.FindByEmailAsync(model.Email);
            if ((checkUser != null) && (checkUser.Id != user.Id) )
            {
                ModelState.AddModelError("", "Email has been registered, please select another one.");
            }

            // validate phone
            if (!Utils.ContainValidPhoneNumber(model.HomePhoneNumber, model.WorkPhoneNumber, model.MobilePhoneNumber))
                ModelState.AddModelError("", "At least one valid phone number is required.");


            if (ModelState.IsValid)
            {
                user.UserName = model.Email;
                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.HomePhoneNumber = model.HomePhoneNumber;
                user.WorkPhoneNumber = model.WorkPhoneNumber;
                user.MobilePhoneNumber = model.WorkPhoneNumber;
                user.Address = model.Address;

                await _userManager.UpdateAsync(user);
                return RedirectToAction("Index");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

    }
}
