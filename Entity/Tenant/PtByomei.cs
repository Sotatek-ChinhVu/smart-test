using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Tenant
{
    [Serializable()]
    /// <summary>
    /// 患者病名
    /// </summary>
    [Table("PT_BYOMEI")]
    public class PtByomei : EmrCloneable<PtByomei>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Key]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        ///     患者を識別するためのシステム固有の番号
        /// </summary>
        //[Key]
        [Column("PT_ID", Order = 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 基本病名コード
        ///     コードを使用しない場合、「0000999」をセット
        /// </summary>
        [Column("BYOMEI_CD")]
        [MaxLength(7)]
        public string ByomeiCd { get; set; }

        /// <summary>
        /// 連番
        ///     患者の病名を識別するためのシステム固有の番号
        /// </summary>
        //[Key]
        [Column("ID", Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        [Column("SORT_NO")]
        [CustomAttribute.DefaultValue(1)]
        public int SortNo { get; set; }

        /// <summary>
        /// 修飾語コード１
        /// </summary>
        [Column("SYUSYOKU_CD1")]
        [MaxLength(7)]
        public string SyusyokuCd1 { get; set; }

        /// <summary>
        /// 修飾語コード２
        /// </summary>
        [Column("SYUSYOKU_CD2")]
        [MaxLength(7)]
        public string SyusyokuCd2 { get; set; }

        /// <summary>
        /// 修飾語コード３
        /// </summary>
        [Column("SYUSYOKU_CD3")]
        [MaxLength(7)]
        public string SyusyokuCd3 { get; set; }

        /// <summary>
        /// 修飾語コード４
        /// </summary>
        [Column("SYUSYOKU_CD4")]
        [MaxLength(7)]
        public string SyusyokuCd4 { get; set; }

        /// <summary>
        /// 修飾語コード５
        /// </summary>
        [Column("SYUSYOKU_CD5")]
        [MaxLength(7)]
        public string SyusyokuCd5 { get; set; }

        /// <summary>
        /// 修飾語コード６
        /// </summary>
        [Column("SYUSYOKU_CD6")]
        [MaxLength(7)]
        public string SyusyokuCd6 { get; set; }

        /// <summary>
        /// 修飾語コード７
        /// </summary>
        [Column("SYUSYOKU_CD7")]
        [MaxLength(7)]
        public string SyusyokuCd7 { get; set; }

        /// <summary>
        /// 修飾語コード８
        /// </summary>
        [Column("SYUSYOKU_CD8")]
        [MaxLength(7)]
        public string SyusyokuCd8 { get; set; }

        /// <summary>
        /// 修飾語コード９
        /// </summary>
        [Column("SYUSYOKU_CD9")]
        [MaxLength(7)]
        public string SyusyokuCd9 { get; set; }

        /// <summary>
        /// 修飾語コード１０
        /// </summary>
        [Column("SYUSYOKU_CD10")]
        [MaxLength(7)]
        public string SyusyokuCd10 { get; set; }

        /// <summary>
        /// 修飾語コード１１
        /// </summary>
        [Column("SYUSYOKU_CD11")]
        [MaxLength(7)]
        public string SyusyokuCd11 { get; set; }

        /// <summary>
        /// 修飾語コード１２
        /// </summary>
        [Column("SYUSYOKU_CD12")]
        [MaxLength(7)]
        public string SyusyokuCd12 { get; set; }

        /// <summary>
        /// 修飾語コード１３
        /// </summary>
        [Column("SYUSYOKU_CD13")]
        [MaxLength(7)]
        public string SyusyokuCd13 { get; set; }

        /// <summary>
        /// 修飾語コード１４
        /// </summary>
        [Column("SYUSYOKU_CD14")]
        [MaxLength(7)]
        public string SyusyokuCd14 { get; set; }

        /// <summary>
        /// 修飾語コード１５
        /// </summary>
        [Column("SYUSYOKU_CD15")]
        [MaxLength(7)]
        public string SyusyokuCd15 { get; set; }

        /// <summary>
        /// 修飾語コード１６
        /// </summary>
        [Column("SYUSYOKU_CD16")]
        [MaxLength(7)]
        public string SyusyokuCd16 { get; set; }

        /// <summary>
        /// 修飾語コード１７
        /// </summary>
        [Column("SYUSYOKU_CD17")]
        [MaxLength(7)]
        public string SyusyokuCd17 { get; set; }

        /// <summary>
        /// 修飾語コード１８
        /// </summary>
        [Column("SYUSYOKU_CD18")]
        [MaxLength(7)]
        public string SyusyokuCd18 { get; set; }

        /// <summary>
        /// 修飾語コード１９
        /// </summary>
        [Column("SYUSYOKU_CD19")]
        [MaxLength(7)]
        public string SyusyokuCd19 { get; set; }

        /// <summary>
        /// 修飾語コード２０
        /// </summary>
        [Column("SYUSYOKU_CD20")]
        [MaxLength(7)]
        public string SyusyokuCd20 { get; set; }

        /// <summary>
        /// 修飾語コード２１
        /// </summary>
        [Column("SYUSYOKU_CD21")]
        [MaxLength(7)]
        public string SyusyokuCd21 { get; set; }

        /// <summary>
        /// 病名
        /// </summary>
        [Column("BYOMEI")]
        [MaxLength(160)]
        public string Byomei { get; set; }

        /// <summary>
        /// 開始日
        /// </summary>
        [Column("START_DATE")]
        [CustomAttribute.DefaultValue(0)]
        public int StartDate { get; set; }

        /// <summary>
        /// 転帰区分
        /// 転帰区分を表す。
        ///      0: 下記以外
        ///      1: 治ゆ
        ///      2: 中止
        ///      3: 死亡
        ///      9: その他
        /// </summary>
        [Column("TENKI_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int TenkiKbn { get; set; }

        /// <summary>
        /// 転帰日
        /// </summary>
        [Column("TENKI_DATE")]
        [CustomAttribute.DefaultValue(0)]
        public int TenkiDate { get; set; }

        /// <summary>
        /// 主病名区分
        ///     0: 主病名以外
        ///     1: 主病名
        /// </summary>
        [Column("SYUBYO_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int SyobyoKbn { get; set; }

        /// <summary>
        /// 慢性疾患区分
        ///     特定疾患療養指導料等の算定対象であるか否かを表す
        ///     00: 対象外
        ///     03: 皮膚科特定疾患指導管理料（１）算定対象
        ///     04: 皮膚科特定疾患指導管理料（２）算定対象
        ///     05: 特定疾患療養指導料／老人慢性疾患生活指導料算定対象
        ///     07: てんかん指導料算定対象 
        ///     08: 特定疾患療養管理料又はてんかん指導料算
        ///     定対象 
        /// </summary>
        [Column("SIKKAN_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int SikkanKbn { get; set; }

        /// <summary>
        /// 難病外来コード
        ///     当該傷病名が難病外来指導管理料の算定対象であるか否かを表す。
        ///     00: 算定対象外
        ///     09: 難病外来指導管理料算定対象
        /// </summary>
        [Column("NANBYO_CD")]
        [CustomAttribute.DefaultValue(0)]
        public int NanByoCd { get; set; }

        /// <summary>
        /// 補足コメント
        /// </summary>
        [Column("HOSOKU_CMT")]
        [MaxLength(80)]
        public string HosokuCmt { get; set; }

        /// <summary>
        /// 保険組み合わせ番号
        ///     0: 共通病名
        /// </summary>
        [Column("HOKEN_PID")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenPid { get; set; }

        /// <summary>
        /// 当月病名区分
        ///     1: 当月病名
        /// </summary>
        [Column("TOGETU_BYOMEI")]
        [CustomAttribute.DefaultValue(0)]
        public int TogetuByomei { get; set; }

        /// <summary>
        /// レセプト非表示区分
        ///     1: 非表示
        /// </summary>
        [Column("IS_NODSP_RECE")]
        [CustomAttribute.DefaultValue(0)]
        public int IsNodspRece { get; set; }

        /// <summary>
        /// カルテ非表示区分
        ///     1: 非表示
        /// </summary>
        [Column("IS_NODSP_KARTE")]
        [CustomAttribute.DefaultValue(0)]
        public int IsNodspKarte { get; set; }

        /// <summary>
        /// 削除区分
        ///     1:削除
        /// </summary>
        [Column("IS_DELETED")]
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
        public string CreateMachine { get; set; }

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
        public string UpdateMachine { get; set; }

        [Column("SEQ_NO")]
        [CustomAttribute.DefaultValue(0)]
        public long SeqNo { get; set; }

        /// <summary>
        /// 重要
        ///     1:重要
        /// </summary>
        [Column("IS_IMPORTANT")]
        [CustomAttribute.DefaultValue(0)]
        public int IsImportant { get; set; }
    }
}