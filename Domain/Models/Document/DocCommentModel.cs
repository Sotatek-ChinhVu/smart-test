namespace Domain.Models.Document;

public class DocCommentModel
{
    public DocCommentModel(int categoryId, string categoryName, string replaceWord)
    {
        CategoryId = categoryId;
        CategoryName = categoryName;
        ReplaceWord = replaceWord;
    }

    public int CategoryId { get; private set; }

    public string CategoryName { get; private set; }

    public string ReplaceWord { get; private set; }
}
