using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.CheckedItemName
{
    public class CheckedItemNameOutputData : IOutputData
    {
        public CheckedItemNameOutputData(CheckedItemNameStatus status, Dictionary<string, string> checkedItemNames)
        {
            Status = status;
            CheckedItemNames = checkedItemNames;
        }

        public CheckedItemNameStatus Status { get; private set; }
        public Dictionary<string, string> CheckedItemNames { get; private set; }
    }
}
