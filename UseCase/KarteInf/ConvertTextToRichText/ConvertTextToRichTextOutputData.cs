using UseCase.Core.Sync.Core;

namespace UseCase.KarteInf.ConvertTextToRichText;

public class ConvertTextToRichTextOutputData : IOutputData
{
    public ConvertTextToRichTextOutputData(ConvertTextToRichTextStatus status)
    {
        Status = status;
    }

    public ConvertTextToRichTextStatus Status { get; private set; }
}
