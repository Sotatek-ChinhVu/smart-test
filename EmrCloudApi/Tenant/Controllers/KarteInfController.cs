using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.KarteInfs;
using EmrCloudApi.Tenant.Requests.KarteInfs;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.KarteInfs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.KarteInfs.GetLists;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class KarteInfController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public KarteInfController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList)]
        public async Task<ActionResult<Response<GetListKarteInfResponse>>> GetList([FromQuery] GetListKarteInfRequest request)
        {
            var input = new GetListKarteInfInputData(request.PtId, request.RaiinNo, request.SinDate, request.IsDeleted);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new GetListKarteInfPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetListKarteInfResponse>>(presenter.Result);
        }
    }
}
