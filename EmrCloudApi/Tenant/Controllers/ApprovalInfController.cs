using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.ApprovalInfo;
using EmrCloudApi.Tenant.Requests.ApprovalInfo;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.ApprovalInf;
using Microsoft.AspNetCore.Mvc;
using UseCase.ApprovalInfo.GetApprovalInfList;
using UseCase.Core.Sync;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalInfController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public ApprovalInfController(UseCaseBus bus)
        {
            _bus = bus;
        }
        [HttpGet(ApiPath.GetList)]
        public async Task<ActionResult<Response<GetApprovalInfListResponse>>> GetList([FromQuery] GetApprovalInfListRequest req)
        {
            var input = new GetApprovalInfListInputData(req.StartDate, req.EndDate, req.DrName, req.KaName);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new GetApprovalInfListPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetApprovalInfListResponse>>(presenter.Result);
        }
    }
}
