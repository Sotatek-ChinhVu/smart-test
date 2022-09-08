using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.Byomei;
using EmrCloudApi.Tenant.Requests.Byomei;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Byomei;
using Microsoft.AspNetCore.Mvc;
using UseCase.Byomei.DiseaseSearch;
using UseCase.Core.Sync;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ByomeiController : ControllerBase
{
    private readonly UseCaseBus _bus;
    public ByomeiController(UseCaseBus bus)
    {
        _bus = bus;
    }
    [HttpGet(ApiPath.GetList)]
    public ActionResult<Response<DiseaseSearchResponse>> DiseaseSearch([FromQuery] DiseaseSearchRequest request)
    {
        var input = new DiseaseSearchInputData(request.IsPrefix, request.IsByomei, request.IsSuffix, request.Keyword, request.PageIndex, request.PageCount);
        var output = _bus.Handle(input);

        var presenter = new DiseaseSearchPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<DiseaseSearchResponse>>(presenter.Result);
    }
}
