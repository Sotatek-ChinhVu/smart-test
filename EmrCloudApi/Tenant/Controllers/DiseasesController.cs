using EmrCloudApi.Tenant.Presenters.Diseases;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Diseases;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Diseases.GetDiseaseList;

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

        [HttpGet("get-disease-list")]
        public ActionResult<Response<GetDiseaseListResponse>> GetDiseaseListMedicalExamination(int hpId, long ptId, int sinDate)
        {
            var input = new GetDiseaseListInputData(hpId, ptId, sinDate);
            var output = _bus.Handle(input);

            var presenter = new GetDiseaseListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetDiseaseListResponse>>(presenter.Result);
        }
    }
}
