using Entity.Tenant;

namespace Reporting.Sijisen.Model
{
    public class CoRaiinKbnDetailModel
    {
        public RaiinKbnDetail RaiinKbnDetail { get; } = new();

        public CoRaiinKbnDetailModel(RaiinKbnDetail raiinKbnDetail)
        {
            RaiinKbnDetail = raiinKbnDetail;
        }

        /// <summary>
        /// 来院区分詳細マスタ
        /// </summary>
        /// <summary>
        ///医療機関識別ID
        /// </summary>
        public int HpId
        {
            get { return RaiinKbnDetail.HpId; }
        }

        /// <summary>
        /// 分類ID
        /// </summary>
        public int GrpCd
        {
            get { return RaiinKbnDetail.GrpCd; }
        }

        /// <summary>
        /// 区分コード
        /// </summary>
        public int KbnCd
        {
            get { return RaiinKbnDetail.KbnCd; }
        }

        /// <summary>
        /// 並び順
        /// </summary>
        public int SortNo
        {
            get { return RaiinKbnDetail.SortNo; }
        }

        /// <summary>
        /// 区分名称
        /// </summary>
        public string KbnName
        {
            get { return RaiinKbnDetail.KbnName ?? string.Empty; }
        }

        /// <summary>
        /// 配色
        /// </summary>
        public string ColorCd
        {
            get { return RaiinKbnDetail.ColorCd ?? string.Empty; }
        }

        /// <summary>
        /// 変更確認
        ///  0:なし 
        ///  1:あり   
        /// </summary>
        public int IsConfirmed
        {
            get { return RaiinKbnDetail.IsConfirmed; }
        }

        /// <summary>
        /// 自動設定
        ///  0:なし            
        ///  1:今回オーダー 
        ///  2:予約オーダー     
        ///  3:すべて
        /// </summary>
        public int IsAuto
        {
            get { return RaiinKbnDetail.IsAuto; }
        }

        /// <summary>
        /// 自動削除
        ///  0:なし 
        ///  1:あり
        /// </summary>
        public int IsAutoDelete
        {
            get { return RaiinKbnDetail.IsAutoDelete; }
        }

        /// <summary>
        /// 削除区分
        ///  1:削除
        /// </summary>
        public int IsDeleted
        {
            get { return RaiinKbnDetail.IsDeleted; }
        }
    }
}
