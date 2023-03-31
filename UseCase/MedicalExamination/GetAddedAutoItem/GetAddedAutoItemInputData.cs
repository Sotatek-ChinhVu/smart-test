using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetAddedAutoItem
{
    public class GetAddedAutoItemInputData : IInputData<GetAddedAutoItemOutputData>
    {
        public GetAddedAutoItemInputData(int hpId, long ptId, int sinDate, List<OrderInfItem> orderInfItems, List<CurrentOrderInf> currentOrderInfs)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            OrderInfItems = orderInfItems;
            CurrentOrderInfs = currentOrderInfs;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public List<OrderInfItem> OrderInfItems { get; private set; }
        public List<CurrentOrderInf> CurrentOrderInfs { get; private set; }
    }

    public class CurrentOrderInf
    {
        public CurrentOrderInf(int orderPosition, int orderDetailPosition, string itemCd, double suryo, int isDeleted)
        {
            OrderPosition = orderPosition;
            OrderDetailPosition = orderDetailPosition;
            ItemCd = itemCd;
            Suryo = suryo;
            IsDeleted = isDeleted;
        }

        public int OrderPosition { get; private set; }
        public int OrderDetailPosition { get; private set; }
        public string ItemCd { get; private set; }
        public double Suryo { get; private set; }
        public int IsDeleted { get; private set; }
    }
}
