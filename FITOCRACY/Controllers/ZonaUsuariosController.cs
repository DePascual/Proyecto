using FITOCRACY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Linq;
using System.IO;
using System.Drawing;
using FITOCRACY.ViewModels;
using System.Net.Mail;
using System.Net;

namespace FITOCRACY.Controllers
{
    public class ZonaUsuariosController : Controller
    {
        private DataBaseController dbController = new DataBaseController();

        // GET: ZonaUsuarios
        public ActionResult Inicio(string id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                ViewBag.idUsu = id;
                return View();
            }

        }

       public ActionResult You(string id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                ViewData["IdUsu"] = id;

                UsuariosViewModel usuVM = new UsuariosViewModel();
                usuVM.usuarioBD = dbController.recuperaUsuario(id);

                int puntos = dbController.pointsToNextLevel(usuVM.usuarioBD);
                ViewData["pointsToNextLevel"] = puntos;

                ViewData["nextLevel"] = usuVM.usuarioBD.Level + 1;
                return View(usuVM);
            }
        }

        public ActionResult Track(string id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                ViewData["IdUsu"] = id;

                List<Workouts> listadoWorkouts = dbController.listadoWorkouts();
                ViewData["listadoWorkouts"] = listadoWorkouts;

                List<Workouts> recentWorkouts = dbController.recentWorkouts(id);
                ViewData["recentWorkouts"] = recentWorkouts;

                List<Tracks> listadoTracks = dbController.listadoTracks();
                ViewData["listadoTracks"] = listadoTracks;

                return View();
            }
        }

        public ActionResult Leaders(string id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                ViewData["IdUsu"] = id;

                List<Usuarios> listadoLeaders = dbController.listadoLeaders();
                ViewData["listadoLeaders"] = listadoLeaders;

                return View();
            }
        }



        public ActionResult Workout(string id, string workout)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                List<Tracks> listadoTracks = dbController.listadoTracks(workout);
                ViewData["listadoTracks"] = listadoTracks;
                ViewData["workout"] = workout;
                ViewData["IdUsu"] = id;

                return PartialView();
            }
        }

        public ActionResult Connect(string id)
        {

            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Message(string areaMessage)
        {

            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                mandarEmail(areaMessage);
                TempData["msg"] = "<script>alert('Your email has sent correctly!! We're working to answer as soon as possible!!');</script>";

                return RedirectToAction("Inicio", "ZonaUsuarios");
            }
        }


        public ActionResult About(string id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                ViewData["IdUsu"] = id;
                return PartialView();
            }
        }

        [HttpPost]
        public ActionResult upload(HttpPostedFileBase file, string idUsu)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {

                var url = Url.RequestContext.RouteData.Values["id"];
                if (file != null)
                {
                    string pic = Path.GetFileName(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/Imagenes/profiles"), pic);
                    file.SaveAs(path);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();

                        dbController.uploadFoto(idUsu, array);
                    }

                    System.IO.File.Delete(path);
                }

                return RedirectToAction("You", "ZonaUsuarios", new { id = idUsu });
            }
        }


        public ActionResult workoutDone(string idUsu, string work)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                int idWorkout = dbController.recuperaIDworkOut(work);
                int pointsWorkout = dbController.recuperaPOINTSworkOut(work);
                dbController.saveWorkout(idUsu, idWorkout, pointsWorkout);

                Usuarios usu = dbController.recuperaUsuario(idUsu);

                ViewData["work"] = work;
                ViewData["usuario"] = usu;

                return View();
            }
        }

        public ActionResult SignOut(string id)
        {
            Usuarios usu = (Usuarios)Session["usuario"];

            if(usu.Id_Usuario.ToString().Equals(id))
            {
                Session["usuario"] = null;
                return RedirectToAction("Inicio", "Home");
            }
            else
            {
                return RedirectToAction("Inicio", "Home");
            }
        }

        public ActionResult Search(string busqueda)
        {
            Usuarios usu = (Usuarios)Session["usuario"];
            List<Tracks> listTracksCon = dbController.listadoTracksCon(busqueda);
            Session["busqueda"] = listTracksCon;
            return RedirectToAction("Track", "ZonaUsuarios", new { id = usu.Id_Usuario });
        }


        public static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public void mandarEmail(string message)
        {
            Usuarios usu = (Usuarios)Session["usuario"];


            MailMessage nuevoCorreo = new MailMessage();
            nuevoCorreo.To.Add(new MailAddress("mail.pruebas.daw@gmail.com"));
            nuevoCorreo.From = new MailAddress("mail.pruebas.daw@gmail.com");
            nuevoCorreo.Subject = usu.Username + " needs our help!!! IMPORTANT!!" ;
            nuevoCorreo.IsBodyHtml = true;

            string body = "Email sendding for " + usu.Username + "!!<br /><hr/> ";
            body += "<span style='color:#1da6da'> "+ message+"</span>";
            body += "<hr/>";
            body += "Email user: "+usu.Email+"<br/>";
            body += "User code: "+usu.Id_Usuario+"<br/>";
            body += "<hr />";
            body += "FITOCRACY Team!!";
            nuevoCorreo.Body = body;

            SmtpClient servidor = new SmtpClient();
            servidor.Host = "smtp.gmail.com";
            servidor.Port = 587;
            servidor.EnableSsl = true;
            servidor.DeliveryMethod = SmtpDeliveryMethod.Network;
            servidor.Credentials = new NetworkCredential("mail.pruebas.daw@gmail.com", "avellanedadaw");
            servidor.Timeout = 2000;

            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
            try
            {
                servidor.Send(nuevoCorreo);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }

            nuevoCorreo.Dispose();

        }
    }
}