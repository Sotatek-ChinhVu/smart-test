using EmrCloudApi.Tenant.Presenters.InsuranceInfor;
using EmrCloudApi.Tenant.Requests.InsuranceInfor;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.IsuranceInfor;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.InsuranceInfor.Get;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsuranceInforController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public InsuranceInforController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpPost("Get")]
        public ActionResult<Response<GetInsuranceInforResponse>> GetInsuranceInfor([FromBody] GetInsuranceInforRequest request)
        {
            var input = new GetInsuranceInforInputData(request.PtId, request.HokenId);
            var output = _bus.Handle(input);

            var presenter = new GetInsuranceInforPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetInsuranceInforResponse>>(presenter.Result);
        }
    }
}
