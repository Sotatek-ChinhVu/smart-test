using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Tenant
{
    [Table(name: "ACCOUNTING_FORM_MST")]
    public class AccountingFormMst : EmrCloneable<AccountingFormMst>
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
        /// 帳票番号
        /// 連番[0..zzzz]
        /// </summary>
        //[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("FORM_NO", Order = 2)]
        public int FormNo { get; set; }

        /// <summary>
        /// 帳票名
        /// 
        /// </summary>
        [Column("FORM_NAME")]
        [MaxLength(100)]
        public string FormName { get; set; } = string.Empty;

        /// <summary>
        /// 帳票タイプ
        /// 0: 1患者1帳票
        /// 1: 1患者1帳票（保険請求ありのみ）
        /// 2: 1患者1帳票（保険外請求ありのみ）
        /// 3: 連記          
        /// </summary>
        [Column("FORM_TYPE")]
        [CustomAttribute.DefaultValue(0)]
        public int FormType { get; set; }

        /// <summary>
        /// 出力順
        /// 0: 患者番号・患者カナ氏名順
        /// 1: 患者カナ氏名・患者番号順          
        /// </summary>
        [Column("PRINT_SORT")]
        [CustomAttribute.DefaultValue(0)]
        public int PrintSort { get; set; }

        /// <summary>
        /// 未精算印刷区分
        /// 0: 未精算分(RAIIN_INF.STATUS<9)は印刷しない
        /// 1: 未精算分(RAIIN_INF.STATUS<9)も印刷する          
        /// </summary>
        [Column("MISEISAN_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int MiseisanKbn { get; set; }

        /// <summary>
        /// 請求差異印刷区分
        /// 0: SYUNO_SEIKYU.SEIKYU_GAKU<>
        ///    SYUNO_SEIKYU.NEW_SEIKYU_GAKU
        ///    の分は印刷しない
        /// 1: SYUNO_SEIKYU.SEIKYU_GAKU<>
        ///    SYUNO_SEIKYU.NEW_SEIKYU_GAKU
        ///    に差がある分も印刷する          
        /// </summary>
        [Column("SAI_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int SaiKbn { get; set; }

        /// <summary>
        /// 未収印刷区分
        /// 0: 未収関係なく印刷
        /// 1: 未収分(SYUNO_SEIKYU.NYUKIN_KBN=1)のみ印刷          
        /// </summary>
        [Column("MISYU_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int MisyuKbn { get; set; }

        /// <summary>
        /// 請求有無印刷区分
        /// 0: SYUNO_SEIKYU.SEIKYU_GAKU=0は印刷しない
        /// 1: SYUNO_SEIKYU.SEIKYU_GAKUに関係なく印刷          
        /// </summary>
        [Column("SEIKYU_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int SeikyuKbn { get; set; }

        /// <summary>
        /// 保険指定
        /// 0: 保険の指定があればその保険のみ印刷、
        ///    指定がなければどんな保険でも対象
        /// 1: PT_HOKEN_INF.HOKEN_KBN = 1,2（社保国保）
        ///    を対象とする          
        /// </summary>
        [Column("HOKEN_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenKbn { get; set; }

        /// <summary>
        /// フォームファイル名
        /// 
        /// </summary>
        [Column("FORM")]
        [MaxLength(100)]
        public string Form { get; set; } = string.Empty;

        /// <summary>
        /// 初期設定ベース
        /// 0: 請求日ベース、1: 入金日ベース
        /// </summary>
        [Column("BASE")]
        [CustomAttribute.DefaultValue(0)]
        public int Base { get; set; }

        /// <summary>
        /// 順番
        /// 
        /// </summary>
        [Column("SORT_NO")]
        [CustomAttribute.DefaultValue(0)]
        public int SortNo { get; set; }

        /// <summary>
        /// 削除フラグ
        /// 1:削除
        /// </summary>
        [Column("IS_DELETED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

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
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        [Column("CREATE_MACHINE")]
        [MaxLength(60)]
        public string CreateMachine { get; set; } = string.Empty;

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
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末
        /// 
        /// </summary>
        [Column("UPDATE_MACHINE")]
        [MaxLength(60)]
        public string UpdateMachine { get; set; }  = string.Empty;

    }
}
