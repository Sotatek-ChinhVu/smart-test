using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Family;
using EmrCloudApi.Requests.Family;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Family;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Family.GetListFamily;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class FamilyController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;
    public FamilyController(UseCaseBus bus, IUserService userService) : base(userService)
    {
        _bus = bus;
    }

    [HttpGet(ApiPath.GetList)]
    public ActionResult<Response<GetListFamilyResponse>> GetList([FromQuery] GetListFamilyRequest request)
    {
        var input = new GetListFamilyInputData(HpId, request.PtId, request.SinDate);
        var output = _bus.Handle(input);

        var presenter = new GetListFamilyPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetListFamilyResponse>>(presenter.Result);
    }
}
