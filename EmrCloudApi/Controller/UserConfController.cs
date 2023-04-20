﻿using Domain.Models.UserConf;
using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.UserConf;
using EmrCloudApi.Requests.UserConf;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.UserConf;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.User.GetUserConfList;
using UseCase.User.GetUserConfModelList;
using UseCase.User.Sagaku;
using UseCase.User.UpdateUserConf;
using UseCase.User.UpsertUserConfList;
using UseCase.UserConf.GetListMedicalExaminationConfig;
using UseCase.UserConf.UpdateAdoptedByomeiConfig;
using UseCase.UserConf.UserSettingParam;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class UserConfController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;

    public UserConfController(UseCaseBus bus, IUserService userService) : base(userService)
    {
        _bus = bus;
    }

    [HttpGet(ApiPath.GetList)]
    public ActionResult<Response<GetUserConfListResponse>> GetList([FromQuery] GetUserConfListRequest request)
    {
        var input = new GetUserConfListInputData(HpId, UserId);
        var output = _bus.Handle(input);

        var presenter = new GetUserConfListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetUserConfListResponse>>(presenter.Result);
    }

    [HttpPut(ApiPath.UpdateAdoptedByomeiConfig)]
    public ActionResult<Response<UpdateAdoptedByomeiConfigResponse>> UpdateAdoptedByomeiConfig([FromBody] UpdateAdoptedByomeiConfigRequest request)
    {
        var input = new UpdateAdoptedByomeiConfigInputData(request.AdoptedValue, HpId, UserId);
        var output = _bus.Handle(input);

        var presenter = new UpdateAdoptedByomeiConfigPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<UpdateAdoptedByomeiConfigResponse>>(presenter.Result);
    }

    [HttpPut(ApiPath.Update)]
    public ActionResult<Response<UpdateUserConfResponse>> Update([FromBody] UpdateUserConfRequest request)
    {
        var input = new UpdateUserConfInputData(HpId, UserId, request.GrpCd, request.Value);
        var output = _bus.Handle(input);

        var presenter = new UpdateUserConfPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<UpdateUserConfResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.Sagaku)]
    public ActionResult<Response<SagakuResponse>> Sagaku([FromQuery] SagakuRequest request)
    {
        var input = new SagakuInputData(HpId, UserId, request.FromRece);
        var output = _bus.Handle(input);

        var presenter = new SagakuPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SagakuResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetListMedicalExaminationConfig)]
    public ActionResult<Response<GetListMedicalExaminationConfigResponse>> GetListMedicalExaminationConfig()
    {
        var input = new GetListMedicalExaminationConfigInputData(HpId, UserId);
        var output = _bus.Handle(input);

        var presenter = new GetListMedicalExaminationConfigPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetListMedicalExaminationConfigResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.UpsertUserConfList)]
    public ActionResult<Response<UpsertUserConfListResponse>> UpsertUserConfList(UpsertUserConfListRequest request)
    {
        var input = new UpsertUserConfListInputData(HpId, UserId, request.userConfs.Select(u => ConvertToModel(UserId, u)).ToList());
        var output = _bus.Handle(input);

        var presenter = new UpsertUserConfListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<UpsertUserConfListResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetList + "ForModel")]
    public ActionResult<Response<GetUserConfModelListResponse>> GetList()
    {
        var input = new GetUserConfModelListInputData(HpId, UserId);
        var output = _bus.Handle(input);

        var presenter = new GetUserConfModelListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetUserConfModelListResponse>>(presenter.Result);
    }

    private static UserConfModel ConvertToModel(int userId, UserConfListItem userConfListItem)
    {
        var userConfModel = new UserConfModel(
                userId,
                userConfListItem.GrpCd,
                userConfListItem.GrpItemCd,
                userConfListItem.GrpItemEdaNo,
                userConfListItem.Val,
                userConfListItem.Param
            );

        return userConfModel;
    }

    [HttpPost(ApiPath.GetUserConfParam)]
    public ActionResult<Response<GetUserConfigParamResponse>> GetUserConfParam([FromBody] GetUserConfigParamRequest request)
    {
        var input = new GetUserConfigParamInputData(HpId, UserId, request.GroupCodes.Select(a => new Tuple<int, int>(a.GrpCd, a.GrpItemCd)).ToList());
        var output = _bus.Handle(input);

        var presenter = new GetUserConfigParamPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetUserConfigParamResponse>>(presenter.Result);
    }
}
