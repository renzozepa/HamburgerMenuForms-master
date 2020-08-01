using System;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

using HamburgerMenu.Helpers;
using ZXing;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

using Xamarin.Forms;

using SQLite;
using System.Linq;
using HamburgerMenu.Tablas;

namespace HamburgerMenu.ViewModels
{
    public class CustomOverlayViewModel : BaseViewModel
    {
        public INavigation Navigation { get; set; }

        public ICommand ScannerCommand { get; set; }

        private string barcodeText;
        private string horaMarcado;

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
                        await page.DisplayAlert("Ayuda", "Coloca el código frente al dispositivo para escanearlo", "OK");
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
                                    INSERT_TAREO(nombre_personal, personal, proyecto, Convert.ToInt32(id_situacion), clase);
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
                                    await page.DisplayAlert("Ayuda", "El trabajador no esta permitido para laborar, su situacion es : " + situacion, "OK");
                                }
                            }
                            else
                            {
                                await Navigation.PopAsync();
                                BarcodeText = result.Text;
                                BarcodeFormat = BarcodeFormatConverter.ConvertEnumToString(result.BarcodeFormat);
                                HoraMarcado = DateTime.Now.ToString();
                                await page.DisplayAlert("Ayuda", "El trabajador no se encuentra registrado", "OK");
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

        public static void INSERT_TAREO(string nombre_personal, Int32 ID_PERSONAL, string ID_PROYECTO, Int32 ID_SITUACION, Int32 ID_CLASE_TRABAJADOR)
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
                        TOKEN = App.Token
                    };
                    conn.Insert(DatosRegistro);                    
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
