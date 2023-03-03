using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.AutoCheckOrder
{
    public class AutoCheckOrderItem : IOutputData
    {
        public AutoCheckOrderItem(int type, string message, int odrInfPosition, int odrInfDetailPosition, TenItemModel tenItemMst, double suryo)
        {
            Type = type;
            Message = message;
            OdrInfPosition = odrInfPosition;
            OdrInfDetailPosition = odrInfDetailPosition;
            TenItemMst = tenItemMst;
            Suryo = suryo;
        }

        public int Type { get; private set; }
        public string Message { get; private set; }
        public int OdrInfPosition { get; private set; }
        public int OdrInfDetailPosition { get; private set; }
        public TenItemModel TenItemMst { get; private set; }
        public double Suryo { get; private set; }
    }
}
