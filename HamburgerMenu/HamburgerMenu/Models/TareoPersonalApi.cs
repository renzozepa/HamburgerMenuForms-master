using System;
using System.Collections.Generic;
using System.Text;

namespace HamburgerMenu.Models
{
    public class TareoPersonalApi
    {        
        public int ID { get; set; }
        public string ID_TAREADOR { get; set; }
        public int ID_PERSONAL { get; set; }
        public string PERSONAL { get; set; }
        public string ID_PROYECTO { get; set; }
        public int ID_SITUACION { get; set; }
        public int ID_CLASE_TRABAJADOR { get; set; }
        public DateTime FECHA_TAREO { get; set; }
        public int TIPO_MARCACION { get; set; }
        public string HORA { get; set; }
        public DateTime FECHA_REGISTRO { get; set; }
        public int SINCRONIZADO { get; set; }
        public DateTime FECHA_SINCRONIZADO { get; set; }
    }
}
