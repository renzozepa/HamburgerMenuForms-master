﻿using HamburgerMenu.Models;
using HamburgerMenu.ServicioApi;
using HamburgerMenu.Tablas;
using Plugin.Connectivity;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HamburgerMenu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Sincronizar : ContentPage
    {
        public static List<TareadorDispositivosApi> LstTareadorDispositivos { get; set; }
        public static List<PersonalTareoApi> LstPersonalTareo { get; set; }
        public static List<TareoPersonalApi> LstTareoPersonal { get; set; }
        public Sincronizar()
        {
            InitializeComponent();
            if (App.Token == null)
            {
                BtnSincroAltaUsuario.IsEnabled = false;
                BtnSincroPersoDisponible.IsEnabled = false;
                BtnSincronizTareoPersonal.IsEnabled = false;
            }
            else if (App.FExpiracion <= DateTime.Now.Date)
            {
                BtnSincroAltaUsuario.IsEnabled = false;
                BtnSincroPersoDisponible.IsEnabled = false;
                BtnSincronizTareoPersonal.IsEnabled = false;
            }
            else
            {
                BtnSincroAltaUsuario.IsEnabled = true;
                BtnSincroPersoDisponible.IsEnabled = true;
                BtnSincronizTareoPersonal.IsEnabled = true;
            }
        }
        public static IEnumerable<PersonalTareo> ValidarExitencia(SQLiteConnection db, string numerodocumento)
        {
            return db.Query<PersonalTareo>("SELECT * FROM PersonalTareo where NUMERO_DOCUIDEN = ? And ID_TAREADOR = ?", numerodocumento, App.Tareador);
        }
        public static IEnumerable<TareoPersonal> ListarTareoPorSincronizar(SQLiteConnection db)
        {
            return db.Query<TareoPersonal>("Select * From TareoPersonal Where SINCRONIZADO = 0 And ID_TAREADOR = ?", App.Tareador);
        }
        private void Btn_SincroPersoDisponible(object sender, EventArgs e)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    LstPersonalTareo = new List<PersonalTareoApi>();
                    var t = Task.Run(async () => LstPersonalTareo = await HaugApi.Metodo.GetAllPersonalTareadorAsync(App.Tareador));

                    t.Wait();

                    using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                    {
                        conn.CreateTable<PersonalTareo>();
                        foreach (PersonalTareoApi itemPersonalTareoApi in LstPersonalTareo)
                        {
                            var DatosRegistro = new PersonalTareo
                            {
                                ID_PERSONAL = int.Parse(itemPersonalTareoApi.ID_PERSONAL.ToString()),
                                NOMBRE = itemPersonalTareoApi.NOMBRE.ToString(),
                                ID_TIPODOCUIDEN = itemPersonalTareoApi.ID_TIPODOCUIDEN.ToString(),
                                TIPODOCUIDEN = itemPersonalTareoApi.TIPODOCUIDEN.ToString(),
                                NUMERO_DOCUIDEN = itemPersonalTareoApi.NUMERO_DOCUIDEN.ToString(),
                                ID_SITUACION = int.Parse(itemPersonalTareoApi.ID_SITUACION.ToString()),
                                SITUACION = itemPersonalTareoApi.SITUACION.ToString(),
                                ID_PROYECTO = itemPersonalTareoApi.ID_PROYECTO.ToString(),
                                PROYECTO = itemPersonalTareoApi.PROYECTO.ToString(),
                                ID_TAREADOR = itemPersonalTareoApi.ID_TAREADOR.ToString(),
                                TAREADOR = itemPersonalTareoApi.TAREADOR.ToString(),
                                ID_CLASE_TRABAJADOR = int.Parse(itemPersonalTareoApi.ID_CLASE_TRABAJADOR.ToString()),
                                CLASE_TRABAJADOR = itemPersonalTareoApi.CLASE_TRABAJADOR.ToString(),
                                ID_USUARIO_SINCRONIZA = 1,
                                FECHA_SINCRONIZADO = DateTime.Now
                            };

                            var db = new SQLiteConnection(App.FilePath);
                            IEnumerable<PersonalTareo> resultado = ValidarExitencia(db, itemPersonalTareoApi.NUMERO_DOCUIDEN.ToString());
                            if (resultado.Count() > 0)
                            {
                                conn.Update(DatosRegistro);
                            }
                            else
                            {
                                conn.Insert(DatosRegistro);
                            }

                        }
                    }
                }
                else
                {
                    DisplayAlert("Haug Tareo", "Verifique su conexion a internet", "Ok");
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        private void Btn_SincronizTareoPersonal(object sender, EventArgs e)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    LstTareoPersonal = new List<TareoPersonalApi>();

                    var db = new SQLiteConnection(App.FilePath);
                    IEnumerable<TareoPersonal> resultado = ListarTareoPorSincronizar(db);

                    foreach (TareoPersonal TareoPersonalApiItem in resultado)
                    {
                        var t = Task.Run(async () => await HaugApi.Metodo.PostJsonHttpClient(
                            TareoPersonalApiItem.ID_TAREADOR, Convert.ToString(TareoPersonalApiItem.ID_PERSONAL), TareoPersonalApiItem.PERSONAL,
                            TareoPersonalApiItem.ID_PROYECTO, Convert.ToString(TareoPersonalApiItem.ID_SITUACION), Convert.ToString(TareoPersonalApiItem.ID_CLASE_TRABAJADOR),
                            TareoPersonalApiItem.FECHA_TAREO, Convert.ToString(TareoPersonalApiItem.TIPO_MARCACION), TareoPersonalApiItem.HORA,
                            TareoPersonalApiItem.FECHA_REGISTRO));
                        UpdTareo(TareoPersonalApiItem.ID);
                    }
                }
                else
                {
                    DisplayAlert("Haug Tareo", "Verifique su conexion a internet", "Ok");
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        public static void UpdTareo(int id)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                {
                    var tareo = conn.Table<TareoPersonal>().FirstOrDefault(j => j.ID == id);

                    if (tareo == null)
                    {
                        throw new Exception("Tareo no encontrado en database!");
                    }

                    tareo.SINCRONIZADO = 1;
                    tareo.FECHA_SINCRONIZADO = DateTime.Now;

                    conn.Update(tareo);
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.InnerException.ToString();
                string mensaje_1 = mensaje;
                throw;
            }
        }
        private void Btn_SincroAltaUsuario(object sender, EventArgs e)
        {

        }
        private void Btn_SincroGetToken(object sender, EventArgs e)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    LstTareadorDispositivos = new List<TareadorDispositivosApi>();
                    var t = Task.Run(async () => LstTareadorDispositivos = await HaugApi.Metodo.GetToken(App.Celular, App.Tareador));

                    t.Wait();

                    using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                    {
                        var Usuario = conn.Table<LoginLocal>().FirstOrDefault(j => j.CELULAR == App.Celular && j.TAREADOR == App.Tareador);

                        if (Usuario == null)
                        {
                            throw new Exception("Usuario no encontrado.!");
                        }
                        if (LstTareadorDispositivos != null)
                        {
                            if (LstTareadorDispositivos.Count > 0)
                            {
                                Usuario.TOKEN = LstTareadorDispositivos[0].TOKEN.ToString();
                                App.Token = LstTareadorDispositivos[0].TOKEN.ToString();

                                if (App.Token != null)
                                {
                                    BtnSincroAltaUsuario.IsEnabled = true;
                                    BtnSincroPersoDisponible.IsEnabled = true;
                                    BtnSincronizTareoPersonal.IsEnabled = true;
                                }

                                Usuario.FECHA_VIGENCIA = Convert.ToDateTime(LstTareadorDispositivos[0].FECHA_VENCIMIENTO);
                                App.FExpiracion = Convert.ToDateTime(LstTareadorDispositivos[0].FECHA_VENCIMIENTO.Value);
                                conn.Update(Usuario);
                            }
                        }
                    }
                }
                else
                {
                    DisplayAlert("Haug Tareo", "Verifique su conexion a internet", "Ok");
                }

            }
            catch (Exception)
            {
                throw;
            }

        }

    }
}