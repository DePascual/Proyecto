using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FITOCRACY.Models;
using FITOCRACY.ViewModels;
using System.Globalization;

namespace FITOCRACY.Controllers
{
    public class CustomUsuController : Controller
    {
        private DataBaseController dbController = new DataBaseController();

        // GET: CustomUsu
        public ActionResult About()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                return PartialView();
            }
        }


        public ActionResult Activities()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                Usuarios usu = (Usuarios)Session["usuario"];

                List<Workouts> recentWorkouts = dbController.recentWorkouts(usu.Id_Usuario.ToString());
                ViewData["recentWorkouts"] = recentWorkouts;
                return PartialView();
            }
        }

        public ActionResult Levels()
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                List<Levels> levels = dbController.recuperaLevels();
                ViewData["listadoLevels"] = levels;
                return PartialView();
            }
        }

       

        //POST
        [HttpPost]
        public ActionResult About(string id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                var url = Url.RequestContext.RouteData.Values["id"];
                ViewData["IdUsu"] = id;
                return PartialView();
            }
        }
     
        [HttpPost]
        public ActionResult descripcion(string areaAboutYou, string idUsu)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                dbController.updateUser(idUsu, areaAboutYou);
                return RedirectToAction("You", "ZonaUsuarios", new { id = idUsu });
            }
        }

        public ActionResult changePass(AboutUsuViewModel aboutVM, string idUsu)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {

                    bool confUsu = dbController.datosCorrectos(idUsu, aboutVM.PasswordChange.actualPassword);

                    if (confUsu == true)
                    {
                        Usuarios usu = dbController.recuperaUsuario(idUsu);
                        dbController.updatePassword(usu, aboutVM.PasswordChange.password);
                        return RedirectToAction("Login", "Home");
                    }
                    else
                    {
                        //Hay un error porque este usuario no deberia estar aqui
                        return RedirectToAction("Inicio", "Home");
                    }
                }
                else
                {
                    var errors = ModelState.SelectMany(x => x.Value.Errors.Select(z => z.Exception));
                    return RedirectToAction("You", "ZonaUsuarios", new { id = idUsu });
                }
            }
        }

        public ActionResult changeBirth(AboutUsuViewModel aboutVm, string idUsu)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    string fecha = aboutVm.UsuarioBirthday.day.Replace(" ", "") + "/" + aboutVm.UsuarioBirthday.month.Split(new char[] { '-' })[0].Replace(" ", "") + "/" + aboutVm.UsuarioBirthday.year.Replace(" ", "");

                    DateTime nac = DateTime.ParseExact(fecha, "dd/MM/yyyy", new CultureInfo("es-ES"));
                    int edad = DateTime.Today.AddTicks(-nac.Ticks).Year - 1;

                    dbController.updateEdad(idUsu, edad);
                    dbController.updateBirthday(idUsu, nac);

                    return RedirectToAction("You", "ZonaUsuarios", new { id = idUsu });
                }
                else
                {
                    var errors = ModelState.SelectMany(x => x.Value.Errors.Select(z => z.Exception));
                    return RedirectToAction("You", "ZonaUsuarios", new { id = idUsu });
                }
            }
        }        
    }
}