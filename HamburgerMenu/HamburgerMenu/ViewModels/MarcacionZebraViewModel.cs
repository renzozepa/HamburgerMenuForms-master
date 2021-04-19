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

namespace HamburgerMenu.ViewModels
{
    public class MarcacionZebraViewModel : BaseViewModel
    {
        public INavigation Navigation { get; set; }

        private string marcado;
        private string horaMarcado;

        private string ultimomarcado;
        private string ultimohoraMarcado;

        private bool resultConsulta = false;

        public static List<TareoPersonalApi> LstTareoPersonal { get; set; }

        public string HoraMarcado
        {
            get => horaMarcado;
            set { horaMarcado = value; OnPropertyChanged(); }
        }
        public string Marcado
        {
            get => marcado;
            set { marcado = value; OnPropertyChanged(); }
        }
        public string UltimoHoraMarcado
        {
            get => ultimohoraMarcado;
            set { ultimohoraMarcado = value; OnPropertyChanged(); }
        }
        public string UltimoMarcado
        {
            get => ultimomarcado;
            set { ultimomarcado = value; OnPropertyChanged(); }
        }
        public MarcacionZebraViewModel(INavigation navigation, Entry entry)
        {
            Navigation = navigation;
            entry.TextChanged += Entry_TextChanged;
            horaMarcado = DateTime.Now.ToString();
        }
        void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            HoraMarcado = DateTime.Now.ToString();
            string CodDocumento = e.NewTextValue.ToString();

            if (!string.IsNullOrWhiteSpace(CodDocumento))
            {
                if (e.ToString().Length >= 8)
                {
                    var db = new SQLiteConnection(App.FilePath);
                    IEnumerable<PersonalTareo> resultado = BuscarTrabajador(db, CodDocumento);
                    if (resultado.Count() > 0)
                    {
                        List<PersonalTareo> listll = (List<PersonalTareo>)resultado;
                        string id_situacion = string.Empty;
                        string situacion = string.Empty;
                        int personal = 0;
                        string nombre_personal = string.Empty;
                        string proyecto = string.Empty;
                        int clase = 0;

                        foreach (PersonalTareo itemPersonalTareo in listll)
                        {
                            id_situacion = itemPersonalTareo.ID_SITUACION.ToString();
                            situacion = itemPersonalTareo.SITUACION.ToString();
                            personal = int.Parse(itemPersonalTareo.ID_PERSONAL.ToString());
                            nombre_personal = itemPersonalTareo.NOMBRE.ToString();
                            proyecto = itemPersonalTareo.ID_PROYECTO.ToString();
                            clase = int.Parse(itemPersonalTareo.ID_CLASE_TRABAJADOR.ToString());
                        }

                        if (id_situacion == "10")
                        {
                            IEnumerable<TareoPersonal> RptValidacion = ValidarExistenciaTareoTrabajador(db, Convert.ToString(personal));
                            if (RptValidacion.Count() > 0)
                            {
                                Application.Current.MainPage.DisplayAlert("Validación", "Ya existe el tipo de marcación que desea registrar", "OK");
                                HoraMarcado = DateTime.Now.ToString();
                                Marcado = string.Empty;
                            }
                            else
                            {
                                InsertarTareo(nombre_personal, personal, proyecto, Convert.ToInt32(id_situacion), clase, CodDocumento);

                                IEnumerable<ConfiguracionLocal> RptConfiguracion = BuscarConfiguracionLocal(db);
                                if (RptConfiguracion.Count() > 0)
                                {
                                    List<ConfiguracionLocal> ListRptConfigLocal = (List<ConfiguracionLocal>)RptConfiguracion;

                                    bool Server = false;
                                    bool LocalServer = false;

                                    foreach (ConfiguracionLocal itemConfiguracionLocal in ListRptConfigLocal)
                                    {
                                        Server = itemConfiguracionLocal.SERVER;
                                        LocalServer = itemConfiguracionLocal.LOCALSERVER;
                                    }

                                    if (Server || LocalServer)
                                    {
                                        InsertarTareoServer();
                                    }

                                }

                                UltimoHoraMarcado = DateTime.Now.ToString();
                                UltimoMarcado = CodDocumento;
                                HoraMarcado = DateTime.Now.ToString();
                                Marcado = string.Empty;
                            }
                        }
                        else
                        {
                            resultConsulta = Application.Current.MainPage.DisplayAlert("Ayuda", "El trabajador no esta permitido para laborar, su situacion es: " + situacion + "\n Desea Registrar tareo al trabajador?", "Si", "No").IsCompleted;

                            if (resultConsulta)
                            {
                                Insert_Trabajador_Sin_Registro(CodDocumento);
                            }
                            else {
                                HoraMarcado = DateTime.Now.ToString();
                                Marcado = string.Empty;
                                Application.Current.MainPage.DisplayAlert("Ayuda", "Cancelado por el usuario", "OK");
                            }
                            
                        }
                    }
                    else
                    {
                        resultConsulta = Application.Current.MainPage.DisplayAlert("Ayuda", "Desea Registrar tareo al trabajador NO REGISTADO", "Si", "No").IsCompleted;

                        if (resultConsulta)
                        {
                            Insert_Trabajador_Sin_Registro(CodDocumento);
                        }
                        else {
                            HoraMarcado = DateTime.Now.ToString();
                            Marcado = string.Empty;
                            Application.Current.MainPage.DisplayAlert("Ayuda", "El trabajador no se encuentra registrado", "OK");
                        }
                        
                    }
                }
            }
        }

