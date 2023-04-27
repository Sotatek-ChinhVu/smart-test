using Helper.Enum;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetListDrugImage
{
    public class GetListDrugImageInputData : IInputData<GetListDrugImageOutputData>
    {
        public GetListDrugImageInputData(ImageTypeDrug type, string yjCd)
        {
            Type = type;
            YjCd = yjCd;
        }

        public ImageTypeDrug Type { get; private set; }

        public string YjCd { get; private set; }
    }
}
