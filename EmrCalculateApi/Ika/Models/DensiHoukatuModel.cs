using Entity.Tenant;

namespace EmrCalculateApi.Ika.Models
{
    public class DensiHoukatuModel 
    {
        public DensiHoukatu DensiHoukatu { get; } = null;

        public DensiHoukatuModel(DensiHoukatu densiHoukatu)
        {
            DensiHoukatu = densiHoukatu;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return DensiHoukatu.HpId; }
        }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        public string ItemCd
        {
            get { return DensiHoukatu.ItemCd ?? string.Empty; }
        }

        /// <summary>
        /// 新設年月日
        /// レコード情報を新設した日付を西暦年4桁、月2桁及び日2桁の8桁で表す。
        /// </summary>
        public int StartDate
        {
            get { return DensiHoukatu.StartDate; }
        }

        /// <summary>
        /// 廃止年月日
        /// "当該診療行為の使用が可能な最終日付を西暦年4桁、月2桁及び日2桁の8桁で表す。 
        /// なお、廃止診療行為でない場合は「99999999」とする。"
        /// </summary>
        public int EndDate
        {
            get { return DensiHoukatu.EndDate; }
        }

        /// <summary>
        /// 対象保険種
        /// "0:健保・労災とも対象
        /// 1:健保のみ対象
        /// 2:労災のみ対象"
        /// </summary>
        public int TargetKbn
        {
            get { return DensiHoukatu.TargetKbn; }
        }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        public long SeqNo
        {
            get { return DensiHoukatu.SeqNo; }
        }

        /// <summary>
        /// 包括単位
        /// "包括する期間を表す
        /// 00: 関連なし
        /// 01: 1日につき
        /// 02: 同一月内
        /// 03: 同時
        /// 05: 手術前1週間
        /// 06: 1手術につき
        /// ※05,06はチェックしない"
        /// </summary>
        public int HoukatuTerm
        {
            get { return DensiHoukatu.HoukatuTerm; }
        }

        /// <summary>
        /// 包括グループ番号
        /// 0 "包括・被包括グループ番号を表す。 
        /// 包括・被包括テーブルの参照先グループを表す。"
        /// </summary>
        public string HoukatuGrpNo
        {
            get { return DensiHoukatu.HoukatuGrpNo ?? string.Empty; }
        }

        /// <summary>
        /// ユーザー設定
        /// "0: システム設定分
        /// 1: ユーザー設定分"
        /// </summary>
        public int UserSetting
        {
            get { return DensiHoukatu.UserSetting; }
        }

        /// <summary>
        /// 無効区分
        /// "0: 有効
        /// 1: 無効"
        /// </summary>
        public int IsInvalid
        {
            get { return DensiHoukatu.IsInvalid; }
        }

        ///// <summary>
        ///// 作成日時
        ///// 
        ///// </summary>
        //public DateTime CreateDate
        //{
        //    get { return DensiHoukatu.CreateDate; }
        //}

        ///// <summary>
        ///// 作成者ID
        ///// 
        ///// </summary>
        //public int CreateId
        //{
        //    get { return DensiHoukatu.CreateId; }
        //}

        ///// <summary>
        ///// 作成端末
        ///// 
        ///// </summary>
        //public string CreateMachine
        //{
        //    get { return DensiHoukatu.CreateMachine; }
        //}

        ///// <summary>
        ///// 更新日時
        ///// 
        ///// </summary>
        //public DateTime UpdateDate
        //{
        //    get { return DensiHoukatu.UpdateDate; }
        //}

        ///// <summary>
        ///// 更新者ID
        ///// 
        ///// </summary>
        //public int UpdateId
        //{
        //    get { return DensiHoukatu.UpdateId; }
        //}

        ///// <summary>
        ///// 更新端末
        ///// 
        ///// </summary>
        //public string UpdateMachine
        //{
        //    get { return DensiHoukatu.UpdateMachine; }
        //}


    }

}
