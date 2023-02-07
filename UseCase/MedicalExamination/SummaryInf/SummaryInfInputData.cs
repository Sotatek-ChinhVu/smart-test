using Domain.Models.MedicalExamination;
using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.SummaryInf
{
    public class SummaryInfInputData : IInputData<SummaryInfOutputData>
    {
        public SummaryInfInputData(int hpId, long ptId, int sinDate, List<CheckedOrderModel> checkedOrderModels, bool isTokysyoOrder, bool isTokysyosenOrder)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            CheckedOrderModels = checkedOrderModels;
            IsTokysyoOrder = isTokysyoOrder;
            IsTokysyosenOrder = isTokysyosenOrder;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public List<CheckedOrderModel> CheckedOrderModels { get; private set; }

        public bool IsTokysyoOrder { get; private set; }

        public bool IsTokysyosenOrder { get; private set; }
    }
}
