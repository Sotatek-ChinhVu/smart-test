using UseCase.Core.Sync.Core;

namespace UseCase.Schema.SaveImage;

public class SaveImageInputData : IInputData<SaveImageOutputData>
{
    public SaveImageInputData(string oldImage, string subFolder, string fileName, Stream streamImage)
    {
        OldImage = oldImage;
        SubFolder = subFolder;
        FileName = fileName;
        StreamImage = streamImage;
    }

    public string OldImage { get; private set; }
    public string SubFolder { get; private set; }
    public string FileName { get; private set; }
    public Stream StreamImage { get; private set; }
}
