namespace Domain.Models.MstItem
{
    public class PaymentMethodMstModel
    {
        public PaymentMethodMstModel(int paymentMethodCd, string payName, string paySname, int sortNo, int isDeleted)
        {
            PaymentMethodCd = paymentMethodCd;
            PayName = payName;
            PaySname = paySname;
            SortNo = sortNo;
            IsDeleted = isDeleted;
        }

        public int PaymentMethodCd { get; private set; }

        public string PayName { get; private set; }

        public string PaySname { get; private set; }

        public int SortNo { get; private set; }

        public int IsDeleted { get; private set; }
    }
}
