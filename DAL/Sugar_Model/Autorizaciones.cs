using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Autorizaciones
    {
        public string SugarId { get; set; } = "";
        public string FechaCreacion { get; set; } = "";
        public string Oportunidad { get; set; } = "";
        public string Estado { get; set; } = "";
        public string Usuario { get; set; } = "";
        public string Aprobacion { get; set; } = "";
        public string Razones { get; set; } = "";
        public string Vigente { get; set; } = "";
        public string UltimaModificacion { get; set; } = "";
        public string AprobacionObligatoria { get; set; } = "";

    }
}
