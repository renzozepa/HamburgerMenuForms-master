using HamburgerMenu.Helpers;
using HamburgerMenu.Models;
using HamburgerMenu.ServicioApi;
using HamburgerMenu.Tablas;
using Plugin.Connectivity;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using ZXing;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace HamburgerMenu.ViewModels
{
    public class CustomOverlayViewModel : BaseViewModel
    {
        public INavigation Navigation { get; set; }
        public ICommand ScannerCommand { get; set; }
        private string barcodeText;
        private string horaMarcado;
        public static List<TareoPersonalApi> LstTareoPersonal { get; set; }

        public string BarcodeText
        {
            get => barcodeText;
            set { barcodeText = value; OnPropertyChanged(); }
        }

        public string HoraMarcado
        {
            get => horaMarcado;
            set { horaMarcado = value; OnPropertyChanged(); }
        }

        private BarcodeFormat _barcodeFormat;

        public string BarcodeFormat
        {
            get => BarcodeFormatConverter.ConvertEnumToString(_barcodeFormat);
            set
            {
                _barcodeFormat = (value != null)
                    ? BarcodeFormatConverter.ConvertStringToEnum(value)
                    : ZXing.BarcodeFormat.QR_CODE;
                OnPropertyChanged();
            }
        }

        public CustomOverlayViewModel(INavigation navigation)
        {
            Navigation = navigation;
            ScannerCommand = new Command(async () => await ScanCode());
            barcodeText = "N/A";
            horaMarcado = DateTime.Now.ToString();
        }

        async Task ScanCode()
        {
            bool resultConsulta = false;

            var options = new MobileBarcodeScanningOptions
            {
                PossibleFormats = new List<BarcodeFormat>()
                {
                    ZXing.BarcodeFormat.EAN_8,
                    ZXing.BarcodeFormat.EAN_13,
                    ZXing.BarcodeFormat.AZTEC,
                    ZXing.BarcodeFormat.QR_CODE,
                    ZXing.BarcodeFormat.CODE_39,
                    ZXing.BarcodeFormat.CODE_93,
                    ZXing.BarcodeFormat.CODE_128
                }
            };
            var overlay = new ZXingDefaultOverlay
            {
                ShowFlashButton = false,
                TopText = "Coloca el código de barras frente al dispositivo",
                BottomText = "El escaneo es automático",
                Opacity = 0.75
            };
            overlay.BindingContext = overlay;

            var page = new ZXingScannerPage(options, overlay)
            {
                Title = "Haug Tareo",
                DefaultOverlayShowFlashButton = true,
            };
            await Navigation.PushAsync(page);

            page.OnScanResult += (result) =>
            {
                page.IsScanning = false;

                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (string.IsNullOrEmpty(result.Text))
                    {
                        await Navigation.PopAsync();
                        await page.DisplayAlert("Ayuda", "Coloca el código frente al dispositivo para escanearlo", "Aceptar");
                    }
                    else
                    {
                        try
                        {
                            var db = new SQLiteConnection(App.FilePath);
                            IEnumerable<PersonalTareo> resultado = SELECT_WHERE(db, result.Text);
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
                                    INSERT_TAREO(nombre_personal, personal, proyecto, Convert.ToInt32(id_situacion), clase , result.Text);

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
                                    await Navigation.PopAsync();
                                    BarcodeText = result.Text;
                                    BarcodeFormat = BarcodeFormatConverter.ConvertEnumToString(result.BarcodeFormat);
                                    HoraMarcado = DateTime.Now.ToString();
                                }
                                else
                                {
                                    await Navigation.PopAsync();
                                    BarcodeText = result.Text;
                                    BarcodeFormat = BarcodeFormatConverter.ConvertEnumToString(result.BarcodeFormat);
                                    HoraMarcado = DateTime.Now.ToString();
                                    await page.DisplayAlert("Ayuda", "El trabajador no esta permitido para laborar, su situacion es : " + situacion, "Aceptar");
                                }
                            }
                            else
                            {
                                resultConsulta = await page.DisplayAlert("Ayuda", "Desea Registrar tareo al trabajador NO REGISTADO", "Si","No");

                                if (resultConsulta) {

                                    Insert_Trabajador_Sin_Registro(result.Text);

                                }
                                else {
                                    await Navigation.PopAsync();
                                    BarcodeText = result.Text;
                                    BarcodeFormat = BarcodeFormatConverter.ConvertEnumToString(result.BarcodeFormat);
                                    HoraMarcado = DateTime.Now.ToString();
                                    await page.DisplayAlert("Ayuda", "Registro cancelado por el usuario.", "Aceptar");
                                }


                                
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }

                    }

                });
            };
        }

        public static IEnumerable<PersonalTareo> SELECT_WHERE(SQLiteConnection db, string numero)
        {
            return db.Query<PersonalTareo>("Select * From PersonalTareo where NUMERO_DOCUIDEN = ? and ID_TAREADOR = ? ", numero, App.Tareador);
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
                    INSERT_TAREO("", 0, "", 0, 0, numero_documento);

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
        public static void INSERT_TAREO(string nombre_personal, int ID_PERSONAL, string ID_PROYECTO, int ID_SITUACION, int ID_CLASE_TRABAJADOR,string dni)
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
