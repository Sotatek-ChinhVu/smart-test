using Entity.Tenant;

namespace EmrCalculateApi.Ika.Models
{
    public class DensiHaihanMonthModel 
    {
        public DensiHaihanMonth DensiHaihanMonth { get; } = null;

        public DensiHaihanMonthModel(DensiHaihanMonth densiHaihanMonth)
        {
            DensiHaihanMonth = densiHaihanMonth;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return DensiHaihanMonth.HpId; }
        }

        /// <summary>
        /// 項目コード１
        /// 
        /// </summary>
        public string ItemCd1
        {
            get { return DensiHaihanMonth.ItemCd1 ?? string.Empty; }
        }

        /// <summary>
        /// 項目コード２
        /// 
        /// </summary>
        public string ItemCd2
        {
            get { return DensiHaihanMonth.ItemCd2 ?? string.Empty; }
        }

        /// <summary>
        /// 背反区分
        /// "背反の条件を表す。 
        /// 1: 診療行為コード①を算定する。 
        /// 2: 診療行為コード②を算定する。 
        /// 3: 何れか一方を算定する。"
        /// </summary>
        public int HaihanKbn
        {
            get { return DensiHaihanMonth.HaihanKbn; }
        }

        /// <summary>
        /// 特例条件
        /// "背反条件に特別な条件がある場合に設定する 
        /// 0: 条件なし
        /// 1: 条件あり "
        /// </summary>
        public int SpJyoken
        {
            get { return DensiHaihanMonth.SpJyoken; }
        }

        /// <summary>
        /// 新設年月日
        /// レコード情報を新設した日付を西暦年4桁、月2桁及び日2桁の8桁で表す。
        /// </summary>
        public int StartDate
        {
            get { return DensiHaihanMonth.StartDate; }
        }

        /// <summary>
        /// 廃止年月日
        /// "当該診療行為の使用が可能な最終日付を西暦年4桁、月2桁及び日2桁の8桁で表す。 
        /// なお、廃止診療行為でない場合は「99999999」とする。"
        /// </summary>
        public int EndDate
        {
            get { return DensiHaihanMonth.EndDate; }
        }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        public long SeqNo
        {
            get { return DensiHaihanMonth.SeqNo; }
        }

        /// <summary>
        /// ユーザー設定
        /// "0: システム設定分
        /// 1: ユーザー設定分"
        /// </summary>
        public int UserSetting
        {
            get { return DensiHaihanMonth.UserSetting; }
        }

        /// <summary>
        /// 対象保険種
        /// "0:健保・労災とも対象
        /// 1:健保のみ対象
        /// 2:労災のみ対象"
        /// </summary>
        public int TargetKbn
        {
            get { return DensiHaihanMonth.TargetKbn; }
        }

        /// <summary>
        /// 無効区分
        /// "0: 有効
        /// 1: 無効"
        /// </summary>
        public int IsInvalid
        {
            get { return DensiHaihanMonth.IsInvalid; }
        }

        ///// <summary>
        ///// 作成日時
        ///// 
        ///// </summary>
        //public DateTime CreateDate
        //{
        //    get { return DensiHaihanMonth.CreateDate; }
        //}

        ///// <summary>
        ///// 作成者ID
        ///// 
        ///// </summary>
        //public int CreateId
        //{
        //    get { return DensiHaihanMonth.CreateId; }
        //}

        ///// <summary>
        ///// 作成端末
        ///// 
        ///// </summary>
        //public string CreateMachine
        //{
        //    get { return DensiHaihanMonth.CreateMachine; }
        //}

        ///// <summary>
        ///// 更新日時
        ///// 
        ///// </summary>
        //public DateTime UpdateDate
        //{
        //    get { return DensiHaihanMonth.UpdateDate; }
        //}

        ///// <summary>
        ///// 更新者ID
        ///// 
        ///// </summary>
        //public int UpdateId
        //{
        //    get { return DensiHaihanMonth.UpdateId; }
        //}

        ///// <summary>
        ///// 更新端末
        ///// 
        ///// </summary>
        //public string UpdateMachine
        //{
        //    get { return DensiHaihanMonth.UpdateMachine; }
        //}


    }

}
