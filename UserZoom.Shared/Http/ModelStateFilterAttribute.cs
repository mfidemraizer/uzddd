using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace UserZoom.Shared.Http
{
    public class ModelStateFilterAttribute : ActionFilterAttribute
    {
        public class Config
        {
            public List<Expression<Func<IHttpController, object>>> ActionWhiteList { get; } = new List<Expression<Func<IHttpController, object>>>();
        }

        public ModelStateFilterAttribute(Config config = null)
        {
            Configuration = config;
        }

        private Config Configuration { get; }

        private bool ActionIsInWhiteList(HttpActionContext actionContext)
        {
            MethodInfo actionMethod = actionContext.ControllerContext.Controller
                                                    .GetType()
                                                    .GetMethod(actionContext.ActionDescriptor.ActionName);

            return Configuration.ActionWhiteList.Any
            (
                methodCall => ((MethodCallExpression)methodCall.Body).Method == actionMethod
            );
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (ActionIsInWhiteList(actionContext) && !actionContext.ModelState.IsValid)
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
        }
    }
}
