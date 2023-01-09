using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.NextOrder;
using EmrCloudApi.Tenant.Requests.NextOrder;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.NextOrder;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.NextOrder.Get;
using UseCase.NextOrder.GetList;
using UseCase.NextOrder.Upsert;
using UseCase.NextOrder.Validation;

namespace EmrCloudApi.Tenant.Controllers;

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

    [HttpPost(ApiPath.Upsert)]
    public ActionResult<Response<UpsertNextOrderListResponse>> Upsert([FromBody] UpsertNextOrderListRequest request)
    {
        var input = new UpsertNextOrderListInputData(request.PtId, HpId, UserId, request.NextOrderItems, new FileItemInputItem(request.FileItem.IsUpdateFile, request.FileItem.ListFileItems));
        var output = _bus.Handle(input);

        var presenter = new UpsertNextOrderListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<UpsertNextOrderListResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.Validate)]
    public ActionResult<Response<ValidationNextOrderListResponse>> Validate([FromBody] ValidationNextOrderListRequest request)
    {
        var input = new ValidationNextOrderListInputData(request.PtId, HpId, UserId, request.NextOrderItems);
        var output = _bus.Handle(input);

        var presenter = new ValidationNextOrderListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<ValidationNextOrderListResponse>>(presenter.Result);
    }
}
