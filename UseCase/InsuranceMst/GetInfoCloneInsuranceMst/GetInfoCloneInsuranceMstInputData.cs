using UseCase.Core.Sync.Core;

namespace UseCase.InsuranceMst.GetInfoCloneInsuranceMst
{
    public class GetInfoCloneInsuranceMstInputData : IInputData<GetInfoCloneInsuranceMstOutputData>
    {
        public GetInfoCloneInsuranceMstInputData(int hpId, int hokenNo, int prefNo, int startDate)
        {
            HpId = hpId;
            HokenNo = hokenNo;
            PrefNo = prefNo;
            StartDate = startDate;
        }

        public int HpId { get; private set; }

        public int HokenNo { get; private set; }

        public int PrefNo { get; private set; }

        public int StartDate { get; private set; }
    }
}
