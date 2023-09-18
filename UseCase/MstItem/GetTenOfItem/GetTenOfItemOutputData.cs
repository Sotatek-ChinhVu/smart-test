using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetTenOfItem
{
    public class GetTenOfItemOutputData : IOutputData
    {
        public GetTenOfItemOutputData(Dictionary<string, double> tenOfItem, GetTenOfItemStatus status)
        {
            TenOfItem = tenOfItem;
            Status = status;
        }

        public Dictionary<string, double> TenOfItem { get; private set; }

        public GetTenOfItemStatus Status { get; private set; }
    }
}
