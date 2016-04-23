using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FITOCRACY.Models
{
    public class Contact
    {
        [Required(ErrorMessage = "Username required")]
        [Display(Name = "User name")]
        public string username { get; set; }

        [Required(ErrorMessage = "Message required")]
        [Display(Name = "Message")]
        public string message { get; set; }
    }
}