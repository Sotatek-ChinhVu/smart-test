using UseCase.Document.GetListDocComment;

namespace EmrCloudApi.Responses.Document;

public class GetListDocCommentResponse
{
    public GetListDocCommentResponse(List<DocCommentOutputItem> listDocComment)
    {
        ListDocComment = listDocComment.Select(item => new DocCommentDto(item)).ToList();
    }

    public List<DocCommentDto> ListDocComment { get; private set; }
}
