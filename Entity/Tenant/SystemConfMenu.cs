using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "system_conf_menu")]
    public class SystemConfMenu : EmrCloneable<SystemConfMenu>
    {
        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("hp_id", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// メニューID
        /// 
        /// </summary>
        
        [Column("menu_id", Order = 2)]
        public int MenuId { get; set; }

        /// <summary>
        /// メニューグループ
        /// 1000..1999: SYSTEM_CONF
        ///                     2000..2999: SYSTEM_GENERATION_CONF          
        ///                     3000..3999: AUTO_SANTEI_MST          
        /// </summary>
        [Column("menu_grp")]
        [CustomAttribute.DefaultValue(0)]
        public int MenuGrp { get; set; }

        /// <summary>
        /// 並び順
        /// 
        /// </summary>
        [Column("sort_no")]
        [CustomAttribute.DefaultValue(0)]
        public int SortNo { get; set; }

        /// <summary>
        /// メニュー
        /// 
        /// </summary>
        [Column("menu_name")]
        [MaxLength(100)]
        public string? MenuName { get; set; } = string.Empty;

        /// <summary>
        /// 分類コード
        /// 設定テーブルのキー情報
        /// </summary>
        [Column("grp_cd")]
        public int GrpCd { get; set; }

        /// <summary>
        /// 分類枝番
        /// 設定テーブルのキー情報
        /// </summary>
        [Column("grp_eda_no")]
        public int GrpEdaNo { get; set; }

        /// <summary>
        /// PATH_INF.分類コード
        /// 0:未使用 >1:PATH_INF.分類コード
        /// </summary>
        [Column("path_grp_cd")]
        [CustomAttribute.DefaultValue(0)]
        public int PathGrpCd { get; set; }

        /// <summary>
        /// パラメーター
        /// 0:なし 1:あり
        /// </summary>
        [Column("is_param")]
        [CustomAttribute.DefaultValue(0)]
        public int IsParam { get; set; }

        /// <summary>
        /// パラメータマスク
        /// 1:パラメータ欄をマスク処理する
        /// </summary>
        [Column("param_mask")]
        [CustomAttribute.DefaultValue(0)]
        public int ParamMask { get; set; }

        /// <summary>
        /// パラメータ入力タイプ
        /// 0:制限なし  1:数字のみ  2:数字、カンマのみ"										
        /// </summary>
        [Column("param_type")]
        [CustomAttribute.DefaultValue(0)]
        public int ParamType { get; set; }

        /// <summary>
        /// パラメーターヒント
        /// パラメーターの入力ヒント
        /// </summary>
        [Column("param_hint")]
        [MaxLength(100)]
        public string? ParamHint { get; set; } = string.Empty;

        /// <summary>
        /// 設定値－最小値
        /// VAL_MIN=0 and VAL_MAX=0の場合はチェックしない
        /// </summary>
        [Column("val_min")]
        [CustomAttribute.DefaultValue(0)]
        public double ValMin { get; set; }

        /// <summary>
        /// 設定値－最大値
        /// VAL_MIN=0 and VAL_MAX=0の場合はチェックしない
        /// </summary>
        [Column("val_max")]
        [CustomAttribute.DefaultValue(0)]
        public double ValMax { get; set; }

        /// <summary>
        /// パラメータ－最小値
        /// PARAM_MIN=0 and PARAM_MAX=0の場合はチェックしない
        /// </summary>
        [Column("param_min")]
        [CustomAttribute.DefaultValue(0)]
        public double ParamMin { get; set; }

        /// <summary>
        /// パラメータ－最大値
        /// PARAM_MIN=0 and PARAM_MAX=0の場合はチェックしない
        /// </summary>
        [Column("param_max")]
        [CustomAttribute.DefaultValue(0)]
        public double ParamMax { get; set; }

        /// <summary>
        /// 診療行為コード
        /// 
        /// </summary>
        [Column("item_cd")]
        [MaxLength(10)]
        public string? ItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 都道府県番号
        /// 指定がある場合は当該都道府県の医療機関のみ表示
        /// </summary>
        [Column("pref_no")]
        [CustomAttribute.DefaultValue(0)]
        public int PrefNo { get; set; }

        /// <summary>
        /// 表示フラグ
        /// 0:非表示 1:表示
        ///                     （ライセンスがない場合は非表示に設定）          
        /// </summary>
        [Column("is_visible")]
        [CustomAttribute.DefaultValue(0)]
        public int IsVisible { get; set; }

        /// <summary>
        /// 管理者区分
        /// "0:一般 7:管理者 9:システム管理者
        /// （USER_MST.MANAGER_KBN）"
        /// </summary>
        [Column("manager_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int ManagerKbn { get; set; }


        [Column("is_value")]
        [CustomAttribute.DefaultValue(1)]
        public int IsValue { get; set; }

        [Column("param_max_length")]
        [CustomAttribute.DefaultValue(0)]
        public int ParamMaxLength { get; set; }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        [Column("create_date")]
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
