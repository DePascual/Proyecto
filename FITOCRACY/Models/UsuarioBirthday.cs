using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FITOCRACY.Models
{
    public class UsuarioBirthday
    {
        [Required(ErrorMessage = "Day required")]
        [Display(Name = "Day")]
        public string day { get; set; }

        [Required(ErrorMessage = "Month required")]
        [Display(Name = "Month")]
        public string month { get; set; }

        [Required(ErrorMessage = "Year required")]
        [Display(Name = "Year")]
        public string year { get; set; }
    }
}