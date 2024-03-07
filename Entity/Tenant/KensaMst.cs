using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "kensa_mst")]
    [Serializable]
    [Index(nameof(KensaItemCd), Name = "kensa_mst_idx01")]
    public class KensaMst : EmrCloneable<KensaMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 検査項目コード
        /// 
        /// </summary>

        [Column("kensa_item_cd", Order = 2)]
        [MaxLength(10)]
        public string KensaItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 連番
        /// 
        /// </summary>

        [Column("kensa_item_seq_no", Order = 3)]
        [CustomAttribute.DefaultValue(0)]
        public int KensaItemSeqNo { get; set; }

        /// <summary>
        /// センターコード
        /// 
        /// </summary>
        [Column("center_cd")]
        [MaxLength(10)]
        public string? CenterCd { get; set; } = string.Empty;

        /// <summary>
        /// 漢字名称
        /// 
        /// </summary>
        [Column("kensa_name")]
        [MaxLength(120)]
        public string? KensaName { get; set; } = string.Empty;

        /// <summary>
        /// カナ名称
        /// 
        /// </summary>
        [Column("kensa_kana")]
        [MaxLength(20)]
        public string? KensaKana { get; set; } = string.Empty;

        /// <summary>
        /// 単位
        /// 
        /// </summary>
        [Column("unit")]
        [MaxLength(20)]
        public string? Unit { get; set; } = string.Empty;

        /// <summary>
        /// 材料コード
        /// 
        /// </summary>
        [Column("material_cd")]
        public int MaterialCd { get; set; }

        /// <summary>
        /// 容器コード
        /// 
        /// </summary>
        [Column("container_cd")]
        public int ContainerCd { get; set; }

        /// <summary>
        /// 男性基準値
        /// 
        /// </summary>
        [Column("male_std")]
        [MaxLength(60)]
        public string? MaleStd { get; set; } = string.Empty;

        /// <summary>
        /// 男性基準値下限
        /// 
        /// </summary>
        [Column("male_std_low")]
        [MaxLength(60)]
        public string? MaleStdLow { get; set; } = string.Empty;

        /// <summary>
        /// 男性基準値上限
        /// 
        /// </summary>
        [Column("male_std_high")]
        [MaxLength(60)]
        public string? MaleStdHigh { get; set; } = string.Empty;

        /// <summary>
        /// 女性基準値
        /// 
        /// </summary>
        [Column("female_std")]
        [MaxLength(60)]
        public string? FemaleStd { get; set; } = string.Empty;

        /// <summary>
        /// 女性基準値下限
        /// 
        /// </summary>
        [Column("female_std_low")]
        [MaxLength(60)]
        public string? FemaleStdLow { get; set; } = string.Empty;

        /// <summary>
        /// 女性基準値上限
        /// 
        /// </summary>
        [Column("female_std_high")]
        [MaxLength(60)]
        public string? FemaleStdHigh { get; set; } = string.Empty;

        /// <summary>
        /// 式
        /// 
        /// </summary>
        [Column("formula")]
        [MaxLength(100)]
        public string? Formula { get; set; } = string.Empty;

        /// <summary>
        /// 小数桁
        /// 
        /// </summary>
        [Column("digit")]
        [CustomAttribute.DefaultValue(0)]
        public int Digit { get; set; }

        /// <summary>
        /// 親検査項目コード
        /// 
        /// </summary>
        [Column("oya_item_cd")]
        [MaxLength(10)]
        public string? OyaItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 親検査項目連番
        /// 
        /// </summary>
        [Column("oya_item_seq_no")]
        [CustomAttribute.DefaultValue(0)]
        public int OyaItemSeqNo { get; set; }

        /// <summary>
        /// 並び順
        /// 
        /// </summary>
        [Column("sort_no")]
        [CustomAttribute.DefaultValue(0)]
        public long SortNo { get; set; }

        /// <summary>
        /// 外注コード１
        /// 
        /// </summary>
        [Column("center_item_cd1")]
        [MaxLength(10)]
        public string? CenterItemCd1 { get; set; } = string.Empty;

        /// <summary>
        /// 外注コード２
        /// 
        /// </summary>
        [Column("center_item_cd2")]
        [MaxLength(10)]
        public string? CenterItemCd2 { get; set; } = string.Empty;

        /// <summary>
        /// 削除区分
        /// 
        /// </summary>
        [Column("is_delete")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDelete { get; set; }

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
        /// 更新者
        /// 
        /// </summary>
        [Column("update_id")]
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
