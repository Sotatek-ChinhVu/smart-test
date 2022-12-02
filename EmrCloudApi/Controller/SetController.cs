using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.SetMst;
using EmrCloudApi.Requests.SetMst;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Schema;
using EmrCloudApi.Responses.SetMst;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Schema.SaveImageSuperSetDetail;
using UseCase.SetMst.CopyPasteSetMst;
using UseCase.SetMst.GetList;
using UseCase.SetMst.GetToolTip;
using UseCase.SetMst.ReorderSetMst;
using UseCase.SetMst.SaveSetMst;
using UseCase.SuperSetDetail.GetSuperSetDetailToDoTodayOrder;
using UseCase.SuperSetDetail.SaveSuperSetDetail;
using UseCase.SuperSetDetail.SaveSuperSetDetail.SaveSetByomeiInput;
using UseCase.SuperSetDetail.SaveSuperSetDetail.SaveSetKarteInput;
using UseCase.SuperSetDetail.SaveSuperSetDetail.SaveSetOrderInput;
using UseCase.SuperSetDetail.SuperSetDetail;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class SetController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;
    public SetController(UseCaseBus bus, IUserService userService) : base(userService)
    {
        _bus = bus;
    }

    [HttpGet(ApiPath.GetList)]
    public ActionResult<Response<GetSetMstListResponse>> GetList([FromQuery] GetSetMstListRequest request)
    {
        var input = new GetSetMstListInputData(HpId, request.SetKbn, request.SetKbnEdaNo, request.TextSearch, request.SinDate);
        var output = _bus.Handle(input);

        var presenter = new GetSetMstListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetSetMstListResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetToolTip)]
    public ActionResult<Response<GetSetMstToolTipResponse>> GetToolTip([FromQuery] GetSetMstToolTipRequest request)
    {
        var input = new GetSetMstToolTipInputData(HpId, request.SetCd);
        var output = _bus.Handle(input);

        var presenter = new GetSetMstToolTipPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetSetMstToolTipResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.Save)]
    public ActionResult<Response<SaveSetMstResponse>> Save([FromBody] SaveSetMstRequest request)
    {
        var input = new SaveSetMstInputData(request.SinDate, request.SetCd, request.SetKbn, request.SetKbnEdaNo, request.GenerationId, request.Level1, request.Level2, request.Level3, request.SetName, request.WeightKbn, request.Color, request.IsDeleted, HpId, UserId, request.IsGroup);
        var output = _bus.Handle(input);

        var presenter = new SaveSetMstPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SaveSetMstResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.Reorder)]
    public ActionResult<Response<ReorderSetMstResponse>> Reorder([FromBody] ReorderSetMstRequest request)
    {
        var input = new ReorderSetMstInputData(HpId, request.DragSetCd, request.DropSetCd, UserId);
        var output = _bus.Handle(input);

        var presenter = new ReorderSetMstPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<ReorderSetMstResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.Paste)]
    public ActionResult<Response<CopyPasteSetMstResponse>> PasteSetMst([FromBody] CopyPasteSetMstRequest request)
    {
        var input = new CopyPasteSetMstInputData(HpId, UserId, request.CopySetCd, request.PasteSetCd);
        var output = _bus.Handle(input);

        var presenter = new CopyPasteSetMstPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<CopyPasteSetMstResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetSuperSetDetail)]
    public ActionResult<Response<GetSuperSetDetailResponse>> GetSuperSetDetail([FromQuery] GetSuperSetDetailRequest request)
    {
        var input = new GetSuperSetDetailInputData(HpId, request.SetCd, request.Sindate);
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
                        UserId,
                        HpId,
                        ConvertToSetByomeiModelInputs(request.SaveSetByomeiRequestItems),
                        new SaveSetKarteInputItem(HpId, request.SetCd, request.SaveSetKarteRequestItem.RichText),
                        ConvertToSetOrderModelInputs(request.SaveSetOrderMstRequestItems)
                    );
        var output = _bus.Handle(input);

        var presenter = new SaveSuperSetDetailPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SaveSuperSetDetailResponse>>(presenter.Result);
    }


    [HttpPost(ApiPath.SaveImageSuperSetDetail)]
    public ActionResult<Response<SaveImageResponse>> SaveImageTodayOrder([FromQuery] SaveImageSuperSetDetailRequest request)
    {
        var input = new SaveImageSuperSetDetailInputData(HpId, request.SetCd, request.Position, request.OldImage, Request.Body);
        var output = _bus.Handle(input);

        var presenter = new SaveImageSuperSetDetailPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SaveImageResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetSuperSetDetailForTodayOrder)]
    public ActionResult<Response<GetSuperSetDetailToDoTodayOrderResponse>> GetSuperSetDetailForTodayOrder([FromQuery] GetSuperSetDetailToDoTodayOrderRequest request)
    {
        var input = new GetSuperSetDetailToDoTodayOrderInputData(request.HpId, request.SetCd, request.SinDate);
        var output = _bus.Handle(input);

        var presenter = new GetSuperSetDetailToDoTodayOrderPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetSuperSetDetailToDoTodayOrderResponse>>(presenter.Result);
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

    private List<SaveSetOrderInfInputItem> ConvertToSetOrderModelInputs(List<SaveSetOrderMstRequestItem> saveSetOrderMstRequestItems)
    {
        var result = saveSetOrderMstRequestItems.Select(mst =>
                new SaveSetOrderInfInputItem(
                        mst.Id,
                        mst.RpNo,
                        mst.RpEdaNo,
                        mst.OdrKouiKbn,
                        mst.RpName,
                        mst.InoutKbn,
                        mst.SikyuKbn,
                        mst.SyohoSbt,
                        mst.SanteiKbn,
                        mst.TosekiKbn,
                        mst.DaysCnt,
                        mst.SortNo,
                        mst.IsDeleted,
                        mst.OrdInfDetails.Select(detail =>
                            new SetOrderInfDetailInputItem(
                                    detail.SinKouiKbn,
                                    detail.ItemCd,
                                    detail.ItemName,
                                    detail.Suryo,
                                    detail.UnitName,
                                    detail.UnitSBT,
                                    detail.TermVal,
                                    detail.KohatuKbn,
                                    detail.SyohoKbn,
                                    detail.SyohoLimitKbn,
                                    detail.DrugKbn,
                                    detail.YohoKbn,
                                    detail.Kokuji1,
                                    detail.Kokuji2,
                                    detail.IsNodspRece,
                                    detail.IpnCd,
                                    detail.IpnName,
                                    detail.Bunkatu,
                                    detail.CmtName,
                                    detail.CmtOpt,
                                    detail.FontColor,
                                    detail.CommentNewline
                                )).ToList()
                    )
            ).ToList();
        return result;
    }
}
