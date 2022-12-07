using EmrCloudApi.Constants;
using EmrCloudApi.Presenters.Document;
using EmrCloudApi.Requests.Document;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Document;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Document.AddTemplateToCategory;
using UseCase.Document.CheckExistFileName;
using UseCase.Document.DeleteDocInf;
using UseCase.Document.GetDocCategoryDetail;
using UseCase.Document.GetListDocCategory;
using UseCase.Document.SaveDocInf;
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
    public ActionResult<Response<GetListDocCategoryResponse>> GetList([FromQuery] GetListDocCategoryRequest request)
    {
        var input = new GetListDocCategoryInputData(HpId, request.PtId);
        var output = _bus.Handle(input);

        var presenter = new GetListDocCategoryPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<GetListDocCategoryResponse>>(presenter.Result);
    }

    [HttpGet(ApiPath.GetDetailDocumentCategory)]
    public ActionResult<Response<GetDocCategoryDetailResponse>> GetDetail([FromQuery] GetDocCategoryDetailRequest request)
    {
        var input = new GetDocCategoryDetailInputData(HpId, request.PtId, request.CategoryCd);
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

    [HttpPost(ApiPath.AddTemplateToCategory)]
    public ActionResult<Response<AddTemplateToCategoryResponse>> AddTemplateToCategory([FromQuery] AddTemplateToCategoryRequest request)
    {
        var input = new AddTemplateToCategoryInputData(HpId, request.FileName, request.CategoryCd, Request.Body);
        var output = _bus.Handle(input);

        var presenter = new AddTemplateToCategoryPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<AddTemplateToCategoryResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.CheckExistFileName)]
    public ActionResult<Response<CheckExistFileNameResponse>> CheckExistFileName([FromBody] CheckExistFileNameRequest request)
    {
        var input = new CheckExistFileNameInputData(HpId, request.FileName, request.CategoryCd, request.PtId, request.IsCheckDocInf);
        var output = _bus.Handle(input);

        var presenter = new CheckExistFileNamePresenter();
        presenter.Complete(output);

        return new ActionResult<Response<CheckExistFileNameResponse>>(presenter.Result);
    }

    [HttpPost(ApiPath.SaveDocInf)]
    public ActionResult<Response<SaveDocInfResponse>> SaveDocInf([FromQuery] SaveDocInfRequest request)
    {
        var input = new SaveDocInfInputData(HpId, UserId, request.PtId, request.SinDate, request.RaiinNo, request.SeqNo, request.CategoryCd, request.FileName, request.DisplayFileName, Request.Body);
        var output = _bus.Handle(input);

        var presenter = new SaveDocInfPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<SaveDocInfResponse>>(presenter.Result);
    }

    [HttpPut(ApiPath.DeleteDocInf)]
    public ActionResult<Response<DeleteDocInfResponse>> DeleteDocInf([FromBody] DeleteDocInfRequest request)
    {
        var input = new DeleteDocInfInputData(HpId, UserId, request.PtId, request.SinDate, request.RaiinNo, request.SeqNo);
        var output = _bus.Handle(input);

        var presenter = new DeleteDocInfPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<DeleteDocInfResponse>>(presenter.Result);
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
