using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "RENKEI_MST")]
    public class RenkeiMst : EmrCloneable<RenkeiMst>
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
        //[Key]
        [Column("RENKEI_ID", Order = 2)]
        public int RenkeiId { get; set; }

        /// <summary>
        /// 連携名称
        /// 
        /// </summary>
        [Column("RENKEI_NAME")]
        [MaxLength(255)]
        public string? RenkeiName { get; set; } = string.Empty;

        /// <summary>
        /// 連携種別
        /// 0:タイミング連携 1:常駐連携
        /// </summary>
        [Column("RENKEI_SBT")]
        [CustomAttribute.DefaultValue(0)]
        public int RenkeiSbt { get; set; }

        /// <summary>
        /// 処理タイプ
        /// 0:個別　1:ファイル連携　2:PG起動連携
        /// </summary>
        [Column("FUNCTION_TYPE")]
        [CustomAttribute.DefaultValue(0)]
        public int FunctionType { get; set; }

        /// <summary>
        /// 無効区分
        /// 0:有効 1:無効
        /// </summary>
        [Column("IS_INVALID")]
        [CustomAttribute.DefaultValue(1)]
        public int IsInvalid { get; set; }

        /// <summary>
        /// 処理順
        /// 常駐連携の処理順
        /// </summary>
        [Column("SORT_NO")]
        [CustomAttribute.DefaultValue(0)]
        public int SortNo { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }
    }
}