        public static IEnumerable<PersonalTareo> BuscarTrabajador(SQLiteConnection db, string documento)
        {
            return db.Query<PersonalTareo>("Select * From PersonalTareo where NUMERO_DOCUIDEN = ? and ID_TAREADOR = ? ", documento, App.Tareador);
        }
        public static IEnumerable<TareoPersonal> ValidarExistenciaTareoTrabajador(SQLiteConnection db, string documento)
        {
            return db.Query<TareoPersonal>("Select * From TareoPersonal where ID_PERSONAL = ? and FECHA_TAREO = ? and TIPO_MARCACION = ? ", documento, App.FMarcacion, App.TipoMarcacion);
        }
        public static void InsertarTareo(string nombre_personal, int ID_PERSONAL, string ID_PROYECTO, int ID_SITUACION, int ID_CLASE_TRABAJADOR, string dni)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                {
                    conn.CreateTable<TareoPersonal>();
                    var DatosRegistro = new TareoPersonal
                    {
                        ID_TAREADOR = App.Tareador,
                        ID_PERSONAL = ID_PERSONAL,
                        PERSONAL = nombre_personal,
                        ID_PROYECTO = ID_PROYECTO,
                        ID_SITUACION = ID_SITUACION,
                        ID_CLASE_TRABAJADOR = ID_CLASE_TRABAJADOR,
                        FECHA_TAREO = App.FMarcacion,
                        TIPO_MARCACION = App.TipoMarcacion,
                        HORA = DateTime.Now.ToString("HH:mm"),
                        FECHA_REGISTRO = DateTime.Now,
                        SINCRONIZADO = 0,
                        TOKEN = App.Token,
                        NUMERO_DOCUIDEN = dni
                    };
                    conn.Insert(DatosRegistro);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public static void Insert_Trabajador_Sin_Registro(string numero_documento)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                {
                    conn.CreateTable<PersonalTareo>();
                    var DatosRegistro = new PersonalTareo
                    {
                        ID_PERSONAL = 0,
                        NOMBRE = "Nuevo Trabajador",
                        ID_TIPODOCUIDEN = "01",
                        TIPODOCUIDEN = "01",
                        NUMERO_DOCUIDEN = numero_documento,
                        ID_SITUACION = 1,
                        SITUACION = "1",
                        ID_PROYECTO = "--",
                        PROYECTO = "--",
                        ID_TAREADOR = App.Tareador,
                        TAREADOR = App.Tareador,
                        ID_CLASE_TRABAJADOR = 0,
                        CLASE_TRABAJADOR = "0",
                        ID_USUARIO_SINCRONIZA = 0,
                        FECHA_SINCRONIZADO = DateTime.Now
                    };
                    conn.Insert(DatosRegistro);
                    InsertarTareo("", 0, "", 0, 0, numero_documento);

                    var db = new SQLiteConnection(App.FilePath);
                    IEnumerable<ConfiguracionLocal> RptConfiguracion = BuscarConfiguracionLocal(db);
                    if (RptConfiguracion.Count() > 0)
                    {
                        List<ConfiguracionLocal> ListRptConfigLocal = (List<ConfiguracionLocal>)RptConfiguracion;

                        bool Server = false;
                        bool LocalServer = false;

                        foreach (ConfiguracionLocal itemConfiguracionLocal in ListRptConfigLocal)
                        {
                            Server = itemConfiguracionLocal.SERVER;
                            LocalServer = itemConfiguracionLocal.LOCALSERVER;
                        }

                        if (Server || LocalServer)
                        {
                            InsertarTareoServer();
                        }

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static IEnumerable<ConfiguracionLocal> BuscarConfiguracionLocal(SQLiteConnection db)
        {
            return db.Query<ConfiguracionLocal>("Select * From ConfiguracionLocal where ID_USUARIO = ? ", App.Usuario);
        }

        public static IEnumerable<TareoPersonal> ListarTareoPorSincronizar(SQLiteConnection db)
        {
            return db.Query<TareoPersonal>("Select * From TareoPersonal Where SINCRONIZADO = 0 And ID_TAREADOR = ?", App.Tareador);
        }
        private static void InsertarTareoServer()
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
                            TareoPersonalApiItem.FECHA_REGISTRO, TareoPersonalApiItem.NUMERO_DOCUIDEN));
                        UpdTareo(TareoPersonalApiItem.ID);
                    }
                }
                else
                {
                    Application.Current.MainPage.DisplayAlert("Haug Tareo", "Verifique su conexion a internet", "Ok");
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
    }
}
