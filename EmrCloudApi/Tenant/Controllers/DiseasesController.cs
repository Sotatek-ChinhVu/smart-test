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
    [ApiController]
    public class DiseasesController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        private readonly IUserService _userService;
        public DiseasesController(UseCaseBus bus, IUserService userService)
        {
            _bus = bus;
            _userService = userService;
        }

        [HttpGet(ApiPath.GetList)]
        public async Task<ActionResult<Response<GetPtDiseaseListResponse>>> GetDiseaseListMedicalExamination([FromQuery] GetPtDiseaseListRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<GetPtDiseaseListResponse>>(new Response<GetPtDiseaseListResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            var input = new GetPtDiseaseListInputData(hpId, request.PtId, request.SinDate, request.HokenId, request.RequestFrom);
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new GetPtDiseaseListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetPtDiseaseListResponse>>(presenter.Result);
        }

        [HttpPost(ApiPath.Upsert)]
        public async Task<ActionResult<Response<UpsertPtDiseaseListResponse>>> Upsert([FromBody] UpsertPtDiseaseListRequest request)
        {
            var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
            if (!validateToken)
            {
                return new ActionResult<Response<UpsertPtDiseaseListResponse>>(new Response<UpsertPtDiseaseListResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
            validateToken = int.TryParse(_userService.GetLoginUser().UserId, out int userId);
            if (!validateToken)
            {
                return new ActionResult<Response<UpsertPtDiseaseListResponse>>(new Response<UpsertPtDiseaseListResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
            }
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
                                                            hpId
                                                        )).ToList(),
                                                        hpId,
                                                        userId
                                                        );
            var output = await Task.Run(() => _bus.Handle(input));

            var presenter = new UpsertPtDiseaseListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<UpsertPtDiseaseListResponse>>(presenter.Result);
        }

    }
}
