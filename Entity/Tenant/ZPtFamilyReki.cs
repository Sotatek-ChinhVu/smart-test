using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 家族歴
    /// </summary>
    [Table(name: "z_pt_family_reki")]
    public class ZPtFamilyReki : EmrCloneable<ZPtFamilyReki>
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
        /// 連番
        /// </summary>
        [Column("id")]
        //[Index("pt_family_reki_idx01", 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Column("hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// </summary>
        [Column("pt_id")]
        //[Index("pt_family_reki_idx01", 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 家族ID
        ///     PT_FAMILY.家族ID
        /// </summary>
        [Column("family_id")]
        //[Index("pt_family_reki_idx01", 3)]
        public long FamilyId { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        [Column("seq_no")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        [Column("sort_no")]
        public int SortNo { get; set; }

        /// <summary>
        /// 病名コード
        /// </summary>
        [Column("byomei_cd")]
        [MaxLength(7)]
        public string? ByomeiCd { get; set; } = string.Empty;

        /// <summary>
        /// 病態コード
        /// </summary>
        [Column("byotai_cd")]
        [MaxLength(7)]
        public string? ByotaiCd { get; set; } = string.Empty;

        /// <summary>
        /// 病名
        /// </summary>
        [Column("byomei")]
        [MaxLength(400)]
        public string? Byomei { get; set; } = string.Empty;

        /// <summary>
        /// コメント
        /// </summary>
        [Column("cmt")]
        [MaxLength(100)]
        public string? Cmt { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        ///     1:削除
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// </summary>
        [Column("create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
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
