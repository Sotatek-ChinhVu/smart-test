using Domain.Models.Diseases;
using Domain.Models.MstItem;
using Domain.Models.TodayOdr;
using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.MedicalExamination;
using EmrCloudApi.Tenant.Requests.MedicalExamination;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.MedicalExamination;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.MedicalExamination.GetByomeiFollowItemCd;
using UseCase.MedicalExamination.GetCheckDisease;
using UseCase.MedicalExamination.UpsertTodayOrd;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckedBeforeSaveMedicalController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public CheckedBeforeSaveMedicalController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpPost(ApiPath.GetCheckDiseases)]
        public ActionResult<Response<GetCheckDiseaseResponse>> GetCheckDiseases([FromBody] GetCheckDiseaseRequest request)
        {
            var input = new GetCheckDiseaseInputData(request.HpId, request.SinDate, request.TodayByomeis.Select(b => new PtDiseaseModel(
                    b.HpId,
                    b.PtId,
                    b.SeqNo,
                    b.ByomeiCd,
                    b.SortNo,
                    b.PrefixList.Select(p => new PrefixSuffixModel(p.Code, p.Name)).ToList(),
                    b.Byomei,
                    b.StartDate,
                    b.TenkiKbn,
                    b.TenkiDate,
                    b.SyubyoKbn,
                    b.SikkanKbn,
                    b.NanByoCd,
                    b.IsNodspRece,
                    b.IsNodspKarte,
                    b.IsDeleted,
                    b.Id,
                    b.IsImportant,
                    0,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    b.HokenPid,
                    b.HosokuCmt
                )).ToList(), request.TodayOdrs.Select(
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
                ).ToList());
            var output = _bus.Handle(input);

            var presenter = new GetCheckDiseasePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetCheckDiseaseResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.GetByomeiOfCheckDiseases)]
        public ActionResult<Response<GetByomeiOfCheckDiseaseResponse>> GetByomeiOfCheckDiseases([FromBody] GetByomeiOfCheckDiseaseRequest request)
        {
            var input = new GetByomeiFollowItemCdInputData(request.IsGridStyle, request.HpId, request.ItemCd, request.SinDate, request.TodayByomeis.Select(
                    b => new CheckedDiseaseModel(
                            b.SikkanCd,
                            b.NanByoCd,
                            b.Byomei,
                            b.ItemCd,
                            b.OdrItemNo,
                            b.OdrItemName,
                            new PtDiseaseModel(
                                b.TodayByomeis.HpId,
                                b.TodayByomeis.PtId,
                                b.TodayByomeis.SeqNo,
                                b.TodayByomeis.ByomeiCd,
                                b.TodayByomeis.SortNo,
                                b.TodayByomeis.PrefixList.Select(p => new PrefixSuffixModel(p.Code, p.Name)).ToList(),
                                b.TodayByomeis.Byomei,
                                b.TodayByomeis.StartDate,
                                b.TodayByomeis.TenkiKbn,
                                b.TodayByomeis.TenkiDate,
                                b.TodayByomeis.SyubyoKbn,
                                b.TodayByomeis.SikkanKbn,
                                b.TodayByomeis.NanByoCd,
                                b.TodayByomeis.IsNodspRece,
                                b.TodayByomeis.IsNodspKarte,
                                b.TodayByomeis.IsDeleted,
                                b.TodayByomeis.Id,
                                b.TodayByomeis.IsImportant,
                                0,
                                string.Empty,
                                string.Empty,
                                string.Empty,
                                string.Empty,
                                b.TodayByomeis.HokenPid,
                                b.TodayByomeis.HosokuCmt
                              ),
                            new ByomeiMstModel(
                                b.ByomeiMst.ByomeiCd,
                                b.ByomeiMst.ByomeiType,
                                b.ByomeiMst.Sbyomei,
                                b.ByomeiMst.KanaName1,
                                string.Empty,
                                string.Empty,
                                string.Empty,
                                string.Empty,
                                false
                             )
                        )
                ).ToList());
            var output = _bus.Handle(input);

            var presenter = new GetByomeiOfCheckDiseasePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetByomeiOfCheckDiseaseResponse>>(presenter.Result);
        }
    }
}
