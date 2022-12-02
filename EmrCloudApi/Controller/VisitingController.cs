using EmrCloudApi.Realtime;
using EmrCloudApi.Constants;
using EmrCloudApi.Messages;
using EmrCloudApi.Presenters.Reception;
using EmrCloudApi.Presenters.VisitingList;
using EmrCloudApi.Requests.Reception;
using EmrCloudApi.Requests.ReceptionVisiting;
using EmrCloudApi.Requests.VisitingList;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Reception;
using EmrCloudApi.Responses.ReceptionVisiting;
using EmrCloudApi.Responses.VisitingList;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Reception.GetList;
using UseCase.Reception.GetSettings;
using UseCase.Reception.UpdateDynamicCell;
using UseCase.Reception.UpdateStaticCell;
using UseCase.ReceptionVisiting.Get;
using UseCase.VisitingList.ReceptionLock;
using UseCase.VisitingList.SaveSettings;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class VisitingController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;
    private readonly IWebSocketService _webSocketService;

    public VisitingController(UseCaseBus bus, IWebSocketService webSocketService, IUserService userService) : base(userService)
    {
        _bus = bus;
        _webSocketService = webSocketService;
    }

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
        var input = new GetReceptionListInputData(HpId, request.SinDate, request.RaiinNo, request.PtId);
        var output = _bus.Handle(input);
        var presenter = new GetReceptionListPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }

    [HttpGet(ApiPath.Get + "ReceptionInfo")]
    public ActionResult<Response<GetReceptionVisitingResponse>> GetList([FromQuery] GetReceptionVisitingRequest request)
    {
        var input = new GetReceptionVisitingInputData(HpId, request.RaiinNo);
        var output = _bus.Handle(input);
        var presenter = new GetReceptionVisitingPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }

    [HttpGet(ApiPath.Get + "Settings")]
    public ActionResult<Response<GetReceptionSettingsResponse>> GetSettings([FromQuery] GetReceptionSettingsRequest req)
    {
        var input = new GetReceptionSettingsInputData(UserId);
        var output = _bus.Handle(input);
        var presenter = new GetReceptionSettingsPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }

    [HttpPost("SaveSettings")]
    public ActionResult<Response<SaveVisitingListSettingsResponse>> SaveSettings([FromBody] SaveVisitingListSettingsRequest req)
    {
        var input = new SaveVisitingListSettingsInputData(req.Settings, HpId, UserId);
        var output = _bus.Handle(input);
        var presenter = new SaveVisitingListSettingsPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }

    [HttpPut(ApiPath.Update + "StaticCell")]
    public async Task<ActionResult<Response<UpdateReceptionStaticCellResponse>>> UpdateStaticCellAsync([FromBody] UpdateReceptionStaticCellRequest req)
    {
        var input = new UpdateReceptionStaticCellInputData(
            HpId, req.SinDate, req.RaiinNo, req.PtId, req.CellName, req.CellValue, UserId);
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
        var input = new UpdateReceptionDynamicCellInputData(HpId, req.SinDate, req.RaiinNo, req.PtId, req.GrpId, req.KbnCd, UserId);
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
