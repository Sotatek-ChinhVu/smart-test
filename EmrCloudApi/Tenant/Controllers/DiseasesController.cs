using EmrCloudApi.Tenant.Constants;
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

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<GetPtDiseaseListResponse>> GetDiseaseListMedicalExamination(int hpId, long ptId, int sinDate, int hokenId , int diseaseViewModel)
        {
            var input = new GetPtDiseaseListInputData(hpId, ptId, sinDate, hokenId, diseaseViewModel);
            var output = _bus.Handle(input);

            var presenter = new GetPtDiseaseListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetPtDiseaseListResponse>>(presenter.Result);
        }
    }
}
