namespace HamburgerMenu.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class SucursalApi
    {
        public string ID_SUCURSAL { get; set; }
        public string DESCRIPCION { get; set; }
        public string RUC { get; set; }        
        public bool ESTADO { get; set; }
        public int ID_USU_REG { get; set; }
        public int ID_USUARIO_MOD { get; set; }
    }
}
