using Domain.Models.AccountDue;

namespace EmrCloudApi.Requests.Accounting
{
    public class SaveAccountingRequest
    {
        public int HpId { get; set; }
        public long PtId { get; set; }
        public int UserId { get; set; }
        public int SumAdjust { get; set; }
        public int ThisWari { get; set; }
        public int ThisCredit { get; set; }
        public int PayType { get; set; }
        public string Comment { get; set; } = string.Empty;
        public List<SyunoSeikyuModel> SyunoSeikyuModels { get; set; } = new();
    }
}
