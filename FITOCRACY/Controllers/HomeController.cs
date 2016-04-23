using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FITOCRACY.Models;
using System.Data.Linq;
using FITOCRACY.ViewModels;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Net;


namespace FITOCRACY.Controllers
{
    public class HomeController : Controller
    {

        private DataBaseController dbController = new DataBaseController();

        // GET
        public ActionResult Inicio()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }
        public ActionResult Registro()
        {
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
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

        public string generaNuevaPassword()
        {
            string newPass = "";
            string caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789%$#@+-=&";
            int n = caracteres.Length;

            Random r = new Random();

            for (int i = 0; i < 12; i++)
            {
                newPass += caracteres[r.Next(n)];
            }
            return newPass;
        }

        public static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public void mandarEmail(Usuarios usu, string newPass)
        {
            MailMessage nuevoCorreo = new MailMessage();
            nuevoCorreo.To.Add(new MailAddress(usu.Email));
            nuevoCorreo.From = new MailAddress("mail.pruebas.daw@gmail.com");
            nuevoCorreo.Subject = "FITOCRACY: Your new password";
            nuevoCorreo.IsBodyHtml = true;

            string body = "Hey " + usu.Username + "!!<br /> ";
            body += "You have forgotten your password for access...Don't worry!!<br /><br /> ";
            body += "Your new password is <span style='color:#1da6da'>"+newPass+"</span><br />";
            body += "Log in to your account whit the new password. To change the password, go to your custom site, and change it!! (If you want do it) <br/>";
            body += "Thanks for your confidence!!<br/>";
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
    

        //POST
        [HttpPost]
        public ActionResult Login(UsuariosViewModel usuVM)
        {
            bool existe;
            UsuarioLogin usuLogin = new UsuarioLogin();
            usuLogin = usuVM.usuarioLogin;

            if (ModelState.IsValid)
            {
                existe = dbController.existeUsuario(usuLogin);

                if (existe == true)
                {
                    int idUsu = dbController.recuperaID(usuLogin);

                    Usuarios usuario = dbController.recuperaUsuario(idUsu.ToString());
                    Session["usuario"] = usuario;

                    string url = (string)(Session["url"]);
                    if (url != null)
                    {
                        if (url.Contains("Coach"))
                        {
                            return Redirect(url);
                        }
                        else
                        {
                            return RedirectToAction("Inicio", "ZonaUsuarios", new { id = idUsu });
                        }
                    }
                    else
                    {
                        return RedirectToAction("Inicio", "ZonaUsuarios", new { id = idUsu });
                    }
                }
                else
                {
                    ViewBag.Error = "Either your username or password appears to be incorrect :(";
                    return View(usuVM);
                }
            }
            else
            {
                var errors = ModelState.SelectMany(x => x.Value.Errors.Select(z => z.Exception));
                return RedirectToAction("Login", "Home");
            }

        }

        //POST
        [HttpPost]
        public ActionResult Registro(UsuariosViewModel usuVM)
        {
            bool existe;

            UsuarioRegistro usuReg = new UsuarioRegistro();
            usuReg = usuVM.usuarioRegistro;

            if (ModelState.IsValid)
            {
                existe = dbController.existeUsuario(usuReg);

                if (existe == true)
                {
                    ViewBag.Error = "The user already exists!!";
                    return View(usuVM);
                }
                else
                {

                    Usuarios nuevoUsuario = new Usuarios();
                    nuevoUsuario.Email = usuReg.email;
                    nuevoUsuario.Username = usuReg.username;
                    nuevoUsuario.Password = usuReg.password;
                    nuevoUsuario.Foto = ImgToDb(new FileInfo(Server.MapPath("~//Content//Imagenes//profiles//nophoto.png")));

                    dbController.insertaUsuario(nuevoUsuario);

                    int idUsu = dbController.recuperaID(nuevoUsuario);

                    Usuarios usuario = dbController.recuperaUsuario(idUsu.ToString());
                    Session["usuario"] = usuario;

                    return RedirectToAction("Inicio", "ZonaUsuarios", new { id = idUsu });
                }
            }
            else
            {
                var errors = ModelState.SelectMany(x => x.Value.Errors.Select(z => z.Exception));
                return View(usuVM);
            }

        }

        [HttpPost]
        public ActionResult ForgotPassword(UsuariosViewModel usuVM)
        {
            UsuarioLogin usu = new UsuarioLogin();
            usu.email = usuVM.usuarioLogin.email;

            string email = usuVM.usuarioLogin.email;

            Usuarios usuBD = dbController.recuperaUsuarioConEmail(usu.email);

            string newPass = generaNuevaPassword();

            dbController.updatePassword(usuBD, newPass);
            mandarEmail(usuBD, newPass);

            TempData["msg"] = "<script>alert('We have sent an email whit your new Password');</script>";

            return RedirectToAction("Login", "Home");
        }


    }

}