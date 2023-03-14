using UseCase.Core.Sync.Core;

namespace UseCase.ReceSeikyu.Save
{
    public class SaveReceSeiKyuOutputData : IOutputData
    {
        public SaveReceSeiKyuOutputData(SaveReceSeiKyuStatus status, List<long> ptIds, int seikyuYm)
        {
            Status = status;
            PtIds = ptIds;
            SeikyuYm = seikyuYm;
        }

        public SaveReceSeiKyuStatus Status { get; private set; }

        public List<long> PtIds { get; private set; } = new List<long>();

        public int SeikyuYm { get; private set; }
    }
}
