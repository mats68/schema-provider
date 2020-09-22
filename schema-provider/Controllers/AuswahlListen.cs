using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace schema_provider.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuswahlListen : ControllerBase
	{
		private static readonly Dictionary<String, String> _auswahlListe_VNBs =
			new Dictionary<String, String>()
			{
				{ "BKW_DE", "BKW (de)" },
				{ "Primeo", "Primeo" },
				{ "Onyx", "Onyx" }
			};

		private static readonly Dictionary<String, String> _auswahlListe_Mitarbeiter =
			new Dictionary<String, String>()
			{
				{ "THMA", "Thaler Matthias" },
				{ "BRDA", "Brunner Daniel" },
				{ "KARO", "Käch Ronnie" }
			};

		private static readonly Dictionary<String, Dictionary<String, String>> _auswahlListen =
			new Dictionary<string, Dictionary<string, string>>()
			{
				{ "vnb", _auswahlListe_VNBs },
				{ "mitarbeiter", _auswahlListe_Mitarbeiter }
			};

		[HttpGet()]
		[Route("api/[controller]/{name}")]
		public async Task<IActionResult> holeAuswahlListe(string name)
		{
			name = name?.ToLower();

			JObject antwort = null;
			HttpStatusCode antwortCode;

			if(_auswahlListen.ContainsKey(name))
			{
				antwortCode = HttpStatusCode.OK;
				antwort =	
					JObject.FromObject(
						new
						{
							Name = name,
							Daten = _auswahlListen[name]
						}
					);

			}
			else
			{
				antwortCode = HttpStatusCode.NotFound;
				antwort =
					JObject.FromObject(
						new
						{
							Name = name,
							Daten = (Dictionary<String, String>)null
						}
					);
			}

			return new JsonNetResult(antwort, antwortCode);
		}
	}
}
