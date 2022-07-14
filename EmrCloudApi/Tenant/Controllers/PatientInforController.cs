using EmrCloudApi.Tenant.Presenters.PatientInformation;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.PatientInformaiton;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.PatientInformation.GetList;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientInforController: ControllerBase
    {
        private readonly UseCaseBus _bus;
        public PatientInforController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpGet("GetList")]
        public ActionResult<Response<GetAllPatientInforResponse>> GetAllPatientInfor()
        {
            var input = new GetAllInputData();
            var output = _bus.Handle(input);

            var present = new GetAllPatientInforPresenter();
            present.Complete(output);

            return new ActionResult<Response<GetAllPatientInforResponse>>(present.Result);
        }
    }
}
