using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "RENKEI_PATH_CONF")]
    public class RenkeiPathConf : EmrCloneable<RenkeiPathConf>
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
        /// 連携ID
        /// 
        /// </summary>
        [Column("RENKEI_ID", Order = 2)]
        public int RenkeiId { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        [Column("SEQ_NO", Order = 3)]
        [CustomAttribute.DefaultValue(0)]
        public int SeqNo { get; set; }

        /// <summary>
        /// 枝番
        /// 
        /// </summary>
        //[Key]
        [Column("EDA_NO", Order = 4)]
        [CustomAttribute.DefaultValue(0)]
        public int EdaNo { get; set; }

        //[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", Order = 5)]
        public long Id { get; set; }

        /// <summary>
        /// パス
        /// 
        /// </summary>
        [Column("PATH")]
        [MaxLength(300)]
        public string? Path { get; set; } = string.Empty;

        /// <summary>
        /// 端末名
        /// "空の場合、端末に関係なく処理する
        /// 特定の端末だけ処理する場合に設定"
        /// </summary>
        [Column("MACHINE")]
        [MaxLength(60)]
        public string? Machine { get; set; } = string.Empty;

        /// <summary>
        /// 文字コード
        /// 0:UTF-8 1:S-JIS
        /// </summary>
        [Column("CHAR_CD")]
        [CustomAttribute.DefaultValue(0)]
        public int CharCd { get; set; }

        /// <summary>
        /// 作業フォルダ
        /// 
        /// </summary>
        [Column("WORK_PATH")]
        [MaxLength(300)]
        public string? WorkPath { get; set; } = string.Empty;

        /// <summary>
        /// 監視間隔（sec）
        /// 
        /// </summary>
        [Column("INTERVAL")]
        [CustomAttribute.DefaultValue(0)]
        public int Interval { get; set; }

        /// <summary>
        /// パラメータ
        /// 
        /// </summary>
        [Column("PARAM")]
        [MaxLength(1000)]
        public string? Param { get; set; } = string.Empty;

        /// <summary>
        /// ユーザー名
        /// 
        /// </summary>
        [Column("USER")]
        [MaxLength(100)]
        public string? User { get; set; } = string.Empty;

        /// <summary>
        /// パスワード
        /// 
        /// </summary>
        [Column("PASSWORD")]
        [MaxLength(100)]
        public string? PassWord { get; set; } = string.Empty;

        /// <summary>
        /// 無効区分
        /// 0:有効 1:無効
        /// </summary>
        [Column("IS_INVALID")]
        [CustomAttribute.DefaultValue(0)]
        public int IsInvalid { get; set; }

        /// <summary>
        /// 備考
        /// 
        /// </summary>
        [Column("BIKO")]
        [MaxLength(300)]
        public string? Biko { get; set; } = string.Empty;

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
