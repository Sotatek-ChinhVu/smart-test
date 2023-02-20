using UseCase.Core.Sync.Core;

namespace UseCase.InsuranceMst.DeleteHokenMaster
{
    public class DeleteHokenMasterInputData : IInputData<DeleteHokenMasterOutputData>
    {
        public DeleteHokenMasterInputData(int hpId, int prefNo, int hokenNo, int hokenEdaNo, int startDate)
        {
            HpId = hpId;
            PrefNo = prefNo;
            HokenNo = hokenNo;
            HokenEdaNo = hokenEdaNo;
            StartDate = startDate;
        }

        public int HpId { get; private set; }

        public int PrefNo { get; private set; }

        public int HokenNo { get; private set; }

        public int HokenEdaNo { get; private set; }

        public int StartDate { get; private set; }
    }
}
