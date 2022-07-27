using EmrCloudApi.Tenant.Presenters.GroupInf;
using EmrCloudApi.Tenant.Presenters.PatientInformation;
using EmrCloudApi.Tenant.Requests.GroupInf;
using EmrCloudApi.Tenant.Requests.PatientInfor;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.GroupInf;
using EmrCloudApi.Tenant.Responses.PatientInformaiton;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.GroupInf.GetList;
using UseCase.PatientInfor.SearchSimple;
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

        [HttpGet("GetPatientById")]
        public ActionResult<Response<GetPatientInforByIdResponse>> GetPatientById([FromQuery] GetByIdRequest request)
        {
            var input = new GetPatientInforByIdInputData(request.PtId);
            var output = _bus.Handle(input);

            var present = new GetPatientInforByIdPresenter();
            present.Complete(output);

            return new ActionResult<Response<GetPatientInforByIdResponse>>(present.Result);
        }

        [HttpGet("GetListDataGroup")]
        public ActionResult<Response<GetListGroupInfResponse>> GetListDataGroup([FromQuery] GetListGroupInfRequest request)
        {
            var input = new GetListGroupInfInputData(request.HpId, request.PtId);
            var output = _bus.Handle(input);

            var present = new GetListGroupInfPresenter();
            present.Complete(output);

            return new ActionResult<Response<GetListGroupInfResponse>>(present.Result);
        [HttpGet("SearchSimple")]
        public ActionResult<Response<SearchPatientInforSimpleResponse>> SearchSimple([FromQuery] SearchPatientInfoSimpleRequest request)
        {
            var input = new SearchPatientInfoSimpleInputData(request.Keyword, request.IsContainMode);
            var output = _bus.Handle(input);

            var present = new SearchPatientInfoSimplePresenter();
            present.Complete(output);

            return new ActionResult<Response<SearchPatientInforSimpleResponse>>(present.Result);
        }
    }
}