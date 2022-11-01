using UseCase.Core.Sync.Core;

namespace UseCase.SystemGenerationConf
{
    public class GetSystemGenerationConfInputData : IInputData<GetSystemGenerationConfOutputData>
    {
        public GetSystemGenerationConfInputData(int hpId, int grpCd, int grpEdaNo, int presentDate, int defaultValue)
        {
            HpId = hpId;
            GrpCd = grpCd;
            GrpEdaNo = grpEdaNo;
            PresentDate = presentDate;
            DefaultValue = defaultValue;
        }

        public int HpId { get; private set; }

        public int GrpCd { get; private set; }

        public int GrpEdaNo { get; private set; }

        public int PresentDate { get; private set; }

        public int DefaultValue { get; private set; }
    }
}
