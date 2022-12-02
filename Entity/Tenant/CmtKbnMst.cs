using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "CMT_KBN_MST")]
    [Index(nameof(HpId), nameof(ItemCd), nameof(StartDate), Name = "CMT_KBN_MST_IDX01")]
    public class CmtKbnMst : EmrCloneable<CmtKbnMst>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Column("HP_ID")]
        public int HpId { get; set; }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        [Column("ITEM_CD")]
        [MaxLength(10)]
        public string ItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 開始日
        /// 
        /// </summary>
        [Column("START_DATE")]
        public int StartDate { get; set; }

        /// <summary>
        /// 終了日
        /// 
        /// </summary>
        [Column("END_DATE")]
        public int EndDate { get; set; }

        /// <summary>
        /// コメント区分
        /// "計算時、コメントを自動で付与する場合、付与するコメントの種類を設定
        /// 0: 自動付与なし
        /// 1: 実施日
        /// 2: 前回日（840000087 :前回実施　　　月　　日）
        /// 3: 初回日（840000085 :初回実施　　　月　　日）
        /// 4: 前回日 or 初回日
        /// 5: 初回算定日（840000085 :初回算定　　　月　　日）
        /// 6: 実施日（列挙）
        /// 7: 実施日（列挙：前月末・翌月頭含む）
        /// 8: 実施日（列挙：項目名あり）
        /// 9: 実施日数（840000096 :実施日数　　　日）※未使用
        /// 10: 前回日 or 初回日（項目名あり）
        /// 11: 数量コメント（RECEDEN_CMT_SELECTに登録がある場合のみ有効)"
        /// </summary>
        [Column("CMT_KBN")]
        public int CmtKbn { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者ID
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
        /// 更新者ID
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

        /// <summary>
        /// ID
        /// 
        /// </summary>
        
        [Column("ID", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
    }
}
