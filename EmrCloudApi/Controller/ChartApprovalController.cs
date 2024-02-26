using Domain.Models.ChartApproval;
using EmrCloudApi.Constants;
using EmrCloudApi.Controller;
using EmrCloudApi.Presenters.ChartApproval;
using EmrCloudApi.Requests.ChartApproval;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.ChartApproval;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.ChartApproval.CheckSaveLogOut;
using UseCase.ChartApproval.GetApprovalInfList;
using UseCase.ChartApproval.SaveApprovalInfList;
using UseCase.Core.Sync;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    public class ChartApprovalController : BaseParamControllerBase
    {
        private readonly UseCaseBus _bus;
        public ChartApprovalController(UseCaseBus bus, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetApprovalInfListResponse>> GetList([FromQuery] GetApprovalInfListRequest req)
        {
            var input = new GetApprovalInfListInputData(HpId, req.StartDate, req.EndDate, req.KaId, req.TantoId, req.ConfirmStartDateEndDate);
            var output = _bus.Handle(input);
            var presenter = new GetApprovalInfListPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetApprovalInfListResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.Save)]
        public ActionResult<Response<SaveApprovalInfListResponse>> Update([FromBody] SaveApprovalInfListRequest request)
        {
            var input = new SaveApprovalInfListInputData(request.ApprovalInfs.Select(x => new ApprovalInfModel(x.Id, HpId, x.PtId, x.SinDate, x.RaiinNo, x.IsDeleted)).ToList(), HpId, UserId);
            var output = _bus.Handle(input);
            var presenter = new SaveApprovalInfListPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SaveApprovalInfListResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.CheckSaveLogOutChartApporval)]
        public ActionResult<Response<CheckSaveLogOutResponse>> CheckSaveLogOut()
        {
            var input = new CheckSaveLogOutInputData(HpId, UserId, DepartmentId);
            var output = _bus.Handle(input);
            var presenter = new CheckSaveLogOutPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<CheckSaveLogOutResponse>>(presenter.Result);
        }
    }
}