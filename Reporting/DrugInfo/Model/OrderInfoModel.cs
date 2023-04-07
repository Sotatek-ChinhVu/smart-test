namespace Reporting.DrugInfo.Model
{
    public class OrderInfoModel
    {
        public long RaiinNo { get; set; }
        public long RpNo { get; set; }
        public int OdrKouiKbn { get; set; }
        public List<OrderInfDetailModel> OrderInfDetailCollection { get; set; }
    }
}
