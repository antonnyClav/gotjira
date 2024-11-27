using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Usuarios
    {
        public string SugarId { get; set; } = "";
        public string Nombre { get; set; } = "";
        public string Departamento { get; set; } = "";
        public string Titulo { get; set; } = "";
        public string InformaA { get; set; } = "";
        public string Mail { get; set; } = "";
        public string Telefono { get; set; } = "";
        
        public string Estado { get; set; } = "";
        public string FechaCreacion { get; set; } = "";
    }
}
