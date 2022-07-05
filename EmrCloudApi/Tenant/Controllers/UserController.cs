using EmrCloudApi.Tenant.Requests.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.User.Create;

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
        public ActionResult<String> Save([FromBody] SaveUserRequest saveUserRequest)
        {
            var input = new CreateUserInputData(saveUserRequest.UserName);
            var output = _bus.Handle(input);

            return Ok("");
        }
    }
}
