using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetAddedAutoItem
{
    public class GetAddedAutoItemOutputData : IOutputData
    {
        public GetAddedAutoItemOutputData(GetAddedAutoItemStatus status, List<AddedAutoItem> addedAutoItems)
        {
            Status = status;
            AddedAutoItems = addedAutoItems;
        }

        public GetAddedAutoItemStatus Status { get; private set; }
        public List<AddedAutoItem> AddedAutoItems { get; private set; }
    }
}
