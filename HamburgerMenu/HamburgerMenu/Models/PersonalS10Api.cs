namespace HamburgerMenu.Models
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
    }
}
