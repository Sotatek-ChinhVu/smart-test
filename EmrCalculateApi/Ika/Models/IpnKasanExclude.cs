using Entity.Tenant;

namespace EmrCalculateApi.Ika.Models
{
    public class IpnKasanExcludeModel 
    {
        public IpnKasanExclude IpnKasanExclude { get; } = null;

        public IpnKasanExcludeModel(IpnKasanExclude ipnKasanExclude)
        {
            IpnKasanExclude = ipnKasanExclude;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return IpnKasanExclude.HpId; }
        }

        /// <summary>
        /// 一般名コード
        /// 
        /// </summary>
        public string IpnNameCd
        {
            get { return IpnKasanExclude.IpnNameCd ?? string.Empty; }
        }

        /// <summary>
        /// 開始日
        /// 
        /// </summary>
        public int StartDate
        {
            get { return IpnKasanExclude.StartDate; }
        }

        /// <summary>
        /// 終了日
        /// 
        /// </summary>
        public int EndDate
        {
            get { return IpnKasanExclude.EndDate; }
        }

        /// <summary>
        /// 連番
        /// 同一一般名コード、開始日内の連番
        /// </summary>
        public int SeqNo
        {
            get { return IpnKasanExclude.SeqNo; }
        }

    }

}
