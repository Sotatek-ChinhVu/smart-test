using Domain.Models.User;
using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.User;
using EmrCloudApi.Tenant.Requests.User;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.User;
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
    public ActionResult<Response<GetUserListResponse>> GetList([FromQuery] GetUserListRequest req)
    {
        var input = new GetUserListInputData(req.SinDate, req.IsDoctorOnly);
        var output = _bus.Handle(input);

        var presenter = new GetUserListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetUserListResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.UpsertList)]
    public ActionResult<Response<UpsertUserResponse>> Upsert([FromBody] UpsertUserRequest upsertUserRequest)
    {
        var updatedUserList = upsertUserRequest.UserInfoList.Select(u => UserInfoRequestToModel(u)).ToList();
        var insertedUserList = upsertUserRequest.UserInfoList.Select(u => UserInfoRequestToModel(u)).ToList();

        var input = new UpsertUserListInputData(updatedUserList, insertedUserList);
        var output = _bus.Handle(input);
        var presenter = new UpsertUserListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<UpsertUserResponse>>(presenter.Result);
    }

    private UserMstModel UserInfoRequestToModel(UserInfoRequest userInfoRequest)
    {
        return
            new UserMstModel
            (
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
