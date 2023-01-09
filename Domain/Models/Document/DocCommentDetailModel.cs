namespace Domain.Models.Document;

public class DocCommentDetailModel
{
    public DocCommentDetailModel(int categoryId, string comment)
    {
        CategoryId = categoryId;
        Comment = comment;
    }

    public int CategoryId { get; private set; }

    public string Comment { get; private set; }
}
