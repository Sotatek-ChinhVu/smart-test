using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "m42_contraindi_drug_main_ex")]
    public class M42ContraindiDrugMainEx : EmrCloneable<M42ContraindiDrugMainEx>
    {
        [Column(name: "hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 医薬品コード
        /// YJコード
        /// </summary>

        [Column("yj_cd", Order = 1)]
        [MaxLength(12)]
        public string YjCd { get; set; } = string.Empty;

        /// <summary>
        /// 添付文書レベル
        /// 1:禁忌　2:原則禁忌　3:慎重投与
        /// </summary>
        
        [Column("tenpu_level", Order = 2)]
        public int TenpuLevel { get; set; }

        /// <summary>
        /// 病態コード
        /// BYで始まり5桁の数字が続く
        /// </summary>
        
        [Column("byotai_cd", Order = 3)]
        [MaxLength(7)]
        public string ByotaiCd { get; set; } = string.Empty;

        /// <summary>
        /// 重篤度グレード
        /// 1:形容詞なし　2:中等度以上　3:重度以上
        /// </summary>
        [Column("stage")]
        public int Stage { get; set; }

        /// <summary>
        /// 既往歴コード
        /// 1:既往歴を含む場合　2:既往歴のみが対象の場合
        /// </summary>
        [Column("kio_cd")]
        [MaxLength(1)]
        public string? KioCd { get; set; } = string.Empty;

        /// <summary>
        /// 家族歴コード
        /// 1:家族歴を含む場合　2:家族歴のみが対象の場合
        /// </summary>
        [Column("family_cd")]
        [MaxLength(1)]
        public string? FamilyCd { get; set; } = string.Empty;

        /// <summary>
        /// 注意コメントコード
        /// CMで始まり5桁の数字が続く
        /// </summary>
        
        [Column("cmt_cd", Order = 4)]
        [MaxLength(7)]
        public string? CmtCd { get; set; } = string.Empty;

        /// <summary>
        /// 機序コード
        /// KJで始まり5桁の数字が続く
        /// </summary>
        [Column("kijyo_cd")]
        [MaxLength(7)]
        public string? KijyoCd { get; set; } = string.Empty;
    }
}
