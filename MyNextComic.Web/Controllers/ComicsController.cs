using MyNextComic.Contracts.Entities;
using MyNextComic.Services;
using MyNextComic.Web.Models.Comic;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using System;

namespace MyNextComic.Web.Controllers
{
    public class ComicsController : Controller
    {
        ComicService comicService = new ComicService();

        // GET: Comics
        public ActionResult Index(string searchString, int genre = 0, int page = 1)
        {
            ComicGridModel model = new ComicGridModel();

            int pageSize = 20;

            var genres = comicService.GetGenres();
            var generos = new List<SelectListItem>();
            generos.Add(new SelectListItem { Value = "0", Text = "All" });
            foreach (var g in genres)
            {
                generos.Add(new SelectListItem { Value = g.Id.ToString(), Text = g.Description });
            }
            generos.Where(x => x.Value == genre.ToString()).FirstOrDefault().Selected = true;
            ViewBag.genres = generos;

            var result = comicService.GetComics(searchString, genre, new List<int>());

            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("ComicList", result.ToPagedList(page, pageSize))
                : View(result.ToPagedList(page, pageSize));
        }

        public ActionResult Issue(int id)
        {
            var result = comicService.GetComic(id);
            if (result.GenreId.HasValue)
            {
                var genre = comicService.GetGenres(Convert.ToInt32(result.GenreId));
                result.GenreDescription = genre.FirstOrDefault().Description;
            }

            result.Rating = Math.Round(comicService.GetRating(result.Id), 0, MidpointRounding.AwayFromZero);

            bool isAuthorized = Session["Authorized"] != null ? (bool)Session["Authorized"] : false;
            if (isAuthorized)
            {
                var userName = (string)Session["UserName"];
                var userRating = comicService.GetUserRating(userName, result.Id);
                ViewBag.UserRating = Math.Round(userRating, 0, MidpointRounding.AwayFromZero);
            }

            return View(result);
        }

        public JsonResult RateComic(double value, int idComic)
        {
            var userName = (string)Session["UserName"];
            var result = comicService.SaveUserRating(userName, idComic, value);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}