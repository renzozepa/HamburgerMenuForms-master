using SQLite;
using System;

namespace HamburgerMenu.Tablas
{
    public class TareoPersonalS10
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string ID_TAREADOR { get; set; }
        public string PROYECTO { get; set; }
        public string CODOBRERO { get; set; }
        public string PERSONAL { get; set; }
        public string DNI { get; set; }
        public int TIPO_MARCACION { get; set; }
        public DateTime FECHA_TAREO { get; set; }        
        public string HORA { get; set; }
        public DateTime FECHA_REGISTRO { get; set; }
        public int SINCRONIZADO { get; set; }
        public DateTime FECHA_SINCRONIZADO { get; set; }
        public string TOKEN { get; set; }
        public string ID_SUCURSAL { get; set; }        
    }
}
