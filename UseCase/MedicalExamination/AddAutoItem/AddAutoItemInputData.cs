using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.AddAutoItem
{
    public class AddAutoItemInputData : IInputData<AddAutoItemOutputData>
    {
        public AddAutoItemInputData(int hpId, int userId, int sinDate, List<AddedOrderInf> addedOrderInfs, List<OrderInfItem> orderInfItems)
        {
            HpId = hpId;
            UserId = userId;
            AddedOrderInfs = addedOrderInfs;
            OrderInfItems = orderInfItems;
            SinDate = sinDate;
        }

        public int HpId { get; private set; }
        public int UserId { get; private set; }
        public int SinDate { get; private set; }
        public List<AddedOrderInf> AddedOrderInfs { get; private set; }
        public List<OrderInfItem> OrderInfItems { get; private set; }
    }

    public class OrderInfItem
    {
        public OrderInfItem(int orderPosition, int orderDetailPosition, string rpName, int inOutKbn, int odrKouiKbn)
        {
            OrderPosition = orderPosition;
            OrderDetailPosition = orderDetailPosition;
            RpName = rpName;
            InOutKbn = inOutKbn;
            OdrKouiKbn = odrKouiKbn;
        }

        public int OrderPosition { get; private set; }
        public int OrderDetailPosition { get; private set; }
        public string RpName { get; private set; }
        public int InOutKbn { get; private set; }
        public int OdrKouiKbn { get; private set; }
    }
}
