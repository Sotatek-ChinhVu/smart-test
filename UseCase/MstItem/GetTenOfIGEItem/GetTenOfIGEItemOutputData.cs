using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetTenOfIGEItem
{
    public class GetTenOfIGEItemOutputData : IOutputData
    {
        public GetTenOfIGEItemOutputData(double tenOfIGEItem, GetTenOfIGEItemStatus status)
        {
            TenOfIGEItem = tenOfIGEItem;
            Status = status;
        }

        public double TenOfIGEItem { get; private set; }

        public GetTenOfIGEItemStatus Status { get; private set; }
    }
}
