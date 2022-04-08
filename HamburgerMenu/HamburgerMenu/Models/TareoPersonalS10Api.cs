using System;
using System.Collections.Generic;
using System.Text;

namespace HamburgerMenu.Models
{
    public class TareoPersonalS10Api
    {
        public int ID { get; set; }
        public string ID_TAREADOR { get; set; }
        public string PROYECTO { get; set; }
        public string CODOBRERO { get; set; }
        public string PERSONAL { get; set; }
        public string DNI { get; set; }
        public int TIPO_MARCACION { get; set; }
        public DateTime FECHA_MARCACION { get; set; }
        public string HORA { get; set; }
        public DateTime FECHA_REGISTRO { get; set; }
        public int SINCRONIZADO { get; set; }
        public DateTime FECHA_SINCRONIZADO { get; set; }
        public string TOKEN { get; set; }
        public string ID_SUCURSAL { get; set; }
        public string ORIGEN { get; set; }
    }
}
