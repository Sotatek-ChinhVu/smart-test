using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.Calculate
{
    public class CalculateInputData : IInputData<CalculateOutputData>
    {
        public CalculateInputData(bool fromRcCheck, bool isSagaku, int hpId, long ptId, int sinDate, int seikyuUp, string prefix)
        {
            FromRcCheck = fromRcCheck;
            IsSagaku = isSagaku;
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            SeikyuUp = seikyuUp;
            Prefix = prefix;
        }

        public bool FromRcCheck { get; private set; }

        public bool IsSagaku { get; private set; }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public int SeikyuUp { get; private set; }

        public string Prefix { get; private set; }
    }
}
