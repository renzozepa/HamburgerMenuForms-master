using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace HamburgerMenu
{    
    public class LoginLocal
    {
        [PrimaryKey,AutoIncrement]
        public int ID { get; set; }
        [MaxLength(255)]
        public string NOMBRE { get; set; }
        [MaxLength(255)]
        public string USUARIO { get; set; }
        [MaxLength(255)]
        public string CONTRASENIA { get; set; }
        [MaxLength(255)]
        public string TAREADOR { get; set; }
        [MaxLength(15)]
        public string CELULAR { get; set; }
        public string TOKEN { get; set; }
        public DateTime FECHA_VIGENCIA { get; set; }
    }
}
