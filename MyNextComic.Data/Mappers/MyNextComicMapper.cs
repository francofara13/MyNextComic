using MyNextComic.Contracts.Entities;
using System;

namespace MyNextComic.Data.Mappers
{
    public class MyNextComicMapper
    {
        public Comics MapComic(Issue issue)
        {
            try
            {
                var comic = new Comics
                {
                    Id_Comic = issue.Id,
                    Id = issue.Id,
                    Description = issue.Description != "" && issue.Description != null ? issue.Description : "",
                    Image = issue.Image != null ? issue.Image.Original_Url : "",
                    Name = issue.Name != "" ? issue.Name : "",
                    Issue_Number = issue.Issue_Number != "" && issue.Issue_Number != null ? issue.Issue_Number : "0",
                    Release_date = issue.Store_Date != "" ? Convert.ToDateTime(issue.Store_Date) : DateTime.MinValue
                };

                return comic;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Issue MapIssue(Comics comic)
        {
            var issue = new Issue
            {
                Id = comic.Id_Comic,
                Name = comic.Name.Replace("\"", ""),
                Description = comic.Description != null ? comic.Description.Replace("\"", "") : "",
                Image = new IssueImage { Original_Url = comic.Image },
                Issue_Number = comic.Issue_Number,
                Store_Date = comic.Release_date.ToString(),
                GenreId = comic.Genre
            };

            return issue;
        }

        public Genre MapGenre(Genres genres)
        {
            var genre = new Genre
            {
                Id = genres.IdGenre,
                Description = genres.GenreDescription
            };

            return genre;
        }
    }
}
