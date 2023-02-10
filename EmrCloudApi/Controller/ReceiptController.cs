using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Receipt;
using EmrCloudApi.Requests.Receipt;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Receipt.GetReceCmt;
using UseCase.Receipt.ReceiptListAdvancedSearch;

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
        var input = new GetListReceCmtInputData(HpId, request.SinDate, request.PtId, request.HokenId);
        var output = _bus.Handle(input);

        var presenter = new GetListReceCmtPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetListReceCmtResponse>>(presenter.Result);
    }

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
}
