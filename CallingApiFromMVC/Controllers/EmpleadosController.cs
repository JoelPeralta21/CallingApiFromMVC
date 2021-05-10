using CallingApiFromMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CallingApiFromMVC.Controllers
{
	public class EmpleadosController : Controller
	{
		public double acum;

		HttpClientHandler _clientHandler = new HttpClientHandler();
		Empleados _empleado  = new Empleados();
		List<Empleados> _empleados = new List<Empleados>();
		Empleado_Pago _empleadopago = new Empleado_Pago();
		List<Empleado_Pago> _empleadopagos = new List<Empleado_Pago>();

		public EmpleadosController()
		{
			_clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
		}
		public IActionResult Index()//GuardoEmpleados
		{
			return View();
		}
		public IActionResult UpdateView()//Actualizar
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> ListaEmpleados()//Veo
		{
			_empleados = new List<Empleados>();
			using (var httpClient = new HttpClient(_clientHandler))
			{
				using (var response = await httpClient.GetAsync("https://localhost:44310/api/Empleados"))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();
					_empleados = JsonConvert.DeserializeObject<List<Empleados>>(apiResponse);
				}
				return View(_empleados);
			}
		}
		

		[HttpGet]
		
		public async Task<IActionResult> GetById(int id)
		{
			//var acum;
			_empleadopagos = new List<Empleado_Pago>();
			using (var httpClient = new HttpClient(_clientHandler))
			{
				using (var response = await httpClient.GetAsync("https://localhost:44310/api/GetPagoEmpleado/" + id))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();
					_empleadopagos = JsonConvert.DeserializeObject<List<Empleado_Pago>>(apiResponse);
					foreach (var a in _empleadopagos)
					{
						acum += Convert.ToDouble(a.Total);
						ViewData["t"] = acum;
						ViewData["NombreCompleto"] = a.NombreCompleto;
						ViewData["Posicion"] = a.Posicion;
						ViewData["Supervisor"] = a.Supervisor;
						ViewData["Departamento"] = a.Departamento;
					}
				}
				return View(_empleadopagos);
			}
		}


		[HttpPost]
		public async Task<Empleados> AddUpdateEmpleados(Empleados empleados)
		{
			_empleado = new Empleados();

			using (var httpClient = new HttpClient(_clientHandler))
			{
				StringContent content = new StringContent(JsonConvert.SerializeObject(empleados),Encoding.UTF8, "application/json");
				using (var response = await httpClient.PostAsync("https://localhost:44310/api/Empleados", content))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();
					_empleado = JsonConvert.DeserializeObject<Empleados>(apiResponse);
				}
				return _empleado;
			}
		}

		[HttpDelete]
		public async Task<string> Delete(int id)
		{
			string msj = "";
			using (var httpClient = new HttpClient(_clientHandler))
			{
				using (var response = await httpClient.DeleteAsync("https://localhost:44310/api/Empleados/" + id))
				{
					msj = await response.Content.ReadAsStringAsync();
				
				}
				return msj;
			}
		}
	}
}
		