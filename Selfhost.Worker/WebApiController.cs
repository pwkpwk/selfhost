using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Selfhost.Worker
{
    [RoutePrefix("api/v1")]
    public class WebApiController : ApiController
    {
        [HttpGet]
        [Route("data")]
        public async Task<HttpResponseMessage> GetData()
        {
            return await Task.FromResult(new HttpResponseMessage() { Content = new StringContent("Data") });
        }
    }
}
