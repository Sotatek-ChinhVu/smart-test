using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "rsv_frame_with")]
    public class RsvFrameWith : EmrCloneable<RsvFrameWith>
    {
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 2)]
        public int HpId { get; set; }

        /// <summary>
        /// 予約枠ID
        /// 
        /// </summary>
        
        [Column("rsv_frame_id", Order = 3)]
        public int RsvFrameId { get; set; }

        /// <summary>
        /// 併表示予約枠ID
        /// 
        /// </summary>
        [Column("with_frame_id", Order = 4)]
        public int WithFrameId { get; set; }

        /// <summary>
        /// 並び順
        /// 
        /// </summary>
        [Column("sort_key")]
        public int SortKey { get; set; }

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
