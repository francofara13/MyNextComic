using MyNextComic.Services;
using MyNextComic.Web.Models.Home;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyNextComic.Web.Controllers
{
    public class HomeController : Controller
    {
        ComicService comicService = new ComicService();

        public ActionResult Index()
        {
            var model = new HomeModel();
            model.TopComics = comicService.GetTopComics();

            return View(model);
        }

        public JsonResult InsertComics()
        {
            var result = comicService.InsertComics();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetRecommendation()
        {
            var RecommenderService = new RecommenderService();
            var result = await RecommenderService.GetRecommendation(10);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}