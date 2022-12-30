using UseCase.Document;

namespace EmrCloudApi.Responses.Document.Dto;

public class DocCommentDetailDto
{
    public DocCommentDetailDto(DocCommentDetailOutputItem detailItem)
    {
        Comment = detailItem.Comment;
    }

    public string Comment { get; private set; }
}
