using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.NextOrder;
using EmrCloudApi.Tenant.Requests.NextOrder;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.NextOrder;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.NextOrder.GetList;
using UseCase.NextOrder.Get;
using System.Threading.Tasks;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class NextOrderController : ControllerBase
{
    private readonly UseCaseBus _bus;
    private readonly IUserService _userService;
    public NextOrderController(UseCaseBus bus, IUserService userService)
    {
        _bus = bus;
        _userService = userService;
    }

    [HttpGet(ApiPath.Get)]
    public async Task<ActionResult<Response<GetNextOrderResponse>>> Get([FromQuery] GetNextOrderRequest request)
    {
        var user = _userService.GetLoginUser();
        int.TryParse(user.HpId, out int hpId);
        int.TryParse(user.UserId, out int userId);

        var input = new GetNextOrderInputData(request.PtId, hpId, request.RsvkrtNo, request.SinDate, request.Type, userId);
        var output = await Task.Run(() => _bus.Handle(input));

        var presenter = new GetNextOrderPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetNextOrderResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetList)]
    public async Task<ActionResult<Response<GetNextOrderListResponse>>> GetList([FromQuery] GetNextOrderListRequest request)
    {
        int.TryParse(_userService.GetLoginUser().HpId, out int hpId);

        var input = new GetNextOrderListInputData(request.PtId, hpId, request.RsvkrtKbn, request.IsDeleted);
        var output = await Task.Run(() => _bus.Handle(input));

        var presenter = new GetNextOrderListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetNextOrderListResponse>>(presenter.Result);
    }
}
