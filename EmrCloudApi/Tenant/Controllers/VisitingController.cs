using EmrCloudApi.Realtime;
using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Messages;
using EmrCloudApi.Tenant.Presenters.Reception;
using EmrCloudApi.Tenant.Presenters.VisitingList;
using EmrCloudApi.Tenant.Requests.Reception;
using EmrCloudApi.Tenant.Requests.VisitingList;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Reception;
using EmrCloudApi.Tenant.Responses.VisitingList;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Reception.GetList;
using UseCase.Reception.GetSettings;
using UseCase.Reception.UpdateDynamicCell;
using UseCase.Reception.UpdateStaticCell;
using UseCase.VisitingList.ReceptionComment;
using UseCase.VisitingList.ReceptionLock;
using UseCase.VisitingList.SaveSettings;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VisitingController : ControllerBase
{
    private readonly UseCaseBus _bus;
    private readonly IWebSocketService _webSocketService;

    public VisitingController(UseCaseBus bus,
        IWebSocketService webSocketService)
    {
        _bus = bus;
        _webSocketService = webSocketService;
    }

    [HttpGet(ApiPath.Get + "ReceptionComment")]
    public ActionResult<Response<GetReceptionCommentResponse>> GetList([FromQuery] GetReceptionCommentRequest request)
    {
        var input = new GetReceptionCommentInputData(request.RaiinNo);
        var output = _bus.Handle(input);
        var presenter = new GetReceptionCommentPresenter();
    [HttpGet(ApiPath.Get + "ReceptionLock")]
    public ActionResult<Response<GetReceptionLockRespone>> GetList([FromQuery] GetReceptionLockRequest request)
    {
        var input = new GetReceptionLockInputData(request.SinDate, request.PtId, request.RaiinNo, request.FunctionCd);
        var output = _bus.Handle(input);
        var presenter = new GetReceptionLockPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }

    [HttpGet(ApiPath.GetList)]
    public ActionResult<Response<GetReceptionListResponse>> GetList([FromQuery] GetReceptionListRequest request)
    {
        var input = new GetReceptionListInputData(request.HpId, request.SinDate, request.RaiinNo, request.PtId);
        var output = _bus.Handle(input);
        var presenter = new GetReceptionListPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }

    [HttpGet(ApiPath.Get + "Settings")]
    public ActionResult<Response<GetReceptionSettingsResponse>> GetSettings([FromQuery] GetReceptionSettingsRequest req)
    {
        var input = new GetReceptionSettingsInputData(req.UserId);
        var output = _bus.Handle(input);
        var presenter = new GetReceptionSettingsPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }

    [HttpPost("SaveSettings")]
    public ActionResult<Response<SaveVisitingListSettingsResponse>> SaveSettings([FromBody] SaveVisitingListSettingsRequest req)
    {
        var input = new SaveVisitingListSettingsInputData(req.UserId, req.Settings);
        var output = _bus.Handle(input);
        var presenter = new SaveVisitingListSettingsPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }

    [HttpPut(ApiPath.Update + "StaticCell")]
    public async Task<ActionResult<Response<UpdateReceptionStaticCellResponse>>> UpdateStaticCellAsync([FromBody] UpdateReceptionStaticCellRequest req)
    {
        var input = new UpdateReceptionStaticCellInputData(
            req.HpId, req.SinDate, req.RaiinNo, req.PtId, req.CellName, req.CellValue);
        var output = _bus.Handle(input);
        switch (output.Status)
        {
            case UpdateReceptionStaticCellStatus.RaiinInfUpdated:
                await _webSocketService.SendMessageAsync(FunctionCodes.RaiinInfChanged,
                    new CommonMessage { RaiinNo = input.RaiinNo });
                break;
            case UpdateReceptionStaticCellStatus.RaiinCmtUpdated:
                await _webSocketService.SendMessageAsync(FunctionCodes.RaiinCmtChanged,
                    new CommonMessage { RaiinNo = input.RaiinNo });
                break;
            case UpdateReceptionStaticCellStatus.PatientCmtUpdated:
                await _webSocketService.SendMessageAsync(FunctionCodes.PatientCmtChanged,
                    new CommonMessage { PtId = input.PtId });
                break;
        }

        var presenter = new UpdateReceptionStaticCellPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }

    [HttpPut(ApiPath.Update + "DynamicCell")]
    public async Task<ActionResult<Response<UpdateReceptionDynamicCellResponse>>> UpdateDynamicCellAsync([FromBody] UpdateReceptionDynamicCellRequest req)
    {
        var input = new UpdateReceptionDynamicCellInputData(
            req.HpId, req.SinDate, req.RaiinNo, req.PtId, req.GrpId, req.KbnCd);
        var output = _bus.Handle(input);
        if (output.Status == UpdateReceptionDynamicCellStatus.Success)
        {
            await _webSocketService.SendMessageAsync(FunctionCodes.RaiinKubunChanged,
                new CommonMessage { RaiinNo = input.RaiinNo });
        }

        var presenter = new UpdateReceptionDynamicCellPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }
}
