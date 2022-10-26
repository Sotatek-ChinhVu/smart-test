using Entity.Tenant;

namespace EmrCalculateApi.Futan.Models
{
    public class SanteiCntCheckModel
    {
        public SanteiCntCheck SanteiCntCheck { get; }

        public SanteiCntCheckModel(SanteiCntCheck santeiCntCheck)
        {
            SanteiCntCheck = santeiCntCheck;
        }

        public int TermCnt => SanteiCntCheck.TermCnt;

        public int TermSbt => SanteiCntCheck.TermSbt;

        public int CntType => SanteiCntCheck.CntType;

        public long MaxCnt => SanteiCntCheck.MaxCnt;

        public string TargetCd => SanteiCntCheck.TargetCd;
    }
}
