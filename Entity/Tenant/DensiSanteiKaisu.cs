using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "DENSI_SANTEI_KAISU")]
    public class DensiSanteiKaisu : EmrCloneable<DensiSanteiKaisu>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", Order = 1)]
        public int Id { get; set; }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        //[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("HP_ID", Order = 2)]
        public int HpId { get; set; }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        //[Key]
        [Column("ITEM_CD", Order = 3)]
        [MaxLength(10)]
        public string ItemCd { get; set; }

        /// <summary>
        /// 算定単位コード
        /// "チェック対象のコードは、
        /// 53:患者あたり, 121:日, 131:月,138:週, 
        /// 141:一連, 142:2週, 
        /// 143:2月, 144:3月, 145:4月, 146:6月, 147:12月, 
        /// 148:5年
        /// 997:初診から1ヶ月算定不可（休日除く）
        /// 998:初診から1ヶ月算定不可
        /// 999:カスタム(TERM_COUNT, TERM_SBTを使用)"
        /// </summary>
        [Column("UNIT_CD")]
        public int UnitCd { get; set; }

        /// <summary>
        /// 算定回数
        /// 算定単位ごとの上限回数を表す。
        /// </summary>
        [Column("MAX_COUNT")]
        [CustomAttribute.DefaultValue(0)]
        public int MaxCount { get; set; }

        /// <summary>
        /// 特例条件
        /// "算定条件に特別な条件がある場合に設定する。 
        /// 0: 条件なし 
        /// 1: 条件あり "
        /// </summary>
        [Column("SP_JYOKEN")]
        [CustomAttribute.DefaultValue(0)]
        public int SpJyoken { get; set; }

        /// <summary>
        /// 新設年月日
        /// レコード情報を新設した日付を西暦年4桁、月2桁及び日2桁の8桁で表す。
        /// </summary>
        [Column("START_DATE")]
        [CustomAttribute.DefaultValue(0)]
        public int StartDate { get; set; }

        /// <summary>
        /// 廃止年月日
        /// "当該診療行為の使用が可能な最終日付を西暦年4桁、月2桁及び日2桁の8桁で表す。 
        /// なお、廃止診療行為でない場合は「99999999」とする。"
        /// </summary>
        [Column("END_DATE")]
        [CustomAttribute.DefaultValue(99999999)]
        public int EndDate { get; set; }

        /// <summary>
        /// 連番
        /// 
        /// </summary>
        //[Key]
        [Column("SEQ_NO", Order = 4)]
        [CustomAttribute.DefaultValue(1)]
        public long SeqNo { get; set; }

        /// <summary>
        /// ユーザー設定
        /// "0: システム設定分
        /// 1: ユーザー設定分"
        /// </summary>
        //[Key]
        [Column("USER_SETTING", Order = 5)]
        [CustomAttribute.DefaultValue(0)]
        public int UserSetting { get; set; }

        /// <summary>
        /// 対象保険種
        /// "0:健保・労災とも対象
        /// 1:健保のみ対象
        /// 2:労災のみ対象"
        /// </summary>
        [Column("TARGET_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int TargetKbn { get; set; }

        /// <summary>
        /// チェック期間数
        /// "TERM_SBTと組み合わせて使用
        /// ※TERM_SBT in (1,4)のときのみ有効
        /// 例）2日の場合、TERM_COUNT=2, TERM_SBT=1と登録"
        /// </summary>
        [Column("TERM_COUNT")]
        [CustomAttribute.DefaultValue(0)]
        public int TermCount { get; set; }

        /// <summary>
        /// チェック期間種別
        /// 0:未指定 2:日 3:暦週 4:暦月
        /// </summary>
        [Column("TERM_SBT")]
        [CustomAttribute.DefaultValue(0)]
        public int TermSbt { get; set; }

        /// <summary>
        /// 無効区分
        /// "0: 有効
        /// 1: 無効"
        /// </summary>
        [Column("IS_INVALID")]
        [CustomAttribute.DefaultValue(0)]
        public int IsInvalid { get; set; }

        /// <summary>
        /// 項目グループコード
        /// ITEM_GRP_MST.ITEM_GRP_CD
        /// </summary>
        [Column("ITEM_GRP_CD")]
        [CustomAttribute.DefaultValue(0)]
        public int ItemGrpCd { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("CREATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者ID
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
        public string CreateMachine { get; set; }

        /// <summary>
        /// 更新日時
        /// 
        /// </summary>
        [Column("UPDATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者ID
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
        public string UpdateMachine { get; set; }

    }
}
