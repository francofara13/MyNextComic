using System.Collections.Generic;

namespace MyNextComic.Contracts.Entities
{
    public class IssuesResponse
    {
        public List<Issue> results { get; set; }
        public string error { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public int number_of_page_results { get; set; }
        public int number_of_total_results { get; set; }
        public int status_code { get; set; }
    }
}
