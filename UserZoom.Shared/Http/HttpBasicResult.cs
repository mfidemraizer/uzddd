using System;
using System.Diagnostics.Contracts;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using UserZoom.Shared.Patterns.AccumulatedResult;

namespace UserZoom.Shared.Http
{
    public class HttpBasicResult : IHttpActionResult
    {
        public HttpBasicResult(ApiController controller, IBasicResult result, HttpStatusCode successCode = HttpStatusCode.OK, HttpStatusCode failCode = HttpStatusCode.BadRequest)
        {
            Controller = controller;
            Result = result;
            SuccessCode = successCode;
            FailCode = failCode;
        }

        private ApiController Controller { get; }
        private IBasicResult Result { get; }
        private HttpStatusCode SuccessCode { get; }
        private HttpStatusCode FailCode { get; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response;

            if(Result.IsSuccessful)
            {
                object serializableResult = null;
                IHasObject singleObject = Result as IHasObject;
                IHasObjects multipleObject = null;

                if(singleObject == null)
                    multipleObject = Result as IHasObjects;

                if (singleObject != null)
                {
                    serializableResult = singleObject.Object;
                }
                else if (multipleObject != null)
                {
                    serializableResult = multipleObject.Objects;
                } 
                
                response = new HttpResponseMessage(SuccessCode);
                response.Content = new ObjectContent<object>(serializableResult, new JsonMediaTypeFormatter());
                
                if(SuccessCode == HttpStatusCode.Created)
                {
                    MethodInfo actionMethod = Controller.GetType().GetMethod(Controller.ActionContext.ActionDescriptor.ActionName);

                    Contract.Assert(actionMethod != null);

                    RouteAttribute routeAttribute = actionMethod.GetCustomAttribute<RouteAttribute>(true);

                    Contract.Assert(routeAttribute != null);

                    // Universal resource identifier
                    response.Headers.Location = Controller.Url.Route
                    (
                        routeAttribute.Name,
                        Controller.ActionContext.ActionArguments
                    ).ToUri();
                }
            }
            else
            {
                response = new HttpResponseMessage(FailCode);

                foreach(var pair in Result.AffectedResources)
                {
                    Controller.ModelState.AddModelError(pair.Key, pair.Value);
                }
            }

            return Task.FromResult(response);
        }
    }
}
