using SQLite;
using System;

namespace HamburgerMenu
{
    public class PersonalTareo
    {
        [PrimaryKey]
        public int ID_PERSONAL { get; set; }

        [MaxLength(80)]
        public string NOMBRE { get; set; }

        [MaxLength(2)]
        public string ID_TIPODOCUIDEN { get; set; }

        [MaxLength(80)]
        public string TIPODOCUIDEN { get; set; }

        [MaxLength(11)]
        public string NUMERO_DOCUIDEN { get; set; }

        public int ID_SITUACION { get; set; }

        public string SITUACION { get; set; }

        [MaxLength(11)]
        public string ID_PROYECTO { get; set; }

        [MaxLength(255)]
        public string PROYECTO { get; set; }

        [MaxLength(80)]
        public string ID_TAREADOR { get; set; }

        [MaxLength(255)]
        public string TAREADOR { get; set; }

        public int ID_CLASE_TRABAJADOR { get; set; }

        public string CLASE_TRABAJADOR { get; set; }

        public int ID_USUARIO_SINCRONIZA { get; set; }
        public DateTime FECHA_SINCRONIZADO { get; set; }

    }
}
