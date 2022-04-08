using Acr.UserDialogs;
using HamburgerMenu.Models;
using HamburgerMenu.ServicioApi;
using HamburgerMenu.Tablas;
using HamburgerMenu.Vistas;
using Plugin.Connectivity;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HamburgerMenu.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public INavigation Navigation { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand RegristrarCommand { get; set; }
        public ICommand UsuariosCommand { get; set; }

        private string _usuario;
        public string Usuario
        {
            get { return _usuario; }
            set { _usuario = value; OnPropertyChanged(); }
        }
        private string _clave;
        public string Clave
        {
            get { return _clave; }
            set { _clave = value; OnPropertyChanged(); }
        }
        public static List<SucursalApi> LstSucursal { get; set; }
        private string _selectedIndexSucursal;
        public string SelectedIndexSucursal
        {
            get { return _selectedIndexSucursal; }
            set
            {
                SetProperty(ref _selectedIndexSucursal, value);
                SelectedDescripcionSucursal = _lstSucursaldisponible[Convert.ToInt32(_selectedIndexSucursal)].ID_SUCURSAL.ToString();
                App.Sucursal = SelectedDescripcionSucursal;
                OnPropertyChanged();
            }
        }
        private string _selectedDescripcionSucursal;
        public string SelectedDescripcionSucursal
        {
            get { return _selectedDescripcionSucursal; }
            set
            {
                SetProperty(ref _selectedDescripcionSucursal, value);
                OnPropertyChanged();
            }
        }
        public List<Sucursal> _lstSucursaldisponible = new List<Sucursal>();
        public List<Sucursal> LstSucursaldisponible
        {
            get { return _lstSucursaldisponible; }
            set
            {
                if (_lstSucursaldisponible != value)
                {
                    _lstSucursaldisponible = value;
                    OnPropertyChanged();
                }
            }
        }
        public void MostrarSucursal()
        {
            string filename = "Tareo.db3";
            string foldername = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string CompletePath = Path.Combine(foldername, filename);

            //var db = new SQLiteConnection(App.FilePath);
            var db = new SQLiteConnection(CompletePath);
            var RSucursal = db.Query<Sucursal>("SELECT * FROM Sucursal");
            if (RSucursal.Count > 0)
                LstSucursaldisponible = RSucursal;
        }
        public LoginViewModel(INavigation navigation)
        {
            Navigation = navigation;
            LoginCommand = new Command(async () => await ValidarUsuario());
            RegristrarCommand = new Command(async () => await Registrarme());
            UsuariosCommand = new Command(async () => await ListarUsuarios());

            //LlenarSucursal();
        }
        async Task ValidarUsuario()
        {
            try
            {
                var db = new SQLiteConnection(App.FilePath);
                IEnumerable<LoginLocal> resultado = ConsultarUsuario(db, Usuario, Clave);
                if (resultado.Count() > 0)
                {
                    List<LoginLocal> listll = (List<LoginLocal>)resultado;
                    foreach (LoginLocal itemLoginLocal in listll)
                    {
                        App.Tareador = itemLoginLocal.TAREADOR.ToString();
                        App.Usuario = Convert.ToInt32(itemLoginLocal.ID.ToString());
                        if (!string.IsNullOrEmpty(itemLoginLocal.TOKEN))
                        {
                            App.Token = itemLoginLocal.TOKEN.ToString();
                            App.FExpiracion = itemLoginLocal.FECHA_VIGENCIA.Date;
                        }

                        if (!string.IsNullOrEmpty(itemLoginLocal.CELULAR))
                        {
                            App.Celular = itemLoginLocal.CELULAR.ToString();
                        }
                    }
                    
                    Limpiar();
                    await App.Current.MainPage.Navigation.PushAsync(new HamburgerMenu());
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Inicio sesión", "Verifique su usuario/contraseña", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Error : " + ex.Message.ToString(), "Aceptar");
            }
        }
        async Task Registrarme()
        {
            await App.Current.MainPage.Navigation.PushAsync(new RegistroUsuario());
        }
        async Task ListarUsuarios()
        {
            await App.Current.MainPage.Navigation.PushAsync(new ConsultaRegistro());
        }
        public IEnumerable<LoginLocal> ConsultarUsuario(SQLiteConnection db, string usuario, string contra)
        {
            db.CreateTable<LoginLocal>();

            if (DoesTableExist(db, "ConfiguracionLocal"))
                db.DropTable<ConfiguracionLocal>();

            db.CreateTable<Tablas.ConfiguracionLocal>();
            db.CreateTable<Tablas.PersonalTareo>();
            db.CreateTable<Tablas.TareoPersonal>();
            db.CreateTable<Tablas.TareoPersonalS10>();
            db.CreateTable<Tablas.Horario>();
            db.CreateTable<Tablas.Sucursal>();
            db.CreateTable<Tablas.Personal>();

            return db.Query<LoginLocal>("Select * From LoginLocal where USUARIO = ? and CONTRASENIA = ?", usuario, contra);
        }
        void Limpiar()
        {
            Usuario = string.Empty;
            Clave = string.Empty;
        }
        public async void LlenarSucursal()
        {
            string filename = "Tareo.db3";
            string foldername = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string CompletePath = Path.Combine(foldername, filename);

            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    LstSucursal = new List<SucursalApi>();
                    var t = Task.Run(async () => LstSucursal = await HaugApi.Metodo.GetAllSucursalApiAsync());

                    t.Wait();

                    float contador = 0;
                    float contador_s = 0;
                    float cn = LstSucursal.Count();

                    using (var dialog = UserDialogs.Instance.Progress("Procesando..."))
                    {
                        //using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                        using (SQLiteConnection conn = new SQLiteConnection(CompletePath))
                        {
                            if (DoesTableExist(conn, "Sucursal"))
                                conn.DropTable<Sucursal>();

                            conn.CreateTable<Sucursal>();
                            foreach (SucursalApi itemSucursalApi in LstSucursal)
                            {
                                var DatosRegistro = new Sucursal
                                {
                                    ID_SUCURSAL = itemSucursalApi.ID_SUCURSAL.ToString(),
                                    DESCRIPCION = itemSucursalApi.DESCRIPCION.ToString(),
                                    RUC = itemSucursalApi.RUC.ToString(),
                                    ESTADO = bool.Parse(itemSucursalApi.ESTADO.ToString()),
                                    ID_USU_REG = int.Parse(itemSucursalApi.ID_USU_REG.ToString()),
                                    ID_USUARIO_MOD = int.Parse(itemSucursalApi.ID_USUARIO_MOD.ToString())
                                };

                                //var db = new SQLiteConnection(App.FilePath);
                                var db = new SQLiteConnection(CompletePath);
                                IEnumerable<Sucursal> resultado = ValidarExitenciaSucursal(db, itemSucursalApi.ID_SUCURSAL.ToString());
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
                    await App.Current.MainPage.DisplayAlert("Sucursal", "Verifique su conexion a internet", "Ok");
                }

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Sucursal", ex.InnerException.ToString(), "Ok");
            }
            finally
            {
                MostrarSucursal();
            }
        }
        private bool DoesTableExist(SQLiteConnection db, string name)
        {
            SQLiteCommand command = db.CreateCommand("SELECT COUNT(1) FROM SQLITE_MASTER WHERE TYPE = @TYPE AND NAME = @NAME");
            command.Bind("@TYPE", "table");
            command.Bind("@NAME", name);

            int result = command.ExecuteScalar<int>();
            return (result > 0);
        }
        public static IEnumerable<Sucursal> ValidarExitenciaSucursal(SQLiteConnection db, string id)
        {
            return db.Query<Sucursal>("SELECT * FROM Sucursal where ID_SUCURSAL = ? ", id);
        }

    }
}
