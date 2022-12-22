using Domain.Models.Document;
using UseCase.Document.GetListDocComment;

namespace Interactor.Document;

public class GetListDocCommentInteractor : IGetListDocCommentInputPort
{
    private readonly IDocumentRepository _documentRepository;

    public GetListDocCommentInteractor(IDocumentRepository documentRepository)
    {
        _documentRepository = documentRepository;
    }

    public GetListDocCommentOutputData Handle(GetListDocCommentInputData inputData)
    {
        try
        {
            var listComment = _documentRepository.GetListDocComment(inputData.ListReplaceWord);
            var listCommentDetail = _documentRepository.GetListDocCommentDetail();
            var result = listComment.Select(detail => new DocCommentOutputItem(
                                                    detail.CategoryId,
                                                    detail.CategoryName,
                                                    detail.ReplaceWord,
                                                    listCommentDetail
                                                    .Where(item => item.CategoryId == detail.CategoryId)
                                                    .Select(item => new DocCommentDetailOutputItem(
                                                            item.CategoryId,
                                                            item.Comment
                                                        )).ToList()
                                            )).ToList();
            return new GetListDocCommentOutputData(result, GetListDocCommentStatus.Successed);
        }
        catch (Exception)
        {
            return new GetListDocCommentOutputData(GetListDocCommentStatus.Failed);
        }
        finally
        {
            _documentRepository.ReleaseResource();
        }
    }
}
