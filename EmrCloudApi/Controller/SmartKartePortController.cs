using Domain.Models.SmartKartePort;
using EmrCloudApi.Presenters.SmartKartePort;
using EmrCloudApi.Requests.SmartKartePort;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SmartKartePort;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.SmartKartePort.GetPort;
using UseCase.SmartKartePort.UpdatePort;

namespace EmrCloudApi.Controller
{
    [Route("api/[controller]")]
    public class SmartKartePortController : AuthorizeControllerBase
    {
        private readonly UseCaseBus _bus;
        public SmartKartePortController(UseCaseBus bus, IUserService userService) : base(userService)
        {
            _bus = bus;
        }

        [HttpPost("UpdatePort")]
        public ActionResult<Response<UpdatePortResponse>> UpdatePort(UpdatePortRequest request)
        {
            var model = new SmartKarteAppSignalRPortModel(request.PortNumber, request.MachineName, request.Ip);
            var input = new UpdatePortInputData(UserId, model);
            var output = _bus.Handle(input);

            var presenter = new UpdatePortPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<UpdatePortResponse>>(presenter.Result);
        }

        [HttpGet("GetPort")]
        public ActionResult<Response<GetPortResponse>> GetPort([FromQuery] GetPortRequest request)
        {
            var input = new GetPortInputData(request.MachineName.Trim(), request.Ip.Trim());
            var output = _bus.Handle(input);
            var presenter = new GetPortPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<GetPortResponse>>(presenter.Result);
        }
    }
}
