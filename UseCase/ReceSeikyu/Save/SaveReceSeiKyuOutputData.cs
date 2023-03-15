using Domain.Models.ReceSeikyu;
using UseCase.Core.Sync.Core;

namespace UseCase.ReceSeikyu.Save
{
    public class SaveReceSeiKyuOutputData : IOutputData
    {
        public SaveReceSeiKyuOutputData(SaveReceSeiKyuStatus status, List<long> ptIds, int seikyuYm, List<ReceInfo> receInfos)
        {
            Status = status;
            PtIds = ptIds;
            SeikyuYm = seikyuYm;
            ReceInfos = receInfos;
        }

        public SaveReceSeiKyuStatus Status { get; private set; }

        public List<long> PtIds { get; private set; } = new List<long>();

        public int SeikyuYm { get; private set; }

        public List<ReceInfo> ReceInfos { get; private set; }
    }
}
