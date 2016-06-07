using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace UserZoom.Shared.Http
{
    public class CustomActionResult : IHttpActionResult
    {
        public CustomActionResult(ApiController controller, bool successful = false)
        {
            Successful = successful;
            Controller = controller;
        }

        private bool Successful { get; }
        private ApiController Controller { get; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response;

            if (Successful)
                response =  new HttpResponseMessage(HttpStatusCode.OK);
            else
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            return Task.FromResult(response);
        }
    }
}
