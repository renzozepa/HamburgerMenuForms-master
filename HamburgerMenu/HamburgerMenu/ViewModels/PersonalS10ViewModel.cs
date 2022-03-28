using HamburgerMenu.Tablas;
using SQLite;
using System;

namespace HamburgerMenu.ViewModels
{   
    public class PersonalS10ViewModel : BaseViewModel
    {
        protected string _CodObrero = string.Empty;
        public string CodObrero {
            get => _CodObrero;
            set
            {
                _CodObrero = value;
                OnPropertyChanged();
            }
        }

        protected string _Descripcion = string.Empty;
        [MaxLength(120)]
        public string Descripcion {
            get => _Descripcion;
            set
            {
                _Descripcion = value;
                OnPropertyChanged();
            }
        }

        protected string _DNI = string.Empty;
        [MaxLength(20)]
        public string DNI {
            get => _DNI;
            set
            {
                _DNI = value;
                OnPropertyChanged();
            }
        }

        protected Guid _NroEsquemaPlanilla;
        public Guid NroEsquemaPlanilla {
            get => _NroEsquemaPlanilla;
            set
            {
                _NroEsquemaPlanilla = value;
                OnPropertyChanged();
            }
        }

        protected string _CodProyecto = string.Empty;
        public string CodProyecto {
            get => _CodProyecto;
            set
            {
                _CodProyecto = value;
                OnPropertyChanged();
            }
        }

        protected string _CodIdentificador = string.Empty;
        [MaxLength(8)]
        public string CodIdentificador {
            get => _CodIdentificador;
            set
            {
                _CodIdentificador = value;
                OnPropertyChanged();
            }
        }

        protected bool _Activo = false;
        public bool Activo {
            get => _Activo;
            set
            {
                _Activo = value;
                OnPropertyChanged();
            }
        }

        public PersonalS10ViewModel(Tablas.Personal personal)
        {
            CodObrero = personal.CodObrero;
            Descripcion = personal.Descripcion;
            DNI = personal.DNI;
            NroEsquemaPlanilla = personal.NroEsquemaPlanilla;
            CodProyecto = personal.CodProyecto;
            CodIdentificador = personal.CodIdentificador;
            Activo = personal.Activo;
        }
        public Tablas.Personal GetPersonalS10()
        {
            return new Tablas.Personal()
            {
                CodObrero = this.CodObrero,
                Descripcion = this.Descripcion,
                DNI = this.DNI,
                NroEsquemaPlanilla = this.NroEsquemaPlanilla,
                CodProyecto = this.CodProyecto,
                CodIdentificador = this.CodIdentificador,
                Activo = this.Activo
            };
        }
    }
}
