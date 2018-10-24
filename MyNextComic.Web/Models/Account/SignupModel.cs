using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyNextComic.Web.Models.Account
{
    public class SignupModel : AccountModel
    {
        [DisplayName("Email")]
        [Required]
        public string Email { get; set; }
    }
}