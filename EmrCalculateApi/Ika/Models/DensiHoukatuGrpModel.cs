using Entity.Tenant;

namespace EmrCalculateApi.Ika.Models
{
    public class DensiHoukatuGrpModel
    {
        public DensiHoukatuGrp DensiHoukatuGrp { get; } = null;

        public DensiHoukatuGrpModel(DensiHoukatuGrp densiHoukatuGrp)
        {
            DensiHoukatuGrp = densiHoukatuGrp;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return DensiHoukatuGrp.HpId; }
        }

        /// <summary>
        /// 包括グループ番号
        /// 
        /// </summary>
        public string HoukatuGrpNo
        {
            get { return DensiHoukatuGrp.HoukatuGrpNo ?? string.Empty; }
        }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        public string ItemCd
        {
            get { return DensiHoukatuGrp.ItemCd ?? string.Empty; }
        }

        /// <summary>
        /// 特例条件
        /// "包括・被包括の条件に特別な条件がある場合に設定する 
        /// 0: 条件なし
        /// 1: 条件あり "
        /// </summary>
        public int SpJyoken
        {
            get { return DensiHoukatuGrp.SpJyoken; }
        }

        /// <summary>
        /// 新設年月日
        /// レコード情報を新設した日付を西暦年4桁、月2桁及び日2桁の8桁で表す。
        /// </summary>
        public int StartDate
        {
            get { return DensiHoukatuGrp.StartDate; }
        }

        /// <summary>
        /// 廃止年月日
        /// "当該診療行為の使用が可能な最終日付を西暦年4桁、月2桁及び日2桁の8桁で表す。 
        /// なお、廃止診療行為でない場合は「99999999」とする。"
        /// </summary>
        public int EndDate
        {
            get { return DensiHoukatuGrp.EndDate; }
        }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        public long SeqNo
        {
            get { return DensiHoukatuGrp.SeqNo; }
        }

        /// <summary>
        /// ユーザー設定
        /// "0: システム設定分
        /// 1: ユーザー設定分"
        /// </summary>
        public int UserSetting
        {
            get { return DensiHoukatuGrp.UserSetting; }
        }

        /// <summary>
        /// 対象保険種
        /// "0:健保・労災とも対象
        /// 1:健保のみ対象
        /// 2:労災のみ対象"
        /// </summary>
        public int TargetKbn
        {
            get { return DensiHoukatuGrp.TargetKbn; }
        }

        /// <summary>
        /// 無効区分
        /// "0: 有効
        /// 1: 無効"
        /// </summary>
        public int IsInvalid
        {
            get { return DensiHoukatuGrp.IsInvalid; }
        }

        ///// <summary>
        ///// 作成日時
        ///// 
        ///// </summary>
        //public DateTime CreateDate
        //{
        //    get { return DensiHoukatuGrp.CreateDate; }
        //}

        ///// <summary>
        ///// 作成者ID
        ///// 
        ///// </summary>
        //public int CreateId
        //{
        //    get { return DensiHoukatuGrp.CreateId; }
        //}

        ///// <summary>
        ///// 作成端末
        ///// 
        ///// </summary>
        //public string CreateMachine
        //{
        //    get { return DensiHoukatuGrp.CreateMachine; }
        //}

        ///// <summary>
        ///// 更新日時
        ///// 
        ///// </summary>
        //public DateTime UpdateDate
        //{
        //    get { return DensiHoukatuGrp.UpdateDate; }
        //}

        ///// <summary>
        ///// 更新者ID
        ///// 
        ///// </summary>
        //public int UpdateId
        //{
        //    get { return DensiHoukatuGrp.UpdateId; }
        //}

        ///// <summary>
        ///// 更新端末
        ///// 
        ///// </summary>
        //public string UpdateMachine
        //{
        //    get { return DensiHoukatuGrp.UpdateMachine; }
        //}


    }

}
