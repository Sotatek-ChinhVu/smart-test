using Domain.Models.User;
using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.User;
using EmrCloudApi.Tenant.Requests.User;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.User;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.User.Create;
using UseCase.User.GetList;
using UseCase.User.UpsertList;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UseCaseBus _bus;
    private readonly IUserService _userService;

    public UserController(UseCaseBus bus, IUserService userService)
    {
        _bus = bus;
        _userService = userService;
    }

    [HttpPost(ApiPath.Update)]
    public async Task<ActionResult<Response<CreateUserResponse>>> Save([FromBody] CreateUserRequest saveUserRequest)
    {
        var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
        if (!validateToken)
        {
            return new ActionResult<Response<CreateUserResponse>>(new Response<CreateUserResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
        }
        var input = new CreateUserInputData(hpId, saveUserRequest.JobCd, saveUserRequest.JobCd, saveUserRequest.KaId, saveUserRequest.KanaName, saveUserRequest.Name, saveUserRequest.Sname, saveUserRequest.LoginId, saveUserRequest.LoginPass, saveUserRequest.MayakuLicenseNo, saveUserRequest.StartDate, saveUserRequest.Endate, saveUserRequest.SortNo, 0, saveUserRequest.RenkeiCd1, saveUserRequest.DrName);
        var output = await Task.Run(() => _bus.Handle(input));

        var presenter = new CreateUserPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<CreateUserResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetList)]
    public async Task<ActionResult<Response<GetUserListResponse>>> GetList([FromQuery] GetUserListRequest req)
    {
        var input = new GetUserListInputData(req.SinDate, req.IsDoctorOnly);
        var output = await Task.Run(() => _bus.Handle(input));

        var presenter = new GetUserListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetUserListResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.UpsertList)]
    public async Task<ActionResult<Response<UpsertUserResponse>>> Upsert([FromBody] UpsertUserRequest upsertUserRequest)
    {
        var validateToken = int.TryParse(_userService.GetLoginUser().UserId, out int userId);
        if (!validateToken)
        {
            return new ActionResult<Response<UpsertUserResponse>>(new Response<UpsertUserResponse> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
        }
        var upsertUserList = upsertUserRequest.UserInfoList.Select(u => UserInfoRequestToModel(u)).ToList();
        var input = new UpsertUserListInputData(upsertUserList, userId);
        var output = await Task.Run(() => _bus.Handle(input));
        var presenter = new UpsertUserListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<UpsertUserResponse>>(presenter.Result);
    }

    private static UserMstModel UserInfoRequestToModel(UserInfoRequest userInfoRequest)
    {
        return
            new UserMstModel
            (
                userInfoRequest.HpId,
                userInfoRequest.Id,
                userInfoRequest.UserId,
                userInfoRequest.JobCd,
                userInfoRequest.ManagerKbn,
                userInfoRequest.KaId,
                userInfoRequest.KanaName,
                userInfoRequest.Name,
                userInfoRequest.Sname,
                userInfoRequest.DoctorName,
                userInfoRequest.LoginId,
                userInfoRequest.Password,
                userInfoRequest.MayakuLicenseNo,
                userInfoRequest.StartDate,
                userInfoRequest.EndDate,
                userInfoRequest.SortNo,
                userInfoRequest.RenkeiCd,
                userInfoRequest.IsDeleted
            );
    }
}
