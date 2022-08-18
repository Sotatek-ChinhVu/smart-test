using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.Diseases;
using EmrCloudApi.Tenant.Requests.Diseases;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Diseases;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Diseases.GetDiseaseList;
using UseCase.Diseases.Upsert;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiseasesController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public DiseasesController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetPtDiseaseListResponse>> GetDiseaseListMedicalExamination([FromQuery] GetPtDiseaseListRequest request)
        {
            var input = new GetPtDiseaseListInputData(request.HpId, request.PtId, request.SinDate, request.HokenId, request.RequestFrom);
            var output = _bus.Handle(input);

            var presenter = new GetPtDiseaseListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetPtDiseaseListResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.Upsert)]
        public ActionResult<Response<UpsertPtDiseaseListResponse>> Upsert([FromBody] UpsertPtDiseaseListRequest request)
        {
            var input = new UpsertPtDiseaseListInputData(request.PtDiseases.Select(r => new UpsertPtDiseaseListInputItem(
                    r.Id,
                    r.HpId,
                    r.PtId,
                    r.SortNo,
                    r.SyusyokuCd1,
                    r.SyusyokuCd2,
                    r.SyusyokuCd3,
                    r.SyusyokuCd4,
                    r.SyusyokuCd5,
                    r.SyusyokuCd6,
                    r.SyusyokuCd7,
                    r.SyusyokuCd8,
                    r.SyusyokuCd9,
                    r.SyusyokuCd10,
                    r.SyusyokuCd11,
                    r.SyusyokuCd12,
                    r.SyusyokuCd13,
                    r.SyusyokuCd14,
                    r.SyusyokuCd15,
                    r.SyusyokuCd16,
                    r.SyusyokuCd17,
                    r.SyusyokuCd18,
                    r.SyusyokuCd19,
                    r.SyusyokuCd20,
                    r.SyusyokuCd21,
                    r.Byomei,
                    r.StartDate,
                    r.TenkiKbn,
                    r.TenkiDate,
                    r.SyubyoKbn,
                    r.SikkanKbn,
                    r.NanByoCd,
                    r.HosokuCmt,
                    r.HokenPid,
                    r.IsNodspRece,
                    r.IsNodspKarte,
                    r.SeqNo,
                    r.IsImportant,
                    r.IsDeleted
                )).ToList());
            var output = _bus.Handle(input);

            var presenter = new UpsertPtDiseaseListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<UpsertPtDiseaseListResponse>>(presenter.Result);
        }
    }
}
