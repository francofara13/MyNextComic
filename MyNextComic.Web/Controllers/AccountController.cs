using MyNextComic.Contracts.Account;
using MyNextComic.Services;
using MyNextComic.Web.Models.Account;
using PagedList;
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
            bool isAuthorized = Session["Authorized"] != null ? (bool)Session["Authorized"] : false;
            if (isAuthorized)
            {
                return RedirectToAction("Account");
            }
            else
            {
                return View();
            }
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

                if (result.ErrorMessage == "")
                {
                    Session["Authorized"] = true;
                    Session["UserName"] = model.UserName;
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    model.ModelError = result.ErrorMessage;
                    return PartialView("_LogIn", model);
                }
            }
            else
            {
                return PartialView("_LogIn", model);
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
                if (result.Success)
                {
                    Session["Authorized"] = true;
                    Session["UserName"] = model.UserName;
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    model.ModelError = result.ErrorMessage;
                    return PartialView("_SignUp", model);
                }
            }
            else
            {
                return PartialView("_SignUp", model);
            }
        }

        public ActionResult Account(AccountProfileModel model, int page = 1)
        {
            bool isAuthorized = Session["Authorized"] != null ? (bool)Session["Authorized"] : false;
            if (isAuthorized)
            {
                model.Name = (string)Session["UserName"];
                var result = accountService.GetUserData(model.Name);

                model.Email = result.Email;
                if (result.ComicList != null && result.ComicList.Count() > 0)
                {
                    model.ComicList = result.ComicList.ToPagedList(page, 4);
                }

                return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("_UserComicList", model.ComicList)
                : View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult LogOut()
        {
            Session["Authorized"] = false;
            Session["UserName"] = null;

            return RedirectToAction("Index","Home");
        }
    }
}