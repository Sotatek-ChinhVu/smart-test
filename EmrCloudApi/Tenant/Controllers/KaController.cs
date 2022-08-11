using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.Ka;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Ka;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.KaMst.GetList;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
[ApiController]
public class KaController : ControllerBase
{
    private readonly UseCaseBus _bus;

    public KaController(UseCaseBus bus)
    {
        _bus = bus;
    }

    [HttpGet(ApiPath.GetList + "Mst")]
    public ActionResult<Response<GetKaMstListResponse>> GetListMst()
    {
        var input = new GetKaMstListInputData();
        var output = _bus.Handle(input);
        var presenter = new GetKaMstListPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }
}
