using UseCase.Core.Sync.Core;

namespace UseCase.InsuranceMst.GetHokenMasterReadOnly
{
    public class GetHokenMasterReadOnlyInputData : IInputData<GetHokenMasterReadOnlyOutputData>
    {
        public GetHokenMasterReadOnlyInputData(int hpId, int hokenNo, int hokenEdaNo, int prefNo, int sinDate)
        {
            HpId = hpId;
            HokenNo = hokenNo;
            HokenEdaNo = hokenEdaNo;
            PrefNo = prefNo;
            SinDate = sinDate;
        }

        public int HpId { get; private set; }

        public int HokenNo { get; private set; }

        public int HokenEdaNo { get; private set; }

        public int PrefNo { get; private set; }

        public int SinDate { get; private set; }
    }
}
