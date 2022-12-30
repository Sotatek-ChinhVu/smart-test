using UseCase.Document.GetListDocComment;

namespace EmrCloudApi.Responses.Document;

public class DocCommentDetailDto
{
    public DocCommentDetailDto(DocCommentDetailOutputItem detailItem)
    {
        Comment = detailItem.Comment;
    }

    public string Comment { get; private set; }
}
