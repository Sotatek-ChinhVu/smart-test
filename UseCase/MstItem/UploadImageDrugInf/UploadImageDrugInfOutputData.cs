using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.UploadImageDrugInf;

public class UploadImageDrugInfOutputData : IOutputData
{
    public UploadImageDrugInfOutputData(string link, UploadImageDrugInfStatus status)
    {
        Link = link;
        Status = status;
    }

    public string Link { get; private set; }

    public UploadImageDrugInfStatus Status { get; private set; }
}
