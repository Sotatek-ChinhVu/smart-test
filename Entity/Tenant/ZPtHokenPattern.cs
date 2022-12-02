using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 患者保険組合せ
    ///		保険メモもしくは、組合せの変更があった場合は履歴管理する。																		
    ///		適用開始日・適用終了日の変更は各テーブルで履歴管理されるため、このテーブルは更新者情報の更新のみとする。																		
    /// </summary>
    [Table("Z_PT_HOKEN_PATTERN")]
    public class ZPtHokenPattern : EmrCloneable<ZPtHokenPattern>
    {
        [Key]
        [Column("OP_ID", Order = 1)]
        public long OpId { get; set; }

        [Column("OP_TYPE")]
        [MaxLength(10)]
        public string? OpType { get; set; } = string.Empty;

        [Column("OP_TIME")]
        public DateTime OpTime { get; set; }

        [Column("OP_ADDR")]
        [MaxLength(100)]
        public string? OpAddr { get; set; } = string.Empty;

        [Column("OP_HOSTNAME")]
        [MaxLength(100)]
        public string? OpHostName { get; set; } = string.Empty;

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Column("HP_ID")]
        //[Index("PT_HOKEN_PATTERN_IDX01", 1)]
        //[Index("PT_HOKEN_PATTERN_UKEY", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        ///		患者を識別するためのシステム固有の番号
        /// </summary>
        [Column("PT_ID")]
        //[Index("PT_HOKEN_PATTERN_IDX01", 2)]
        //[Index("PT_HOKEN_PATTERN_UKEY", 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 保険ID
        ///		患者別に保険情報を識別するための固有の番号
        /// </summary>
        [Column("HOKEN_PID")]
        //[Index("PT_HOKEN_PATTERN_UKEY", 3)]
        public int HokenPid { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        [Column("SEQ_NO")]
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
        [Column("HOKEN_KBN")]
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
        [Column("HOKEN_SBT_CD")]
        public int HokenSbtCd { get; set; }

        /// <summary>
        /// 主保険 保険ID
        /// </summary>
        [Column("HOKEN_ID")]
        public int HokenId { get; set; }

        /// <summary>
        /// 公費１ 保険ID
        /// </summary>
        [Column("KOHI1_ID")]
        public int Kohi1Id { get; set; }

        /// <summary>
        /// 公費２ 保険ID
        /// </summary>
        [Column("KOHI2_ID")]
        public int Kohi2Id { get; set; }

        /// <summary>
        /// 公費３ 保険ID
        /// </summary>
        [Column("KOHI3_ID")]
        public int Kohi3Id { get; set; }

        /// <summary>
        /// 公費４ 保険ID
        /// </summary>
        [Column("KOHI4_ID")]
        public int Kohi4Id { get; set; }

        /// <summary>
        /// 保険メモ
        /// </summary>
        [Column("HOKEN_MEMO")]
        [MaxLength(400)]
        public string? HokenMemo { get; set; } = string.Empty;

        /// <summary>
        /// 適用開始日
        ///		主保険の適用開始日(主保険を持たない場合は公費１ or 労災)										
        /// </summary>
        [Column("START_DATE")]
        //[Index("PT_HOKEN_PATTERN_IDX01", 3)]
        public int StartDate { get; set; }

        /// <summary>
        /// 適用終了日
        ///		主保険の適用終了日(主保険を持たない場合は公費１ or 労災)										
        /// </summary>
        [Column("END_DATE")]
        //[Index("PT_HOKEN_PATTERN_IDX01", 4)]
        public int EndDate { get; set; }

        /// <summary>
        /// 削除区分
        ///		1:削除
        /// </summary>
        [Column("IS_DELETED")]
        //[Index("PT_HOKEN_PATTERN_IDX01", 5)]
        //[Index("PT_HOKEN_PATTERN_UKEY", 4)]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時	
        /// </summary>
        [Column("CREATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者		
        /// </summary>
        [Column(name: "CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末			
        /// </summary>
        [Column(name: "CREATE_MACHINE")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時			
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者			
        /// </summary>
        [Column(name: "UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末			
        /// </summary>
        [Column(name: "UPDATE_MACHINE")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; }  = string.Empty;
    }
}