using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FITOCRACY.Models
{
    public class UsuarioTarjeta
    {

        [Required(ErrorMessage = "Card Number required")]
        [Display(Name = "Card Number")]
        [StringLength(16, ErrorMessage = "Put 16 characters in the Card Number", MinimumLength = 16)]
        public string cardNumber { get; set; }


        [Required(ErrorMessage = "Security Code required")]
        [Display(Name = "Security Code")]
        [StringLength(3, ErrorMessage ="Put 3 characters in the Security Code", MinimumLength =3)]
        public string securityCode { get; set; }

        [Required(ErrorMessage = "Month required")]
        [Display(Name = "Month")]
        public string month { get; set; }

        [Required(ErrorMessage = "Year required")]
        [Display(Name = "Year")]
        public string year { get; set; }

    }
}