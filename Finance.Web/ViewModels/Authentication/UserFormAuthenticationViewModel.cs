﻿using System.ComponentModel.DataAnnotations;

namespace Finance.Web.ViewModels.Authentication
{
    public class UserFormAuthenticationViewModel
    {
        [Required(ErrorMessage = "User Name is required.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
