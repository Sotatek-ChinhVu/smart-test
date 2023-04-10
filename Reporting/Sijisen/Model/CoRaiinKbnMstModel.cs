using Entity.Tenant;

namespace Reporting.Sijisen.Model
{
    public class CoRaiinKbnMstModel
    {
        public RaiinKbnMst RaiinKbnMst { get; } = new();

        public CoRaiinKbnMstModel(RaiinKbnMst raiinKbnMst)
        {
            RaiinKbnMst = raiinKbnMst;
        }

        /// <summary>
        /// 来院区分情報
        /// </summary>
        /// <summary>
        ///医療機関識別ID
        /// </summary>
        public int HpId
        {
            get { return RaiinKbnMst.HpId; }
        }

        /// <summary>
        /// 分類ID
        /// </summary>
        public int GrpCd
        {
            get { return RaiinKbnMst.GrpCd; }
        }

        /// <summary>
        /// 並び順
        /// </summary>
        public int SortNo
        {
            get { return RaiinKbnMst.SortNo; }
        }

        /// <summary>
        /// 分類名称
        /// </summary>
        public string GrpName
        {
            get { return RaiinKbnMst.GrpName ?? string.Empty; }
        }

        /// <summary>
        /// 削除区分
        ///  1:削除
        /// </summary>
        public int IsDeleted
        {
            get { return RaiinKbnMst.IsDeleted; }
        }

    }
}
