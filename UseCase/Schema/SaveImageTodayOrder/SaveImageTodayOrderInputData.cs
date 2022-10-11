using UseCase.Core.Sync.Core;

namespace UseCase.Schema.SaveImageTodayOrder;

public class SaveImageTodayOrderInputData : IInputData<SaveImageTodayOrderOutputData>
{
    public SaveImageTodayOrderInputData(int hpId, long ptId, long raiinNo, string oldImage, Stream streamImage)
    {
        HpId = hpId;
        PtId = ptId;
        RaiinNo = raiinNo;
        OldImage = oldImage;
        StreamImage = streamImage;
    }

    public int HpId { get; private set; }
    public long PtId { get; private set; }
    public long RaiinNo { get; private set; }
    public string OldImage { get; private set; }
    public Stream StreamImage { get; private set; }
}
