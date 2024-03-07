using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "sta_csv")]
    public class StaCsv : EmrCloneable<StaCsv>
    {
        /// <summary>
        /// ID
        /// 
        /// </summary>
        
        [Column("id", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Column("hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 帳票ID
        /// 
        /// </summary>
        [Column("report_id")]
        public int ReportId { get; set; }

        /// <summary>
        /// 行番号
        /// 
        /// </summary>
        [Column("row_no")]
        public int RowNo { get; set; }

        /// <summary>
        /// 設定名称
        /// 
        /// </summary>
        [Column("conf_name")]
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
        [Column("data_sbt")]
        public int DataSbt { get; set; }

        /// <summary>
        /// 選択項目
        /// カラム名をカンマ区切り
        /// </summary>
        [Column("columns")]
        [MaxLength(1000)]
        public string? Columns { get; set; } = string.Empty;

        /// <summary>
        /// 出力順
        /// 
        /// </summary>
        [Column("sort_kbn")]
        public int SortKbn { get; set; }

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
        /// 
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
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
