﻿using EmrCloudApi.Tenant.Constants;
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
            var input = new GetCheckDiseaseInputData(request.HpId, request.SinDate, request.TodayByomeis, request.TodayOdrs.Select(
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
            var input = new GetByomeiFollowItemCdInputData(request.IsGridStyle, request.HpId, request.ItemCd, request.SinDate, request.TodayByomeis);
            var output = _bus.Handle(input);

            var presenter = new GetByomeiOfCheckDiseasePresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetByomeiOfCheckDiseaseResponse>>(presenter.Result);
        }
    }
}
