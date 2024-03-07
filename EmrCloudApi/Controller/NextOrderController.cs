using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.NextOrder;
using EmrCloudApi.Requests.NextOrder;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.NextOrder;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.NextOrder.Check;
using UseCase.NextOrder.CheckNextOrdHaveOdr;
using UseCase.NextOrder.Get;
using UseCase.NextOrder.GetList;
using UseCase.NextOrder.Upsert;
using UseCase.NextOrder.Validation;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class NextOrderController : BaseParamControllerBase
{
    private readonly UseCaseBus _bus;
    public NextOrderController(UseCaseBus bus, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _bus = bus;
    }

    [HttpGet(ApiPath.Get)]
    public ActionResult<Response<GetNextOrderResponse>> Get([FromQuery] GetNextOrderRequest request)
    {
        var input = new GetNextOrderInputData(request.PtId, HpId, request.RsvkrtNo, request.SinDate, request.RsvkrtKbn, UserId);
        var output = _bus.Handle(input);

        var presenter = new GetNextOrderPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetNextOrderResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetList)]
    public ActionResult<Response<GetNextOrderListResponse>> GetList([FromQuery] GetNextOrderListRequest request)
    {
        var input = new GetNextOrderListInputData(request.PtId, HpId, request.IsDeleted);
        var output = _bus.Handle(input);

        var presenter = new GetNextOrderListPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetNextOrderListResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.Upsert)]
    public ActionResult<Response<UpsertNextOrderListResponse>> Upsert([FromBody] UpsertNextOrderListRequest request)
    {
        var input = new UpsertNextOrderListInputData(request.PtId, HpId, UserId, request.NextOrderItems);
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

    [HttpGet(ApiPath.CheckNextOrdHaveOdr)]
    public ActionResult<Response<CheckNextOrdHaveOrdResponse>> CheckNextOrdHaveOdr([FromQuery] CheckNextOrdHaveOdrRequest request)
    {
        var input = new CheckNextOrdHaveOdrInputData(request.PtId, HpId, request.SinDate);
        var output = _bus.Handle(input);

        var presenter = new CheckNextOrdHaveOrdPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<CheckNextOrdHaveOrdResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.CheckUpsertNextOrder)]
    public ActionResult<Response<CheckUpsertNextOrderResponse>> CheckUpsertNextOrder([FromQuery] CheckUpsertNextOrderRequest request)
    {
        var input = new CheckUpsertNextOrderInputData(HpId, request.PtId, request.RsvDate);
        var output = _bus.Handle(input);

        var presenter = new CheckUpsertNextOrderPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<CheckUpsertNextOrderResponse>>(presenter.Result);
    }
}
