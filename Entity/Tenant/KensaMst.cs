using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "KENSA_MST")]
    [Serializable]
    public class KensaMst : EmrCloneable<KensaMst>
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
        /// 検査項目コード
        /// 
        /// </summary>
        [Key]
        [Column("KENSA_ITEM_CD", Order = 2)]
        [MaxLength(10)]
        public string KensaItemCd { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        [Key]
        [Column("KENSA_ITEM_SEQ_NO", Order = 3)]
        [CustomAttribute.DefaultValue(0)]
        public int KensaItemSeqNo { get; set; }

        /// <summary>
        /// センターコード
        /// 
        /// </summary>
        [Column("CENTER_CD")]
        [MaxLength(10)]
        public string CenterCd { get; set; }

        /// <summary>
        /// 漢字名称
        /// 
        /// </summary>
        [Column("KENSA_NAME")]
        [MaxLength(120)]
        public string KensaName { get; set; }

        /// <summary>
        /// カナ名称
        /// 
        /// </summary>
        [Column("KENSA_KANA")]
        [MaxLength(20)]
        public string KensaKana { get; set; }

        /// <summary>
        /// 単位
        /// 
        /// </summary>
        [Column("UNIT")]
        [MaxLength(20)]
        public string Unit { get; set; }

        /// <summary>
        /// 材料コード
        /// 
        /// </summary>
        [Column("MATERIAL_CD")]
        public int MaterialCd { get; set; }

        /// <summary>
        /// 容器コード
        /// 
        /// </summary>
        [Column("CONTAINER_CD")]
        public int ContainerCd { get; set; }

        /// <summary>
        /// 男性基準値
        /// 
        /// </summary>
        [Column("MALE_STD")]
        [MaxLength(60)]
        public string MaleStd { get; set; }

        /// <summary>
        /// 男性基準値下限
        /// 
        /// </summary>
        [Column("MALE_STD_LOW")]
        [MaxLength(60)]
        public string MaleStdLow { get; set; }

        /// <summary>
        /// 男性基準値上限
        /// 
        /// </summary>
        [Column("MALE_STD_HIGH")]
        [MaxLength(60)]
        public string MaleStdHigh { get; set; }

        /// <summary>
        /// 女性基準値
        /// 
        /// </summary>
        [Column("FEMALE_STD")]
        [MaxLength(60)]
        public string FemaleStd { get; set; }

        /// <summary>
        /// 女性基準値下限
        /// 
        /// </summary>
        [Column("FEMALE_STD_LOW")]
        [MaxLength(60)]
        public string FemaleStdLow { get; set; }

        /// <summary>
        /// 女性基準値上限
        /// 
        /// </summary>
        [Column("FEMALE_STD_HIGH")]
        [MaxLength(60)]
        public string FemaleStdHigh { get; set; }

        /// <summary>
        /// 式
        /// 
        /// </summary>
        [Column("FORMULA")]
        [MaxLength(100)]
        public string Formula { get; set; }

        /// <summary>
        /// 小数桁
        /// 
        /// </summary>
        [Column("DIGIT")]
        [CustomAttribute.DefaultValue(0)]
        public int Digit { get; set; }

        /// <summary>
        /// 親検査項目コード
        /// 
        /// </summary>
        [Column("OYA_ITEM_CD")]
        [MaxLength(10)]
        public string OyaItemCd { get; set; }

        /// <summary>
        /// 親検査項目連番
        /// 
        /// </summary>
        [Column("OYA_ITEM_SEQ_NO")]
        [CustomAttribute.DefaultValue(0)]
        public int OyaItemSeqNo { get; set; }

        /// <summary>
        /// 並び順
        /// 
        /// </summary>
        [Column("SORT_NO")]
        [CustomAttribute.DefaultValue(0)]
        public long SortNo { get; set; }

        /// <summary>
        /// 外注コード１
        /// 
        /// </summary>
        [Column("CENTER_ITEM_CD1")]
        [MaxLength(10)]
        public string CenterItemCd1 { get; set; }

        /// <summary>
        /// 外注コード２
        /// 
        /// </summary>
        [Column("CENTER_ITEM_CD2")]
        [MaxLength(10)]
        public string CenterItemCd2 { get; set; }

        /// <summary>
        /// 削除区分
        /// 
        /// </summary>
        [Column("IS_DELETE")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDelete { get; set; }

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
