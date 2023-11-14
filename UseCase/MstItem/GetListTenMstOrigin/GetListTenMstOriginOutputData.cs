using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetListTenMstOrigin
{
    public class GetListTenMstOriginOutputData : IOutputData
    {
        public GetListTenMstOriginOutputData(List<TenMstOriginModel> tenMsts, GetListTenMstOriginStatus status)
        {
            TenMsts = tenMsts;
            Status = status;
        }

        public List<TenMstOriginModel> TenMsts { get; private set; }

        public GetListTenMstOriginStatus Status { get; private set; }

        public List<int> StartDateDisplay
        {
            get
            {
                var result = TenMsts.Select(x => x.StartDate).ToList();
                return result ?? new();
            }
        }
    }
}
