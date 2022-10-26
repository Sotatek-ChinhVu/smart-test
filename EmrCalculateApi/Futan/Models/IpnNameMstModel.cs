using Entity.Tenant;

namespace EmrCalculateApi.Futan.Models
{
    public class IpnNameMstModel
    {
        public IpnNameMst IpnNameMst { get; }

        public IpnNameMstModel(IpnNameMst ipnNameMst)
        {
            IpnNameMst = ipnNameMst;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return IpnNameMst.HpId; }
        }

        /// <summary>
        /// 一般名コード
        /// 
        /// </summary>
        public string IpnNameCd
        {
            get { return IpnNameMst.IpnNameCd; }
        }

        /// <summary>
        /// 開始日
        /// 
        /// </summary>
        public int StartDate
        {
            get { return IpnNameMst.StartDate; }
        }

        /// <summary>
        /// 終了日
        /// 
        /// </summary>
        public int EndDate
        {
            get { return IpnNameMst.EndDate; }
        }

        /// <summary>
        /// 一般名
        /// 
        /// </summary>
        public string IpnName
        {
            get { return IpnNameMst.IpnName; }
        }

        /// <summary>
        /// 連番
        /// 同一一般名コード、開始日内の連番
        /// </summary>
        public int SeqNo
        {
            get { return IpnNameMst.SeqNo; }
        }

        /// <summary>
        /// 削除区分
        /// 1:削除
        /// </summary>
        public int IsDeleted
        {
            get { return IpnNameMst.IsDeleted; }
        }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get { return IpnNameMst.CreateDate; }
        }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        public int CreateId
        {
            get { return IpnNameMst.CreateId; }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return IpnNameMst.CreateMachine ?? string.Empty; }
        }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        public DateTime UpdateDate
        {
            get { return IpnNameMst.UpdateDate; }
        }

        /// <summary>
        /// 更新者
        /// 
        /// </summary>
        public int UpdateId
        {
            get { return IpnNameMst.UpdateId; }
        }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        public string UpdateMachine
        {
            get { return IpnNameMst.UpdateMachine ?? string.Empty; }
        }
    }
}
