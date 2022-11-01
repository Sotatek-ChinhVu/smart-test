using Entity.Tenant;

namespace Domain.Models.MstItem
{
    public class IpnNameMstModel
    {
        public IpnNameMst IpnNameMst { get; }

        public IpnNameMstModel(IpnNameMst ipnNameMst)
        {
            IpnNameMst = ipnNameMst;
        }
        public int HpId { get; private set; }
        public string IpnNameCd { get; private set; } = string.Empty;
        public int StartDate { get; private set; }
        public int EndDate { get; private set; }
        public string IpnName { get; private set; } = string.Empty;
        public int SeqNo { get; private set; }
        public int IsDeleted { get; private set; }
        public DateTime CreateDate { get; private set; }
        public int CreateId { get; private set; }
        public string CreateMachine { get; private set; } = string.Empty;
        public DateTime UpdateDate { get; private set; }
        public int UpdateId { get; private set; }
        public string UpdateMachine { get; private set; } = string.Empty;
    }
}
