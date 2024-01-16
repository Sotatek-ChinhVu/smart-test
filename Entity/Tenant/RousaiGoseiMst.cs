using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "rousai_gosei_mst")]
    public class RousaiGoseiMst : EmrCloneable<RousaiGoseiMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 合成グループ
        /// 
        /// </summary>
        
        [Column("gosei_grp", Order = 2)]
        public int GoseiGrp { get; set; }

        /// <summary>
        /// 合成項目コード
        /// 
        /// </summary>
        
        [Column("gosei_item_cd", Order = 3)]
        [MaxLength(10)]
        public string GoseiItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 診療行為コード
        /// 
        /// </summary>
        
        [Column("item_cd", Order = 4)]
        [MaxLength(10)]
        public string ItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 四肢加算区分
        /// 
        /// </summary>
        
        [Column("sisi_kbn", Order = 5)]
        public int SisiKbn { get; set; }

        /// <summary>
        /// 使用開始日
        /// 
        /// </summary>
        
        [Column("start_date", Order = 6)]
        public int StartDate { get; set; }

        /// <summary>
        /// 使用終了日
        /// 
        /// </summary>
        [Column("end_date")]
        public int EndDate { get; set; }

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
