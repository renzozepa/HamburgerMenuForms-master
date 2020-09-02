using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace HamburgerMenu.Tablas
{
    public class ConfiguracionLocal
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }        
        public bool LOCAL { get; set; }
        public bool SERVER { get; set; }
        public bool LOCALSERVER { get; set; }
    }
}
