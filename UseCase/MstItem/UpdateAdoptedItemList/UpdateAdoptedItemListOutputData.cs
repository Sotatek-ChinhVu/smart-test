using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.UpdateAdoptedItemList
{
    public class UpdateAdoptedItemListOutputData : IOutputData
    {
        public UpdateAdoptedItemListStatus Status { get; private set; }

        public UpdateAdoptedItemListOutputData(UpdateAdoptedItemListStatus status)
        {
            Status = status;
        }
    }
}
