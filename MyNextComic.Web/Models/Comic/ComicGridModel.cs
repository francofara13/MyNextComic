using MyNextComic.Contracts.Entities;
using System.Collections.Generic;

namespace MyNextComic.Web.Models.Comic
{
    public class ComicGridModel
    {
        public List<Issue> Comics { get; set; }

        public ComicGridModel()
        {
            this.Comics = new List<Issue>();
        }
    }
}