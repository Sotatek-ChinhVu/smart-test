using Domain.Models.Diseases;
using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.Diseases;
using EmrCloudApi.Tenant.Requests.Diseases;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Diseases;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Diseases.GetDiseaseList;
using UseCase.Diseases.Upsert;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    public class DiseasesController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        public DiseasesController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetPtDiseaseListResponse>> GetDiseaseListMedicalExamination([FromQuery] GetPtDiseaseListRequest request)
        {

            var input = new GetPtDiseaseListInputData(HpId, request.PtId, request.SinDate, request.HokenId, request.RequestFrom);
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
                                                            r.PtId,
                                                            r.SortNo,
                                                            r.PrefixList.Select(p => new PrefixSuffixModel(p.Code, p.Name)).ToList(),
                                                            r.SuffixList.Select(p => new PrefixSuffixModel(p.Code, p.Name)).ToList(),
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
                                                            r.IsDeleted,
                                                            r.ByomeiCd,
                                                            HpId
                                                        )).ToList(),
                                                        HpId,
                                                        UserId
                                                        );
            var output = _bus.Handle(input);

            var presenter = new UpsertPtDiseaseListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<UpsertPtDiseaseListResponse>>(presenter.Result);
        }

    }
}
