using Acr.UserDialogs;
using HamburgerMenu.Models;
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
        public static List<HorarioApi> LstHorario { get; set; }
        public static List<PersonalTareoApi> LstPersonalTareo { get; set; }
        public static List<TareoPersonalS10Api> LstTareoPersonal { get; set; }
        public static List<PersonalS10Api> LstPersonalS10 { get; set; }
        public Sincronizar()
        {
            InitializeComponent();
            //if (App.Token == null)
            //{
            //    BtnSincroAltaUsuario.IsEnabled = false;
            //    BtnSincroPersoDisponible.IsEnabled = false;
            //    BtnSincronizTareoPersonal.IsEnabled = false;
            //    BtnSincronizHorario.IsEnabled = false;
            //}
            //else if (App.FExpiracion <= DateTime.Now.Date)
            //{
            //    BtnSincroAltaUsuario.IsEnabled = false;
            //    BtnSincroPersoDisponible.IsEnabled = false;
            //    BtnSincronizTareoPersonal.IsEnabled = false;
            //    BtnSincronizHorario.IsEnabled = false;
            //}
            //else
            //{
            //    BtnSincroAltaUsuario.IsEnabled = true;
            //    BtnSincroPersoDisponible.IsEnabled = true;
            //    BtnSincronizTareoPersonal.IsEnabled = true;
            //    BtnSincronizHorario.IsEnabled = true;
            //}
        }
        public static IEnumerable<PersonalTareo> ValidarExitencia(SQLiteConnection db, string numerodocumento)
        {
            return db.Query<PersonalTareo>("SELECT * FROM PersonalTareo where NUMERO_DOCUIDEN = ? And ID_TAREADOR = ?", numerodocumento, App.Tareador);
        }
        public static IEnumerable<Tablas.Personal> ValidarExitenciaS10(SQLiteConnection db, string numerodocumento)
        {
            return db.Query<Tablas.Personal>("SELECT * FROM Personal where DNI = ? ", numerodocumento);
        }
        public static IEnumerable<TareoPersonalS10> ListarTareoPorSincronizar(SQLiteConnection db)
        {
            return db.Query<TareoPersonalS10>("Select * From TareoPersonalS10 Where SINCRONIZADO = 0 And ID_TAREADOR = ?", App.Tareador);
        }
        private async void Btn_SincroPersoDisponible(object sender, EventArgs e)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    LstPersonalTareo = new List<PersonalTareoApi>();
                    var t = Task.Run(async () => LstPersonalTareo = await HaugApi.Metodo.GetAllPersonalTareadorAsync(App.Tareador));

                    t.Wait();

                    float contador = 0;
                    float contador_s = 0;
                    float cn = LstPersonalTareo.Count();

                    using (var dialog = UserDialogs.Instance.Progress("Procesando..."))
                    {
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
                                    FECHA_SINCRONIZADO = DateTime.Now,
                                    ID_HORARIO = itemPersonalTareoApi.ID_HORARIO.ToString()
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

                                if (contador_s <= cn)
                                {
                                    await Task.Delay(1);
                                    contador = (contador_s / cn) * 100;
                                    dialog.PercentComplete = Convert.ToInt32(contador);
                                    contador_s = contador_s + 1;
                                }
                            }
                        }
                    }

                }
                else
                {
                    await DisplayAlert("Haug Tareo", "Verifique su conexion a internet", "Ok");
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
                    int contador = 0;

                    using (var dialog = UserDialogs.Instance.Progress("Procesando"))
                    {
                        LstTareoPersonal = new List<TareoPersonalS10Api>();

                        var db = new SQLiteConnection(App.FilePath);
                        IEnumerable<TareoPersonalS10> resultado = ListarTareoPorSincronizar(db);

                        foreach (TareoPersonalS10 TareoPersonalApiItem in resultado)
                        {
                            var t = Task.Run(async () => await HaugApi.Metodo.PostJsonHttpClient(
                            TareoPersonalApiItem.ID_TAREADOR, TareoPersonalApiItem.PROYECTO, TareoPersonalApiItem.CODOBRERO,
                            TareoPersonalApiItem.PERSONAL, TareoPersonalApiItem.DNI, Convert.ToString(TareoPersonalApiItem.TIPO_MARCACION),
                            TareoPersonalApiItem.FECHA_TAREO, TareoPersonalApiItem.HORA, TareoPersonalApiItem.FECHA_REGISTRO, TareoPersonalApiItem.NroEsquemaPlanilla,
                            TareoPersonalApiItem.CodInsumo, TareoPersonalApiItem.Insumo, TareoPersonalApiItem.CodOcupacion, TareoPersonalApiItem.Ocupacion
                            ));

                            UpdTareo(TareoPersonalApiItem.ID);

                            contador += contador;
                            dialog.PercentComplete = (contador / resultado.Count()) * 100;
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
        public static void UpdTareo(int id)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                {
                    var tareo = conn.Table<TareoPersonalS10>().FirstOrDefault(j => j.ID == id);

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
                                    //BtnSincroAltaUsuario.IsEnabled = true;
                                    //BtnSincroPersoDisponible.IsEnabled = true;
                                    //BtnSincronizTareoPersonal.IsEnabled = true;
                                    //BtnSincronizHorario.IsEnabled = true;
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
        private async void Btn_SincronizHorario(object sender, EventArgs e)
        {
            try
            {                
                if (CrossConnectivity.Current.IsConnected)
                {
                    LstHorario = new List<HorarioApi>();
                    var t = Task.Run(async () => LstHorario = await HaugApi.Metodo.GetAllHorarioApiAsync());                    

                    t.Wait();

                    float contador = 0;
                    float contador_s = 0;
                    float cn = LstHorario.Count();

                    using (var dialog = UserDialogs.Instance.Progress("Procesando..."))
                    {
                        using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                        {
                            if (DoesTableExist(conn, "Horario"))
                                conn.DropTable<Horario>();

                            conn.CreateTable<Horario>();
                            foreach (HorarioApi itemHorarioApi in LstHorario)
                            {
                                var DatosRegistro = new Horario
                                {
                                    ID_HORARIO = itemHorarioApi.ID_HORARIO.ToString(),
                                    DESCRIPCION = itemHorarioApi.DESCRIPCION.ToString(),
                                    COLOQUIAL = itemHorarioApi.COLOQUIAL.ToString(),
                                    HORA_INICIO = itemHorarioApi.HORA_INICIO.ToString(),
                                    HORA_FIN = itemHorarioApi.HORA_FIN.ToString(),
                                    TOLERA_ING = int.Parse(itemHorarioApi.TOLERA_ING.ToString()),
                                    TOLERA_SAL = int.Parse(itemHorarioApi.TOLERA_SAL.ToString()),
                                    ESTADO = bool.Parse(itemHorarioApi.ESTADO.ToString()),
                                    ID_USU_REG = int.Parse(itemHorarioApi.ID_USU_REG.ToString()),
                                    ID_USUARIO_MOD = int.Parse(itemHorarioApi.ID_USUARIO_MOD.ToString())
                                };

                                var db = new SQLiteConnection(App.FilePath);
                                IEnumerable<Horario> resultado = ValidarExitenciaHorario(db, itemHorarioApi.ID_HORARIO.ToString());
                                if (resultado.Count() > 0)
                                {
                                    conn.Update(DatosRegistro);
                                }
                                else
                                {
                                    conn.Insert(DatosRegistro);
                                }

                                if (contador_s <= cn)
                                {
                                    await Task.Delay(1);
                                    contador = (contador_s / cn) * 100;
                                    dialog.PercentComplete = Convert.ToInt32(contador);
                                    contador_s = contador_s + 1;
                                }
                            }
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Horarios", "Verifique su conexion a internet", "Ok");
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Horarios", ex.InnerException.ToString(), "Ok");
            }
        }
        public static IEnumerable<Horario> ValidarExitenciaHorario(SQLiteConnection db, string id)
        {
            return db.Query<Horario>("SELECT * FROM Horario where ID_HORARIO = ? ", id);
        }

        private bool DoesTableExist(SQLiteConnection db, string name)
        {
            SQLiteCommand command = db.CreateCommand("SELECT COUNT(1) FROM SQLITE_MASTER WHERE TYPE = @TYPE AND NAME = @NAME");
            command.Bind("@TYPE", "table");
            command.Bind("@NAME", name);

            int result = command.ExecuteScalar<int>();
            return (result > 0);
        }

        private async void BtnSincroPersonalS10_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    LstPersonalS10 = new List<PersonalS10Api>();
                    var t = Task.Run(async () => LstPersonalS10 = await HaugApi.Metodo.GetAllPersonalS10TareadorAsync(App.Tareador));

                    t.Wait();

                    int contador = 0;
                    //float contador_s = 0;
                    int cn = LstPersonalS10.Count();

                    using (var dialog = UserDialogs.Instance.Progress("Procesando..."))
                    {
                        using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                        {
                            conn.CreateTable<Tablas.Personal>();
                            foreach (PersonalS10Api itemPersonalS10Api in LstPersonalS10)
                            {
                                var DatosRegistro = new Tablas.Personal
                                {
                                    CodObrero = itemPersonalS10Api.CodObrero.ToString(),
                                    Descripcion = itemPersonalS10Api.Descripcion.ToString(),
                                    DNI = itemPersonalS10Api.DNI.ToString(),
                                    NroEsquemaPlanilla = itemPersonalS10Api.NroEsquemaPlanilla,
                                    CodProyecto = itemPersonalS10Api.CodProyecto.ToString(),
                                    CodIdentificador = itemPersonalS10Api.CodIdentificador.ToString(),
                                    Activo = itemPersonalS10Api.Activo,
                                    CodInsumo = itemPersonalS10Api.CodInsumo.ToString(),
                                    Insumo = itemPersonalS10Api.Insumo.ToString(),
                                    CodOcupacion = itemPersonalS10Api.CodOcupacion.ToString(),
                                    Ocupacion = itemPersonalS10Api.Ocupacion.ToString()
                                };

                                var db = new SQLiteConnection(App.FilePath);
                                IEnumerable<Tablas.Personal> resultado = ValidarExitenciaS10(db, itemPersonalS10Api.DNI.ToString());
                                if (resultado.Count() > 0)
                                {
                                    conn.Update(DatosRegistro);
                                }
                                else
                                {
                                    conn.Insert(DatosRegistro);
                                }
                                await Task.Delay(1);
                                contador += contador;
                                dialog.PercentComplete = (contador / cn) * 100;
                            }
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Haug Tareo", "Verifique su conexion a internet", "Ok");
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}