using Domain.Models.SetMst;
using Domain.Models.SuperSetDetail;
using EmrCloudApi.Constants;
using EmrCloudApi.Messages;
using EmrCloudApi.Presenters.SetMst;
using EmrCloudApi.Realtime;
using EmrCloudApi.Requests.SetMst;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SetMst;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UseCase.Core.Sync;
using UseCase.MainMenu.GetOdrSetName;
using UseCase.MainMenu.SaveOdrSet;
using UseCase.SetMst.CopyPasteSetMst;
using UseCase.SetMst.GetList;
using UseCase.SetMst.GetListSetGenerationMst;
using UseCase.SetMst.GetToolTip;
using UseCase.SetMst.ReorderSetMst;
using UseCase.SetMst.SaveSetMst;
using UseCase.SuperSetDetail.GetConversion;
using UseCase.SuperSetDetail.GetSuperSetDetailToDoTodayOrder;
using UseCase.SuperSetDetail.SaveConversion;
using UseCase.SuperSetDetail.SaveSuperSetDetail;
using UseCase.SuperSetDetail.SaveSuperSetDetail.SaveSetByomeiInput;
using UseCase.SuperSetDetail.SaveSuperSetDetail.SaveSetKarteInput;
using UseCase.SuperSetDetail.SaveSuperSetDetail.SaveSetOrderInput;
using UseCase.SuperSetDetail.SuperSetDetail;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class SetController : BaseParamControllerBase
{
    private readonly UseCaseBus _bus;
    private readonly IWebSocketService _webSocketService;
    public SetController(UseCaseBus bus, IHttpContextAccessor httpContextAccessor, IWebSocketService webSocketService) : base(httpContextAccessor)
    {
        _bus = bus;
        _webSocketService = webSocketService;
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
    public async Task<ActionResult<Response<GetSetMstListResponse>>> Save([FromBody] SaveSetMstRequest request)
    {
        var input = new SaveSetMstInputData(request.PtId, request.RaiinNo, request.SinDate, request.SetCd, request.SetKbn, request.SetKbnEdaNo, request.SetName, request.WeightKbn, request.Color, request.IsDeleted, HpId, UserId, request.IsGroup, request.IsAddNew);
        var output = _bus.Handle(input);

        if (output.Status == SaveSetMstStatus.Successed)
        {
            // send string data to socket
            var socketData = JsonSerializer.Serialize(new SuperSetMessage { ReorderSetMstModels = output.SetMstList });
            await _webSocketService.SendMessageAsync(FunctionCodes.SupserSetSaveChanged, socketData);
        }

        var presenter = new SaveSetMstPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetSetMstListResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.Reorder)]
    public async Task<ActionResult<Response<ReorderSetMstResponse>>> Reorder([FromBody] ReorderSetMstRequest request)
    {
        var input = new ReorderSetMstInputData(HpId, request.PtId, request.RaiinNo, request.SinDate, request.DragSetCd, request.DropSetCd, UserId);
        var output = _bus.Handle(input);

        if (output.Status == ReorderSetMstStatus.Successed)
        {
            // send string data to socket
            var socketData = JsonSerializer.Serialize(new SuperSetMessage { ReorderSetMstModels = output.setMstModels ?? new() });
            await _webSocketService.SendMessageAsync(FunctionCodes.SupserSetReorderChanged, socketData);
        }

        var presenter = new ReorderSetMstPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<ReorderSetMstResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.Paste)]
    public async Task<ActionResult<Response<CopyPasteSetMstResponse>>> PasteSetMst([FromBody] CopyPasteSetMstRequest request)
    {
        var input = new CopyPasteSetMstInputData(HpId, UserId, request.PtId, request.RaiinNo, request.SinDate, request.GenerationId, request.CopySetCd, request.PasteSetCd, request.PasteToOtherGroup, request.CopySetKbnEdaNo, request.CopySetKbn, request.PasteSetKbnEdaNo, request.PasteSetKbn);
        var output = _bus.Handle(input);

        if (output.Status == CopyPasteSetMstStatus.Successed)
        {
            // send string data to socket
            var socketData = JsonSerializer.Serialize(new SuperSetMessage { ReorderSetMstModels = output.SetMstModels ?? new() });
            await _webSocketService.SendMessageAsync(FunctionCodes.SuperCopyPasteChanged, socketData);
        }

        var presenter = new CopyPasteSetMstPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<CopyPasteSetMstResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetSuperSetDetail)]
    public ActionResult<Response<GetSuperSetDetailResponse>> GetSuperSetDetail([FromQuery] GetSuperSetDetailRequest request)
    {
        var input = new GetSuperSetDetailInputData(HpId, UserId, request.SetCd, request.Sindate);
        var output = _bus.Handle(input);

        var presenter = new GetSuperSetDetailPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetSuperSetDetailResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.SaveSuperSetDetail)]
    public ActionResult<Response<SaveSuperSetDetailResponse>> SaveSuperSetDetail([FromBody] SaveSuperSetDetailRequest request)
    {
        var input = new SaveSuperSetDetailInputData(
                        request.PtId,
                        request.RaiinNo,
                        request.SinDate,
                        request.SetCd,
                        UserId,
                        HpId,
                        ConvertToSetByomeiModelInputs(request.SaveSetByomeiRequestItems),
                        new SaveSetKarteInputItem(HpId, request.SetCd, request.SaveSetKarteRequestItem.RichText, request.SaveSetKarteRequestItem.Text),
                        ConvertToSetOrderModelInputs(request.SaveSetOrderMstRequestItems),
                        new FileItemInputItem(request.FileItem.IsUpdateFile, request.FileItem.ListFileItems)
                    );
        var output = _bus.Handle(input);

        var presenter = new SaveSuperSetDetailPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SaveSuperSetDetailResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetSuperSetDetailForTodayOrder)]
    public ActionResult<Response<GetSuperSetDetailToDoTodayOrderResponse>> GetSuperSetDetailForTodayOrder([FromQuery] GetSuperSetDetailToDoTodayOrderRequest request)
    {
        var input = new GetSuperSetDetailToDoTodayOrderInputData(HpId, UserId, request.SetCd, request.SinDate);
        var output = _bus.Handle(input);

        var presenter = new GetSuperSetDetailToDoTodayOrderPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetSuperSetDetailToDoTodayOrderResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.GetConversion)]
    public ActionResult<Response<GetConversionResponse>> GetConversion([FromBody] GetConversionRequest request)
    {
        var input = new GetConversionInputData(HpId, request.ItemCd, request.SinDate, request.ItemName, request.Quantity, request.UnitName);
        var output = _bus.Handle(input);

        var presenter = new GetConversionPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetConversionResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.SaveConversion)]
    public ActionResult<Response<SaveConversionResponse>> SaveConversion([FromBody] SaveConversionRequest request)
    {
        var input = new SaveConversionInputData(HpId, UserId, request.SourceItemCd, request.ConversionItemCd, request.DeleteConversionItemCdList);
        var output = _bus.Handle(input);

        var presenter = new SaveConversionPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SaveConversionResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.GetOdrSetName)]
    public ActionResult<Response<GetOdrSetNameResponse>> GetOdrSetName([FromBody] GetOdrSetNameRequest request)
    {
        var setCheckBoxStatus = new SetCheckBoxStatusModel(request.CheckBoxStatus.SetKbnChecked1,
                                                           request.CheckBoxStatus.SetKbnChecked2,
                                                           request.CheckBoxStatus.SetKbnChecked3,
                                                           request.CheckBoxStatus.SetKbnChecked4,
                                                           request.CheckBoxStatus.SetKbnChecked5,
                                                           request.CheckBoxStatus.SetKbnChecked6,
                                                           request.CheckBoxStatus.SetKbnChecked7,
                                                           request.CheckBoxStatus.SetKbnChecked8,
                                                           request.CheckBoxStatus.SetKbnChecked9,
                                                           request.CheckBoxStatus.SetKbnChecked10,
                                                           request.CheckBoxStatus.JihiChecked,
                                                           request.CheckBoxStatus.KihonChecked,
                                                           request.CheckBoxStatus.TokuChecked,
                                                           request.CheckBoxStatus.YohoChecked,
                                                           request.CheckBoxStatus.DiffChecked,
                                                           request.CheckBoxStatus.BuiChecked,
                                                           request.CheckBoxStatus.FreeCommentChecked);
        var input = new GetOdrSetNameInputData(HpId, setCheckBoxStatus, request.GenerationId, request.TimeExpired, request.ItemName);
        var output = _bus.Handle(input);

        var presenter = new GetOdrSetNamePresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetOdrSetNameResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.SaveOdrSet)]
    public async Task<ActionResult<Response<SaveOdrSetResponse>>> SaveOdrSet([FromBody] SaveOdrSetRequest request)
    {
        var odrSetList = request.SetNameModelList.Select(item => new OdrSetNameModel(
                                                                     item.SetCd,
                                                                     item.RowNo,
                                                                     item.ItemCd,
                                                                     item.CmtOpt,
                                                                     item.Quantity,
                                                                     item.SetOrdInfId))
                                                  .ToList();
        var input = new SaveOdrSetInputData(HpId, UserId, request.SinDate, odrSetList, request.UpdateSetNameList.Select(item => new OdrSetNameModel(item.SetCd, item.SetName)).ToList());
        var output = _bus.Handle(input);

        if (output.Status == SaveOdrSetStatus.Successed && output.SetMstModels.Any())
        {
            // send string data to socket
            var socketData = JsonSerializer.Serialize(new SuperSetMessage { ReorderSetMstModels = output.SetMstModels ?? new() });
            await _webSocketService.SendMessageAsync(FunctionCodes.SuperCopyPasteChanged, socketData);
        }
        var presenter = new SaveOdrSetPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SaveOdrSetResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetSetGenerationMstList)]
    public ActionResult<Response<GetSetGenerationMstListResponse>> GetSetGenerationMstList()
    {
        var input = new GetSetGenerationMstListInputData(HpId);
        var output = _bus.Handle(input);

        var presenter = new GetSetGenerationMstListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetSetGenerationMstListResponse>>(presenter.Result);
    }

    private List<SaveSetByomeiInputItem> ConvertToSetByomeiModelInputs(List<SaveSetByomeiRequestItem> requestItems)
    {
        return requestItems.Select(request => new SaveSetByomeiInputItem(
                request.Id,
                request.IsSyobyoKbn,
                request.SikkanKbn,
                request.NanByoCd,
                request.DisplayByomei,  // displayByomei will set the data to SetByomei.Byomei in the database
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
