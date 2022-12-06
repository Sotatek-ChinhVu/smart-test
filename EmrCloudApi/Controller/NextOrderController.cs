using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.NextOrder;
using EmrCloudApi.Requests.NextOrder;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.NextOrder;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.NextOrder.Get;
using UseCase.NextOrder.GetList;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class NextOrderController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;
    public NextOrderController(UseCaseBus bus, IUserService userService) : base(userService)
    {
        _bus = bus;
    }

    [HttpGet(ApiPath.Get)]
    public ActionResult<Response<GetNextOrderResponse>> Get([FromQuery] GetNextOrderRequest request)
    {
        var input = new GetNextOrderInputData(request.PtId, HpId, request.RsvkrtNo, request.SinDate, request.Type, UserId);
        var output = _bus.Handle(input);

        var presenter = new GetNextOrderPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetNextOrderResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetList)]
    public ActionResult<Response<GetNextOrderListResponse>> GetList([FromQuery] GetNextOrderListRequest request)
    {
        var input = new GetNextOrderListInputData(request.PtId, HpId, request.RsvkrtKbn, request.IsDeleted);
        var output = _bus.Handle(input);

        var presenter = new GetNextOrderListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetNextOrderListResponse>>(presenter.Result);
    }
}
