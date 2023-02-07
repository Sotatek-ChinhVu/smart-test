using Domain.Models.InsuranceMst;
using UseCase.Core.Sync.Core;

namespace UseCase.InsuranceMst.GetSelectMaintenance
{
    public class GetSelectMaintenanceOutputData : IOutputData
    {
        public IEnumerable<SelectMaintenanceModel> Datas { get; private set; }

        public GetSelectMaintenanceStatus Status { get; private set; }

        public GetSelectMaintenanceOutputData(IEnumerable<SelectMaintenanceModel> data, GetSelectMaintenanceStatus status)
        {
            Datas = data;
            Status = status;
        }
    }
}
