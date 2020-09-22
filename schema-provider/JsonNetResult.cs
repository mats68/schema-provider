using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace schema_provider
{
   public class JsonNetResult : ActionResult
   {
      private readonly JObject _data;
      private readonly HttpStatusCode _code;

      public JsonNetResult(JObject data, HttpStatusCode status = HttpStatusCode.OK)
      {
         _data = data;
         _code = status;   
      }

		public override void ExecuteResult(ActionContext context)
		{
         var response = context.HttpContext.Response;

         response.StatusCode = (int)_code;
         response.ContentType = "application/json";
         response.BodyWriter.WriteAsync(
            Encoding.UTF8.GetBytes(
               _data.ToString(Newtonsoft.Json.Formatting.None)
            )
         );
      }
   }
}
