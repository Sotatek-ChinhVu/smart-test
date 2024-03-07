using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.ColumnSetting;
using EmrCloudApi.Requests.JsonSetting;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.JsonSetting;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.JsonSetting.Get;
using UseCase.JsonSetting.GetAll;
using UseCase.JsonSetting.Upsert;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class JsonSettingController : BaseParamControllerBase
{
    private readonly UseCaseBus _bus;

    public JsonSettingController(UseCaseBus bus, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _bus = bus;
    }

    [HttpGet(ApiPath.Get)]
    public ActionResult<Response<GetJsonSettingResponse>> Get([FromQuery] GetJsonSettingRequest req)
    {
        var input = new GetJsonSettingInputData(HpId, UserId, req.Key);
        var output = _bus.Handle(input);
        var presenter = new GetJsonSettingPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }

    [HttpGet(ApiPath.GetList)]
    public ActionResult<Response<GetAllJsonSettingResponse>> GetList()
    {
        var input = new GetAllJsonSettingInputData(HpId, UserId);
        var output = _bus.Handle(input);
        var presenter = new GetAllJsonSettingPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }

    [HttpPost(ApiPath.Upsert)]
    public ActionResult<Response<UpsertJsonSettingResponse>> Upsert([FromBody] UpsertJsonSettingRequest req)
    {
        var input = new UpsertJsonSettingInputData(HpId, req.Setting);
        var output = _bus.Handle(input);
        var presenter = new UpsertJsonSettingPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }
}
