using MyNextComic.Contracts.Entities;
using System.Collections.Generic;

namespace MyNextComic.Web.Models.Home
{
    public class HomeModel
    {
        public List<Issue> TopComics { get; set; }
    }
}