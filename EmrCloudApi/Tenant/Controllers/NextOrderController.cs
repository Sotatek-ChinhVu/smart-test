using EmrCloudApi.Constants;
using EmrCloudApi.Controller;
using EmrCloudApi.Presenters.NextOrder;
using EmrCloudApi.Requests.NextOrder;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.NextOrder;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.NextOrder.Get;
using UseCase.NextOrder.GetList;
using UseCase.NextOrder.Upsert;

namespace EmrCloudApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class NextOrderController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;
    private readonly IUserService _userService;
    public NextOrderController(UseCaseBus bus, IUserService userService) : base(userService)
    {
        _bus = bus;
        _userService = userService;
    }

    [HttpGet(ApiPath.Get)]
    public async Task<ActionResult<Response<GetNextOrderResponse>>> Get([FromQuery] GetNextOrderRequest request)
    {
        var input = new GetNextOrderInputData(request.PtId, HpId, request.RsvkrtNo, request.SinDate, request.Type, UserId);
        var output = await Task.Run(() => _bus.Handle(input));

        var presenter = new GetNextOrderPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetNextOrderResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetList)]
    public async Task<ActionResult<Response<GetNextOrderListResponse>>> GetList([FromQuery] GetNextOrderListRequest request)
    {
        var input = new GetNextOrderListInputData(request.PtId, HpId, request.RsvkrtKbn, request.IsDeleted);
        var output = await Task.Run(() => _bus.Handle(input));

        var presenter = new GetNextOrderListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetNextOrderListResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.Upsert)]
    public async Task<ActionResult<Response<UpsertNextOrderListResponse>>> Upsert([FromBody] UpsertNextOrderListRequest request)
    {
        var input = new UpsertNextOrderListInputData(request.PtId, HpId, UserId, request.NextOrderItems);
        var output = await Task.Run(() => _bus.Handle(input));

        var presenter = new UpsertNextOrderListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<UpsertNextOrderListResponse>>(presenter.Result);
    }
}
