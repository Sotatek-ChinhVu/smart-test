using Entity.Tenant;

namespace Reporting.Sijisen.Model
{
    public class CoRaiinKbnInfModel
    {
        public RaiinKbnInf RaiinKbnInf { get; } = null;
        public RaiinKbnDetail RaiinKbnDetail { get; } = null;

        public CoRaiinKbnInfModel(RaiinKbnInf raiinKbnInf, RaiinKbnDetail raiinKbnDetail)
        {
            RaiinKbnInf = raiinKbnInf;
            RaiinKbnDetail = raiinKbnDetail;
        }

        /// <summary>
        /// 来院区分情報
        /// </summary>
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId
        {
            get { return RaiinKbnInf.HpId; }
        }

        /// <summary>
        /// 患者ID
        /// </summary>
        public long PtId
        {
            get { return RaiinKbnInf.PtId; }
        }

        /// <summary>
        /// 診療日
        /// </summary>
        public int SinDate
        {
            get { return RaiinKbnInf.SinDate; }
        }

        /// <summary>
        /// 来院番号
        /// </summary>
        public long RaiinNo
        {
            get { return RaiinKbnInf.RaiinNo; }
        }

        /// <summary>
        /// コメント区分
        /// </summary>
        public int GrpId
        {
            get { return RaiinKbnInf.GrpId; }
        }

        /// <summary>
        /// 連番
        /// </summary>
        public long SeqNo
        {
            get { return RaiinKbnInf.SeqNo; }
        }

        /// <summary>
        /// 区分コード
        /// </summary>
        public int KbnCd
        {
            get { return RaiinKbnInf.KbnCd; }
        }

        /// <summary>
        /// 削除区分
        ///  1:削除
        /// </summary>
        /// 
        public int IsDelete
        {
            get { return RaiinKbnInf.IsDelete; }
        }

        /// <summary>
        /// 区分名称
        /// </summary>
        public string KbnName
        {
            get { return RaiinKbnDetail != null ? (RaiinKbnDetail.KbnName ?? "") : ""; }
        }
    }
}
