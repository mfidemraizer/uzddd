using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace UserZoom.Shared.Http
{
    public abstract class ApiControllerBase : ApiController
    {
        public Task<IHttpActionResult> CustomOk(bool successful = true)
        {
            return Task.FromResult<IHttpActionResult>(new CustomActionResult(this, successful));
        }
    }
}
