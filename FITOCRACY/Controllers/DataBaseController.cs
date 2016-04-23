using FITOCRACY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FITOCRACY.Controllers
{
    public class DataBaseController : Controller
    {
        #region Usuario

        //....RECUPERAR....
        public Usuarios recuperaUsuario(string id)
        {
            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            var usuBD = (from usu in fitDB.Usuarios
                         where usu.Id_Usuario == Int32.Parse(id)
                         select usu).Single();

            return usuBD;
        }

        public Usuarios recuperaUsuarioConEmail(string email)
        {
            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            var usuBD = (from usu in fitDB.Usuarios
                         where usu.Email == email
                         select usu).Single();

            return usuBD;
        }

        public int recuperaID(Usuarios nuevoUsuario)
        {
            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            var usuBD = (from usu in fitDB.Usuarios
                         where usu.Email == nuevoUsuario.Email
                         && usu.Password == nuevoUsuario.Password
                         select usu).Single();

            return usuBD.Id_Usuario;

        }

        public int recuperaID(UsuarioLogin usuLogin)
        {
            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            var usuBD = (from usu in fitDB.Usuarios
                         where usu.Email == usuLogin.email
                         && usu.Password == usuLogin.password
                         select usu).Single();

            return usuBD.Id_Usuario;

        }

        //....EXISTE....
        public Boolean existeUsuario(UsuarioLogin usuLogin)
        {
            bool existe;

            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            var usuBD = from usu in fitDB.Usuarios
                        where usu.Email == usuLogin.email
                        && usu.Password == usuLogin.password
                        select usu;

            return existe = usuBD.Any() ? existe = true : existe = false;
        }

        public Boolean existeUsuario(UsuarioRegistro usuReg)
        {
            bool existe;

            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            var usuBD = from usu in fitDB.Usuarios
                        where usu.Email == usuReg.email
                        select usu;

            return existe = usuBD.Any() ? existe = true : existe = false;
        }

        public Boolean datosCorrectos(string idUsu, string actualPassword)
        {
            bool correcto;
            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            var usuBD = from usu in fitDB.Usuarios
                        where usu.Id_Usuario== Int32.Parse(idUsu)
                        && usu.Password == actualPassword
                        select usu;

            return correcto = usuBD.Any() ? correcto = true : correcto = false;
        }

        //....INSERTAR....
        public void insertaUsuario(Usuarios nuevoUsuario)
        {
            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            fitDB.Usuarios.InsertOnSubmit(nuevoUsuario);
            try
            {
                fitDB.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        //....MODIFICACIONES....
        public void updateUser(string id, string areaAboutYou)
        {
            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            var usuBD = (from usu in fitDB.Usuarios
                         where usu.Id_Usuario == int.Parse(id)
                         select usu).Single();

            usuBD.Description = areaAboutYou;
            try
            {
                fitDB.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void updatePassword(Usuarios usu, string newPass)
        {
            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            var usuBD = (from u in fitDB.Usuarios
                         where u.Email == usu.Email
                         select u).Single();

            usuBD.Password = newPass;
            try
            {
                fitDB.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void updateEdad(string idUsu, int edad)
        {
            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            var usuBD = (from u in fitDB.Usuarios
                         where u.Id_Usuario == Int32.Parse(idUsu)
                         select u).Single();

            usuBD.Age = edad;
            try
            {
                fitDB.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void updateBirthday(string idUsu, DateTime fecha)
        {
            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            var usuBD = (from u in fitDB.Usuarios
                         where u.Id_Usuario == Int32.Parse(idUsu)
                         select u).Single();

            usuBD.Birthday = fecha;
            try
            {
                fitDB.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void saveWorkout(string id, int idWorkout, int pointsWorkout)
        {
            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            var usuBD = (from usu in fitDB.Usuarios
                         where usu.Id_Usuario == Int32.Parse(id)
                         select usu).Single();

            usuBD.Points += pointsWorkout;

            int levelAct = levelActual(usuBD.Points);

            if (levelAct != usuBD.Level)
            {
                usuBD.Level = levelAct;
            }

            usuBD.WorkOuts += "$" + idWorkout.ToString();
            try
            {
                fitDB.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        //....CARGA FOTO....
        public void uploadFoto(string idUsu, byte[] array)
        {
            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            var usuBD = (from usu in fitDB.Usuarios
                         where usu.Id_Usuario == int.Parse(idUsu)
                         select usu).Single();

            usuBD.Foto = array;
            try
            {
                fitDB.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void logUsuEntre(int idUsu, string idEnt)
        {
            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();

            Usuario_Entrenamiento usu_ent = new Usuario_Entrenamiento();
            usu_ent.Id_Usuario = idUsu;
            usu_ent.Id_Entrenamiento = idEnt;

            fitDB.Usuario_Entrenamiento.InsertOnSubmit(usu_ent);
            try
            {
                fitDB.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        #endregion

        #region Tarjetas
        public bool existeTarjetaUsu(int id)
        {
            bool existe;

            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            var tarjetaBD = from t in fitDB.Tarjetas
                            where t.Id_Usuario == id
                            select t;

            return existe = tarjetaBD.Any() ? existe = true : existe = false;
        }


        public void grabaTarjeta(int id, UsuarioTarjeta usuTarjeta)
        {
            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();

            Tarjetas tarj = new Tarjetas();
            tarj.Id_Usuario = id;
            tarj.CardNumber = usuTarjeta.cardNumber;
            tarj.SecurityCode = usuTarjeta.securityCode;
            tarj.Caducity = usuTarjeta.month.Split(new char[] { '-' })[1] + usuTarjeta.year;

            fitDB.Tarjetas.InsertOnSubmit(tarj);

            try
            {
                fitDB.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void updateTarjeta(int id, UsuarioTarjeta usuTarjeta)
        {
            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            var tarjetaBD = (from t in fitDB.Tarjetas
                             where t.Id_Usuario == id
                             select t).Single();
            tarjetaBD.CardNumber = usuTarjeta.cardNumber;
            tarjetaBD.SecurityCode = usuTarjeta.securityCode;
            tarjetaBD.Caducity = usuTarjeta.month.Split(new char[] { '-' })[1] + usuTarjeta.year;
            try
            {
                fitDB.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        #endregion

        #region Entrenamiento

        public List<Workouts> listadoWorkouts()
        {
            List<Workouts> listadoW = new List<Workouts>();

            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            var workoutsBD = (from workout in fitDB.Workouts
                              select workout).ToArray();

            foreach (var workout in workoutsBD)
            {
                listadoW.Add(workout);
            }

            return listadoW;
        }

        public List<Workouts> recentWorkouts(string id)
        {
            List<Workouts> listadoW = new List<Workouts>();

            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();

            Usuarios usuBD = (from usu in fitDB.Usuarios
                            where usu.Id_Usuario == Int32.Parse(id)
                            select usu).Single();

            if(usuBD.WorkOuts != null)
            {
                List<string> workoutsUsu = (from u in fitDB.Usuarios
                                            where u.Id_Usuario == Int32.Parse(id)
                                            select u.WorkOuts).Single().Split(new char[] { '$' }).ToList();


                foreach (var work in workoutsUsu)
                {
                    if (work != "")
                    {
                        var workoutBD = (from workout in fitDB.Workouts
                                         where workout.Id_Workout == Int32.Parse(work)
                                         select workout).Single();

                        if (!listadoW.Contains(workoutBD))
                        {
                            listadoW.Add(workoutBD);
                        }
                    }
                }
            }           
            return listadoW;
        }

        public List<Tracks> listadoTracks(string workout)
        {
            List<Tracks> listadoT = new List<Tracks>();

            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            List<string> tracksWorkoutBD = (from work in fitDB.Workouts
                                            where work.Nombre == workout
                                            select work.Tracks).Single().Split(new char[] { ':' }).ToList();

            foreach (var track in tracksWorkoutBD)
            {
                var trackDB = (from t in fitDB.Tracks
                               where t.IdTrack == track
                               select t).Single();
                listadoT.Add(trackDB);
            }
            return listadoT;
        }

        public List<Tracks> listadoTracks()
        {
            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            List<Tracks> tracksBD = (from t in fitDB.Tracks
                                     orderby t.Nombre
                                     select t).ToList();
            return tracksBD;
        }

        public List<Tracks> listadoTracksCon(string busqueda)
        {
            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            List<Tracks> tracksBD = (from t in fitDB.Tracks
                                     where t.Nombre.Contains(busqueda)
                                     orderby t.Nombre
                                     select t).ToList();
            return tracksBD;
        }



        public int recuperaIDworkOut(string work)
        {
            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            var idWorkoutBD = (from w in fitDB.Workouts
                               where w.Nombre == work
                               select w.Id_Workout).Single();
            return idWorkoutBD.Value;
        }

        public int recuperaPOINTSworkOut(string work)
        {
            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            var pointsWorkoutBD = (from w in fitDB.Workouts
                                   where w.Nombre == work
                                   select w.Puntos).Single();
            return pointsWorkoutBD;
        }

        public int levelActual(int points)
        {
            int levelAct = 0;

            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            List<Levels> levelList = (from l in fitDB.Levels
                                      select l).ToList();

            for (int i = 0; i < levelList.Count(); i++)
            {
                if (points > levelList[i].Points && points < levelList[i + 1].Points)
                {
                    levelAct = levelList[i].Level;
                }

                if (points == levelList[i].Points)
                {
                    levelAct = levelList[i].Level;
                }
            }

            return levelAct;
        }

        public int pointsToNextLevel(Usuarios usuarioBD)
        {
            int puntos = 0;

            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            List<Levels> levelList = (from l in fitDB.Levels
                                      select l).ToList();

            for (int i = 0; i < levelList.Count(); i++)
            {
                if (levelList[i].Level == usuarioBD.Level)
                {
                    puntos = levelList[i + 1].Points - usuarioBD.Points;
                }
            }

            return puntos;
        }

        #endregion

        #region Entrenadores
        public void insertaEntrenador(Entrenadores nuevoEntrenador)
        {
            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            fitDB.Entrenadores.InsertOnSubmit(nuevoEntrenador);
            try
            {
                fitDB.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public List<Entrenadores> recuperaEntrenadores()
        {
            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            List<Entrenadores> entrenadoresList = (from entrenador in fitDB.Entrenadores
                                                   select entrenador).ToList();
            return entrenadoresList;
        }

        public Entrenadores recuperaEntrenador(string id)
        {
            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            Entrenadores entrenador = (from entren in fitDB.Entrenadores
                                       where entren.Id_Entrenador == id
                                       select entren).Single();
            return entrenador;
        }

        public List<Entrenamientos> recuperaEntrenamientos()
        {
            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            List<Entrenamientos> entrenamientosList = (from entrenamiento in fitDB.Entrenamientos
                                                       select entrenamiento).ToList();
            return entrenamientosList;
        }

        public Entrenamientos recuperaEntrenamiento(string id)
        {
            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            Entrenamientos entrenamiento = (from entren in fitDB.Entrenamientos
                                            where entren.Id_Entrenamiento == id
                                            select entren).Single();
            return entrenamiento;
        }

        public List<Entrenamientos> recuperaEntrenamientoFamilia(string familia)
        {
            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            List<Entrenamientos> entrenamientosList = (from entrenamiento in fitDB.Entrenamientos
                                                       where entrenamiento.Familia == familia
                                                       select entrenamiento).ToList();
            return entrenamientosList;
        }

        public void insertaFotoEntrenamiento(byte[] foto, string id)
        {
            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            var entrenamientoDb = (from entr in fitDB.Entrenamientos
                                   where entr.Id_Entrenamiento == id
                                   select entr).Single();

            entrenamientoDb.Foto = foto;
            try
            {
                fitDB.SubmitChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        #endregion

        #region Levels
        public List<Levels> recuperaLevels()
        {
            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();

            List<Levels> listadoLevels = (from level in fitDB.Levels
                                          select level).ToList();
            return listadoLevels;
        }

        #endregion

        #region Leaders
        public List<Usuarios> listadoLeaders()
        {
            FitocracyDBDataContext fitDB = new FitocracyDBDataContext();
            List<Usuarios> ususBD = (from usu in fitDB.Usuarios
                                     orderby usu.Level descending
                                     select usu).ToList();
            return ususBD;
        }

        #endregion
    }
}