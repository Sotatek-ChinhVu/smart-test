using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.InsuranceMst;
using EmrCloudApi.Requests.InsuranceMst;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.InsuranceMst;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.InsuranceMst.GetMasterDetails;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsuranceMstController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        public InsuranceMstController(UseCaseBus bus, IUserService userService) : base(userService) =>_bus = bus;

        [HttpGet(ApiPath.GetList + "InsuranceMstDetail")]
        public ActionResult<Response<GetInsuranceMasterDetailResponse>> GetList([FromQuery] GetInsuranceMasterDetailRequest request)
        {
            var input = new GetInsuranceMasterDetailInputData(HpId, request.FHokenNo, request.FHokenSbtKbn , request.IsJitan , request.IsTaken);
            var output = _bus.Handle(input);
            var presenter = new GetInsuranceMasterDetailPresenter();
            presenter.Complete(output);
            return Ok(presenter.Result);
        }
    }
}
