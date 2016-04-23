using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FITOCRACY.Models
{
    public class PasswordChange
    {
        [Required(ErrorMessage = "Actual Password required")]
        [DataType(DataType.Password)]
        [Display(Name = "Actual Password")]
        public string actualPassword { get; set; }

        [Required(ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }

        [Required(ErrorMessage = "RePassword required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("password", ErrorMessage = "The password and confirmation password do not match.")]
        public string confPassword { get; set; }
    }
}