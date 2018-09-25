using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNextComic.Contracts.Entities
{
    public class Issue
    {
        public int Id { get; set; }
        public string Issue_Number { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Store_Date { get; set; }
        public IssueImage Image { get; set; }
    }
}
