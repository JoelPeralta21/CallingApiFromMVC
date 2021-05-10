using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallingApiFromMVC.Models
{
	public class Empleados
	{
		public int IdEmpleado { get; set; }
		public string NombreCompleto { get; set; }
		public string Posicion { get; set; }
		public string Departamento { get; set; }
		public string Supervisor { get; set; }
	}
}
