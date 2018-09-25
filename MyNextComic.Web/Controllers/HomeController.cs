using MyNextComic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

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

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult InsertComics()
        {
            var comicService = new ComicService();
            var result = comicService.InsertComics();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetRecommendation()
        {
            var RecommenderService = new RecommenderService();
            var result = await RecommenderService.GetRecommendation();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}