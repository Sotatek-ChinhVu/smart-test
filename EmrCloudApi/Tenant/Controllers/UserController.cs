using EmrCloudApi.Tenant.Presenters;
using EmrCloudApi.Tenant.Presenters.User;
using EmrCloudApi.Tenant.Requests.User;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.User.Create;
using UseCase.User.GetList;

namespace EmrCloudApi.Tenant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public UserController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpPost("Save")]
        public ActionResult<String> Save([FromBody] CreateUserRequest saveUserRequest)
        {
            var input = new CreateUserInputData(saveUserRequest.UserName);
            var output = _bus.Handle(input);

            var presenter = new CreateUserPresenter();
            presenter.Complete(output);

            return null;
        }

        [HttpGet("GetList")]
        public ActionResult<Response<GetUserListResponse>> GetList()
        {
            var input = new GetUserListInputData();
            var output = _bus.Handle(input);

            var presenter = new GetUserListPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<GetUserListResponse>>(presenter.Result);
        }
    }
}
