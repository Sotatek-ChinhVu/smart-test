using Microsoft.AspNetCore.Mvc;
using SuperAdmin.Presenters.Admin;
using SuperAdmin.Requests.Admin;
using SuperAdmin.Responses;
using SuperAdmin.Responses.Admin;
using UseCase.Core.Sync;
using UseCase.SuperAdmin.Login;

namespace SuperAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UseCaseBus _bus;
        public AdminController(UseCaseBus bus)
        {
            _bus = bus;
        }

        [HttpGet("Login")]
        public ActionResult<Response<LoginResponse>> GetListMst([FromQuery] LoginRequest request)
        {
            var input = new LoginInputData(request.LoginId, request.Password);
            var output = _bus.Handle(input);
            var presenter = new LoginPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<LoginResponse>>(presenter.Result);
        }
    }
}
