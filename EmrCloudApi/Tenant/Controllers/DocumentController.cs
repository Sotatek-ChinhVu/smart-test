using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.Document;
using EmrCloudApi.Tenant.Requests.Document;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Document;
using EmrCloudApi.Tenant.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Document.GetDocCategoryDetail;
using UseCase.Document.GetListDocCategory;

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
    public ActionResult<Response<GetListDocCategoryResponse>> GetList()
    {
        var input = new GetListDocCategoryInputData(HpId);
        var output = _bus.Handle(input);

        var presenter = new GetListDocCategoryPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetListDocCategoryResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetDetailDocumentCategory)]
    public ActionResult<Response<GetDocCategoryDetailResponse>> GetDetail([FromQuery] GetDocCategoryDetailRequest request)
    {
        var input = new GetDocCategoryDetailInputData(HpId, request.CategoryCd);
        var output = _bus.Handle(input);

        var presenter = new GetDocCategoryDetailPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetDocCategoryDetailResponse>>(presenter.Result);
    }
}
