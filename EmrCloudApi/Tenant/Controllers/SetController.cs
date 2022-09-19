using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.SetMst;
using EmrCloudApi.Tenant.Requests.SetMst;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Schema;
using EmrCloudApi.Tenant.Responses.SetMst;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Schema.SaveImageSuperSetDetail;
using UseCase.SetMst.CopyPasteSetMst;
using UseCase.SetMst.GetList;
using UseCase.SetMst.ReorderSetMst;
using UseCase.SetMst.SaveSetMst;
using UseCase.SuperSetDetail.SaveSuperSetDetail;
using UseCase.SuperSetDetail.SaveSuperSetDetail.SaveSetByomeiInput;
using UseCase.SuperSetDetail.SaveSuperSetDetail.SaveSetKarteInput;
using UseCase.SuperSetDetail.SuperSetDetail;

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
    public ActionResult<Response<GetSuperSetDetailResponse>> GetSuperSetDetail([FromQuery] GetSuperSetDetailRequest request)
    {
        var input = new GetSuperSetDetailInputData(request.HpId, request.SetCd, request.Sindate);
        var output = _bus.Handle(input);

        var presenter = new GetSuperSetDetailPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetSuperSetDetailResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.SaveSuperSetDetail)]
    public ActionResult<Response<SaveSuperSetDetailResponse>> SaveSuperSetDetail([FromBody] SaveSuperSetDetailRequest request)
    {
        var input = new SaveSuperSetDetailInputData(
                request.SetCd,
                request.UserId,
                request.HpId,
                ConvertToSetByomeiModelInputs(request.SaveSetByomeiRequestItems),
                new SaveSetKarteInputItem(request.HpId, request.SetCd, request.SaveSetKarteRequestItem.RichText)
            );
        var output = _bus.Handle(input);

        var presenter = new SaveSuperSetDetailPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SaveSuperSetDetailResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.SaveImageSuperSetDetail)]
    public ActionResult<Response<SaveImageResponse>> SaveImageTodayOrder([FromQuery] SaveImageSuperSetDetailRequest request)
    {
        var input = new SaveImageSuperSetDetailInputData(request.HpId, request.SetCd, request.Position, request.OldImage, Request.Body);
        var output = _bus.Handle(input);

        var presenter = new SaveImageSuperSetDetailPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SaveImageResponse>>(presenter.Result);
    }

    private List<SaveSetByomeiInputItem> ConvertToSetByomeiModelInputs(List<SaveSetByomeiRequestItem> requestItems)
    {
        return requestItems.Select(request => new SaveSetByomeiInputItem(
                request.Id,
                request.IsSyobyoKbn,
                request.SikkanKbn,
                request.NanByoCd,
                request.FullByomei,
                request.IsSuspected,
                request.IsDspRece,
                request.IsDspKarte,
                request.ByomeiCmt,
                request.ByomeiCd,
                request.PrefixSuffixList.Select(pre => new PrefixSuffixInputItem(
                            pre.Code,
                            pre.Name
                        )).ToList()
            )).ToList();
    }
}
