using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FITOCRACY.Models;

namespace FITOCRACY.ViewModels
{
    public class UsuariosViewModel
    {
        public UsuarioLogin usuarioLogin { get; set; }
        public UsuarioRegistro usuarioRegistro { get; set; }
        public Usuarios usuarioBD { get; set; }
    }
}