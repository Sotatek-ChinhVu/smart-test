using EmrCloudApi.Tenant.Presenters.InsuranceList;
using EmrCloudApi.Tenant.Presenters.PatientInformation;
using EmrCloudApi.Tenant.Requests.Insurance;
using EmrCloudApi.Tenant.Requests.PatientInfor;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.InsuranceList;
using EmrCloudApi.Tenant.Responses.PatientInformaiton;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Insurance.GetList;
using UseCase.PatientInformation.GetById;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientInforController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public PatientInforController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpPost("GetPatientById")]
        public ActionResult<Response<GetPatientInforByIdResponse>> GetPatientById([FromBody] GetByIdRequest request)
        {
            var input = new GetPatientInforByIdInputData(request.PtId);
            var output = _bus.Handle(input);

            var present = new GetPatientInforByIdPresenter();
            present.Complete(output);

            return new ActionResult<Response<GetPatientInforByIdResponse>>(present.Result);
        }

        [HttpPost("InsuranceListByPtId")]
        public ActionResult<Response<GetInsuranceListResponse>> GetInsuranceListByPtId([FromBody] GetInsuranceListRequest request)
        {
            var input = new GetInsuranceListInputData(request.PtId);
            var output = _bus.Handle(input);

            var presenter = new GetInsuranceListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetInsuranceListResponse>>(presenter.Result);
        }
    }
}