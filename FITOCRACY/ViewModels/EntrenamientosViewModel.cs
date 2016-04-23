using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FITOCRACY.Models;
namespace FITOCRACY.ViewModels
{
    public class EntrenamientosViewModel
    {
        public Entrenamientos entrenamientos { get; set; }
        public Entrenadores entrenadores { get; set; }
        public UsuarioTarjeta usuTarjeta { get; set; }
    }
}