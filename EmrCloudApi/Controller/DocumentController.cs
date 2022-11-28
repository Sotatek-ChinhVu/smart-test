using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Document;
using EmrCloudApi.Requests.Document;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Document;
using EmrCloudApi.Services;
using EmrCloudApi.Tenant.Presenters.Document;
using EmrCloudApi.Tenant.Requests.Document;
using EmrCloudApi.Tenant.Responses.Document;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Document.GetDocCategoryDetail;
using UseCase.Document.GetListDocCategory;
using UseCase.Document.SaveListDocCategory;
using UseCase.Document.SortDocCategory;

namespace EmrCloudApi.Controller;

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

    [HttpPost(ApiPath.SaveListDocumentCategory)]
    public ActionResult<Response<SaveListDocCategoryResponse>> SaveList([FromBody] SaveListDocCategoryRequest request)
    {
        var input = new SaveListDocCategoryInputData(HpId, UserId, ConvertToListDocCategoryItem(request));
        var output = _bus.Handle(input);

        var presenter = new SaveListDocCategoryPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SaveListDocCategoryResponse>>(presenter.Result);
    }

    [HttpPut(ApiPath.SortDocCategory)]
    public ActionResult<Response<SortDocCategoryResponse>> SortDocCategory([FromBody] SortDocCategoryRequest request)
    {
        var input = new SortDocCategoryInputData(HpId, UserId, request.MoveInCd, request.MoveOutCd);
        var output = _bus.Handle(input);

        var presenter = new SortDocCategoryPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SortDocCategoryResponse>>(presenter.Result);
    }

    private List<SaveListDocCategoryInputItem> ConvertToListDocCategoryItem(SaveListDocCategoryRequest request)
    {
        return request.ListDocCategory.Select(item => new SaveListDocCategoryInputItem(
                                                    item.CategoryCd,
                                                    item.CategoryName,
                                                    item.SortNo,
                                                    false
                                              )).ToList();
    }
}
