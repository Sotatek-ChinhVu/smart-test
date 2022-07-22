using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.KarteInfs;
using EmrCloudApi.Tenant.Presenters.MedicalExamination;
using EmrCloudApi.Tenant.Requests.MedicalExamination;
using EmrCloudApi.Tenant.Requests.MedicalExamination.KarteInfs;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.MedicalExamination;
using EmrCloudApi.Tenant.Responses.MedicalExamination.KarteInfs;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.MedicalExamination.GetHistory;
using UseCase.MedicalExamination.KarteInfs.GetLists;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalExaminationController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public MedicalExaminationController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetHistoryList)]
        public ActionResult<Response<GetMedicalExaminationHistoryResponse>> GetList([FromQuery] GetMedicalExaminationHistoryRequest request)
        {
            var input = new GetMedicalExaminationHistoryInputData(request.PtId, request.HpId, request.SinDate, request.PageIndex, request.PageSize);
            var output = _bus.Handle(input);

            var presenter = new GetMedicalExaminationHistoryPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetMedicalExaminationHistoryResponse>>(presenter.Result);
        }
    }
}
