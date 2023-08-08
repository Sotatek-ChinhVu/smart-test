using Helper.Enum;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetListDrugImage;

public class GetListDrugImageInputData : IInputData<GetListDrugImageOutputData>
{
    public GetListDrugImageInputData(ImageTypeDrug type, string yjCd, string selectedImage)
    {
        Type = type;
        YjCd = yjCd;
        SelectedImage = selectedImage;
    }

    public ImageTypeDrug Type { get; private set; }

    public string YjCd { get; private set; }

    public string SelectedImage { get; private set; }
}
