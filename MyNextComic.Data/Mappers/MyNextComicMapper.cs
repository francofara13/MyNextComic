using MyNextComic.Contracts.Entities;
using System;

namespace MyNextComic.Data.Mappers
{
    public class MyNextComicMapper
    {
        public Comics MapComic(Issue issue)
        {
            var comic = new Comics
            {
                Id = issue.Id,
                Description = issue.Description != "" ? issue.Description : "",
                Image = issue.Image.Original_Url,
                Name = issue.Name != "" ? issue.Name : "",
                Issue_Number = issue.Issue_Number,
                Release_date = issue.Store_Date != "" ? Convert.ToDateTime(issue.Store_Date) : DateTime.MinValue
            };

            return comic;
        }
    }
}
