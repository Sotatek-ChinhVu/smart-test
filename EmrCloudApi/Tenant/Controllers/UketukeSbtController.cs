using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.UketukeSbt;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.UketukeSbt;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.UketukeSbtMst.GetList;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UketukeSbtController : ControllerBase
{
    private readonly UseCaseBus _bus;

    public UketukeSbtController(UseCaseBus bus)
    {
        _bus = bus;
    }

    [HttpGet(ApiPath.GetList + "Mst")]
    public ActionResult<Response<GetUketukeSbtMstListResponse>> GetListMst()
    {
        var input = new GetUketukeSbtMstListInputData();
        var output = _bus.Handle(input);
        var presenter = new GetUketukeSbtMstListPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }
}
