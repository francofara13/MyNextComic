using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNextComic.Contracts.Entities
{
    public class IssueImage
    {
        public string Icon_Url { get; set; }
        public string Medium_Url { get; set; }
        public string Screen_Url { get; set; }
        public string Screen_LargeUrl { get; set; }
        public string Small_Url { get; set; }
        public string Super_Url { get; set; }
        public string Thumb_Url { get; set; }
        public string Tiny_Url { get; set; }
        public string Original_Url { get; set; }
    }
}
