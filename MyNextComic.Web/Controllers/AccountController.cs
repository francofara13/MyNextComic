using MyNextComic.Contracts.Account;
using MyNextComic.Services;
using MyNextComic.Web.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyNextComic.Web.Controllers
{
    public class AccountController : Controller
    {
        AccountService accountService = new AccountService();

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LogIn(AccountModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User()
                {
                    UserName = model.UserName,
                    Password = model.Password
                };
                var result = accountService.LogInUser(user);

                return RedirectToAction("Account", "Account");
            }
            else
            {
                return View("Index");
            }
        }
        public ActionResult SignUp(SignupModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User()
                {
                    UserName = model.UserName,
                    Password = model.Password,
                    Email = model.Email
                };
                var result = accountService.InsertUser(user);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("Index");
            }
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Account()
        {

            return View();
        }
    }
}