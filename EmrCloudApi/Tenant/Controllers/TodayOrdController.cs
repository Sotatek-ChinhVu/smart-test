using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.MedicalExamination;
using EmrCloudApi.Tenant.Requests.MedicalExamination;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.MedicalExamination;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.MedicalExamination.UpsertTodayOrd;
using UseCase.OrdInfs.ValidationTodayOrd;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodayOrdController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public TodayOrdController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpPost(ApiPath.Upsert)]
        public ActionResult<Response<UpsertTodayOdrResponse>> Upsert([FromBody] UpsertTodayOdrRequest request)
        {
            var input = new UpsertTodayOrdInputData(request.SyosaiKbn, request.JikanKbn, request.HokenPid, request.SanteiKbn, request.TantoId, request.KaId, request.UketukeTime, request.SinStartTime, request.SinEndTime, request.OdrInfs.Select(
                    o => new OdrInfItemInputData(
                            o.HpId,
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
                                            od.HpId,
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
                    request.KarteItem.HpId,
                    request.KarteItem.RaiinNo,
                    request.KarteItem.PtId,
                    request.KarteItem.SinDate,
                    request.KarteItem.Text,
                    request.KarteItem.IsDeleted,
                    request.KarteItem.RichText)
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
                        o.HpId,
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
                            od.HpId,
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
                    request.Karte.HpId,
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
    }
}
