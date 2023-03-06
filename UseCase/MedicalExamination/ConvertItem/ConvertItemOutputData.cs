using UseCase.Core.Sync.Core;
using UseCase.MedicalExamination.GetCheckedOrder;

namespace UseCase.MedicalExamination.ConvertItem
{
    public class ConvertItemOutputData : IOutputData
    {
        public ConvertItemOutputData(ConvertItemStatus status, List<OdrInfItem> result)
        {
            Status = status;
            Result = result;
        }

        public ConvertItemStatus Status { get; private set; }
        public List<OdrInfItem> Result { get; private set; }
    }
}
