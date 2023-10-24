namespace Domain.Models.MstItem
{
    public class IpnNameMstModel
    {
        public IpnNameMstModel(int hpId, string ipnNameCd, int startDate, int endDate, string ipnName, int seqNo, int isDeleted, bool modelModified)
        {
            HpId = hpId;
            IpnNameCd = ipnNameCd;
            StartDate = startDate;
            EndDate = endDate;
            IpnName = ipnName;
            SeqNo = seqNo;
            IsDeleted = isDeleted;
            ModelModified = modelModified;
            IpnNameCdOrigin = string.Empty;
        }

        public IpnNameMstModel(int hpId)
        {
            HpId = hpId;
            IpnNameCd = string.Empty;
            IpnName = string.Empty;
            IpnNameCdOrigin = string.Empty;
        }

        public IpnNameMstModel()
        {
            IpnNameCd = string.Empty;
            IpnName = string.Empty;
            IpnNameCdOrigin = string.Empty;
        }

        public IpnNameMstModel(int hpId, string ipnNameCd, int startDate, int endDate, string ipnName, int seqNo, int isDeleted, bool modelModified, string ipnNameCdOrigin)
        {
            HpId = hpId;
            IpnNameCd = ipnNameCd;
            StartDate = startDate;
            EndDate = endDate;
            IpnName = ipnName;
            SeqNo = seqNo;
            IsDeleted = isDeleted;
            ModelModified = modelModified;
            IpnNameCdOrigin = ipnNameCdOrigin;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId { get; private set; }

        /// <summary>
        /// 一般名コード
        /// 
        /// </summary>
        public string IpnNameCd { get; private set; }

        public string IpnNameCdOrigin { get; private set; }

        /// <summary>
        /// 開始日
        /// 
        /// </summary>
        public int StartDate { get; private set; }

        /// <summary>
        /// 終了日
        /// 
        /// </summary>
        public int EndDate { get; private set; }

        /// <summary>
        /// 一般名
        /// 
        /// </summary>
        public string IpnName { get; private set; }

        /// <summary>
        /// 連番
        /// 同一一般名コード、開始日内の連番
        /// </summary>
        public int SeqNo { get; private set; }

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        public int IsDeleted { get; private set; }

        
        public bool ModelModified { get; private set; }
    }
}
