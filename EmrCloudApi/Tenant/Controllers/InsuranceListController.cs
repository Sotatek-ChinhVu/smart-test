using EmrCloudApi.Tenant.Presenters.InsuranceList;
using EmrCloudApi.Tenant.Requests.InsuranceList;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.InsuranceList;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.InsuranceList.GetInsuranceListById;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsuranceListController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public InsuranceListController(UseCaseBus bus)
        {
            _bus = bus;
        }
        
        [HttpPost("InsuranceListByPtId")]
        public ActionResult<Response<GetInsuranceListByIdResponse>> GetInsuranceListByPtId([FromBody] GetInsuranceListByIdRequest request)
        {
            var input = new GetInsuranceListByIdInputData(request.ptId);
            var output = _bus.Handle(input);

            var presenter = new GetInsuranceListByIdPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetInsuranceListByIdResponse>>(presenter.Result);
        }
    }
}
