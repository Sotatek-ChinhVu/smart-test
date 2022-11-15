using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.NextOrder;
using EmrCloudApi.Tenant.Presenters.NextOrder;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.NextOrder;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.NextOrder.Get;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NextOrderController : ControllerBase
{
    private readonly UseCaseBus _bus;
    public NextOrderController(UseCaseBus bus)
    {
        _bus = bus;
    }

    [HttpGet(ApiPath.Get)]
    public async Task<ActionResult<Response<GetNextOrderResponse>>> Get([FromQuery] GetNextOrderRequest request)
    {
        var input = new GetNextOrderInputData(request.PtId, request.HpId, request.RsvkrtNo, request.SinDate, request.Type, request.UserId);
        var output = await Task.Run(() => _bus.Handle(input));

        var presenter = new GetNextOrderPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetNextOrderResponse>>(presenter.Result);
    }
}
