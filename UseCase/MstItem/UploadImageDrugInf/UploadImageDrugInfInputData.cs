using Helper.Enum;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.UploadImageDrugInf;

public class UploadImageDrugInfInputData : IInputData<UploadImageDrugInfOutputData>
{
    public UploadImageDrugInfInputData(ImageTypeDrug type, string yjCd, bool isDeleted, Stream streamImage)
    {
        Type = type;
        YjCd = yjCd;
        IsDeleted = isDeleted;
        StreamImage = streamImage;
    }

    public ImageTypeDrug Type { get; private set; }

    public string YjCd { get; private set; }

    public bool IsDeleted { get; private set; }

    public Stream StreamImage { get; private set; }
}
