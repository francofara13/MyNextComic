using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using MyNextComic.Contracts.Entities;
using MyNextComic.Services;

namespace MyNextComic.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public JsonResult GetComics()
        {
            ComicsService service = new ComicsService();
            service.InsertComics();
            
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public class issueResponse
        {
            public List<Issue> results { get; set; }
            public string error { get; set; }
            public int limit { get; set; }
            public int offset { get; set; }
            public int number_of_page_results { get; set; }
            public int number_of_total_results { get; set; }
            public int status_code { get; set; }
        }
    }
}