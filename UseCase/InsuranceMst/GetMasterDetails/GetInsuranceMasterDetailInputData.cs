using UseCase.Core.Sync.Core;

namespace UseCase.InsuranceMst.GetMasterDetails
{
    public class GetInsuranceMasterDetailInputData : IInputData<GetInsuranceMasterDetailOutputData>
    {
        public int HpId { get; private set; }

        public int FHokenNo { get; private set; }

        public int FHokenSbtKbn { get; private set; }

        public bool IsJitan { get; private set; }

        public bool IsTaken { get; private set; }

        public GetInsuranceMasterDetailInputData(int hpId, int fHokenNo, int fHokenSbtKbn, bool isJitan, bool isTaken)
        {
            HpId = hpId;
            FHokenNo = fHokenNo;
            FHokenSbtKbn = fHokenSbtKbn;
            IsJitan = isJitan;
            IsTaken = isTaken;
        }
    }
}
