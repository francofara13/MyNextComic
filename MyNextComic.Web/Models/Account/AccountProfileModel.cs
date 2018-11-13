using PagedList;
using System.Collections.Generic;

namespace MyNextComic.Web.Models.Account
{
    public class AccountProfileModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public IPagedList<Contracts.Entities.Issue> ComicList { get; set; }
    }
}