using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "priority_haihan_mst")]
    public class PriorityHaihanMst : EmrCloneable<PriorityHaihanMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 背反グループコード
        /// 
        /// </summary>
        
        [Column("haihan_grp", Order = 2)]
        public long HaihanGrp { get; set; }

        /// <summary>
        /// 算定数
        /// 2～8
        /// </summary>
        [Column("count")]
        [CustomAttribute.DefaultValue(1)]
        public int Count { get; set; }

        /// <summary>
        /// 項目コード１
        /// 
        /// </summary>
        [Column("item_cd1")]
        [MaxLength(10)]
        public string? ItemCd1 { get; set; } = string.Empty;

        /// <summary>
        /// 項目コード２
        /// 
        /// </summary>
        [Column("item_cd2")]
        [MaxLength(10)]
        public string? ItemCd2 { get; set; } = string.Empty;

        /// <summary>
        /// 項目コード３
        /// 
        /// </summary>
        [Column("item_cd3")]
        [MaxLength(10)]
        public string? ItemCd3 { get; set; } = string.Empty;

        /// <summary>
        /// 項目コード４
        /// 
        /// </summary>
        [Column("item_cd4")]
        [MaxLength(10)]
        public string? ItemCd4 { get; set; } = string.Empty;

        /// <summary>
        /// 項目コード５
        /// 
        /// </summary>
        [Column("item_cd5")]
        [MaxLength(10)]
        public string? ItemCd5 { get; set; } = string.Empty;

        /// <summary>
        /// 項目コード６
        /// 
        /// </summary>
        [Column("item_cd6")]
        [MaxLength(10)]
        public string? ItemCd6 { get; set; } = string.Empty;

        /// <summary>
        /// 項目コード７
        /// 
        /// </summary>
        [Column("item_cd7")]
        [MaxLength(10)]
        public string? ItemCd7 { get; set; } = string.Empty;

        /// <summary>
        /// 項目コード８
        /// 
        /// </summary>
        [Column("item_cd8")]
        [MaxLength(10)]
        public string? ItemCd8 { get; set; } = string.Empty;

        /// <summary>
        /// 項目コード９
        /// 
        /// </summary>
        [Column("item_cd9")]
        [MaxLength(10)]
        public string? ItemCd9 { get; set; } = string.Empty;

        /// <summary>
        /// 特例条件
        /// "背反条件に特別な条件がある場合に設定する 
        /// 0: 条件なし
        /// 1: 条件あり "
        /// </summary>
        [Column("sp_jyoken")]
        [CustomAttribute.DefaultValue(0)]
        public int SpJyoken { get; set; }

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
        /// チェック期間数
        /// "TERM_SBTと組み合わせて使用
        /// ※TERM_SBT in (1,4)のときのみ有効
        /// 例）2日の場合、TERM_CNT=2, TERM_SBT=1と登録"
        /// </summary>
        [Column("term_cnt")]
        [CustomAttribute.DefaultValue(0)]
        public int TermCnt { get; set; }

        /// <summary>
        /// チェック期間種別
        /// 0:未指定 1:来院 2:日 3:暦週 4:暦月 5:週 6:月 9:患者あたり
        /// </summary>
        [Column("term_sbt")]
        [CustomAttribute.DefaultValue(0)]
        public int TermSbt { get; set; }

        /// <summary>
        /// ユーザー設定
        /// "0: システム設定分
        /// 1: ユーザー設定分"
        /// </summary>
        
        [Column("user_setting", Order = 4)]
        [CustomAttribute.DefaultValue(0)]
        public int UserSetting { get; set; }

        /// <summary>
        /// 対象保険種
        /// "0:健保・労災とも対象
        /// 1:健保のみ対象
        /// 2:労災のみ対象"
        /// </summary>
        [Column("target_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int TargetKbn { get; set; }

        /// <summary>
        /// 無効区分
        /// "0: 有効
        /// 1: 無効"
        /// </summary>
        [Column("is_invalid")]
        [CustomAttribute.DefaultValue(0)]
        public int IsInvalid { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
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
        public string? UpdateMachine { get; set; }  = string.Empty;
    }
}
