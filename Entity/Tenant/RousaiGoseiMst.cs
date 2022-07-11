using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "ROUSAI_GOSEI_MST")]
    public class RousaiGoseiMst : EmrCloneable<RousaiGoseiMst>
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
        /// 合成グループ
        /// 
        /// </summary>
        //[Key]
        [Column("GOSEI_GRP", Order = 2)]
        public int GoseiGrp { get; set; }

        /// <summary>
        /// 合成項目コード
        /// 
        /// </summary>
        //[Key]
        [Column("GOSEI_ITEM_CD", Order = 3)]
        [MaxLength(10)]
        public string GoseiItemCd { get; set; }

        /// <summary>
        /// 診療行為コード
        /// 
        /// </summary>
        //[Key]
        [Column("ITEM_CD", Order = 4)]
        [MaxLength(10)]
        public string ItemCd { get; set; }

        /// <summary>
        /// 四肢加算区分
        /// 
        /// </summary>
        //[Key]
        [Column("SISI_KBN", Order = 5)]
        public int SisiKbn { get; set; }

        /// <summary>
        /// 使用開始日
        /// 
        /// </summary>
        //[Key]
        [Column("START_DATE", Order = 6)]
        public int StartDate { get; set; }

        /// <summary>
        /// 使用終了日
        /// 
        /// </summary>
        [Column("END_DATE")]
        public int EndDate { get; set; }

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
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("CREATE_MACHINE")]
        [MaxLength(60)]
        public string CreateMachine { get; set; }

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
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("UPDATE_MACHINE")]
        [MaxLength(60)]
        public string UpdateMachine { get; set; }

    }
}
