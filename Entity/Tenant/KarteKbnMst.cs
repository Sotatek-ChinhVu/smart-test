using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "KARTE_KBN_MST")]
    public class KarteKbnMst : EmrCloneable<KarteKbnMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Key]
        [Column("HP_ID", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int HpId { get; set; }

        /// <summary>
        /// カルテ区分
        /// >100は、ユーザー任意設定
        /// </summary>
        //[Key]
        [Column("KARTE_KBN", Order = 2)]
        public int KarteKbn { get; set; }

        /// <summary>
        /// 区分名称
        /// 
        /// </summary>
        [Column("KBN_NAME")]
        [MaxLength(10)]
        public string KbnName { get; set; } = string.Empty;

        /// <summary>
        /// 区分略称
        /// カルテ画面所見に表示する略称
        /// </summary>
        [Column("KBN_SHORT_NAME")]
        [MaxLength(1)]
        public string KbnShortName { get; set; } = string.Empty;

        /// <summary>
        /// 画像使用可否
        /// 1: 画像（シェーマ）使用可
        /// </summary>
        [Column("CAN_IMG")]
        [CustomAttribute.DefaultValue(0)]
        public int CanImg { get; set; }

        /// <summary>
        /// 並び順
        /// ※並び順は、カルテ区分>100の場合、101からの採番にする
        /// </summary>
        [Column("SORT_NO")]
        [CustomAttribute.DefaultValue(1)]
        public int SortNo { get; set; }

        /// <summary>
        /// 削除区分
        /// 1: 削除
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        [Column("CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("CREATE_MACHINE")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// 
        /// </summary>
        [Column("UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("UPDATE_MACHINE")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; }  = string.Empty;

    }
}
