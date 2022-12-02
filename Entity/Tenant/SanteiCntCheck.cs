using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "SANTEI_CNT_CHECK")]
    public class SanteiCntCheck : EmrCloneable<SanteiCntCheck>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// グループコード
        /// CALC_GRP.SANTEI_GRP_CD
        /// </summary>
        //[Key]
        [Column("SANTEI_GRP_CD", Order = 2)]
        public int SanteiGrpCd { get; set; }

        /// <summary>
        /// 連番
        /// 同一グループコード内のチェック順番
        /// </summary>
        //[Key]
        [Column("SEQ_NO", Order = 3)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 適用開始日
        /// YYYYMMDD
        /// </summary>
        [Column("START_DATE")]
        public int StartDate { get; set; }

        /// <summary>
        /// 適用終了日
        /// YYYYMMDD
        /// </summary>
        [Column("END_DATE")]
        public int EndDate { get; set; }

        /// <summary>
        /// チェック期間数
        /// "TERM_SBTと組み合わせて使用
        /// ※TERM_SBT in (1,4)のときのみ有効
        /// 例）2日の場合、UNIT_CNT=2, TERM_SBT=1と登録"
        /// </summary>
        [Column("TERM_CNT")]
        public int TermCnt { get; set; }

        /// <summary>
        /// チェック期間種別
        /// 0:未指定 1:来院 2:日 3:暦週 4:暦月 5:週 6:月 9:患者あたり
        /// </summary>
        [Column("TERM_SBT")]
        public int TermSbt { get; set; }

        /// <summary>
        /// 算定回数カウント方法
        /// "0:算定数量ベース
        /// 1:算定回数ベース
        /// 2:オーダー数量ベース
        /// 3:オーダー回数ベース"
        /// </summary>
        [Column("CNT_TYPE")]
        public int CntType { get; set; }

        /// <summary>
        /// 算定回数上限
        /// 
        /// </summary>
        [Column("MAX_CNT")]
        public long MaxCnt { get; set; }

        /// <summary>
        /// 単位
        /// 
        /// </summary>
        [Column("UNIT_NAME")]
        [MaxLength(10)]
        public string? UnitName { get; set; } = string.Empty;

        /// <summary>
        /// 上限超動作
        /// "0:警告
        /// 1:上限でカットする
        /// 2:対象項目に置き換える
        /// 3:上限を超えるとき(数量2以上)はカット、
        ///  上限を超えているときは置き換え
        /// 4:対象項目を追加する"
        /// </summary>
        [Column("ERR_KBN")]
        public int ErrKbn { get; set; }

        /// <summary>
        /// 対象項目コード
        /// 
        /// </summary>
        [Column("TARGET_CD")]
        [MaxLength(10)]
        public string? TargetCd { get; set; } = string.Empty;

        /// <summary>
        /// 特殊条件
        /// "0:通常条件
        /// 1:リハビリ特殊処理(起算日からの経過日数チェック)"
        /// </summary>
        [Column("SP_CONDITION")]
        public int SpCondition { get; set; }

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
