using UseCase.Core.Sync;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.GetListTenMstOrigin;
using EmrCloudApi.Presenters.TenMstMaintenance;
using EmrCloudApi.Requests.TenMstMaintenance;
using UseCase.TenMstMaintenance.GetListTenMstOrigin;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class TenMstMaintenanceController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;

        public TenMstMaintenanceController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        
        [HttpGet(ApiPath.GetListTenMstOrigin)]
        public ActionResult<Response<GetListTenMstOriginResponse>> GetListTenMstOrigin([FromQuery] GetListTenMstOriginRequest request)
        {
            var input = new GetListTenMstOriginInputData(request.ItemCd);
            var output = _bus.Handle(input);
            var presenter = new GetListTenMstOriginPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetListTenMstOriginResponse>>(presenter.Result);
        }
    }
}
