using Domain.Models.AccountDue;

namespace EmrCloudApi.Requests.Accounting
{
    public class CheckAccountingStatusRequest
    {
        public int HpId { get; set; }
        public long PtId { get; set; }
        public int SinDate { get; set; }
        public long RaiinNo { get; set; }
        public int DebitBalance { get; set; }
        public int SumAdjust { get; set; }
        public int ThisCredit { get; set; }
        public int Wari { get; set; }
        public bool IsDisCharge { get; set; }
        public bool IsDeletedSyuno { get; set; }
        public bool IsSaveAccounting { get; set; }
        public List<SyunoSeikyuModel> SyunoSeikyus { get; set; }
    }
}
