using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.GetInsuranceMasterLinkage
{
    public class GetInsuranceMasterLinkageInputData : IInputData<GetInsuranceMasterLinkageOutputData>
    {
        public GetInsuranceMasterLinkageInputData(int hpId, string futansyaNo)
        {
            HpId = hpId;
            FutansyaNo = futansyaNo;
        }

        public int HpId { get; private set; }
        public string FutansyaNo { get; private set; }
    }
}
