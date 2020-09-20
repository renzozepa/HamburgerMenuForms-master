using System;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

using HamburgerMenu.Helpers;

using SQLite;
using System.Linq;
using HamburgerMenu.Tablas;
using Xamarin.Forms;

namespace HamburgerMenu.ViewModels
{
    public class MarcacionZebraViewModel : BaseViewModel
    {
        public INavigation Navigation { get; set; }

        private string marcado;
        private string horaMarcado;
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
        public MarcacionZebraViewModel(INavigation navigation,Entry entry)
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
                            InsertarTareo(nombre_personal, personal, proyecto, Convert.ToInt32(id_situacion), clase);                        
                            HoraMarcado = DateTime.Now.ToString();
                            Marcado = string.Empty;
                        }
                        else
                        {                                                 
                            HoraMarcado = DateTime.Now.ToString();
                            Marcado = string.Empty;
                            Application.Current.MainPage.DisplayAlert("Ayuda", "El trabajador no esta permitido para laborar, su situacion es : " + situacion, "OK");
                        }
                    }
                    else
                    {
                        HoraMarcado = DateTime.Now.ToString();
                        Marcado = string.Empty;
                        Application.Current.MainPage.DisplayAlert("Ayuda", "El trabajador no se encuentra registrado", "OK");
                    }
                }
            }
        }
        public static IEnumerable<PersonalTareo> BuscarTrabajador(SQLiteConnection db, string documento)
        {
            return db.Query<PersonalTareo>("Select * From PersonalTareo where NUMERO_DOCUIDEN = ? and ID_TAREADOR = ? ", documento, App.Tareador);
        }
        public static void InsertarTareo(string nombre_personal, Int32 ID_PERSONAL, string ID_PROYECTO, Int32 ID_SITUACION, Int32 ID_CLASE_TRABAJADOR)
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
