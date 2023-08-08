using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetListDrugImage;

public class GetListDrugImageOutputData : IOutputData
{
    public GetListDrugImageOutputData(GetListDrugImageStatus status, List<DrugImageOutputItem> imageList)
    {
        Status = status;
        ImageList = imageList;
    }

    public GetListDrugImageStatus Status { get; private set; }

    public List<DrugImageOutputItem> ImageList { get; private set; }
}
