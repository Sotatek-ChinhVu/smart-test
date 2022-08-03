using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.UketukeSbtDayInf;
using EmrCloudApi.Tenant.Requests.UketukeSbtDayInf;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.UketukeSbtDayInf;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.UketukeSbtDayInf.Upsert;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UketukeSbtDayInfController : ControllerBase
{
    private readonly UseCaseBus _bus;

    public UketukeSbtDayInfController(UseCaseBus bus)
    {
        _bus = bus;
    }

    [HttpPost(ApiPath.Upsert)]
    public ActionResult<Response<UpsertUketukeSbtDayInfResponse>> Upsert([FromBody] UpsertUketukeSbtDayInfRequest req)
    {
        var input = new UpsertUketukeSbtDayInfInputData(req.SinDate, req.UketukeSbt, req.SeqNo);
        var output = _bus.Handle(input);
        var presenter = new UpsertUketukeSbtDayInfPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }
}
