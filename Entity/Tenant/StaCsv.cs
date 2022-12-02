using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "STA_CSV")]
    public class StaCsv : EmrCloneable<StaCsv>
    {
        /// <summary>
        /// ID
        /// 
        /// </summary>
        [Key]
        [Column("ID", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Column("HP_ID")]
        public int HpId { get; set; }

        /// <summary>
        /// 帳票ID
        /// 
        /// </summary>
        [Column("REPORT_ID")]
        public int ReportId { get; set; }

        /// <summary>
        /// 行番号
        /// 
        /// </summary>
        [Column("ROW_NO")]
        public int RowNo { get; set; }

        /// <summary>
        /// 設定名称
        /// 
        /// </summary>
        [Column("CONF_NAME")]
        [MaxLength(100)]
        public string? ConfName { get; set; } = string.Empty;

        /// <summary>
        /// データ種別
        /// 1:患者情報
        ///                     2:保険情報          
        ///                     3:病名情報          
        ///                     4:来院情報          
        ///                     5:診療情報（オーダー）          
        ///                     6:診療情報（算定）          
        ///                     7:カルテ情報          
        ///                     8:検査情報          
        /// </summary>
        [Column("DATA_SBT")]
        public int DataSbt { get; set; }

        /// <summary>
        /// 選択項目
        /// カラム名をカンマ区切り
        /// </summary>
        [Column("COLUMNS")]
        [MaxLength(1000)]
        public string? Columns { get; set; } = string.Empty;

        /// <summary>
        /// 出力順
        /// 
        /// </summary>
        [Column("SORT_KBN")]
        public int SortKbn { get; set; }

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
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}
