using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.UketukeSbt;
using EmrCloudApi.Tenant.Requests.UketukeSbt;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.UketukeSbt;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.UketukeSbtMst.GetBySinDate;
using UseCase.UketukeSbtMst.GetList;
using UseCase.UketukeSbtMst.GetNext;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
public class UketukeSbtController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;

    public UketukeSbtController(UseCaseBus bus, IUserService userService) : base(userService)
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

    [HttpGet(ApiPath.Get + "BySinDate")]
    public ActionResult<Response<GetUketukeSbtMstBySinDateResponse>> GetBySinDate([FromQuery] GetUketukeSbtMstBySinDateRequest req)
    {
        var input = new GetUketukeSbtMstBySinDateInputData(req.SinDate);
        var output = _bus.Handle(input);
        var presenter = new GetUketukeSbtMstBySinDatePresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }

    [HttpGet(ApiPath.Get + "Next")]
    public ActionResult<Response<GetNextUketukeSbtMstResponse>> GetNext([FromQuery] GetNextUketukeSbtMstRequest req)
    {
        var input = new GetNextUketukeSbtMstInputData(req.SinDate, req.CurrentKbnId, UserId);
        var output = _bus.Handle(input);
        var presenter = new GetNextUketukeSbtMstPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }
}
