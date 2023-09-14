using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.SetSendaiGeneration;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.GetSendaiGeneration;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.SetSendaiGeneration.GetList;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class SetSendaiGenerationController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        public SetSendaiGenerationController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpGet(ApiPath.GetList)]
        public ActionResult<Response<SetSendaiGenerationGetListResponse>> GetList()
        {
            var input = new SetSendaiGenerationInputData(HpId);
            var output = _bus.Handle(input);

            var presenter = new SetSendaiGenerationGetListPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<SetSendaiGenerationGetListResponse>>(presenter.Result);
        }
    }
}
