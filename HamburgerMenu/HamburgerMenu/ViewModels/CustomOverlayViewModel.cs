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

            bool Server = false;
            bool LocalServer = false;
            bool RegistrarTrabajadorEstado = false;

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
                            IEnumerable<Tablas.Personal> resultado = SELECT_WHERE(db, result.Text);
                            if (resultado.Count() > 0)
                            {
                                List<Tablas.Personal> listll = (List<Tablas.Personal>)resultado;

                                string CodObrero = string.Empty;
                                string Empleado = string.Empty;
                                bool Activo = false;
                                Guid NroEsquemaPlanilla = Guid.Empty;
                                string CodInsumo = string.Empty;
                                string Insumo = string.Empty;
                                string CodOcupacion = string.Empty;
                                string Ocupacion = string.Empty;
                                string proy = string.Empty;
                                string multi_proyecto = string.Empty;

                                foreach (Tablas.Personal itemPersonalTareo in listll)
                                {
                                    CodObrero = itemPersonalTareo.CodObrero.ToString();
                                    Empleado = itemPersonalTareo.Descripcion.ToString();
                                    Activo = itemPersonalTareo.Activo;
                                    NroEsquemaPlanilla = itemPersonalTareo.NroEsquemaPlanilla;
                                    CodInsumo = itemPersonalTareo.CodInsumo.ToString();
                                    Insumo = itemPersonalTareo.Insumo.ToString();
                                    CodOcupacion = itemPersonalTareo.CodOcupacion.ToString();
                                    Ocupacion = itemPersonalTareo.Ocupacion.ToString();
                                    proy = itemPersonalTareo.CodProyectoNoProd.ToString();

                                    if (App.Multi_Proyecto == "1")
                                        multi_proyecto = App.Proyecto;
                                    else
                                        multi_proyecto = proy;
                                }                                

                                if (Activo == true)
                                {
                                    IEnumerable<ConfiguracionLocal> RptConfiguracion = BuscarConfiguracionLocal(db);
                                    if (RptConfiguracion.Count() > 0)
                                    {
                                        List<ConfiguracionLocal> ListRptConfigLocal = (List<ConfiguracionLocal>)RptConfiguracion;

                                        foreach (ConfiguracionLocal itemConfiguracionLocal in ListRptConfigLocal)
                                        {
                                            Server = itemConfiguracionLocal.SERVER;
                                            LocalServer = itemConfiguracionLocal.LOCALSERVER;
                                            RegistrarTrabajadorEstado = itemConfiguracionLocal.REGMARCACIONESTADO;
                                        }
                                    }

                                    IEnumerable<TareoPersonalS10> RptValidacion = ValidarExistenciaTareoTrabajador(db, Convert.ToString(result.Text));
                                    if (RptValidacion.Count() > 0)
                                    {
                                        await Application.Current.MainPage.DisplayAlert("Validación", "Ya existe el tipo de marcación que desea registrar", "OK");
                                        BarcodeText = result.Text;
                                        BarcodeFormat = BarcodeFormatConverter.ConvertEnumToString(result.BarcodeFormat);
                                        HoraMarcado = DateTime.Now.ToString();
                                    }
                                    else
                                    {
                                        InsertarTareo(CodObrero, Empleado, result.Text, NroEsquemaPlanilla, CodInsumo, Insumo, CodOcupacion, Ocupacion, multi_proyecto);

                                        if (Server || LocalServer)
                                        {
                                            InsertarTareoServer();
                                        }

                                        await Navigation.PopAsync();
                                        BarcodeText = result.Text;
                                        BarcodeFormat = BarcodeFormatConverter.ConvertEnumToString(result.BarcodeFormat);
                                        HoraMarcado = DateTime.Now.ToString();
                                    }

                                }
                                else
                                {
                                    await Application.Current.MainPage.DisplayAlert("Validación", "Trabajador no existe o no esta activo para tareo.", "OK");
                                    HoraMarcado = DateTime.Now.ToString();
                                }
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert("Validación", "Trabajador no existe o no esta activo para tareo.", "OK");
                                HoraMarcado = DateTime.Now.ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            await page.DisplayAlert("Error", ex.Message.ToString(), "Aceptar");
                        }
                    }
                });
            };
        }

        public static IEnumerable<Tablas.Personal> SELECT_WHERE(SQLiteConnection db, string numero)
        {
            db.CreateTable<Tablas.Personal>();
            return db.Query<Tablas.Personal>("Select * From Personal where DNI = ? and CodIdentificador = ? and Activo = true ", numero, App.Tareador);
        }
        public static void InsertarTareo(string codobrero, string empleado, string dni, Guid esquemaplanilla, string codinsumo, string insumo, string codocupacion, string ocupacion, string proyecto)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                {
                    conn.CreateTable<TareoPersonalS10>();
                    var DatosRegistro = new TareoPersonalS10
                    {
                        ID_TAREADOR = App.Tareador,
                        PROYECTO = proyecto,
                        CODOBRERO = codobrero,
                        PERSONAL = empleado,
                        DNI = dni,
                        TIPO_MARCACION = App.TipoMarcacion,
                        FECHA_TAREO = App.FMarcacion,
                        HORA = DateTime.Now.ToString("HH:mm"),
                        FECHA_REGISTRO = DateTime.Now,
                        SINCRONIZADO = 0,
                        TOKEN = App.Token,
                        ID_SUCURSAL = App.Sucursal,
                        NroEsquemaPlanilla = esquemaplanilla,
                        CodInsumo = codinsumo,
                        Insumo = insumo,
                        CodOcupacion = codocupacion,
                        Ocupacion = ocupacion
                    };
                    conn.Insert(DatosRegistro);
                }
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error : Insertar Local", ex.Message.ToString(), "OK");
            }
        }
        public static IEnumerable<ConfiguracionLocal> BuscarConfiguracionLocal(SQLiteConnection db)
        {
            return db.Query<ConfiguracionLocal>("Select * From ConfiguracionLocal where ID_USUARIO = ? ", App.Usuario);
        }
        public static IEnumerable<TareoPersonalS10> ListarTareoPorSincronizar(SQLiteConnection db)
        {
            db.CreateTable<TareoPersonalS10>();
            return db.Query<TareoPersonalS10>("Select * From TareoPersonalS10 Where SINCRONIZADO = 0 And ID_TAREADOR = ?", App.Tareador);
        }
        private static void InsertarTareoServer()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    LstTareoPersonal = new List<TareoPersonalApi>();

                    var db = new SQLiteConnection(App.FilePath);
                    IEnumerable<TareoPersonalS10> resultado = ListarTareoPorSincronizar(db);

                    foreach (TareoPersonalS10 TareoPersonalApiItem in resultado)
                    {
                        var t = Task.Run(async () => await HaugApi.Metodo.PostJsonHttpClient(
                            TareoPersonalApiItem.ID_TAREADOR, TareoPersonalApiItem.PROYECTO, TareoPersonalApiItem.CODOBRERO,
                            TareoPersonalApiItem.PERSONAL, TareoPersonalApiItem.DNI, Convert.ToString(TareoPersonalApiItem.TIPO_MARCACION),
                            TareoPersonalApiItem.FECHA_TAREO, TareoPersonalApiItem.HORA,TareoPersonalApiItem.FECHA_REGISTRO, TareoPersonalApiItem.NroEsquemaPlanilla,
                            TareoPersonalApiItem.CodInsumo, TareoPersonalApiItem.Insumo, TareoPersonalApiItem.CodOcupacion, TareoPersonalApiItem.Ocupacion
                            ));
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
                Application.Current.MainPage.DisplayAlert("Haug Tareo : Error", "Verifique su conexion a internet", "Ok");
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
        public static IEnumerable<TareoPersonalS10> ValidarExistenciaTareoTrabajador(SQLiteConnection db, string documento)
        {
            db.CreateTable<TareoPersonalS10>();
            return db.Query<TareoPersonalS10>("Select * From TareoPersonalS10 where DNI = ? and FECHA_TAREO = ? and TIPO_MARCACION = ? ", documento, App.FMarcacion, App.TipoMarcacion);
        }
    }
}
