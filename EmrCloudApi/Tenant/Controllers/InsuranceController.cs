using EmrCloudApi.Tenant.Presenters.InsuranceList;
using EmrCloudApi.Tenant.Requests.Insurance;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.InsuranceList;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Insurance.GetList;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsuranceController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public InsuranceController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpPost("InsuranceListByPtId")]
        public ActionResult<Response<GetInsuranceListResponse>> GetInsuranceListByPtId([FromBody] GetInsuranceListRequest request)
        {
            var input = new GetInsuranceListInputData(request.ptId);
            var output = _bus.Handle(input);

            var presenter = new GetInsuranceListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetInsuranceListResponse>>(presenter.Result);
        }
    }
}