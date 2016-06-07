using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using UserZoom.Domain.TaskManagement;
using UserZoom.Shared.Http;

namespace WebApiHost.Controllers
{
    public class DataDto
    {
        public string Text { get; set; }
    }

    [CustomFilter]
    [RoutePrefix("api/v1/echo")]
    public sealed class EchoController : ApiControllerBase
    {
        public EchoController(ITaskService taskService)
        {
            TaskService = taskService;
        }

        private ITaskService TaskService { get; }
        
        [HttpGet, Route("{text}")]
        public Task<IHttpActionResult> EchoAsync(string text)
        {

            return CustomOk(true);
            //
            //return Ok(new { Text = text });
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> SaveText(DataDto dto)
        {
            
            return Ok();
        }
    }
}
