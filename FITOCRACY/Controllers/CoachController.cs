using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FITOCRACY.Models;
using System.IO;

namespace FITOCRACY.Controllers
{
    public class CoachController : Controller
    {
        private DataBaseController dbController = new DataBaseController();

        public ActionResult InicioCoach()
        {
            List<Entrenadores>entrenadoresList = dbController.recuperaEntrenadores();
            ViewData["listadoEntrenadores"] = entrenadoresList;

            List<Entrenamientos> entrenamientosWeightLoss= dbController.recuperaEntrenamientoFamilia("Weight Loss");
            ViewData["listadoWeightLoss"] = entrenamientosWeightLoss;

            List<Entrenamientos> entrenamientosMuscleGain = dbController.recuperaEntrenamientoFamilia("Muscle Gain");
            ViewData["listadoBodyWeight"] = entrenamientosMuscleGain;

            List<Entrenamientos> entrenamientosOne= dbController.recuperaEntrenamientoFamilia("One");
            ViewData["listadoOne"] = entrenamientosOne;

            return View();
        }

        public ActionResult FatLoss()
        {
            return View();
        }

        public ActionResult AtHome()
        {
            return View();
        }
        public ActionResult Beginner()
        {
            return View();
        }

        public ActionResult NUtrition()
        {
            return View();
        }

        public ActionResult Women()
        {
            return View();
        }

        //..............Detalle Entrenamiento.......
        public ActionResult DetalleEntrenamiento(string id, string idEntrenador)
        {
            Entrenamientos entrenamiento = dbController.recuperaEntrenamiento(id);
            ViewData["entrenamiento"] = entrenamiento;

            Entrenadores entrenador = dbController.recuperaEntrenador(idEntrenador);
            return View(entrenador);
        }

        //...........Mis Métodos..................

        private byte[] ImgToDb(FileInfo info)
        {
            byte[] content = new byte[info.Length];
            FileStream imagestream = info.OpenRead();
            imagestream.Read(content, 0, content.Length);
            imagestream.Close();
            return content;
        }

        public Entrenadores infoEntrenador(string id)
        {
            Entrenadores entrenador = new Entrenadores();
            return entrenador = dbController.recuperaEntrenador(id);
        }

        

        //..............POST.......................
        public ActionResult Compra(string idEntr)
         {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                return RedirectToAction("Inicio", "Compra", new { id = idEntr });
            }
        }

        [HttpPost]
        public ActionResult CargaCoach()
        {
            Entrenadores nuevoEntrenador = new Entrenadores();
            nuevoEntrenador.Id_Entrenador = "1007A";
            nuevoEntrenador.Nombre = "Berzinator";
            nuevoEntrenador.Apellidos = "Ator";
            nuevoEntrenador.Descripcion = "Tim Berzins (Berzinator) owns and operates Berzinator Fitness Designs, a training and online coaching company based just outside of Philadelphia. With a focus on maximizing aesthetics, Tim is never satisfied with the status quo.";
            nuevoEntrenador.Foto = ImgToDb(new FileInfo(Server.MapPath("~//Content//Imagenes//Berzinator.jpg")));

            dbController.insertaEntrenador(nuevoEntrenador);

            return RedirectToAction("InicioCoach", "Coach");
        }

        public ActionResult CargaEntrenamientos()
        {
            dbController.insertaFotoEntrenamiento(ImgToDb(new FileInfo(Server.MapPath("~//Content//Imagenes//3.jpg"))), "20111000A");

            return RedirectToAction("InicioCoach", "Coach");
        }

    }
    
}