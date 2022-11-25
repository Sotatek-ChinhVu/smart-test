using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.Document;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Document;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Document.GetListDocCategoryMst;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]

public class DocumentController : AuthorizeControllerBase
{
    private readonly UseCaseBus _bus;
    public DocumentController(UseCaseBus bus, IUserService userService) : base(userService)
    {
        _bus = bus;
    }

    [HttpGet(ApiPath.GetListDocumentCategory)]
    public ActionResult<Response<GetListDocCategoryMstResponse>> GetList()
    {
        var input = new GetListDocCategoryMstInputData(HpId);
        var output = _bus.Handle(input);

        var presenter = new GetListDocCategoryMstPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetListDocCategoryMstResponse>>(presenter.Result);
    }
}
