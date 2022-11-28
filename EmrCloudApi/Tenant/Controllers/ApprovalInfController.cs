using Domain.Models.ApprovalInfo;
using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.ApprovalInfo;
using EmrCloudApi.Tenant.Requests.ApprovalInfo;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.ApprovalInf;
using EmrCloudApi.Tenant.Responses.ApprovalInfo;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.ApprovalInfo.GetApprovalInfList;
using UseCase.ApprovalInfo.UpdateApprovalInfList;
using UseCase.Core.Sync;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    public class ApprovalInfController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        public ApprovalInfController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList)]
        public async Task<ActionResult<Response<GetApprovalInfListResponse>>> GetList([FromQuery] GetApprovalInfListRequest req)
        {
            var input = new GetApprovalInfListInputData(HpId, req.StartDate, req.EndDate, req.KaId, req.TantoId);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new GetApprovalInfListPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetApprovalInfListResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.Update)]
        public async Task<ActionResult<Response<UpdateApprovalInfListResponse>>> Update([FromBody] UpdateApprovalInfRequest request)
        {
            var input = new UpdateApprovalInfListInputData(request.ApprovalIfnList.Select(x => new ApprovalInfModel(
                                                            x.Id,
                                                            x.HpId,
                                                            x.PtId,
                                                            x.SinDate,
                                                            x.RaiinNo,
                                                            x.SeqNo,
                                                            x.IsDeleted,
                                                            x.CreateDate,
                                                            x.CreateId,
                                                            x.CreateMachine,
                                                            x.UpdateDate,
                                                            x.UpdateId,
                                                            x.UpdateMachine
                                                            )).ToList()
                                                            );
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new UpdateApprovalInfListPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<UpdateApprovalInfListResponse>>(presenter.Result);
        }
    }
}
