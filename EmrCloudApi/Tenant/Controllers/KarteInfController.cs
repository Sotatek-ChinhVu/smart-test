using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.KarteInfs;
using EmrCloudApi.Tenant.Requests.KarteInfs;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.KarteInfs;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.KarteInfs.GetLists;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    public class KarteInfController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        public KarteInfController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetListKarteInfResponse>> GetList([FromQuery] GetListKarteInfRequest request)
        {
            var input = new GetListKarteInfInputData(request.PtId, request.RaiinNo, request.SinDate, request.IsDeleted);
            var output = _bus.Handle(input);

            var presenter = new GetListKarteInfPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetListKarteInfResponse>>(presenter.Result);
        }
    }
}
