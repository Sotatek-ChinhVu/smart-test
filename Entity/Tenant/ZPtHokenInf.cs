using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 患者保険情報
    /// </summary>
    [Table("z_pt_hoken_inf")]
    public class ZPtHokenInf : EmrCloneable<ZPtHokenInf>
    {
        
        [Column("op_id", Order = 1)]
        public long OpId { get; set; }

        [Column("op_type")]
        [MaxLength(10)]
        public string? OpType { get; set; } = string.Empty;

        [Column("op_time")]
        public DateTime OpTime { get; set; }

        [Column("op_addr")]
        [MaxLength(100)]
        public string? OpAddr { get; set; } = string.Empty;

        [Column("op_hostname")]
        [MaxLength(100)]
        public string? OpHostName { get; set; } = string.Empty;

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Column("hp_id")]
        //[Index("pt_hoken_inf_ukey", 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 患者ID
        ///		患者を識別するためのシステム固有の番号
        /// </summary>
        [Column("pt_id")]
        //[Index("pt_hoken_inf_ukey", 2)]
        public long PtId { get; set; }

        /// <summary>
        /// 保険ID
        ///		患者別に保険情報を識別するための固有の番号
        /// </summary>
        [Column("hoken_id")]
        //[Index("pt_hoken_inf_ukey", 3)]
        public int HokenId { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        [Column("seq_no")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SeqNo { get; set; }

        /// <summary>
        /// 保険番号
        ///		保険マスタに登録された保険番号
        /// </summary>
        [Column("hoken_no")]
        public int HokenNo { get; set; }

        /// <summary>
        /// 枝番
        /// </summary>
        [Column("eda_no")]
        [MaxLength(2)]
        public string? EdaNo { get; set; } = string.Empty;

        /// <summary>
        /// 保険番号枝番
        ///		保険マスタに登録された保険番号枝番
        /// </summary>
        [Column("hoken_eda_no")]
        public int HokenEdaNo { get; set; }

        /// <summary>
        /// 保険者番号
        /// </summary>
        [Column("hokensya_no")]
        [MaxLength(8)]
        public string? HokensyaNo { get; set; } = string.Empty;

        /// <summary>
        /// 記号
        /// </summary>
        [Column("kigo")]
        [MaxLength(80)]
        public string? Kigo { get; set; } = string.Empty;

        /// <summary>
        /// 番号
        /// </summary>
        [Column("bango")]
        [MaxLength(80)]
        public string? Bango { get; set; } = string.Empty;

        /// <summary>
        /// 本人家族区分
        ///		1:本人
        ///		2:家族
        /// </summary>
        [Column("honke_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int HonkeKbn { get; set; }

        /// <summary>
        /// 保険区分
        ///		0:自費 
        ///		1:社保 
        ///		2:国保
        ///		
        ///		11:労災(短期給付)
        ///		12:労災(傷病年金)
        ///		13:アフターケア
        ///		14:自賠責
        /// </summary>
        [Column("hoken_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int HokenKbn { get; set; }

        /// <summary>
        /// 保険者番号の桁数
        ///		0桁: [0]自費保険
        /// 	4桁: [01]社保
        /// 	6桁: [100]国保
        /// 	8桁: 保険者番号の前2桁
        /// 		[67]退職国保
        /// 		[39]後期高齢
        /// 		[01]協会健保、[02]船員、[03,04]日雇、
        /// 		[06]組合健保、[07]自衛官、
        /// 		[31..34]共済組合
        /// 			[31]国家公務員共済組合
        /// 			[32]地方公務員等共済組合
        /// 			[33]警察共済組合
        /// 			[34]公立学校共済組合
        ///　				日本私立学校振興・共済事業団
        /// 		[63,72..75]特定共済組合
        /// </summary>
        [Column(name: "houbetu")]
        [MaxLength(3)]
        public string? Houbetu { get; set; } = string.Empty;

        /// <summary>
        /// 被保険者名
        /// </summary>
        [Column("hokensya_name")]
        [MaxLength(100)]
        public string? HokensyaName { get; set; } = string.Empty;

        /// <summary>
        /// 被保険者郵便番号
        /// </summary>
        [Column("hokensya_post")]
        [MaxLength(7)]
        public string? HokensyaPost { get; set; } = string.Empty;

        /// <summary>
        /// 被保険者住所
        /// </summary>
        [Column("hokensya_address")]
        [MaxLength(100)]
        public string? HokensyaAddress { get; set; } = string.Empty;

        /// <summary>
        /// 被保険者電話番号
        /// </summary>
        [Column("hokensya_tel")]
        [MaxLength(15)]
        public string? HokensyaTel { get; set; } = string.Empty;

        /// <summary>
        /// 継続区分
        ///		1:任意継続
        /// </summary>
        [Column("keizoku_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int KeizokuKbn { get; set; }

        /// <summary>
        /// 資格取得日
        ///		yyyymmdd	
        /// </summary>
        [Column("sikaku_date")]
        [CustomAttribute.DefaultValue(0)]
        public int SikakuDate { get; set; }

        /// <summary>
        /// 交付日
        ///		yyyymmdd	
        /// </summary>
        [Column("kofu_date")]
        [CustomAttribute.DefaultValue(0)]
        public int KofuDate { get; set; }

        /// <summary>
        /// 適用開始日
        ///		yyyymmdd	
        /// </summary>
        [Column("start_date")]
        [CustomAttribute.DefaultValue(0)]
        public int StartDate { get; set; }

        /// <summary>
        /// 適用終了日
        ///		yyyymmdd	
        /// </summary>
        [Column("end_date")]
        [CustomAttribute.DefaultValue(0)]
        public int EndDate { get; set; }

        /// <summary>
        /// 負担率
        ///		※未使用
        /// </summary>
        [Column("rate")]
        [CustomAttribute.DefaultValue(0)]
        public int Rate { get; set; }

        /// <summary>
        /// 一部負担限度額
        ///		※未使用
        /// </summary>
        [Column("gendogaku")]
        [CustomAttribute.DefaultValue(0)]
        public int Gendogaku { get; set; }

        /// <summary>
        /// 高額療養費区分
        ///		70歳以上
        ///			0:一般
        ///			3:上位(～2018/07)
        ///			4:低所Ⅱ
        ///			5:低所Ⅰ
        ///			6:特定収入(～2008/12)
        ///			26:現役Ⅲ
        ///			27:現役Ⅱ
        ///			28:現役Ⅰ
        ///		70歳未満
        ///			0:限度額認定証なし
        ///			17:上位[A] (～2014/12)
        ///			18:一般[B] (～2014/12)
        ///			19:低所[C] (～2014/12)
        ///			26:区ア／標準報酬月額83万円以上
        ///			27:区イ／標準報酬月額53..79万円
        ///			28:区ウ／標準報酬月額28..50万円
        ///			29:区エ／標準報酬月額26万円以下
        ///			30:区オ／低所得者
        /// </summary>
        [Column("kogaku_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuKbn { get; set; }

        /// <summary>
        /// 高額療養費処理区分
        ///		1:高額委任払い 
        ///		2:適用区分一般
        /// </summary>
        [Column("kogaku_type")]
        [CustomAttribute.DefaultValue(0)]
        public int KogakuType { get; set; }

        /// <summary>
        /// 限度額特例対象年月１
        ///		yyyymm
        /// </summary>
        [Column("tokurei_ym1")]
        [CustomAttribute.DefaultValue(0)]
        public int TokureiYm1 { get; set; }

        /// <summary>
        /// 限度額特例対象年月２
        ///		yyyymm
        /// </summary>
        [Column("tokurei_ym2")]
        [CustomAttribute.DefaultValue(0)]
        public int TokureiYm2 { get; set; }

        /// <summary>
        /// 多数回該当適用開始年月
        ///		yyyymm
        /// </summary>
        [Column("tasukai_ym")]
        [CustomAttribute.DefaultValue(0)]
        public int TasukaiYm { get; set; }

        /// <summary>
        /// 職務上区分
        ///		1:職務上
        ///		2:下船後３月以内 
        ///		3:通勤災害
        /// </summary>
        [Column("syokumu_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int SyokumuKbn { get; set; }

        /// <summary>
        /// 国保減免区分
        ///		1:減額 
        ///		2:免除 
        ///		3:支払猶予
        /// </summary>
        [Column("genmen_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int GenmenKbn { get; set; }

        /// <summary>
        /// 国保減免割合
        ///		※不要？
        /// </summary>
        [Column("genmen_rate")]
        [CustomAttribute.DefaultValue(0)]
        public int GenmenRate { get; set; }

        /// <summary>
        /// 国保減免金額
        ///		※不要？
        /// </summary>
        [Column("genmen_gaku")]
        [CustomAttribute.DefaultValue(0)]
        public int GenmenGaku { get; set; }

        /// <summary>
        /// 特記事項１
        /// </summary>
        [Column("tokki1")]
        [MaxLength(2)]
        public string? Tokki1 { get; set; } = string.Empty;

        /// <summary>
        /// 特記事項２
        /// </summary>
        [Column("tokki2")]
        [MaxLength(2)]
        public string? Tokki2 { get; set; } = string.Empty;

        /// <summary>
        /// 特記事項３
        /// </summary>
        [Column("tokki3")]
        [MaxLength(2)]
        public string? Tokki3 { get; set; } = string.Empty;

        /// <summary>
        /// 特記事項４
        /// </summary>
        [Column("tokki4")]
        [MaxLength(2)]
        public string? Tokki4 { get; set; } = string.Empty;

        /// <summary>
        /// 特記事項５
        /// </summary>
        [Column("tokki5")]
        [MaxLength(2)]
        public string? Tokki5 { get; set; } = string.Empty;

        /// <summary>
        /// 労災交付番号
        ///		短期給付: 労働保険番号
        ///		傷病年金: 年金証書番号
        ///		アフターケア: 健康管理手帳番号
        /// </summary>
        [Column("rousai_kofu_no")]
        [MaxLength(14)]
        public string? RousaiKofuNo { get; set; } = string.Empty;

        /// <summary>
        /// 労災災害区分
        ///		1:業務中の災害 
        ///		2:通勤途上の災害
        /// </summary>
        [Column("rousai_saigai_kbn")]
        [CustomAttribute.DefaultValue(0)]
        public int RousaiSaigaiKbn { get; set; }

        /// <summary>
        /// 労災事業所名
        /// </summary>
        [Column("rousai_jigyosyo_name")]
        [MaxLength(80)]
        public string? RousaiJigyosyoName { get; set; } = string.Empty;

        /// <summary>
        /// 労災都道府県名
        /// </summary>
        [Column("rousai_pref_name")]
        [MaxLength(10)]
        public string? RousaiPrefName { get; set; } = string.Empty;

        /// <summary>
        /// 労災所在地郡市区名
        /// </summary>
        [Column("rousai_city_name")]
        [MaxLength(20)]
        public string? RousaiCityName { get; set; } = string.Empty;

        /// <summary>
        /// 労災傷病年月日
        ///		yyyymmdd	
        /// </summary>
        [Column("rousai_syobyo_date")]
        [CustomAttribute.DefaultValue(0)]
        public int RousaiSyobyoDate { get; set; }

        /// <summary>
        /// 労災傷病コード
        /// </summary>
        [Column("rousai_syobyo_cd")]
        [MaxLength(2)]
        public string? RousaiSyobyoCd { get; set; } = string.Empty;

        /// <summary>
        /// 労災労働局コード
        /// </summary>
        [Column("rousai_roudou_cd")]
        [MaxLength(2)]
        public string? RousaiRoudouCd { get; set; } = string.Empty;

        /// <summary>
        /// 労災監督署コード
        /// </summary>
        [Column("rousai_kantoku_cd")]
        [MaxLength(2)]
        public string? RousaiKantokuCd { get; set; } = string.Empty;

        /// <summary>
        /// 労災レセ請求回数
        /// </summary>
        [Column("rousai_rece_count")]
        [CustomAttribute.DefaultValue(0)]
        public int RousaiReceCount { get; set; }

        /// <summary>
        /// 自賠保険会社名
        /// </summary>
        [Column("jibai_hoken_name")]
        [MaxLength(100)]
        public string? JibaiHokenName { get; set; } = string.Empty;

        /// <summary>
        /// 自賠保険担当者
        /// </summary>
        [Column("jibai_hoken_tanto")]
        [MaxLength(40)]
        public string? JibaiHokenTanto { get; set; } = string.Empty;

        /// <summary>
        /// 自賠保険連絡先
        /// </summary>
        [Column("jibai_hoken_tel")]
        [MaxLength(15)]
        public string? JibaiHokenTel { get; set; } = string.Empty;

        /// <summary>
        /// 自賠受傷日
        ///		yyyymmdd	
        /// </summary>
        [Column("jibai_jyusyou_date")]
        [CustomAttribute.DefaultValue(0)]
        public int JibaiJyusyouDate { get; set; }

        /// <summary>
        /// 削除区分
        ///		1:削除
        /// </summary>
        [Column("is_deleted")]
        //[Index("pt_hoken_inf_ukey", 4)]
        [CustomAttribute.DefaultValue(0)]
        public int IsDeleted { get; set; }

        /// <summary>
        /// 作成日時	
        /// </summary>
        [Column("create_date")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者		
        /// </summary>
        [Column(name: "create_id")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末			
        /// </summary>
        [Column(name: "create_machine")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時			
        /// </summary>
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者			
        /// </summary>
        [Column(name: "update_id")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末			
        /// </summary>
        [Column(name: "update_machine")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; }  = string.Empty;

        /// <summary>
        /// 労災療養開始日
        /// 
        /// </summary>
        [Column("ryoyo_start_date")]
        [CustomAttribute.DefaultValue(0)]
        public int RyoyoStartDate { get; set; }

        /// <summary>
        /// 労災療養終了日
        /// 
        /// </summary>
        [Column("ryoyo_end_date")]
        [CustomAttribute.DefaultValue(0)]
        public int RyoyoEndDate { get; set; }
    }
}