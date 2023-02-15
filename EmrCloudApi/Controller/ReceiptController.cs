using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Receipt;
using EmrCloudApi.Requests.Receipt;
using EmrCloudApi.Requests.Receipt.RequestItem;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Receipt;
using UseCase.Receipt.GetListSyoukiInf;
using UseCase.Receipt.GetReceCmt;
using UseCase.Receipt.ReceiptListAdvancedSearch;
using UseCase.Receipt.SaveListReceCmt;
using UseCase.Receipt.SaveListSyoukiInf;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class ReceiptController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;
    public ReceiptController(UseCaseBus bus, IUserService userService) : base(userService)
    {
        _bus = bus;
    }

    [HttpPost(ApiPath.GetList)]
    public ActionResult<Response<ReceiptListAdvancedSearchResponse>> GetList([FromBody] ReceiptListAdvancedSearchRequest request)
    {
        var input = ConvertToReceiptListAdvancedSearchInputData(HpId, request);
        var output = _bus.Handle(input);

        var presenter = new ReceiptListAdvancedSearchPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<ReceiptListAdvancedSearchResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetListReceCmt)]
    public ActionResult<Response<GetListReceCmtResponse>> GetListReceCmt([FromQuery] GetListReceCmtRequest request)
    {
        var input = new GetListReceCmtInputData(HpId, request.SinYm, request.PtId, request.HokenId);
        var output = _bus.Handle(input);

        var presenter = new GetListReceCmtPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetListReceCmtResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.SaveListReceCmt)]
    public ActionResult<Response<SaveListReceCmtResponse>> SaveListReceCmt([FromBody] SaveListReceCmtRequest request)
    {
        var listReceCmtItem = request.ReceCmtList.Select(item => ConvertToReceCmtItem(item)).ToList();
        var input = new SaveListReceCmtInputData(HpId, UserId, request.PtId, request.SinYm, request.HokenId, listReceCmtItem);
        var output = _bus.Handle(input);

        var presenter = new SaveListReceCmtPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SaveListReceCmtResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetListSyoukiInf)]
    public ActionResult<Response<GetListSyoukiInfResponse>> GetListSyoukiInf([FromQuery] GetListSyoukiInfRequest request)
    {
        var input = new GetListSyoukiInfInputData(HpId, request.SinYm, request.PtId, request.HokenId);
        var output = _bus.Handle(input);

        var presenter = new GetListSyoukiInfPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetListSyoukiInfResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.SaveListSyoukiInf)]
    public ActionResult<Response<SaveListSyoukiInfResponse>> SaveListSyoukiInf([FromBody] SaveListSyoukiInfRequest request)
    {
        var listReceSyoukiInf = request.SyoukiInfList.Select(item => ConvertToSyoukiInfItem(item)).ToList();
        var input = new SaveListSyoukiInfInputData(HpId, UserId, request.PtId, request.SinYm, request.HokenId, listReceSyoukiInf);
        var output = _bus.Handle(input);

        var presenter = new SaveListSyoukiInfPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SaveListSyoukiInfResponse>>(presenter.Result);
    }

    #region Private function
    private ReceiptListAdvancedSearchInputData ConvertToReceiptListAdvancedSearchInputData(int hpId, ReceiptListAdvancedSearchRequest request)
    {
        var itemList = request.ItemList.Select(item => new ItemSearchInputItem(
                                                                                    item.ItemCd,
                                                                                    item.InputName,
                                                                                    item.RangeSeach,
                                                                                    item.Amount,
                                                                                    item.OrderStatus,
                                                                                    item.IsComment
                                                                               )).ToList();

        var byomeiCdList = request.ByomeiCdList.Select(item => new SearchByoMstInputItem(
                                                                                            item.ByomeiCd,
                                                                                            item.InputName,
                                                                                            item.IsComment
                                                                                        )).ToList();

        return new ReceiptListAdvancedSearchInputData(
                hpId,
                request.SeikyuYm,
                request.Tokki,
                request.IsAdvanceSearch,
                request.HokenSbts,
                request.IsAll,
                request.IsNoSetting,
                request.IsSystemSave,
                request.IsSave1,
                request.IsSave2,
                request.IsSave3,
                request.IsTempSave,
                request.IsDone,
                request.ReceSbtCenter,
                request.ReceSbtRight,
                request.HokenHoubetu,
                request.Kohi1Houbetu,
                request.Kohi2Houbetu,
                request.Kohi3Houbetu,
                request.Kohi4Houbetu,
                request.IsIncludeSingle,
                request.HokensyaNoFrom,
                request.HokensyaNoTo,
                request.HokensyaNoFromLong,
                request.HokensyaNoToLong,
                request.PtId,
                request.PtIdFrom,
                request.PtIdTo,
                request.PtSearchOption,
                request.TensuFrom,
                request.TensuTo,
                request.LastRaiinDateFrom,
                request.LastRaiinDateTo,
                request.BirthDayFrom,
                request.BirthDayTo,
                itemList,
                request.ItemQuery,
                request.IsOnlySuspectedDisease,
                request.ByomeiQuery,
                byomeiCdList,
                request.IsFutanIncludeSingle,
                request.FutansyaNoFromLong,
                request.FutansyaNoToLong,
                request.KaId,
                request.DoctorId,
                request.Name,
                request.IsTestPatientSearch,
                request.IsNotDisplayPrinted,
                request.GroupSearchModels,
                request.SeikyuKbnAll,
                request.SeikyuKbnDenshi,
                request.SeikyuKbnPaper
            );
    }

    private ReceCmtItem ConvertToReceCmtItem(SaveListReceCmtRequestItem requestItem)
    {
        return new ReceCmtItem(
                                    requestItem.Id,
                                    requestItem.SeqNo,
                                    requestItem.CmtKbn,
                                    requestItem.CmtSbt,
                                    requestItem.Cmt,
                                    requestItem.CmtData,
                                    requestItem.ItemCd,
                                    requestItem.IsDeleted
                               );
    }

    private SyoukiInfItem ConvertToSyoukiInfItem(SaveListSyoukiInfRequestItem requestItem)
    {
        return new SyoukiInfItem(
                                    requestItem.SeqNo,
                                    requestItem.SortNo,
                                    requestItem.SyoukiKbn,
                                    requestItem.SyoukiKbnStartYm,
                                    requestItem.Syouki,
                                    requestItem.IsDeleted
                               );
    }
    #endregion
}
