using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetTenOfHRTItem
{
    public class GetTenOfHRTItemOutputData : IOutputData
    {
        public GetTenOfHRTItemOutputData(double tenOfHRTItem, GetTenOfHRTItemStatus status)
        {
            TenOfHRTItem = tenOfHRTItem;
            Status = status;
        }

        public double TenOfHRTItem { get; private set; }

        public GetTenOfHRTItemStatus Status { get; private set; }
    }
}
