using FITOCRACY.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FITOCRACY.Controllers
{
    public class ImageController : Controller
    {

        public ActionResult Show(string id)
        {
            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            var img = (from i in fitDB.Usuarios
                       where i.Id_Usuario == int.Parse(id)
                       select i.Foto).Single().ToArray();

            return File(img, "image/jpg");
        }

        public ActionResult showEntrenador(string id)
        {
            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            var img = (from i in fitDB.Entrenadores
                       where i.Id_Entrenador == id
                       select i.Foto).Single().ToArray();

            return File(img, "image/jpg");
        }

        public ActionResult showFotoEntrenamiento(string id)
        {
            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            var img = (from i in fitDB.Entrenamientos
                       where i.Id_Entrenamiento == id
                       select i.Foto).Single().ToArray();

            return File(img, "image/jpg");
        }

    }    
}