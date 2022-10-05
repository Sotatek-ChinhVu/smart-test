using UseCase.Core.Sync.Core;

namespace UseCase.Schema.SaveImageSuperSetDetail;

public class SaveImageSuperSetDetailOutputData : IOutputData
{
    public SaveImageSuperSetDetailOutputData(string urlImage, SaveImageSuperSetDetailStatus status)
    {
        UrlImage = urlImage;
        Status = status;
    }

    public SaveImageSuperSetDetailOutputData(SaveImageSuperSetDetailStatus status)
    {
        UrlImage = string.Empty;
        Status = status;
    }

    public string UrlImage { get; private set; }
    public SaveImageSuperSetDetailStatus Status { get; private set; }
}
