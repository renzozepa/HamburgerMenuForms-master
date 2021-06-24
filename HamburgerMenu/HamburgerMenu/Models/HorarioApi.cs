namespace HamburgerMenu.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class HorarioApi
    {
        public string ID_HORARIO { get; set; }
        public string DESCRIPCION { get; set; }
        public string COLOQUIAL { get; set; }
        public string HORA_INICIO { get; set; }
        public string HORA_FIN { get; set; }
        public int TOLERA_ING { get; set; }
        public int TOLERA_SAL { get; set; }
        public bool ESTADO { get; set; }
        public int ID_USU_REG { get; set; }        
        public int ID_USUARIO_MOD { get; set; }        
    }
}
