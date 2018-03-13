using Selfhost.Tools;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Selfhost.Worker
{
    [RoutePrefix("api/v1")]
    public class WebApiController : ApiController
    {
        private readonly IPlop _plop;

        public WebApiController(IPlop plop)
        {
            _plop = plop;
        }

        [HttpGet]
        [Route("data")]
        public async Task<HttpResponseMessage> GetData()
        {
            return await Task.FromResult(new HttpResponseMessage() { Content = new StringContent("Data") });
        }
    }
}
