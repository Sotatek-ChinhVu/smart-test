using UseCase.Core.Sync.Core;

namespace UseCase.Schema.SaveImage;

public class SaveImageOutputData : IOutputData
{
    public SaveImageOutputData(string urlImage, SaveImageStatus status)
    {
        UrlImage = urlImage;
        Status = status;
    }

    public string UrlImage { get; private set; }
    public SaveImageStatus Status { get; private set; }
}
