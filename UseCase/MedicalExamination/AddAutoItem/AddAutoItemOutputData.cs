using UseCase.Core.Sync.Core;
using UseCase.OrdInfs.GetListTrees;

namespace UseCase.MedicalExamination.AddAutoItem
{
    public class AddAutoItemOutputData : IOutputData
    {
        public AddAutoItemOutputData(AddAutoItemStatus status, List<OdrInfItem> odrInfItemInputDatas)
        {
            Status = status;
            OdrInfItemInputDatas = odrInfItemInputDatas;
        }

        public AddAutoItemStatus Status { get; private set; }
        public List<OdrInfItem> OdrInfItemInputDatas { get; private set; }
    }
}
