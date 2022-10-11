using UseCase.Core.Sync.Core;

namespace UseCase.Schema.SaveImageTodayOrder;

public class SaveImageTodayOrderOutputData : IOutputData
{
    public SaveImageTodayOrderOutputData(string urlImage, SaveImageTodayOrderStatus status)
    {
        UrlImage = urlImage;
        Status = status;
    }

    public SaveImageTodayOrderOutputData(SaveImageTodayOrderStatus status)
    {
        UrlImage = string.Empty;
        Status = status;
    }

    public string UrlImage { get; private set; }
    public SaveImageTodayOrderStatus Status { get; private set; }
}
