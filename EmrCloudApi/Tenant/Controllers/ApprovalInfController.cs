using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.ApprovalInf;
using EmrCloudApi.Tenant.Requests.ApprovalInf;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.ApprovalInf;
using Microsoft.AspNetCore.Mvc;
using UseCase.ApprovalInf.GetApprovalInfList;
using UseCase.Core.Async;

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
            var input = new GetApprovalInfListInputData(req.StarDate, req.EndDate, req.DrName, req.KaName);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new GetApprovalInfListPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetApprovalInfListResponse>>(presenter.Result);
        }
    }
}
