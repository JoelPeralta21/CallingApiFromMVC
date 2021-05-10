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
	public class PagosController : Controller
	{
		public double acum;
		public int id;
		public string nomb;

		HttpClientHandler _clientHandler = new HttpClientHandler();
		Pagos _pago = new Pagos();
		List<Pagos> _pagos = new List<Pagos>();
		Empleados _empleado = new Empleados();
		List<Empleados> _empleados = new List<Empleados>();

		public PagosController()
		{
			_clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
		}
		public IActionResult SavePagos()//GuardoPagos
		{
			return View();
		}
		[HttpGet]
		public async Task<IActionResult> ListaPagos()//Veo la lista de todos los pagos
		{
			_pagos = new List<Pagos>();
			_empleados = new List<Empleados>();
			using (var httpClient = new HttpClient(_clientHandler))
			{
				using (var response = await httpClient.GetAsync("https://localhost:44310/api/Pagos"))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();
					_pagos = JsonConvert.DeserializeObject<List<Pagos>>(apiResponse);
					foreach (var a in _pagos)
					{
						acum += a.Total;
						ViewData["t"] = acum;
						id = a.IdEmpleado;
						var rest = await httpClient.GetAsync("https://localhost:44310/api/Empleados/" + id);
						string apirest = await rest.Content.ReadAsStringAsync();
						_empleado = JsonConvert.DeserializeObject<Empleados>(apirest);
						nomb = _empleado.NombreCompleto;
						//if (!string.IsNullOrEmpty(nomb))
						//{
						//	ViewData["id"] = nomb;
						//}
						a.NombreEmpleado = nomb;
					}
				}
				return View(_pagos);
			}
		}

		[HttpGet]
		public async Task<IActionResult> ListaPagosPorPersona()//Veo por persona
		{
			_pagos = new List<Pagos>();
			using (var httpClient = new HttpClient(_clientHandler))
			{
				using (var response = await httpClient.GetAsync("https://localhost:44310/api/Pagos"))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();
					_pagos = JsonConvert.DeserializeObject<List<Pagos>>(apiResponse);
				}
				return View(_pagos);
			}
		}

		[HttpGet]
		public async Task<Pagos> GetById(int id)
		{
			_pago = new Pagos();
			using (var httpClient = new HttpClient(_clientHandler))
			{
				using (var response = await httpClient.GetAsync("https://localhost:44310/api/Pagos/" + id))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();
					_pago = JsonConvert.DeserializeObject<Pagos>(apiResponse);
				}
				return _pago;
			}
		}

		[HttpPost]
		public async Task<Pagos> AddUpdatePagos(Pagos pago)
		{
			_pago = new Pagos();

			using (var httpClient = new HttpClient(_clientHandler))
			{
				StringContent content = new StringContent(JsonConvert.SerializeObject(pago), Encoding.UTF8, "application/json");
				using (var response = await httpClient.PostAsync("https://localhost:44310/api/Pagos", content))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();
					_pago = JsonConvert.DeserializeObject<Pagos>(apiResponse);
				}
				return _pago;
			}
		}

		[HttpDelete]
		public async Task<string> Delete(int id)
		{
			string msj = "";
			using (var httpClient = new HttpClient(_clientHandler))
			{
				using (var response = await httpClient.DeleteAsync("https://localhost:44310/api/Pagos/" + id))
				{
					msj = await response.Content.ReadAsStringAsync();

				}
				return msj;
			}
		}
	}
}
