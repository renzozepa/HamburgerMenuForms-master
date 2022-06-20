namespace HamburgerMenu.Tablas
{
    using SQLite;
    using System;
    public class Personal
    {
        [PrimaryKey]
        [MaxLength(8)]
        public string CodObrero { get; set; }

        [MaxLength(120)]
        public string Descripcion { get; set; }

        [MaxLength(20)]
        public string DNI { get; set; }

        public Guid NroEsquemaPlanilla { get; set; }

        public string CodProyecto { get; set; }

        [MaxLength(8)]
        public string CodIdentificador { get; set; }

        public bool Activo { get; set; }
        public string CodInsumo { get; set; }
        public string Insumo { get; set; }

        public string CodOcupacion { get; set; }
        public string Ocupacion { get; set; }
        public string CodProyectoNoProd { get; set; }
    }
}
