using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.InsuranceList;
using EmrCloudApi.Presenters.MedicalExamination;
using EmrCloudApi.Requests.Insurance;
using EmrCloudApi.Requests.MedicalExamination;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.InsuranceList;
using EmrCloudApi.Responses.MedicalExamination;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Insurance.GetComboList;
using UseCase.Insurance.GetDefaultSelectPattern;
using UseCase.MedicalExamination.AddAutoItem;
using UseCase.MedicalExamination.GetAddedAutoItem;
using UseCase.MedicalExamination.UpsertTodayOrd;
using UseCase.OrdInfs.ValidationTodayOrd;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class TodayOrdController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        public TodayOrdController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpPost(ApiPath.Upsert)]
        public ActionResult<Response<UpsertTodayOdrResponse>> Upsert([FromBody] UpsertTodayOdrRequest request)
        {
            var input = new UpsertTodayOrdInputData(request.SyosaiKbn, request.JikanKbn, request.HokenPid, request.SanteiKbn, request.TantoId, request.KaId, request.UketukeTime, request.SinStartTime, request.SinEndTime, request.OdrInfs.Select(
                    o => new OdrInfItemInputData(
                            HpId,
                            o.RaiinNo,
                            o.RpNo,
                            o.RpEdaNo,
                            o.PtId,
                            o.SinDate,
                            o.HokenPid,
                            o.OdrKouiKbn,
                            o.RpName,
                            o.InoutKbn,
                            o.SikyuKbn,
                            o.SyohoSbt,
                            o.SanteiKbn,
                            o.TosekiKbn,
                            o.DaysCnt,
                            o.SortNo,
                            o.Id,
                            o.OdrDetails.Select(
                                    od => new OdrInfDetailItemInputData(
                                            HpId,
                                            od.RaiinNo,
                                            od.RpNo,
                                            od.RpEdaNo,
                                            od.RowNo,
                                            od.PtId,
                                            od.SinDate,
                                            od.SinKouiKbn,
                                            od.ItemCd,
                                            od.ItemName,
                                            od.Suryo,
                                            od.UnitName,
                                            od.UnitSbt,
                                            od.TermVal,
                                            od.KohatuKbn,
                                            od.SyohoKbn,
                                            od.SyohoLimitKbn,
                                            od.DrugKbn,
                                            od.YohoKbn,
                                            od.Kokuji1,
                                            od.Kokuji2,
                                            od.IsNodspRece,
                                            od.IpnCd,
                                            od.IpnName,
                                            od.JissiKbn,
                                            od.JissiDate,
                                            od.JissiId,
                                            od.JissiMachine,
                                            od.ReqCd,
                                            od.Bunkatu,
                                            od.CmtName,
                                            od.CmtOpt,
                                            od.FontColor,
                                            od.CommentNewline
                                        )
                                ).ToList(),
                            o.IsDeleted
                        )
                ).ToList(),
                new KarteItemInputData(
                    HpId,
                    request.KarteItem.RaiinNo,
                    request.KarteItem.PtId,
                    request.KarteItem.SinDate,
                    request.KarteItem.Text,
                    request.KarteItem.IsDeleted,
                    request.KarteItem.RichText),
                UserId,
                request.KarteFileItems
            );
            var output = _bus.Handle(input);

            var presenter = new UpsertTodayOdrPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<UpsertTodayOdrResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.Validate)]
        public ActionResult<Response<ValidationTodayOrdResponse>> Validate([FromBody] ValidationTodayOrdRequest request)
        {
            var input = new ValidationTodayOrdInputData(
                request.SyosaiKbn,
                request.JikanKbn,
                request.HokenPid,
                request.SanteiKbn,
                request.TantoId,
                request.KaId,
                request.UketukeTime,
                request.SinStartTime,
                request.SinEndTime,
                request.OdrInfs.Select(o =>
                    new ValidationOdrInfItem(
                        HpId,
                        o.RaiinNo,
                        o.RpNo,
                        o.RpEdaNo,
                        o.PtId,
                        o.SinDate,
                        o.HokenPid,
                        o.OdrKouiKbn,
                        o.RpName,
                        o.InoutKbn,
                        o.SikyuKbn,
                        o.SyohoSbt,
                        o.SanteiKbn,
                        o.TosekiKbn,
                        o.DaysCnt,
                        o.SortNo,
                        o.IsDeleted,
                        o.Id,
                        o.OdrDetails.Select(od => new ValidationOdrInfDetailItem(
                            HpId,
                            od.RaiinNo,
                            od.RpNo,
                            od.RpEdaNo,
                            od.RowNo,
                            od.PtId,
                            od.SinDate,
                            od.SinKouiKbn,
                            od.ItemCd,
                            od.ItemName,
                            od.Suryo,
                            od.UnitName,
                            od.UnitSbt,
                            od.TermVal,
                            od.KohatuKbn,
                            od.SyohoKbn,
                            od.SyohoLimitKbn,
                            od.DrugKbn,
                            od.YohoKbn,
                            od.Kokuji1,
                            od.Kokuji2,
                            od.IsNodspRece,
                            od.IpnCd,
                            od.IpnName,
                            od.JissiKbn,
                            od.JissiDate,
                            od.JissiId,
                            od.JissiMachine,
                            od.ReqCd,
                            od.Bunkatu,
                            od.CmtName,
                            od.CmtOpt,
                            od.FontColor,
                            od.CommentNewline
                        )).ToList()
                    )
               ).ToList(),
                new ValidationKarteItem(
                    HpId,
                    request.Karte.RaiinNo,
                    request.Karte.PtId,
                    request.Karte.SinDate,
                    request.Karte.Text,
                    request.Karte.IsDeleted,
                    request.Karte.RichText
                )
               );
            var output = _bus.Handle(input);

            var presenter = new ValidationTodayOrdPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<ValidationTodayOrdResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetDefaultSelectPattern)]
        public ActionResult<Response<GetDefaultSelectPatternResponse>> Validate([FromQuery] GetDefaultSelectPatternRequest request)
        {
            var input = new GetDefaultSelectPatternInputData(
                HpId,
                request.PtId,
                request.SinDate,
                request.HistoryPid,
                request.SelectedHokenPid);

            var output = _bus.Handle(input);

            var presenter = new GetDefaultSelectPatternPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetDefaultSelectPatternResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetInsuranceComboList)]
        public ActionResult<Response<GetInsuranceComboListResponse>> GetInsuranceComboList([FromQuery] GetInsuranceComboListRequest request)
        {
            var input = new GetInsuranceComboListInputData(HpId, request.PtId, request.SinDate);
            var output = _bus.Handle(input);
            var presenter = new GetInsuranceComboListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetInsuranceComboListResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.GetAddedAutoItem)]
        public ActionResult<Response<GetAddedAutoItemResponse>> GetAddedAutoItem([FromBody] GetAddedAutoItemRequest request)
        {
            var input = new GetAddedAutoItemInputData(HpId, request.PtId, request.SinDate, request.OrderInfItems, request.CurrentOrderInfs);
            var output = _bus.Handle(input);
            var presenter = new GetAddedAutoItemPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetAddedAutoItemResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.AddAutoItem)]
        public ActionResult<Response<AddAutoItemResponse>> AddAutoItem([FromBody] AddAutoItemRequest request)
        {
            var input = new AddAutoItemInputData(HpId, request.UserId, request.SinDate, request.AddedOrderInfs, request.OrderInfItems);
            var output = _bus.Handle(input);
            var presenter = new AddAutoItemPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<AddAutoItemResponse>>(presenter.Result);
        }
    }
}
