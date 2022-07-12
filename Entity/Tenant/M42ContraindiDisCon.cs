using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "M42_CONTRAINDI_DIS_CON")]
    public class M42ContraindiDisCon : EmrCloneable<M42ContraindiDisCon>
    {
        /// <summary>
        /// 病態コード
        /// BY で始まり5桁の数字が続く
        /// </summary>
        [Key]
        [Column("BYOTAI_CD", Order = 1)]
        [MaxLength(7)]
        public string ByotaiCd { get; set; } = string.Empty;

        /// <summary>
        /// 標準化病態
        /// あれば原則、標準病名を設定。よく使われる同義語を（）内に記載
        /// </summary>
        [Column("STANDARD_BYOTAI")]
        [MaxLength(400)]
        public string StandardByotai { get; set; } = string.Empty;

        /// <summary>
        /// 病態抽出区分
        /// 1:代表的病態　3:準代表的病態　7:詳細病態
        /// </summary>
        [Column("BYOTAI_KBN")]
        public int ByotaiKbn { get; set; }

        /// <summary>
        /// 基本病名
        /// 標準化病態を、標準病名マスターにおける病名表記と照合し、対応する病名がある場合、これを設定する
        /// </summary>
        [Column("BYOMEI")]
        [MaxLength(400)]
        public string Byomei { get; set; } = string.Empty;

        /// <summary>
        /// ＩＣＤ１０コード
        /// 基本病名に対応するICD10コード
        /// </summary>
        [Column("ICD10")]
        [MaxLength(5)]
        public string Icd10 { get; set; } = string.Empty;

        /// <summary>
        /// レセ電算コード
        /// 基本病名に対応する傷病名のコード
        /// </summary>
        [Column("RECE_CD")]
        [MaxLength(33)]
        public string ReceCd { get; set; } = string.Empty;

    }
}
