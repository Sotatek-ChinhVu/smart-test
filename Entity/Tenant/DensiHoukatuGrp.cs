using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "densi_houkatu_grp")]
    [Index(nameof(HpId), nameof(ItemCd), nameof(StartDate), nameof(EndDate), nameof(TargetKbn), nameof(IsInvalid), Name = "densi_houkatu_grp_idx02")]
    public class DensiHoukatuGrp : EmrCloneable<DensiHoukatuGrp>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 包括グループ番号
        /// 
        /// </summary>
        
        [Column("houkatu_grp_no", Order = 2)]
        [MaxLength(7)]
        public string HoukatuGrpNo { get; set; } = string.Empty;

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        
        [Column("item_cd", Order = 3)]
        [MaxLength(10)]
        public string ItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 特例条件
        /// "包括・被包括の条件に特別な条件がある場合に設定する 
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
        
        [Column("start_date", Order = 4)]
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
        /// 連番
        /// 
        /// </summary>
        
        [Column("seq_no", Order = 5)]
        [CustomAttribute.DefaultValue(1)]
        public long SeqNo { get; set; }

        /// <summary>
        /// ユーザー設定
        /// "0: システム設定分
        /// 1: ユーザー設定分"
        /// </summary>
        
        [Column("user_setting", Order = 6)]
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
