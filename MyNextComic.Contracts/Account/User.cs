using System.Collections.Generic;

namespace MyNextComic.Contracts.Account
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public IEnumerable<Entities.Issue> ComicList { get; set; }
    }
}
