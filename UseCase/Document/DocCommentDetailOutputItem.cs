namespace UseCase.Document;

public class DocCommentDetailOutputItem
{
    public DocCommentDetailOutputItem(int categoryId, string comment)
    {
        CategoryId = categoryId;
        Comment = comment;
    }

    public int CategoryId { get; private set; }

    public string Comment { get; private set; }
}
