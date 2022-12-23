using Domain.Models.Document;
using UseCase.Document;
using UseCase.Document.ConfirmReplaceDocParam;

namespace Interactor.Document;

public class ConfirmReplaceDocParamInteractor : IConfirmReplaceDocParamInputPort
{
    private readonly IDocumentRepository _documentRepository;

    public ConfirmReplaceDocParamInteractor(IDocumentRepository documentRepository)
    {
        _documentRepository = documentRepository;
    }

    public ConfirmReplaceDocParamOutputData Handle(ConfirmReplaceDocParamInputData inputData)
    {
        try
        {
            var listComment = _documentRepository.GetListDocComment(new List<string>());
            var listReplaceWord = listComment.Select(item => item.ReplaceWord)
                                             .ToList();
            foreach (var replaceWord in listReplaceWord)
            {
                if (!inputData.TextFile.Contains("@" + replaceWord + "@"))
                {
                    var itemRemove = listComment.FirstOrDefault(item => item.ReplaceWord.Equals(replaceWord));
                    if (itemRemove != null)
                    {
                        listComment.Remove(itemRemove);
                    }
                }
            }
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
            return new ConfirmReplaceDocParamOutputData(result, ConfirmReplaceDocParamStatus.Successed);
        }
        catch (Exception)
        {
            return new ConfirmReplaceDocParamOutputData(ConfirmReplaceDocParamStatus.Failed);
        }
        finally
        {
            _documentRepository.ReleaseResource();
        }
    }
}
