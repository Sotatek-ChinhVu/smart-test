using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetTenOfKNItem
{
    public class GetTenOfKNItemOutputData : IOutputData
    {
        public GetTenOfKNItemOutputData(double latestSedai, GetTenOfKNItemStatus status)
        {
            LatestSedai = latestSedai;
            Status = status;
        }

        public double LatestSedai { get; private set; }

        public GetTenOfKNItemStatus Status { get; private set; }
    }
}
