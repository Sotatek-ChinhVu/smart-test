using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "calc_status")]
    [Index(nameof(HpId), nameof(PtId), nameof(SinDate), nameof(Status), nameof(CreateMachine), Name = "calc_status_idx01")]
    [Index(nameof(HpId), nameof(Status), nameof(CreateMachine), Name = "calc_status_idx02")]
    public class CalcStatus : EmrCloneable<CalcStatus>
    {
        /// <summary>
        /// 計算ID
        /// SEQUENCE
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("calc_id", Order = 1)]
        public long CalcId { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        [Column("hp_id")]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        [Column("pt_id")]
        public long PtId { get; set; }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        [Column("sin_date")]
        public int SinDate { get; set; }

        /// <summary>
        /// 請求情報更新
        /// 0:反映しない　1:反映する
        /// </summary>
        [Column("seikyu_up")]
        [CustomAttribute.DefaultValue(0)]
        public int SeikyuUp { get; set; }

        /// <summary>
        /// 計算モード
        /// 0:通常計算　1:連続計算　2:試算
        /// </summary>
        [Column("calc_mode")]
        [CustomAttribute.DefaultValue(0)]
        public int CalcMode { get; set; }

        /// <summary>
        /// RECE_STATUS.STATUS_KBN=8のとき、0に戻すかどうか
        /// 0:クリアしない　1:クリアする
        /// </summary>
        [Column("clear_rece_chk")]
        [CustomAttribute.DefaultValue(0)]
        public int ClearReceChk { get; set; }

        /// <summary>
        /// 状態
        /// 0:未済 1:計算中 8:異常終了 9:終了
        /// </summary>
        [Column("status")]
        [CustomAttribute.DefaultValue(0)]
        public int Status { get; set; }

        /// <summary>
        /// 備考
        /// 
        /// </summary>
        [Column("biko")]
        [MaxLength(200)]
        public string? Biko { get; set; } = string.Empty;

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
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
        [CustomAttribute.DefaultValueSql("current_timestamp")]
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
