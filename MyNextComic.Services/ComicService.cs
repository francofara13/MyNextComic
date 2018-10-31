using MyNextComic.Contracts.Entities;
using RestSharp;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Transactions;
using MyNextComic.Data;
using System;
using MyNextComic.Data.Mappers;
using System.Data.Entity.Validation;
using System.Linq;
using System.Data;

namespace MyNextComic.Services
{
    public class ComicService
    {
        public List<Issue> GetAllComics(int numberOfCalls, int startingOffset, int pageResults)
        {
            var result = new List<Issue>();

            var api_key = "f4f09d38e76dd9b82640fe34672187c7eab5ef73";

            pageResults = pageResults > 100 ? 100 : pageResults;
            for (int i = 0; i < numberOfCalls; i++)
            {
                var client = new RestClient("http://comicvine.gamespot.com/api");

                var request = new RestRequest("issues", Method.GET);
                request.AddParameter("api_key", api_key);
                request.AddParameter("field_list", "description,id,image,issue_number,name,store_date");
                request.AddParameter("limit", pageResults);
                request.AddParameter("offset", startingOffset);
                request.AddParameter("format", "json");

                IRestResponse response = client.Execute(request);
                IssuesResponse issues = JsonConvert.DeserializeObject<IssuesResponse>(response.Content);
                issues.results = issues.results.Where(x => x.Name != null && x.Name != "").ToList();
                result.AddRange(issues.results);

                startingOffset = startingOffset + pageResults;
            }

            return result;
        }

        public bool InsertComics()
        {
            var result = false;
            try
            {
                var totalCalls = 670;
                var startingOffset = 116400;
                var numberOfCalls = 10;
                var limit = 100;
                for (int i = 0; i < totalCalls; i++)
                {
                    var issues = GetAllComics(numberOfCalls, startingOffset, limit);

                    using (TransactionScope scope = new TransactionScope())
                    {
                        MyNextComicEntities context = null;
                        try
                        {
                            context = new MyNextComicEntities();
                            context.Configuration.AutoDetectChangesEnabled = false;

                            var mapper = new MyNextComicMapper();

                            int count = 0;
                            foreach (var issue in issues)
                            {
                                ++count;
                                context = AddToContext(context, mapper.MapComic(issue), count, 100, true);
                            }

                            context.SaveChanges();
                        }
                        catch (DbEntityValidationException e)
                        {

                        }
                        finally
                        {
                            if (context != null)
                                context.Dispose();
                        }

                        scope.Complete();
                    }

                    result = true;

                    startingOffset = startingOffset + (numberOfCalls * limit);
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        private MyNextComicEntities AddToContext(MyNextComicEntities context, Comics issue, int count, int commitCount, bool recreateContext)
        {
            if (!(context.Comics.Any(x => x.Id_Comic == issue.Id_Comic)))
            {
                context.Comics.Add(issue);
            }

            //context.SaveChanges();

            //context.Set<Comics>().Add(issue);

            if (count % commitCount == 0)
            {
                context.SaveChanges();
                if (recreateContext)
                {
                    context.Dispose();
                    context = new MyNextComicEntities();
                    context.Configuration.AutoDetectChangesEnabled = false;
                }
            }

            return context;
        }

        public List<Issue> GetComics(string searchString)
        {
            var result = new List<Issue>();

            using (TransactionScope scope = new TransactionScope())
            {
                MyNextComicEntities context = null;

                try
                {
                    context = new MyNextComicEntities();
                    context.Configuration.AutoDetectChangesEnabled = false;
                    var comics = new List<Comics>();
                    if (String.IsNullOrEmpty(searchString))
                    {
                        comics = context.Comics.OrderBy(x => x.Id).ToList();
                    }
                    else
                    {
                        comics = context.Comics.Where(x => x.Name.ToLower().Contains(searchString.ToLower())).OrderBy(x => x.Id).ToList();
                    }
                    
                    var mapper = new MyNextComicMapper();

                    foreach (var comic in comics)
                    {
                        result.Add(mapper.MapIssue(comic));
                    }
                }
                catch (DbEntityValidationException e)
                {
                }

                context.Dispose();
                scope.Complete();
            }
            
            return result;
        }

        public Issue GetComic(int id)
        {
            Issue result = null;
            using (TransactionScope scope = new TransactionScope())
            {
                MyNextComicEntities context = null;

                try
                {
                    context = new MyNextComicEntities();
                    context.Configuration.AutoDetectChangesEnabled = false;

                    var issue = context.Comics.Where(x => x.Id_Comic == id).ToList().FirstOrDefault();

                    var mapper = new MyNextComicMapper();

                    result = mapper.MapIssue(issue);
                }
                catch (DbEntityValidationException e)
                {
                }

                context.Dispose();
                scope.Complete();
            }

            return result;
        }

        public Genre GetGenres(int genreId)
        {
            Genre result = null;
            using (TransactionScope scope = new TransactionScope())
            {
                MyNextComicEntities context = null;

                try
                {
                    context = new MyNextComicEntities();
                    context.Configuration.AutoDetectChangesEnabled = false;

                    var genre = context.Genres.Where(x => x.IdGenre == genreId).ToList().FirstOrDefault();

                    var mapper = new MyNextComicMapper();

                    result = mapper.MapGenre(genre);

                }
                catch (DbEntityValidationException e)
                {
                }

                context.Dispose();
                scope.Complete();
            }

            return result;
        }

        public double GetRating(int comicId)
        {
            double result = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                MyNextComicEntities context = null;

                try
                {
                    context = new MyNextComicEntities();
                    context.Configuration.AutoDetectChangesEnabled = false;
                    var values = context.Preferences.Where(x => x.ItemID == comicId).Select(x => x.Value).ToList();
                    if (values.Count > 0)
                    {
                        result = values.Average();
                    }
                }
                catch (DbEntityValidationException e)
                {
                }

                context.Dispose();
                scope.Complete();
            }

            return result;
        }
    }
}
