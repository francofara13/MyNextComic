using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNextComic.Contracts.Account
{
    public class UserServiceResponse
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}
