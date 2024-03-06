using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "santei_auto_order")]
    public class SanteiAutoOrder : EmrCloneable<SanteiAutoOrder>
    {
        /// <summary>
        /// ID
        /// </summary>
        
        [Column("id", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 2)]
        public int HpId { get; set; }

        /// <summary>
        /// グループコード
        /// SANTEI_GRP_MST.SANTEI_GRP_CD
        /// </summary>
        
        [Column("santei_grp_cd", Order = 3)]
        [CustomAttribute.DefaultValue(0)]
        public int SanteiGrpCd { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        
        [Column("seq_no", Order = 4)]
        [CustomAttribute.DefaultValue(0)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 適用開始日
        /// YYYYMMDD
        /// </summary>
        [Column("start_date")]
        [CustomAttribute.DefaultValue(0)]
        public int StartDate { get; set; }

        /// <summary>
        /// 適用終了日
        /// YYYYMMDD
        /// </summary>
        [Column("end_date")]
        [CustomAttribute.DefaultValue(99999999)]
        public int EndDate { get; set; }

        /// <summary>
        /// 追加方法
        /// "1:別Rpに追加する
        /// 2:同一Rpに追加する"
        /// </summary>
        [Column("add_type")]
        [CustomAttribute.DefaultValue(0)]
        public int AddType { get; set; }

        /// <summary>
        /// 追加対象
        /// "1:詳細のいずれか1つの項目を追加する
        /// 　（詳細が複数の時のみ有効）
        /// 2:詳細のすべての項目を追加する"
        /// </summary>
        [Column("add_target")]
        [CustomAttribute.DefaultValue(0)]
        public int AddTarget { get; set; }

        /// <summary>
        /// チェック期間数
        /// "TERM_SBTと組み合わせて使用
        /// ※TERM_SBT in (1,4)のときのみ有効
        /// 例）2日の場合、TERM_CNT=2, TERM_SBT=1と登録"
        /// </summary>
        [Column("term_cnt")]
        [CustomAttribute.DefaultValue(0)]
        public int TermCnt { get; set; }

        /// <summary>
        /// チェック期間種別
        /// 0:未指定 1:来院 2:日 3:暦週 4:暦月 5:週 6:月 9:患者あたり
        /// </summary>
        [Column("term_sbt")]
        [CustomAttribute.DefaultValue(0)]
        public int TermSbt { get; set; }

        /// <summary>
        /// 算定回数基準
        /// "0:算定数量ベース
        /// 1:算定回数ベース
        /// 2:オーダー数量ベース
        /// 3:オーダー回数ベース"
        /// </summary>
        [Column("cnt_type")]
        [CustomAttribute.DefaultValue(0)]
        public int CntType { get; set; }

        /// <summary>
        /// 算定回数上限
        /// 
        /// </summary>
        [Column("max_cnt")]
        [CustomAttribute.DefaultValue(0)]
        public long MaxCnt { get; set; }

        /// <summary>
        /// 特殊条件
        /// "0:通常条件
        /// 1:リハビリ特殊処理(起算日からの経過日数チェック)"
        /// </summary>
        [Column("sp_condition")]
        [CustomAttribute.DefaultValue(0)]
        public int SpCondition { get; set; }

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
