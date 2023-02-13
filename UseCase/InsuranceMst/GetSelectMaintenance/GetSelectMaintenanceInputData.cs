using UseCase.Core.Sync.Core;

namespace UseCase.InsuranceMst.GetSelectMaintenance
{
    public class GetSelectMaintenanceInputData : IInputData<GetSelectMaintenanceOutputData>
    {
        public GetSelectMaintenanceInputData(int hpId, int hokenNo, int hokenEdaNo, int prefNo, int startDate)
        {
            HpId = hpId;
            HokenNo = hokenNo;
            HokenEdaNo = hokenEdaNo;
            PrefNo = prefNo;
            StartDate = startDate;
        }

        public int HpId { get; private set; }

        public int HokenNo { get; private set; }

        public int HokenEdaNo { get; private set; }

        public int PrefNo { get; private set; }

        public int StartDate { get; private set; }
    }
}
