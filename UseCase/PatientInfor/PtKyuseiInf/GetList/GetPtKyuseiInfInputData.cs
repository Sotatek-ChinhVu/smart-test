using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.PtKyuseiInf.GetList
{
    public class GetPtKyuseiInfInputData : IInputData<GetPtKyuseiInfOutputData>
    {
        public GetPtKyuseiInfInputData(int hpId, long ptId, bool isDeleted)
        {
            HpId = hpId;
            PtId = ptId;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public bool IsDeleted { get; private set; }
    }
}
