using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MyNextComic.Web.Models.Account
{
    public class AccountModel
    {
        [DisplayName("Nombre de Usuario")]
        [Required]
        public string UserName { get; set; }

        [DisplayName("Contraseña")]
        [Required]
        public string Password { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }
    }
}