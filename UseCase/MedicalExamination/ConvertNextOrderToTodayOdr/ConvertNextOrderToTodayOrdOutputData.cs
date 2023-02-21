using UseCase.Core.Sync.Core;
using UseCase.OrdInfs.GetListTrees;

namespace UseCase.MedicalExamination.ConvertNextOrderToTodayOdr
{
    public class ConvertNextOrderToTodayOrdOutputData : IOutputData
    {
        public ConvertNextOrderToTodayOrdOutputData(ConvertNextOrderToTodayOrdStatus status, List<OdrInfItem> odrInfs)
        {
            Status = status;
            OdrInfs = odrInfs;
        }

        public ConvertNextOrderToTodayOrdStatus Status { get; private set; }
        public List<OdrInfItem> OdrInfs { get; private set; }
    }
}
