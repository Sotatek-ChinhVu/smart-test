using Entity.Tenant;

namespace EmrCalculateApi.Ika.Models
{
    public class DensiHojyoModel
    {
        public DensiHojyo DensiHojyo { get; } = null;

        public DensiHojyoModel(DensiHojyo densiHojyo)
        {
            DensiHojyo = densiHojyo;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return DensiHojyo.HpId; }
        }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        public string ItemCd
        {
            get { return DensiHojyo.ItemCd ?? string.Empty; }
        }

        /// <summary>
        /// 包括単位１
        /// "包括する期間を表す
        /// 00: 関連なし
        /// 01: 1日につき
        /// 02: 同一月内
        /// 03: 同時
        /// 05: 手術前1週間
        /// 06: 1手術につき
        /// ※05,06はチェックしない"
        /// </summary>
        public int HoukatuTerm1
        {
            get { return DensiHojyo.HoukatuTerm1; }
        }

        /// <summary>
        /// 包括グループ番号１
        /// 0 "包括・被包括グループ番号を表す。 
        /// 包括・被包括テーブルの参照先グループを表す。"
        /// </summary>
        public string HoukatuGrpNo1
        {
            get { return DensiHojyo.HoukatuGrpNo1 ?? string.Empty; }
        }

        /// <summary>
        /// 包括単位２
        /// HOUKATU_TERM1と同じ
        /// </summary>
        public int HoukatuTerm2
        {
            get { return DensiHojyo.HoukatuTerm2; }
        }

        /// <summary>
        /// 包括グループ番号２
        /// 0 HOUKATU_GRP_NO1と同じ
        /// </summary>
        public string HoukatuGrpNo2
        {
            get { return DensiHojyo.HoukatuGrpNo2 ?? string.Empty; }
        }

        /// <summary>
        /// 包括単位３
        /// HOUKATU_TERM1と同じ
        /// </summary>
        public int HoukatuTerm3
        {
            get { return DensiHojyo.HoukatuTerm3; }
        }

        /// <summary>
        /// 包括グループ番号３
        /// 0 HOUKATU_GRP_NO1と同じ
        /// </summary>
        public string HoukatuGrpNo3
        {
            get { return DensiHojyo.HoukatuGrpNo3 ?? string.Empty; }
        }

        /// <summary>
        /// 背反識別（1日につき）
        /// "背反関連テーブル（１日につき）
        /// との関連の有無 
        /// 0: 関連なし 
        /// 1: 関連あり"
        /// </summary>
        public int HaihanDay
        {
            get { return DensiHojyo.HaihanDay; }
        }

        /// <summary>
        /// 背反識別（同一月内）
        /// "背反関連テーブル（同一月内）
        /// との関連の有無 
        /// 0: 関連なし 
        /// 1: 関連あり"
        /// </summary>
        public int HaihanMonth
        {
            get { return DensiHojyo.HaihanMonth; }
        }

        /// <summary>
        /// 背反識別（同時）
        /// "背反関連テーブル（同時）
        /// との関連の有無 
        /// 0: 関連なし 
        /// 1: 関連あり"
        /// </summary>
        public int HaihanKarte
        {
            get { return DensiHojyo.HaihanKarte; }
        }

        /// <summary>
        /// 背反識別（1週間につき)
        /// "背反関連テーブル（1週間につき）
        /// との関連の有無 
        /// 0: 関連なし 
        /// 1: 関連あり"
        /// </summary>
        public int HaihanWeek
        {
            get { return DensiHojyo.HaihanWeek; }
        }

        /// <summary>
        /// 入院基本識別
        /// "当該診療行為と入院基本料加算との算定可否を表す。 
        /// 入院基本料テーブルの参照先グループを表す。 "
        /// </summary>
        public int NyuinId
        {
            get { return DensiHojyo.NyuinId; }
        }

        /// <summary>
        /// 算定回数関連
        /// "算定回数テーブルとの関連の有無 
        /// 0: 関連なし 
        /// 1: 関連あり"
        /// </summary>
        public int SanteiKaisu
        {
            get { return DensiHojyo.SanteiKaisu; }
        }

        /// <summary>
        /// 新設年月日
        /// レコード情報を新設した日付を西暦年4桁、月2桁及び日2桁の8桁で表す。
        /// </summary>
        public int StartDate
        {
            get { return DensiHojyo.StartDate; }
        }

        /// <summary>
        /// 廃止年月日
        /// "当該診療行為の使用が可能な最終日付を西暦年4桁、月2桁及び日2桁の8桁で表す。 
        /// なお、廃止診療行為でない場合は「99999999」とする。"
        /// </summary>
        public int EndDate
        {
            get { return DensiHojyo.EndDate; }
        }

        ///// <summary>
        ///// 作成日時
        ///// 
        ///// </summary>
        //public DateTime CreateDate
        //{
        //    get { return DensiHojyo.CreateDate; }
        //}

        ///// <summary>
        ///// 作成者ID
        ///// 
        ///// </summary>
        //public int CreateId
        //{
        //    get { return DensiHojyo.CreateId; }
        //}

        ///// <summary>
        ///// 作成端末
        ///// 
        ///// </summary>
        //public string CreateMachine
        //{
        //    get { return DensiHojyo.CreateMachine; }
        //}

        ///// <summary>
        ///// 更新日時
        ///// 
        ///// </summary>
        //public DateTime UpdateDate
        //{
        //    get { return DensiHojyo.UpdateDate; }
        //}

        ///// <summary>
        ///// 更新者ID
        ///// 
        ///// </summary>
        //public int UpdateId
        //{
        //    get { return DensiHojyo.UpdateId; }
        //}

        ///// <summary>
        ///// 更新端末
        ///// 
        ///// </summary>
        //public string UpdateMachine
        //{
        //    get { return DensiHojyo.UpdateMachine; }
        //}


    }

}
