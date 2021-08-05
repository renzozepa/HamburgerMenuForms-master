using HamburgerMenu.Tablas;
using SQLite;
using System;

namespace HamburgerMenu.ViewModels
{
    public class PersonalViewModel : BaseViewModel
    {
        protected int _ID_PERSONAL = 0;
        protected string _NOMBRE = string.Empty;
        protected string _ID_TIPODOCUIDEN = string.Empty;
        protected string _TIPODOCUIDEN = string.Empty;
        protected string _NUMERO_DOCUIDEN = string.Empty;
        protected int _ID_SITUACION = 0;
        protected string _SITUACION = string.Empty;
        protected string _ID_PROYECTO = string.Empty;
        protected string _PROYECTO = string.Empty;
        protected string _ID_TAREADOR = string.Empty;
        protected string _TAREADOR = string.Empty;
        protected int _ID_CLASE_TRABAJADOR = 0;
        protected string _CLASE_TRABAJADOR = string.Empty;
        protected int _ID_USUARIO_SINCRONIZA = 0;
        protected DateTime _FECHA_SINCRONIZADO;
        protected string _ID_HORARIO = string.Empty;

        [PrimaryKey]
        public int ID_PERSONAL
        {
            get => _ID_PERSONAL;
            set
            {
                _ID_PERSONAL = value;
                OnPropertyChanged();
            }
        }

        [MaxLength(80)]
        public string NOMBRE
        {
            get => _NOMBRE;
            set
            {
                _NOMBRE = value;
                OnPropertyChanged();
            }
        }

        [MaxLength(2)]
        public string ID_TIPODOCUIDEN
        {
            get => _ID_TIPODOCUIDEN;
            set
            {
                _ID_TIPODOCUIDEN = value;
                OnPropertyChanged();
            }
        }

        [MaxLength(80)]
        public string TIPODOCUIDEN
        {
            get => _TIPODOCUIDEN;
            set
            {
                _TIPODOCUIDEN = value;
                OnPropertyChanged();
            }
        }

        [MaxLength(11)]
        public string NUMERO_DOCUIDEN
        {
            get => _NUMERO_DOCUIDEN;
            set
            {
                _NUMERO_DOCUIDEN = value;
                OnPropertyChanged();
            }
        }

        public int ID_SITUACION
        {
            get => _ID_SITUACION;
            set
            {
                _ID_SITUACION = value;
                OnPropertyChanged();
            }
        }

        public string SITUACION
        {
            get => _SITUACION;
            set
            {
                _SITUACION = value;
                OnPropertyChanged();
            }
        }

        [MaxLength(11)]
        public string ID_PROYECTO
        {
            get => _ID_PROYECTO;
            set
            {
                _ID_PROYECTO = value;
                OnPropertyChanged();
            }
        }

        [MaxLength(255)]
        public string PROYECTO
        {
            get => _PROYECTO;
            set
            {
                _PROYECTO = value;
                OnPropertyChanged();
            }
        }

        [MaxLength(80)]
        public string ID_TAREADOR
        {
            get => _ID_TAREADOR;
            set
            {
                _ID_TAREADOR = value;
                OnPropertyChanged();
            }
        }

        [MaxLength(255)]
        public string TAREADOR
        {
            get => _TAREADOR;
            set
            {
                _TAREADOR = value;
                OnPropertyChanged();
            }
        }

        public int ID_CLASE_TRABAJADOR
        {
            get => _ID_CLASE_TRABAJADOR;
            set
            {
                _ID_CLASE_TRABAJADOR = value;
                OnPropertyChanged();
            }
        }

        public string CLASE_TRABAJADOR
        {
            get => _CLASE_TRABAJADOR;
            set
            {
                _CLASE_TRABAJADOR = value;
                OnPropertyChanged();
            }
        }

        public int ID_USUARIO_SINCRONIZA
        {
            get => _ID_USUARIO_SINCRONIZA;
            set
            {
                _ID_USUARIO_SINCRONIZA = value;
                OnPropertyChanged();
            }
        }
        public DateTime FECHA_SINCRONIZADO
        {
            get => _FECHA_SINCRONIZADO;
            set
            {
                _FECHA_SINCRONIZADO = value;
                OnPropertyChanged();
            }
        }
        public string ID_HORARIO
        {
            get => _ID_HORARIO;
            set
            {
                _ID_HORARIO = value;
                OnPropertyChanged();
            }
        }

        public PersonalViewModel()
        {

        }

        public PersonalViewModel(PersonalTareo personal)
        {
            ID_PERSONAL = personal.ID_PERSONAL;
            NOMBRE = personal.NOMBRE;
            ID_TIPODOCUIDEN = personal.ID_TIPODOCUIDEN;
            TIPODOCUIDEN = personal.TIPODOCUIDEN;
            NUMERO_DOCUIDEN = personal.NUMERO_DOCUIDEN;
            ID_SITUACION = personal.ID_SITUACION;
            SITUACION = personal.SITUACION;
            ID_PROYECTO = personal.ID_PROYECTO;
            PROYECTO = personal.PROYECTO;
            ID_TAREADOR = personal.ID_TAREADOR;
            TAREADOR = personal.TAREADOR;
            ID_CLASE_TRABAJADOR = personal.ID_CLASE_TRABAJADOR;
            CLASE_TRABAJADOR = personal.CLASE_TRABAJADOR;
            ID_USUARIO_SINCRONIZA = personal.ID_USUARIO_SINCRONIZA;
            FECHA_SINCRONIZADO = personal.FECHA_SINCRONIZADO;
            ID_HORARIO = personal.ID_HORARIO;
        }

        public PersonalTareo GetPersonalTareo()
        {
            return new PersonalTareo()
            {
                ID_PERSONAL = this.ID_PERSONAL,
                NOMBRE = this.NOMBRE,
                ID_TIPODOCUIDEN = this.ID_TIPODOCUIDEN,
                TIPODOCUIDEN = this.TIPODOCUIDEN,
                NUMERO_DOCUIDEN = this.NUMERO_DOCUIDEN,
                ID_SITUACION = this.ID_SITUACION,
                SITUACION = this.SITUACION,
                ID_PROYECTO = this.ID_PROYECTO,
                PROYECTO = this.PROYECTO,
                ID_TAREADOR = this.ID_TAREADOR,
                TAREADOR = this.TAREADOR,
                ID_CLASE_TRABAJADOR = this.ID_CLASE_TRABAJADOR,
                CLASE_TRABAJADOR = this.CLASE_TRABAJADOR,
                ID_USUARIO_SINCRONIZA = this.ID_USUARIO_SINCRONIZA,
                FECHA_SINCRONIZADO = this.FECHA_SINCRONIZADO,
                ID_HORARIO = this.ID_HORARIO
            };
        }

    }
}
