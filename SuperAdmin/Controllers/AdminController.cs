using Helper.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SuperAdmin.Presenters.Admin;
using SuperAdmin.Requests.Admin;
using SuperAdmin.Responses;
using SuperAdmin.Responses.Admin;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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

        [HttpPost("Login")]
        public ActionResult<Response<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            var input = new LoginInputData(request.LoginId, request.Password);
            var output = _bus.Handle(input);
            var presenter = new LoginPresenter();
            presenter.Complete(output);
            return new ActionResult<Response<LoginResponse>>(presenter.Result);
        }
    }
}
