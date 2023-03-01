using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.ConvertInputItemToTodayOdr
{
    public class ConvertInputItemToTodayOrdOutputData : IOutputData
    {
        public ConvertInputItemToTodayOrdOutputData(ConvertInputItemToTodayOrdStatus status, Dictionary<string, bool> yakkaOfOdrDetails)
        {
            Status = status;
            YakkaOfOdrDetails = yakkaOfOdrDetails;
        }

        public ConvertInputItemToTodayOrdStatus Status { get; private set; }
        public Dictionary<string, bool> YakkaOfOdrDetails { get; private set; }
    }
}
