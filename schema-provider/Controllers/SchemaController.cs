using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace schema_provider.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SchemaController : ControllerBase
	{
		const String QuellPfad = @"C:\Projects\schemagenerator\src\assets\schemas";

		private Dictionary<String, String> _schemas =
			new Dictionary<string, string>();

		public SchemaController()
		{
			foreach(String schemaFilePath in Directory.EnumerateFiles(QuellPfad, "schema_*.js"))
			{
				_schemas.Add(
					Regex.Match(
						Path.GetFileNameWithoutExtension(schemaFilePath),
						"^schema_(.*)$",
						RegexOptions.IgnoreCase
					).Groups[1]?.Value?.ToUpper()
						?? Guid.NewGuid().ToString(),
					schemaFilePath
				);
			}
		}

		[HttpGet()]
		[Route("api/[controller]/{typ}")]
		public async Task<IActionResult> holeSchema(string typ)
		{
			if (this._schemas.ContainsKey(typ?.ToUpper() ?? String.Empty))
			{
				Console.WriteLine($"Schema: '{typ}' wird zurückgegeben.");
				return new FileStreamResult(System.IO.File.OpenRead(this._schemas[typ]), Microsoft.Net.Http.Headers.MediaTypeHeaderValue.Parse("text/javascript"));
			}
			else
			{
				Console.WriteLine($"Schema: '{typ}' wurde nicht gefunden!");
				return NotFound();
			}
		}
	}
}
