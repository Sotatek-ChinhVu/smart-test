namespace Domain.Models.Document;

public class DocCommentModel
{
    public DocCommentModel(int categoryId, string categoryName, string replaceWord, List<DocCommentDetailModel> listDocCommentDetails)
    {
        CategoryId = categoryId;
        CategoryName = categoryName;
        ReplaceWord = replaceWord;
        ListDocCommentDetails = listDocCommentDetails;
    }

    public int CategoryId { get; private set; }

    public string CategoryName { get; private set; }

    public string ReplaceWord { get; private set; }

    public List<DocCommentDetailModel> ListDocCommentDetails { get; private set; }
}
