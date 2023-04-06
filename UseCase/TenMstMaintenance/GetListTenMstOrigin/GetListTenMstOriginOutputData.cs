using Domain.Models.TenMstMaintenance;
using UseCase.Core.Sync.Core;

namespace UseCase.TenMstMaintenance.GetListTenMstOrigin
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

        public IEnumerable<int> StartDateDisplay
        {
            get => TenMsts.Select(x => x.StartDate);
        }
    }
}
