using UseCase.Core.Sync.Core;
using OdrInfItemOfTodayOrder = UseCase.OrdInfs.GetListTrees.OdrInfItem;


namespace UseCase.MedicalExamination.ConvertItem
{
    public class ConvertItemOutputData : IOutputData
    {
        public ConvertItemOutputData(ConvertItemStatus status, List<OdrInfItemOfTodayOrder> result)
        {
            Status = status;
            Result = result;
        }

        public ConvertItemStatus Status { get; private set; }
        public List<OdrInfItemOfTodayOrder> Result { get; private set; }
    }
}
