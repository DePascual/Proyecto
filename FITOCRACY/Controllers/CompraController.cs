using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FITOCRACY.Models;
using FITOCRACY.ViewModels;
using System.Net.Mail;
using System.Net;

namespace FITOCRACY.Controllers
{
    public class CompraController : Controller
    {

        private DataBaseController dbController = new DataBaseController();

        public ActionResult Inicio(string id)
        {

            EntrenamientosViewModel entreVM = new EntrenamientosViewModel();

            Entrenamientos entrenamiento = dbController.recuperaEntrenamiento(id);
            entreVM.entrenamientos = entrenamiento;

            return View(entreVM);
        }

        [HttpPost]
        public ActionResult Inicio(EntrenamientosViewModel entreVM, string idEnt)
        {

            if (ModelState.IsValid)
            {
                UsuarioTarjeta usuTarjeta = new UsuarioTarjeta();
                usuTarjeta = entreVM.usuTarjeta;

                Usuarios usu = (Usuarios)Session["usuario"];

                bool existe = dbController.existeTarjetaUsu(usu.Id_Usuario);

                if(existe == true)
                {
                    dbController.updateTarjeta(usu.Id_Usuario, usuTarjeta);
                }
                else
                {
                    dbController.grabaTarjeta(usu.Id_Usuario, usuTarjeta);
                }

                dbController.logUsuEntre(usu.Id_Usuario, idEnt);
              
                mandarEmail(usu, idEnt);

                return RedirectToAction("InicioCoach", "Coach");
            }
            else
            {
                var errors = ModelState.SelectMany(x => x.Value.Errors.Select(z => z.Exception));
                ViewBag.Error= errors;
                return RedirectToAction("Inicio", "Compra", idEnt);
            }         
        }
        public static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        public void mandarEmail(Usuarios usu, string idEnt)
        {
            Entrenamientos entrenamientoComprado = dbController.recuperaEntrenamiento(idEnt);

            MailMessage nuevoCorreo = new MailMessage();
            nuevoCorreo.To.Add(new MailAddress(usu.Email));
            nuevoCorreo.From = new MailAddress("mail.pruebas.daw@gmail.com");
            nuevoCorreo.Subject = "FITOCRACY: Your new training";
            nuevoCorreo.IsBodyHtml = true;

            string body = "Hey " + usu.Username + "!!<br /> ";
            body += "Your new training <span style='color:#1da6da'>" + entrenamientoComprado.NombreEntrenamiento + "</span> has been deal correctly !! Enjoy it!!!<br /> ";
            body += "Be happy and strong!!";
            body += "<hr />";
            body += "FITOCRACY Team!!";
            nuevoCorreo.Body = body;
            //nuevoCorreo.Attachments.Add(new Attachment(Server.MapPath("~/facturas/" + miCliente.email + "_" + today + ".pdf")));

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