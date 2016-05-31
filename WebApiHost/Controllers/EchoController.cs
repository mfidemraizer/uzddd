using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using UserZoom.Domain.TaskManagement;

namespace WebApiHost.Controllers
{
    public class DataDto
    {
        public string Text { get; set; }
    }

    [RoutePrefix("api/v1/echo")]
    public sealed class EchoController : ApiController
    {
        public EchoController(ITaskService taskService)
        {
            TaskService = taskService;
        }

        private ITaskService TaskService { get; }

        [HttpGet, Route("{text}")]
        public async Task<IHttpActionResult> EchoAsync(string text)
        {
            return Ok(new { Text = text });
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> SaveText(DataDto dto)
        {
            return Ok();
        }
    }
}
