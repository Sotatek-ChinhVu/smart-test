using UseCase.Core.Sync.Core;

namespace UseCase.KarteInf.ConvertTextToRichText;

public class ConvertTextToRichTextInputData : IInputData<ConvertTextToRichTextOutputData>
{
    public ConvertTextToRichTextInputData(int hpId, long ptId)
    {
        HpId = hpId;
        PtId = ptId;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }
}
