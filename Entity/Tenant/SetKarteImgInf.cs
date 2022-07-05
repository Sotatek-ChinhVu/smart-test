using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// セットカルテ画像情報
    /// </summary>
    [Table(name: "SET_KARTE_IMG_INF")]
    public class SetKarteImgInf : EmrCloneable<SetKarteImgInf>
    {
        /// <summary>
        /// ID
        /// 
        /// </summary>
        [Key]
        [Column("ID", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Column("HP_ID")]
        public int HpId { get; set; }

        /// <summary>
        /// セットコード
        /// </summary>
        [Column("SET_CD")]
        public int SetCd { get; set; }

        /// <summary>
        /// カルテ区分
        ///    KARTE_KBN_MST.KARTE_KBN
        /// </summary>
        [Column("KARTE_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int KarteKbn { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        [Column("SEQ_NO")]
        [CustomAttribute.DefaultValue(1)]
        public long SeqNo { get; set; }

        /// <summary>
        /// 表示位置
        /// </summary>
        [Column("POSITION")]
        [CustomAttribute.DefaultValue(0)]
        public long Position { get; set; }

        /// <summary>
        /// ファイル名
        /// </summary>
        [Column("FILE_NAME")]
        [MaxLength(100)]
        public string FileName { get; set; }
    }
}
