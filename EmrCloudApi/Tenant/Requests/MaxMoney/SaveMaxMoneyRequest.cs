using Domain.Models.MaxMoney;

namespace EmrCloudApi.Tenant.Requests.MaxMoney
{
    public class SaveMaxMoneyRequest
    {
        public int HpId { get; set; }
        public long PtId { get; set; }
        public int KohiId { get; set; }
        public int SinYM { get; set; }
        public List<LimitListModel> ListLimits { get; set; } =  new List<LimitListModel>();
    }
}
