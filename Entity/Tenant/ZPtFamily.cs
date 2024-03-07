using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 家族情報
    /// </summary>
    [Table(name: "z_pt_family")]
    public class ZPtFamily : EmrCloneable<ZPtFamily>
    {
        
        [Column("op_id", Order = 1)]
        public long OpId { get; set; }

        [Column("op_type")]
        [MaxLength(10)]
        public string? OpType { get; set; } = string.Empty;

        [Column("op_time")]
        public DateTime OpTime { get; set; }

        [Column("op_addr")]
        [MaxLength(100)]
        public string? OpAddr { get; set; } = string.Empty;

        [Column("op_hostname")]
        [MaxLength(100)]
        public string? OpHostName { get; set; } = string.Empty;

        /// <summary>
        /// 家族ID
        ///     患者の家族を識別するための番号
        /// </summary>
        [Column("family_id")]
        //[Index("pt_family_idx01", 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FamilyId { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Column("hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        ///     患者を識別するためのシステム固有の番号
        /// </summary>
        [Column("pt_id")]
        //[Index("pt_family_idx01", 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        [Column("seq_no")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// 続柄コード
        /// </summary>
        [Column("zokugara_cd")]
        [Required]
        [MaxLength(10)]
        public string? ZokugaraCd { get; set; } = string.Empty;

        /// <summary>
        /// 並び順
        /// </summary>
        [Column("sort_no")]
        [CustomAttribute.DefaultValue(1)]
        public int SortNo { get; set; }

        /// <summary>
        /// 親ID
        ///     孫の親の家族ID
        /// </summary>
        [Column("parent_id")]
        [CustomAttribute.DefaultValue(0)]
        public int ParentId { get; set; }

        /// <summary>
        /// 家族患者ID
        /// </summary>
        [Column("family_pt_id")]
        //[Index("pt_family_idx01", 3)]
        [CustomAttribute.DefaultValue(0)]
        public long FamilyPtId { get; set; }

        /// <summary>
        /// カナ氏名
        /// </summary>
        [Column("kana_name")]
        [MaxLength(100)]
        public string? KanaName { get; set; } = string.Empty;

        /// <summary>
        /// 氏名
        /// </summary>
        [Column("name")]
        [MaxLength(100)]
        public string? Name { get; set; } = string.Empty;

        /// <summary>
        /// 性別
        ///     1:男
        ///     2:女
        /// </summary>
        [Column("sex")]
        public int Sex { get; set; }

        /// <summary>
        /// 生年月日
        ///     yyyymmdd
        /// </summary>
        [Column("birthday")]
        public int Birthday { get; set; }

        /// <summary>
        /// 死亡区分
        ///     0:生存
        ///     1:死亡
        ///     2:消息不明
        /// </summary>
        [Column("is_dead")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDead { get; set; }

        /// <summary>
        /// 別居フラグ
        ///     1:別居
        /// </summary>
        [Column("is_separated")]
        [CustomAttribute.DefaultValue(0)]
        public int IsSeparated { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        [Column("biko")]
        [MaxLength(120)]
        public string? Biko { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        ///     1:削除
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
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
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        [Column("update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// </summary>
        [Column("update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; }  = string.Empty;
    }
}
