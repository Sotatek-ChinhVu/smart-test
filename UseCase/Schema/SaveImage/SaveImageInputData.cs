using UseCase.Core.Sync.Core;

namespace UseCase.Schema.SaveImage;

public class SaveImageInputData : IInputData<SaveImageOutputData>
{
    public SaveImageInputData(long ptId, string oldImage, Stream streamImage)
    {
        PtId = ptId;
        OldImage = oldImage;
        StreamImage = streamImage;
    }
    public long PtId { get; private set; }
    public string OldImage { get; private set; }
    public Stream StreamImage { get; private set; }
}
