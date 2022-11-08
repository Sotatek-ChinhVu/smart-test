using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.NextOrder;
using EmrCloudApi.Tenant.Requests.NextOrder;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.NextOrder;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.NextOrder.GetList;

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

    [HttpGet(ApiPath.GetList)]
    public async Task<ActionResult<Response<GetNextOrderListResponse>>> GetList([FromQuery] GetNextOrderListRequest request)
    {
        var input = new GetNextOrderListInputData(request.PtId, request.HpId, request.RsvkrtKbn, request.IsDeleted);
        var output = await Task.Run(() => _bus.Handle(input));

        var presenter = new GetNextOrderListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetNextOrderListResponse>>(presenter.Result);
    }
}
