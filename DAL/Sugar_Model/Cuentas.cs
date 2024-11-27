using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Cuentas
    {
        public string SugarId { get; set; } = "";
        public string Nombre { get; set; } = "";

        public string Industria { get; set; } = "";
        
        public string Ciudad { get; set; } = "";
        public string Telefono { get; set; } = "";
        public string Vertical { get; set; } = "";
        public string VersionSoftware { get; set; } = "";
        public string Usuario { get; set; } = "";
        public string PaisEmisionFactura { get; set; } = "";
        public string AlcanceDeCuenta { get; set; } = "";
    }
}
