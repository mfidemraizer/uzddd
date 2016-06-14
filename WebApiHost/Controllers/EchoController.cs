using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using UserZoom.Domain;
using UserZoom.Domain.TaskManagement;
using UserZoom.Shared.Http;
using WebApiHost.Dto;

namespace WebApiHost.Controllers
{
    public class DataDto
    {
        public string Text { get; set; }
    }

    //[CustomFilter]
    [RoutePrefix("api/v1/tasks")]
    public sealed class TaskController : ApiControllerBase
    {
        public TaskController(ITaskService taskService, IMapper mapper)
        {
            TaskService = taskService;
            Mapper = mapper;
        }

        private ITaskService TaskService { get; }
        private IMapper Mapper { get; }

        [HttpPost, Route("", Name = "createTask")]
        public Task<IHttpActionResult> CreateAsync(UZTaskCreationDto dto)
        {
            return OkOrBadRequest(TaskService.AddAsync(Mapper.Map<UZTaskCreationDto, UZTask>(dto)));
        }

        //[HttpPost, Route("")]
        //public async Task<IHttpActionResult> SaveText(DataDto dto)
        //{
        //    return Ok();
        //}
    }
}
