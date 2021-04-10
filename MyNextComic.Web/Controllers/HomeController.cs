using MyNextComic.Services;
using MyNextComic.Web.Models.Home;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Collections.Generic;

namespace MyNextComic.Web.Controllers
{
    public class HomeController : Controller
    {
        ComicService comicService = new ComicService();
        AccountService accountService = new AccountService();

        public ActionResult Index()
        {
            var model = new HomeModel();
            model.TopComics = comicService.GetTopComics();

            return View(model);
        }

        public async Task<ActionResult> GetRecommendation()
        {
            var RecommenderService = new RecommenderService();
            var userId = 0;
            bool isAuthorized = Session["Authorized"] != null ? (bool)Session["Authorized"] : false;
            if (isAuthorized)
            {
                var userName = (string)Session["UserName"];
                var user = accountService.GetUserData(userName);
                userId = user.UserId;
            }

            List<Contracts.Entities.Issue> result;

            if (userId == 0)
            {
                result = comicService.GetTopComics();
                result.RemoveRange(5, 3);
            }
            else
            {
                var recommendations = await RecommenderService.GetRecommendation(userId);
                result = comicService.GetComics("", 0, recommendations);
            }

            var model = new HomeModel() { RecommendedComics = result };

            return PartialView("_RecommendationList", model);
        }
    }
}