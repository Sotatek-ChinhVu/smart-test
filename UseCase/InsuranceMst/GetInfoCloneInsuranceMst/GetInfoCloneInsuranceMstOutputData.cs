using UseCase.Core.Sync.Core;

namespace UseCase.InsuranceMst.GetInfoCloneInsuranceMst
{
    public class GetInfoCloneInsuranceMstOutputData : IOutputData
    {
        public GetInfoCloneInsuranceMstOutputData(int hokenEdaNo, int sortNo, GetInfoCloneInsuranceMstStatus status)
        {
            HokenEdaNo = hokenEdaNo;
            SortNo = sortNo;
            Status = status;
        }

        public int HokenEdaNo { get; private set; }

        public int SortNo { get; private set; }

        public GetInfoCloneInsuranceMstStatus Status { get; private set; }
    }
}
