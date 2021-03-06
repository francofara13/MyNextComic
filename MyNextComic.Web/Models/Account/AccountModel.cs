﻿using System.ComponentModel.DataAnnotations;
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

        public string ModelError { get; set; }

        public bool IsAdmin { get; set; }
    }
}