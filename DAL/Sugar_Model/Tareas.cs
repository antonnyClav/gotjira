using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Tareas
    {
		public string SugarId { get; set; } = "";

		public string Cuenta { get; set; } = "";

		public string Oportunidad { get; set; } = "";

		public string Tipo { get; set; } = "";

		public string FechaVencimiento { get; set; } = "";

		public string Estado { get; set; } = "";

		public string Usuario { get; set; } = "";

		public string FechaCreacion { get; set; } = "";

		public string Asunto { get; set; } = "";

		public string ExpertoAsignado { get; set; } = "";

		public string Prioridad { get; set; } = "";

		public string UltimaModificacion { get; set; } = "";
	}
}
