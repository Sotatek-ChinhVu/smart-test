using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.ColumnSetting;
using EmrCloudApi.Tenant.Requests.JsonSetting;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.JsonSetting;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.JsonSetting.Get;
using UseCase.JsonSetting.Upsert;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JsonSettingController : ControllerBase
{
    private readonly UseCaseBus _bus;

    public JsonSettingController(UseCaseBus bus)
    {
        _bus = bus;
    }

    [HttpGet(ApiPath.Get)]
    public async Task<ActionResult<Response<GetJsonSettingResponse>>> Get([FromQuery] GetJsonSettingRequest req)
    {
        var input = new GetJsonSettingInputData(req.UserId, req.Key);
        var output = await Task.Run(() => _bus.Handle(input));
        var presenter = new GetJsonSettingPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }

    [HttpPost(ApiPath.Upsert)]
    public async Task<ActionResult<Response<UpsertJsonSettingResponse>>> Upsert([FromBody] UpsertJsonSettingRequest req)
    {
        var input = new UpsertJsonSettingInputData(req.Setting);
        var output = await Task.Run(() => _bus.Handle(input));
        var presenter = new UpsertJsonSettingPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }
}
