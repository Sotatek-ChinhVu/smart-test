using Domain.Models.AccountDue;

namespace EmrCloudApi.Requests.Accounting
{
    public class SaveAccountingRequest
    {
        public int SumAdjust { get; set; }
        public int ThisWari { get; set; }
        public int ThisCredit { get; set; }
        public int PayType { get; set; }
        public string Comment { get; set; } = string.Empty;
        public int UketukeSbt { get; set; }
        public List<SyunoSeikyuModel> SyunoSeikyuModels { get; set; } = new();
    }
}
