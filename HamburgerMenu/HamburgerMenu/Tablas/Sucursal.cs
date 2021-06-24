namespace HamburgerMenu.Tablas
{
    using SQLite;
    using System;
    public class Sucursal
    {
        [PrimaryKey]
        public string ID_SUCURSAL { get; set; }
        [MaxLength(80)]
        public string DESCRIPCION { get; set; }
        [MaxLength(80)]
        public string RUC { get; set; }        
        public bool ESTADO { get; set; }
        public int ID_USU_REG { get; set; }
        public int ID_USUARIO_MOD { get; set; }
    }
}
