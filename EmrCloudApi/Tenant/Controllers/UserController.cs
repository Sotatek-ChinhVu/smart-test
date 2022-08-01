using EmrCloudApi.Tenant.Constants;
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

        [HttpPost(ApiPath.Update)]
        public ActionResult<Response<CreateUserResponse>> Save([FromBody] CreateUserRequest saveUserRequest)
        {
            var input = new CreateUserInputData(saveUserRequest.HpId, saveUserRequest.JobCd, saveUserRequest.JobCd, saveUserRequest.KaId, saveUserRequest.KanaName, saveUserRequest.Name, saveUserRequest.Sname, saveUserRequest.LoginId, saveUserRequest.LoginPass, saveUserRequest.MayakuLicenseNo, saveUserRequest.StartDate, saveUserRequest.Endate, saveUserRequest.SortNo, 0, saveUserRequest.RenkeiCd1, saveUserRequest.DrName);
            var output = _bus.Handle(input);

            var presenter = new CreateUserPresenter();
            presenter.Complete(output);

            return new ActionResult<Response<CreateUserResponse>>(presenter.Result);
        }

        [HttpGet(ApiPath.GetList)]
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
