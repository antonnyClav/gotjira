using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Oportunidades
    {
        public string SugarId { get; set; } = "";
        public string Numero { get; set; } = "";
        public string Nombre { get; set; } = "";
        public string Cuenta { get; set; } = "";
        public string EtapaVenta { get; set; } = "";
        public string Monto { get; set; } = "";
        public string FechaCierre { get; set; } = "";
        public string Usuario { get; set; } = "";
        public string PrevistoProbable { get; set; } = "";
        public string Perdido { get; set; } = "";
        public string DireccionComercial { get; set; } = "";
        public string Tipo { get; set; } = "";
        public string FechaInicio { get; set; } = "";
        public string FechaRenovacion { get; set; } = "";
        public string UltimaModificacion { get; set; } = "";        
        public string CantidadConvertida { get; set; } = "";

    }
}
