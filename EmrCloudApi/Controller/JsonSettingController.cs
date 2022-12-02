using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.ColumnSetting;
using EmrCloudApi.Requests.JsonSetting;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.JsonSetting;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.JsonSetting.Get;
using UseCase.JsonSetting.Upsert;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class JsonSettingController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;

    public JsonSettingController(UseCaseBus bus, IUserService userService) : base(userService)
    {
        _bus = bus;
    }

    [HttpGet(ApiPath.Get)]
    public ActionResult<Response<GetJsonSettingResponse>> Get([FromQuery] GetJsonSettingRequest req)
    {
        var input = new GetJsonSettingInputData(UserId, req.Key);
        var output = _bus.Handle(input);
        var presenter = new GetJsonSettingPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }

    [HttpPost(ApiPath.Upsert)]
    public ActionResult<Response<UpsertJsonSettingResponse>> Upsert([FromBody] UpsertJsonSettingRequest req)
    {
        var input = new UpsertJsonSettingInputData(req.Setting);
        var output = _bus.Handle(input);
        var presenter = new UpsertJsonSettingPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }
}
