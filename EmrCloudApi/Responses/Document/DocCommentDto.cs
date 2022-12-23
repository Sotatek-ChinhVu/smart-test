using UseCase.Document;

namespace EmrCloudApi.Responses.Document;

public class DocCommentDto
{
    public DocCommentDto(DocCommentOutputItem commentItem)
    {
        CategoryId = commentItem.CategoryId;
        CategoryName = commentItem.CategoryName;
        ReplaceWord = commentItem.ReplaceWord;
        DocCommentDetails = commentItem.DocCommentDetails.Select(item => new DocCommentDetailDto(item)).ToList();
    }

    public int CategoryId { get; private set; }

    public string CategoryName { get; private set; }

    public string ReplaceWord { get; private set; }

    public List<DocCommentDetailDto> DocCommentDetails { get; private set; }
}
