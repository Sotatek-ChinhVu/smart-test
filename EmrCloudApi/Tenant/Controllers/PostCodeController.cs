using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.PostCodeMst;
using EmrCloudApi.Tenant.Requests.PostCodeMst;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.PostCodeMst;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Async;
using UseCase.PostCodeMst.Search;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostCodeController : ControllerBase
    {
        private readonly UseCaseBus _bus;

        public PostCodeController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<SearchPostCodeRespone>> GetList([FromQuery] SearchPostCodeRequest request)
        {
            var input = new SearchPostCodeInputData(request.PostCode1, request.PostCode2, request.Address);
            var output = _bus.Handle(input);
            var presenter = new SearchPostCodePresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }
    }
}
