using System;
using System.Collections.Generic;
using System.Text;

namespace HamburgerMenu.Models
{
    public class TareadorDispositivosApi
    {
        public int ID { get; set; }        
        public string DESCRIPCION { get; set; }
        public string NUMERO_CELULAR { get; set; }
        public string ID_TAREADOR { get; set; }
        public string TOKEN { get; set; }
        public DateTime? FECHA_SOLICITUD { get; set; }
        public DateTime? FECHA_ALTA { get; set; }
        public DateTime? FECHA_VENCIMIENTO { get; set; }
        public Boolean ESTADO { get; set; }
        public int ID_USU_REG { get; set; }
        public DateTime? FECHA_REGISTRO { get; set; }
        public int ID_USUARIO_MOD { get; set; }
        public DateTime? FECHA_MODIFICACION { get; set; }
        public int MULTI_PROYECTO { get; set; }
        public string CodProyectoNoProd { get; set; }
    }
}
