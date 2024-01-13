using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Entity.Tenant
{
    [Table(name: "kensa_set")]
    [Serializable]
    [Index(nameof(HpId), nameof(SetId), Name = "kensa_set_pkey")]
    public class KensaSet
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        [MaxLength(2)]
        public int HpId { get; set; }

        /// <summary>
        /// セットコード
        /// 
        /// </summary>

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("set_id", Order = 2)]
        [MaxLength(9)]
        public int SetId { get; set; }

        /// <summary>
        /// セット名称
        /// 
        /// </summary>
        [Column("set_name")]
        [MaxLength(30)]
        public string? SetName { get; set; } = string.Empty;

        /// <summary>
        /// 並び順
        /// 
        /// </summary>
        [Column("sort_no")]
        [MaxLength(9)]
        public int SortNo { get; set; }

        /// <summary>
        /// 削除フラグ
        /// 
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        [MaxLength(1)]
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
        [MaxLength(8)]
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
        [MaxLength(8)]
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
