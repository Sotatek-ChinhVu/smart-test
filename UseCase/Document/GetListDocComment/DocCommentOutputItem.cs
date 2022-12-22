namespace UseCase.Document.GetListDocComment;

public class DocCommentOutputItem
{
    public DocCommentOutputItem(int categoryId, string categoryName, string replaceWord, List<DocCommentDetailOutputItem> docCommentDetails)
    {
        CategoryId = categoryId;
        CategoryName = categoryName;
        ReplaceWord = replaceWord;
        DocCommentDetails = docCommentDetails;
    }

    public int CategoryId { get; private set; }

    public string CategoryName { get; private set; }

    public string ReplaceWord { get; private set; }

    public List<DocCommentDetailOutputItem> DocCommentDetails { get; private set; }
}
