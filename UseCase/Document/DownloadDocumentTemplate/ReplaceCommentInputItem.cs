namespace UseCase.Document.DownloadDocumentTemplate;

public class ReplaceCommentInputItem
{
    public ReplaceCommentInputItem(string replaceKey, string replaceValue)
    {
        ReplaceKey = replaceKey;
        ReplaceValue = replaceValue;
    }

    public string ReplaceKey { get; private set; }

    public string ReplaceValue { get; private set; }
}
