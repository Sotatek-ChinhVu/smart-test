using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.KarteFilter;
using EmrCloudApi.Tenant.Requests.KarteFilter;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.KarteFilter;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.KarteFilter.GetListKarteFilter;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KarteFilterController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public KarteFilterController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetKarteFilterMstResponse>> GetList([FromQuery] GetKarteFilterMstRequest request)
        {
            var input = new KarteFilterInputData(request.SinDate);
            var output = _bus.Handle(input);

            var presenter = new GetKarteFilterMstPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetKarteFilterMstResponse>>(presenter.Result);
        }
    }
}
