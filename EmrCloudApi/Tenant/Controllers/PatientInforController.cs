using EmrCloudApi.Tenant.Presenters.PatientInformation;
using EmrCloudApi.Tenant.Requests.PatientInformation;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.PatientInformaiton;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.PatientInformation.GetById;
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

        [HttpGet("GetById")]
        public ActionResult<Response<GetPatientInforByIdResponse>> GetById(int ptId = 0)
        {
            var input = new GetPatientInforByIdInputData(ptId);
            var output = _bus.Handle(input);

            var present = new GetPatientInforByIdPresenter();
            present.Complete(output);

            return new ActionResult<Response<GetPatientInforByIdResponse>>(present.Result);
        }
    }
}
