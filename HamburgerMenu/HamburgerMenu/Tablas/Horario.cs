namespace HamburgerMenu.Tablas
{
    using SQLite;
    using System;

    public class Horario
    {
        [PrimaryKey]
        public string ID_HORARIO { get; set; }
        [MaxLength(80)]
        public string DESCRIPCION { get; set; }
        [MaxLength(80)]
        public string COLOQUIAL { get; set; }
        [MaxLength(5)]
        public string HORA_INICIO { get; set; }
        [MaxLength(5)]
        public string HORA_FIN { get; set; }
        public int TOLERA_ING { get; set; }
        public int TOLERA_SAL { get; set; }
        public bool ESTADO { get; set; }
        public int ID_USU_REG { get; set; }        
        public int ID_USUARIO_MOD { get; set; }
    }
}
