﻿using Domain.Models.User;
using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.User;
using EmrCloudApi.Requests.User;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.User;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.User.CheckedLockMedicalExamination;
using UseCase.User.Create;
using UseCase.User.GetAllPermission;
using UseCase.User.GetList;
using UseCase.User.GetPermissionByScreenCode;
using UseCase.User.UpsertList;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class UserController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;

    public UserController(UseCaseBus bus, IUserService userService) : base(userService)
    {
        _bus = bus;
    }

    [HttpPost(ApiPath.Update)]
    public ActionResult<Response<CreateUserResponse>> Save([FromBody] CreateUserRequest saveUserRequest)
    {
        var input = new CreateUserInputData(HpId, saveUserRequest.JobCd, saveUserRequest.JobCd, saveUserRequest.KaId, saveUserRequest.KanaName, saveUserRequest.Name, saveUserRequest.Sname, saveUserRequest.LoginId, saveUserRequest.LoginPass, saveUserRequest.MayakuLicenseNo, saveUserRequest.StartDate, saveUserRequest.Endate, saveUserRequest.SortNo, 0, saveUserRequest.RenkeiCd1, saveUserRequest.DrName);
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
        var upsertUserList = upsertUserRequest.UserInfoList.Select(u => UserInfoRequestToModel(u, HpId)).ToList();
        var input = new UpsertUserListInputData(upsertUserList, UserId);
        var output = _bus.Handle(input);
        var presenter = new UpsertUserListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<UpsertUserResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.CheckLockMedicalExamination)]
    public ActionResult<Response<CheckedLockMedicalExaminationResponse>> CheckLockMedicalExamination([FromQuery] CheckedLockMedicalExaminationRequest request)
    {
        var input = new CheckedLockMedicalExaminationInputData(HpId, request.PtId, request.RaiinNo, request.SinDate, Token, UserId);
        var output = _bus.Handle(input);
        var presenter = new CheckedLockMedicalExaminationPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<CheckedLockMedicalExaminationResponse>>(presenter.Result);
    }


    [HttpGet(ApiPath.GetPermissionByScreen)]
    public ActionResult<Response<GetPermissionByScreenResponse>> GetPermissionByScreen([FromQuery] GetPermissionByScreenRequest request)
    {
        var input = new GetPermissionByScreenInputData(HpId, UserId, request.PermissionCode);
        var output = _bus.Handle(input);
        var presenter = new GetPermissionByScreenPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetPermissionByScreenResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetAllPermission)]
    public ActionResult<Response<GetAllPermissionResponse>> GetAllPermission([FromQuery] GetPermissionByScreenRequest request)
    {
        var input = new GetAllPermissionInputData(HpId, UserId);
        var output = _bus.Handle(input);
        var presenter = new GetAllPermissionPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetAllPermissionResponse>>(presenter.Result);
    }

    private static UserMstModel UserInfoRequestToModel(UserInfoRequest userInfoRequest, int HpId)
    {
        return
            new UserMstModel
            (
                HpId,
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
