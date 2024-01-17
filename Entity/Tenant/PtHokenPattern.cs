using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 患者保険組合せ
    ///		保険メモもしくは、組合せの変更があった場合は履歴管理する。																		
    ///		適用開始日・適用終了日の変更は各テーブルで履歴管理されるため、このテーブルは更新者情報の更新のみとする。																		
    /// </summary>
    [Table("pt_hoken_pattern")]
    [Index(nameof(HpId), nameof(PtId), nameof(StartDate), nameof(EndDate), nameof(IsDeleted), Name = "pt_hoken_pattern_idx01")]
    public class PtHokenPattern : EmrCloneable<PtHokenPattern>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        ///		患者を識別するためのシステム固有の番号
        /// </summary>
        
        [Column("pt_id", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 保険ID
        ///		患者別に保険情報を識別するための固有の番号
        /// </summary>
        
        [Column("hoken_pid", Order = 3)]
        public int HokenPid { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        
        [Column("seq_no", Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// 保険区分
        ///		0:自費
        ///		1:社保
        ///		2:国保
        ///		11:労災(短期給付)
        ///		12:労災(傷病年金)
        ///		13:アフターケア
        ///		14:自賠責
        /// </summary>
        [Column("hoken_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenKbn { get; set; }

        /// <summary>
        /// 保険種別コード
        ///     0: 下記以外
        ///         左から          
        ///             1桁目 - 1:社保 2:国保 3:後期 4:退職 5:公費          
        ///             2桁目 - 組合せ数          
        ///             3桁目 - 1:単独 2:２併 .. 5:５併          
        ///         例) 社保単独     = 111    
        ///             社保２併(54)     = 122    
        ///             社保２併(マル長+54)     = 132    
        ///             国保単独     = 211    
        ///             国保２併(54)     = 222    
        ///             国保２併(マル長+54)     = 232    
        ///             公費単独(12)     = 511    
        ///             公費２併(21+12)     = 522    
        /// </summary>
        [Column("hoken_sbt_cd")]
        public int HokenSbtCd { get; set; }

        /// <summary>
        /// 主保険 保険ID
        /// </summary>
        [Column("hoken_id")]
        public int HokenId { get; set; }

        /// <summary>
        /// 公費１ 保険ID
        /// </summary>
        [Column("kohi1_id")]
        public int Kohi1Id { get; set; }

        /// <summary>
        /// 公費２ 保険ID
        /// </summary>
        [Column("kohi2_id")]
        public int Kohi2Id { get; set; }

        /// <summary>
        /// 公費３ 保険ID
        /// </summary>
        [Column("kohi3_id")]
        public int Kohi3Id { get; set; }

        /// <summary>
        /// 公費４ 保険ID
        /// </summary>
        [Column("kohi4_id")]
        public int Kohi4Id { get; set; }

        /// <summary>
        /// 保険メモ
        /// </summary>
        [Column("hoken_memo")]
        [MaxLength(400)]
        public string? HokenMemo { get; set; } = string.Empty;

        /// <summary>
        /// 適用開始日
        ///		主保険の適用開始日(主保険を持たない場合は公費１ or 労災)										
        /// </summary>
        [Column("start_date")]
        public int StartDate { get; set; }

        /// <summary>
        /// 適用終了日
        ///		主保険の適用終了日(主保険を持たない場合は公費１ or 労災)										
        /// </summary>
        [Column("end_date")]
        public int EndDate { get; set; }

        /// <summary>
        /// 削除区分
        ///		1:削除
        /// </summary>
        [Column("is_deleted")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時	
        /// </summary>
        [Column("create_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者		
        /// </summary>
        [Column(name: "create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末			
        /// </summary>
        [Column(name: "create_machine")]
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
        [Column(name: "update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末			
        /// </summary>
        [Column(name: "update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; } = string.Empty;
    }
}