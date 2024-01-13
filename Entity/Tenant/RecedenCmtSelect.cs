using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "receden_cmt_select")]
    [Index(nameof(HpId), nameof(ItemCd), nameof(StartDate), nameof(CommentCd), nameof(IsInvalid), Name = "receden_cmt_select_idx01")]
    public class RecedenCmtSelect : EmrCloneable<RecedenCmtSelect>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 診療行為コード
        /// 
        /// </summary>
        
        [Column("item_cd", Order = 4)]
        [MaxLength(10)]
        public string ItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 適用開始日
        /// 
        /// </summary>
        
        [Column("start_date", Order = 5)]
        public int StartDate { get; set; }

        /// <summary>
        /// 適用終了日
        /// 
        /// </summary>
        [Column("end_date")]
        public int EndDate { get; set; }

        /// <summary>
        /// 順番
        /// 
        /// </summary>
        [Column("sort_no")]
        public int SortNo { get; set; }

        /// <summary>
        /// コメントコード
        /// コメントマスターの請求コード
        /// </summary>
        
        [Column("comment_cd", Order = 6)]
        [MaxLength(10)]
        public string? CommentCd { get; set; } = string.Empty;

        /// <summary>
        /// 有効区分
        /// 0:有効、1:無効
        /// </summary>
        [Column("is_invalid")]
        [CustomAttribute.DefaultValue(0)]
        public int IsInvalid { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者ID
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
        /// 更新者ID
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

        /// <summary>
        /// 項番
        /// 記載要領別表Ⅰの「項番」列の値
        /// </summary>
        
        [Column("item_no", Order = 2)]
        [CustomAttribute.DefaultValue(0)]
        public int ItemNo { get; set; }

        /// <summary>
        /// 区分
        /// 記載要領別表Ⅰの「区分」列の値
        /// </summary>
        [Column("kbn_no")]
        [MaxLength(64)]
        public string? KbnNo { get; set; } = string.Empty;

        /// <summary>
        /// 枝番
        /// 項番内に複数の条件がある場合、条件ごとに連番
        /// </summary>
        
        [Column("eda_no", Order = 3)]
        [CustomAttribute.DefaultValue(0)]
        public int EdaNo { get; set; }

        /// <summary>
        /// 患者の状態
        /// 記載要領別表Ⅰに収載されている患者の状態コード
        /// </summary>
        [Column("pt_status")]
        [CustomAttribute.DefaultValue(0)]
        public int PtStatus { get; set; }

        /// <summary>
        /// 条件区分
        /// 00:「01,02,03」以外
        /// 01:対象の診療行為の算定が条件であって、
        /// 　それ以外の条件がない場合
        /// 02:対象の診療行為の算定が条件であって、
        /// 　入院又は入院外のいずれかで算定した場合
        /// 03:対象の診療行為の算定が条件であって、
        /// 　複数回算定した場合
        /// </summary>
        [Column("cond_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int CondKbn { get; set; }

        /// <summary>
        /// 非算定理由コメント
        /// 0:「1」以外のコメント
        /// 1:対象の診療行為を算定しなかった場合であって、
        /// 条件に合致する場合に記録するコメント
        /// </summary>
        [Column("not_santei_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int NotSanteiKbn { get; set; }

        /// <summary>
        /// 入外区分
        /// 条件区分が「02」の場合、いずれの条件か表す
        /// 1:入院
        /// 2:入院外
        /// </summary>
        [Column("nyugai_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int NyugaiKbn { get; set; }

        /// <summary>
        /// 算定回数
        /// 条件区分が「03」の場合、コメントコードの記録が必要となる対象の診療行為の算定回数を表す
        /// </summary>
        [Column("santei_cnt")]
        [CustomAttribute.DefaultValue(0)]
        public int SanteiCnt { get; set; }
    }
}
