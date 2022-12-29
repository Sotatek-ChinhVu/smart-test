using Entity.Tenant;

namespace EmrCalculateApi.Ika.Models
{
    public class CmtKbnMstModel 
    {
        public CmtKbnMst CmtKbnMst { get; } = null;

        public CmtKbnMstModel(CmtKbnMst cmtKbnMst)
        {
            CmtKbnMst = cmtKbnMst;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return CmtKbnMst.HpId; }
        }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        public string ItemCd
        {
            get { return CmtKbnMst.ItemCd; }
        }

        /// <summary>
        /// 開始日
        /// 
        /// </summary>
        public int StartDate
        {
            get { return CmtKbnMst.StartDate; }
        }

        /// <summary>
        /// 終了日
        /// 
        /// </summary>
        public int EndDate
        {
            get { return CmtKbnMst.EndDate; }
        }

        /// <summary>
        /// コメント区分
        /// "計算時、コメントを自動で付与する場合、付与するコメントの種類を設定
        /// 0: 自動付与なし
        /// 1: 実施日
        /// 2: 前回日（840000087 :前回実施　　　月　　日）
        /// 3: 初回日（840000085 :初回実施　　　月　　日）
        /// 4: 前回日 or 初回日
        /// 5: 初回算定日（840000085 :初回算定　　　月　　日）
        /// 6: 実施日（列挙）
        /// 7: 実施日（列挙：前月末・翌月頭含む）
        /// 8: 実施日（列挙：項目名あり）
        /// 9: 実施日数（840000096 :実施日数　　　日）※未使用
        /// 10: 前回日 or 初回日（項目名あり）
        /// 11: 数量コメント（RECEDEN_CMT_SELECTに登録がある場合のみ有効)"
        /// </summary>
        public int CmtKbn
        {
            get { return CmtKbnMst.CmtKbn; }
        }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get { return CmtKbnMst.CreateDate; }
        }

        /// <summary>
        /// 作成者ID
        /// 
        /// </summary>
        public int CreateId
        {
            get { return CmtKbnMst.CreateId; }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return CmtKbnMst.CreateMachine; }
        }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        public DateTime UpdateDate
        {
            get { return CmtKbnMst.UpdateDate; }
        }

        /// <summary>
        /// 更新者ID
        /// 
        /// </summary>
        public int UpdateId
        {
            get { return CmtKbnMst.UpdateId; }
        }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        public string UpdateMachine
        {
            get { return CmtKbnMst.UpdateMachine; }
        }

        /// <summary>
        /// ID
        /// 
        /// </summary>
        public long Id
        {
            get { return CmtKbnMst.Id; }
        }


    }

}
