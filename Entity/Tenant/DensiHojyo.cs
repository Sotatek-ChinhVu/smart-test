using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "densi_hojyo")]
    public class DensiHojyo : EmrCloneable<DensiHojyo>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        
        [Column("item_cd", Order = 2)]
        [MaxLength(10)]
        public string ItemCd { get; set; } = string.Empty;

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
        [Column("houkatu_term1")]
        [CustomAttribute.DefaultValue(0)]
        public int HoukatuTerm1 { get; set; }

        /// <summary>
        /// 包括グループ番号１
        /// 0 "包括・被包括グループ番号を表す。 
        /// 包括・被包括テーブルの参照先グループを表す。"
        /// </summary>
        [Column("houkatu_grp_no1")]
        [CustomAttribute.DefaultValue(0)]
        [MaxLength(7)]
        public string? HoukatuGrpNo1 { get; set; } = string.Empty;

        /// <summary>
        /// 包括単位２
        /// HOUKATU_TERM1と同じ
        /// </summary>
        [Column("houkatu_term2")]
        [CustomAttribute.DefaultValue(0)]
        public int HoukatuTerm2 { get; set; }

        /// <summary>
        /// 包括グループ番号２
        /// 0 HOUKATU_GRP_NO1と同じ
        /// </summary>
        [Column("houkatu_grp_no2")]
        [CustomAttribute.DefaultValue(0)]
        [MaxLength(7)]
        public string? HoukatuGrpNo2 { get; set; } = string.Empty;

        /// <summary>
        /// 包括単位３
        /// HOUKATU_TERM1と同じ
        /// </summary>
        [Column("houkatu_term3")]
        [CustomAttribute.DefaultValue(0)]
        public int HoukatuTerm3 { get; set; }

        /// <summary>
        /// 包括グループ番号３
        /// 0 HOUKATU_GRP_NO1と同じ
        /// </summary>
        [Column("houkatu_grp_no3")]
        [CustomAttribute.DefaultValue(0)]
        [MaxLength(7)]
        public string? HoukatuGrpNo3 { get; set; } = string.Empty;

        /// <summary>
        /// 背反識別（1日につき）
        /// "背反関連テーブル（１日につき）
        /// との関連の有無 
        /// 0: 関連なし 
        /// 1: 関連あり"
        /// </summary>
        [Column("haihan_day")]
        [CustomAttribute.DefaultValue(0)]
        public int HaihanDay { get; set; }

        /// <summary>
        /// 背反識別（同一月内）
        /// "背反関連テーブル（同一月内）
        /// との関連の有無 
        /// 0: 関連なし 
        /// 1: 関連あり"
        /// </summary>
        [Column("haihan_month")]
        [CustomAttribute.DefaultValue(0)]
        public int HaihanMonth { get; set; }

        /// <summary>
        /// 背反識別（同時）
        /// "背反関連テーブル（同時）
        /// との関連の有無 
        /// 0: 関連なし 
        /// 1: 関連あり"
        /// </summary>
        [Column("haihan_karte")]
        [CustomAttribute.DefaultValue(0)]
        public int HaihanKarte { get; set; }

        /// <summary>
        /// 背反識別（1週間につき)
        /// "背反関連テーブル（1週間につき）
        /// との関連の有無 
        /// 0: 関連なし 
        /// 1: 関連あり"
        /// </summary>
        [Column("haihan_week")]
        [CustomAttribute.DefaultValue(0)]
        public int HaihanWeek { get; set; }

        /// <summary>
        /// 入院基本識別
        /// "当該診療行為と入院基本料加算との算定可否を表す。 
        /// 入院基本料テーブルの参照先グループを表す。 "
        /// </summary>
        [Column("nyuin_id")]
        [CustomAttribute.DefaultValue(0)]
        public int NyuinId { get; set; }

        /// <summary>
        /// 算定回数関連
        /// "算定回数テーブルとの関連の有無 
        /// 0: 関連なし 
        /// 1: 関連あり"
        /// </summary>
        [Column("santei_kaisu")]
        [CustomAttribute.DefaultValue(0)]
        public int SanteiKaisu { get; set; }

        /// <summary>
        /// 新設年月日
        /// レコード情報を新設した日付を西暦年4桁、月2桁及び日2桁の8桁で表す。
        /// </summary>
        
        [Column("start_date", Order = 3)]
        [CustomAttribute.DefaultValue(0)]
        public int StartDate { get; set; }

        /// <summary>
        /// 廃止年月日
        /// "当該診療行為の使用が可能な最終日付を西暦年4桁、月2桁及び日2桁の8桁で表す。 
        /// なお、廃止診療行為でない場合は「99999999」とする。"
        /// </summary>
        [Column("end_date")]
        [CustomAttribute.DefaultValue(99999999)]
        public int EndDate { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者ID
        /// 
        /// </summary>
        [Column("create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("update_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者ID
        /// 
        /// </summary>
        [Column("update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
