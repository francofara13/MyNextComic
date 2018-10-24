using MyNextComic.Contracts.Entities;
using MyNextComic.Services;
using MyNextComic.Web.Models.Comic;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;

namespace MyNextComic.Web.Controllers
{
    public class ComicsController : Controller
    {
        ComicService comicService = new ComicService();

        // GET: Comics
        public ActionResult Index(string searchString, int page = 1)
        {
            var result = comicService.GetComics(searchString);
            
            ComicGridModel model = new ComicGridModel();

            int pageSize = 20;

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("ComicList", result.ToPagedList(page, pageSize))
                : View(result.ToPagedList(page, pageSize));
        }

        public ActionResult Issue()
        {

        }
    }
}