using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.SetMst;
using EmrCloudApi.Tenant.Requests.SetMst;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.SetMst;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.SetMst.CopyPasteSetMst;
using UseCase.SetMst.GetList;
using UseCase.SetMst.ReorderSetMst;
using UseCase.SetMst.SaveSetMst;
using UseCase.SupperSetDetail.SupperSetDetail;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SetController : ControllerBase
{
    private readonly UseCaseBus _bus;
    public SetController(UseCaseBus bus)
    {
        _bus = bus;
    }

    [HttpGet(ApiPath.GetList)]
    public ActionResult<Response<GetSetMstListResponse>> GetList([FromQuery] GetSetMstListRequest request)
    {
        var input = new GetSetMstListInputData(request.HpId, request.SetKbn, request.SetKbnEdaNo, request.TextSearch, request.SinDate);
        var output = _bus.Handle(input);

        var presenter = new GetSetMstListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetSetMstListResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.Save)]
    public ActionResult<Response<SaveSetMstResponse>> Save([FromBody] SaveSetMstRequest request)
    {
        var input = new SaveSetMstInputData(request.SinDate, request.SetCd, request.SetKbn, request.SetKbnEdaNo, request.GenerationId, request.Level1, request.Level2, request.Level3, request.SetName, request.WeightKbn, request.Color, request.IsDeleted, request.IsGroup);
        var output = _bus.Handle(input);

        var presenter = new SaveSetMstPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SaveSetMstResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.Reorder)]
    public ActionResult<Response<ReorderSetMstResponse>> Reorder([FromBody] ReorderSetMstRequest request)
    {
        var input = new ReorderSetMstInputData(request.HpId, request.DragSetCd, request.DropSetCd);
        var output = _bus.Handle(input);

        var presenter = new ReorderSetMstPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<ReorderSetMstResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.Paste)]
    public ActionResult<Response<CopyPasteSetMstResponse>> PasteSetMst([FromBody] CopyPasteSetMstRequest request)
    {
        var input = new CopyPasteSetMstInputData(request.HpId, request.UserId, request.CopySetCd, request.PasteSetCd);
        var output = _bus.Handle(input);

        var presenter = new CopyPasteSetMstPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<CopyPasteSetMstResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetSuperSetDetail)]
    public ActionResult<Response<GetSuperSetDetailResponse>> GetSetByomeiList([FromQuery] GetSuperSetDetailRequest request)
    {
        var input = new GetSupperSetDetailInputData(request.HpId, request.SetCd, request.Sindate);
        var output = _bus.Handle(input);

        var presenter = new GetSuperSetDetailPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetSuperSetDetailResponse>>(presenter.Result);
    }
}
