using UseCase.Core.Sync.Core;

namespace UseCase.Schema.SaveImageSuperSetDetail;

public class SaveImageSuperSetDetailInputData : IInputData<SaveImageSuperSetDetailOutputData>
{
    public SaveImageSuperSetDetailInputData(int hpId, int setCd, int position, string oldImage, Stream streamImage)
    {
        HpId = hpId;
        SetCd = setCd;
        Position = position;
        OldImage = oldImage;
        StreamImage = streamImage;
    }

    public int HpId { get; private set; }
    public int SetCd { get; private set; }
    public int Position { get; private set; }
    public string OldImage { get; private set; }
    public Stream StreamImage { get; private set; }
}
