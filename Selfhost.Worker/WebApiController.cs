namespace Selfhost.Worker
{
    using Selfhost.Tools;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

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
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent("Data", System.Text.Encoding.UTF8, "text/plain");
            return await Task.FromResult(response);
        }
    }
}
