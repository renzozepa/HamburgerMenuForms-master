﻿namespace HamburgerMenu.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class PersonalS10Api
    {
        public string CodObrero { get; set; }

        public string Descripcion { get; set; }

        public string DNI { get; set; }

        public Guid NroEsquemaPlanilla { get; set; }

        public string CodProyecto { get; set; }

        public string CodIdentificador { get; set; }

        public bool Activo { get; set; }
        public string CodInsumo { get; set; }
        public string Insumo { get; set; }

        public string CodOcupacion { get; set; }
        public string Ocupacion { get; set; }
        public string CodProyectoNoProd { get; set; }
    }
}
