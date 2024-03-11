using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.WeightedSetConfirmation;
using EmrCloudApi.Requests.WeightedSetConfirmation;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.WeightedSetConfirmation;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.WeightedSetConfirmation.CheckOpen;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeightedSetConfirmationController : BaseParamControllerBase
    {
        private readonly UseCaseBus _bus;
        public WeightedSetConfirmationController(UseCaseBus bus, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _bus = bus;
        }

        [HttpPost(ApiPath.IsOpenWeightChecking)]
        public ActionResult<Response<IsOpenWeightCheckingResponse>> IsOpenWeightChecking([FromBody] IsOpenWeightCheckingRequest request)
        {
            var input = new IsOpenWeightCheckingInputData(HpId, request.SinDate, request.LastDate, request.LastWeight);
            var output = _bus.Handle(input);
            var presenter = new IsOpenWeightCheckingPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<IsOpenWeightCheckingResponse>>(presenter.Result);
        }
    }
}
