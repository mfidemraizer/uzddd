using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using UserZoom.Shared.Patterns.AccumulatedResult;

namespace UserZoom.Shared.Http
{
    public abstract class ApiControllerBase : ApiController
    {
        protected Task<IHttpActionResult> CreatedOrBadRequest(IBasicResult result)
        {
            return Task.FromResult<IHttpActionResult>
            (
                new HttpBasicResult(this, result, HttpStatusCode.Created, HttpStatusCode.BadRequest)
            );
        }
        protected async Task<IHttpActionResult> OkOrBadRequest(Task<IBasicResult> result)
        {
            return new HttpBasicResult(this, await result, HttpStatusCode.OK, HttpStatusCode.BadRequest);
        }

        protected Task<IHttpActionResult> CustomOk(bool successful = true)
        {
            return Task.FromResult<IHttpActionResult>(new CustomActionResult(this, successful));
        }
    }
}
