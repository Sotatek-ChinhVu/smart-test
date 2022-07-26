using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    /// <summary>
    /// 点数マスタ
    /// </summary>
    [Table("TEN_MST_MOTHER")]
    public class TenMstMother : EmrCloneable<TenMstMother>
    {
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        [Key]
        [Column("HP_ID", Order = 1)]
        public int HpId { get; set; }

        /// <summary>
        /// 診療行為コード
        /// </summary>
        //[Key]
        [Column("ITEM_CD", Order = 2)]
        [MaxLength(10)]
        public string ItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 有効開始年月日
        ///     yyyymmdd
        /// </summary>
        //[Key]
        [Column("START_DATE", Order = 3)]
        public int StartDate { get; set; }

        /// <summary>
        /// 有効終了年月日
        ///     yyyymmdd
        /// </summary>
        [Required]
        [Column("END_DATE")]
        public int EndDate { get; set; }


        /// <summary>
        /// マスター種別
        ///     S: 診療行為
        ///     Y: 医薬品
        ///     T: 特材
        ///     C: コメント
        ///     R: 労災
        ///     U: 労災特定器材
        ///     D: 労災コメントマスタ
        /// </summary>
        [Column("MASTER_SBT")]
        [MaxLength(1)]
        public string MasterSbt { get; set; } = string.Empty;

        /// <summary>
        /// 診療行為区分
        /// </summary>
        [Required]
        [Column("SIN_KOUI_KBN")]
        public int SinKouiKbn { get; set; }

        /// <summary>
        /// 漢字名称
        /// </summary>
        [Column("NAME")]
        [MaxLength(240)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// カナ名称１
        /// </summary>
        [Column("KANA_NAME1")]
        [MaxLength(120)]
        public string KanaName1 { get; set; } = string.Empty;

        /// <summary>
        /// カナ名称２
        /// </summary>
        [Column("KANA_NAME2")]
        [MaxLength(120)]
        public string KanaName2 { get; set; } = string.Empty;

        /// <summary>
        /// カナ名称３
        /// </summary>
        [Column("KANA_NAME3")]
        [MaxLength(120)]
        public string KanaName3 { get; set; } = string.Empty;

        /// <summary>
        /// カナ名称４
        /// </summary>
        [Column("KANA_NAME4")]
        [MaxLength(120)]
        public string KanaName4 { get; set; } = string.Empty;

        /// <summary>
        /// カナ名称５
        /// </summary>
        [Column("KANA_NAME5")]
        [MaxLength(120)]
        public string KanaName5 { get; set; } = string.Empty;

        /// <summary>
        /// カナ名称６
        /// </summary>
        [Column("KANA_NAME6")]
        [MaxLength(120)]
        public string KanaName6 { get; set; } = string.Empty;

        /// <summary>
        /// カナ名称７
        /// </summary>
        [Column("KANA_NAME7")]
        [MaxLength(120)]
        public string KanaName7 { get; set; } = string.Empty;

        /// <summary>
        /// 領収証用名称
        ///     漢字名称以外を領収証に印字する場合、設定する
        /// </summary>
        [Column("RYOSYU_NAME")]
        [MaxLength(240)]
        public string RyosyuName { get; set; } = string.Empty;

        /// <summary>
        /// 請求用名称
        ///     計算後、請求用の名称
        /// </summary>
        [Column("RECE_NAME")]
        [MaxLength(240)]
        public string ReceName { get; set; } = string.Empty;

        /// <summary>
        /// 点数識別
        ///     1: 金額（整数部7桁、小数部2桁）
        ///     2: 都道府県購入価格
        ///     3: 点数（プラス）
        ///     4: 都道府県購入価格（点数）、金額（整数部のみ）
        ///     5: %加算
        ///     6: %減算
        ///     7: 減点診療行為
        ///     8: 点数（マイナス）
        ///     9: 乗算割合
        ///     10: 除算金額（金額を１０で除す。） ※ベントナイト用
        ///     11: 乗算金額（金額を10で乗ずる。） ※ステミラック注用
        ///     99: 労災円項目
        /// </summary>
        [Required]
        [Column("TEN_ID")]
        public int TenId { get; set; }

        /// <summary>
        /// 点数
        /// </summary>
        [Column("TEN")]
        [CustomAttribute.DefaultValue(0)]
        public double Ten { get; set; }

        /// <summary>
        /// レセ単位コード
        /// </summary>
        [Column("RECE_UNIT_CD")]
        [MaxLength(3)]
        public string ReceUnitCd { get; set; } = string.Empty;

        /// <summary>
        /// レセ単位名称
        /// </summary>
        [Column("RECE_UNIT_NAME")]
        [MaxLength(24)]
        public string ReceUnitName { get; set; } = string.Empty;

        /// <summary>
        /// オーダー単位名称
        ///     オーダー時に使用する単位
        /// </summary>
        [Column("ODR_UNIT_NAME")]
        [MaxLength(24)]
        public string OdrUnitName { get; set; } = string.Empty;

        /// <summary>
        /// 数量換算単位名称
        ///     薬剤情報提供書の全数量に換算した値を表示する場合の当該医薬品の換算単位名称を表す。
        /// </summary>
        [Column("CNV_UNIT_NAME")]
        [MaxLength(24)]
        public string CnvUnitName { get; set; } = string.Empty;

        /// <summary>
        /// オーダー単位換算値
        ///     薬剤情報提供書の全数量に換算した値を表示する場合、当該医薬品の保険請求上の単位からオーダー単位へ換算するための値を表す。
        [Column("ODR_TERM_VAL")]
        [CustomAttribute.DefaultValue(0)]
        public double OdrTermVal { get; set; }

        /// <summary>
        /// 数量換算単位換算値
        ///     薬剤情報提供書の全数量に換算した値を表示する場合、当該医薬品の保険請求上の単位から数量換算単位へ換算するための値を表す。
        /// </summary>
        [Column("CNV_TERM_VAL")]
        [CustomAttribute.DefaultValue(0)]
        public double CnvTermVal { get; set; }

        /// <summary>
        /// 既定数量
        ///     0は未設定
        /// </summary>
        [Column("DEFAULT_VAL")]
        [CustomAttribute.DefaultValue(0)]
        public double DefaultVal { get; set; }

        /// <summary>
        /// 採用区分
        ///     0: 未採用
        ///     1: 採用
        /// </summary>
        [Column("IS_ADOPTED")]
        [CustomAttribute.DefaultValue(0)]
        public int IsAdopted { get; set; }


        /// <summary>
        /// 後期高齢者医療適用区分
        ///     0: 社保・後期高齢者ともに適用される診療行為
        ///     1: 社保のみに適用される診療行為
        ///     2: 後期高齢者のみに適用される診療行為
        /// </summary>
        [Column("KOUKI_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int KoukiKbn { get; set; }

        /// <summary>
        /// 包括対象検査
        ///     0: 1～12以外の診療行為 
        ///     1: 血液化学検査の包括項目 
        ///     2: 内分泌学的検査の包括項目 
        ///     3: 肝炎ウイルス関連検査の包括項目 
        ///     5: 腫瘍マーカーの包括項目 
        ///     6: 出血・凝固検査の包括項目 
        ///     7: 自己抗体検査の包括項目 
        ///     8: 内分泌負荷試験の包括項目 
        ///     9: 感染症免疫学的検査のうち、ウイルス抗体価（定性・半定量・定量） 
        ///     10: 感染症免疫学的検査のうち、グロブリンクラス別ウイルス抗体価 
        ///     11:血漿蛋白免疫学的検査のうち、特異的ＩｇＥ半定量・定量及びアレルゲン刺激性遊離ヒスタミン（ＨＲＴ） 
        ///     12: 悪性腫瘍遺伝子検査の包括項目
        /// </summary>
        [Column("HOKATU_KENSA")]
        [CustomAttribute.DefaultValue(0)]
        public int HokatuKensa { get; set; }

        /// <summary>
        /// 傷病名関連区分
        ///     0: 3～9以外の診療行為 
        ///     3: 皮膚科特定疾患指導管理料（Ⅰ） 
        ///     4: 皮膚科特定疾患指導管理料（Ⅱ） 
        ///     5: 特定疾患療養管理料、特定疾患処方管理加算１（処方料）、特定疾患処方管理加算１（処方箋料）、特定疾患処方管理加算２（処方料）、特定疾患処方管理加算２（処方箋料） 
        ///     7: てんかん指導料 
        ///     9: 難病外来指導管理料
        /// </summary>
        [Column("BYOMEI_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int ByomeiKbn { get; set; }

        /// <summary>
        /// 医学管理料
        ///     2以上の医学管理等を行った場合に、主たる医学管理等の所定点数を算定する背反関係がある診療行為に限り、コードを設定する。
        /// </summary>
        [Column("IGAKUKANRI")]
        [CustomAttribute.DefaultValue(0)]
        public int Igakukanri { get; set; }

        /// <summary>
        /// 実日数カウント
        ///     0: 実日数に含めない
        ///     1: 実日数に含める
        /// </summary>
        [Column("JITUDAY_COUNT")]
        [CustomAttribute.DefaultValue(0)]
        public int JitudayCount { get; set; }

        /// <summary>
        /// 実日数
        ///     0: 1～4以外の診療行為 
        ///     1: 算定回数が診療実日数以下の診療行為 
        ///     2: 初診料、再診料、外来診療料等 
        ///     3: 入院基本料、特定入院料 
        ///     4: 外泊
        /// </summary>
        [Column("JITUDAY")]
        [CustomAttribute.DefaultValue(0)]
        public int Jituday { get; set; }

        /// <summary>
        /// 日数・回数
        ///     実日数=0, 日数回数=0 - 算定回数と実日数の確認を要しない
        ///     実日数=1, 日数回数=0 - 算定回数が実日数以下である確認を要する
        ///     実日数 = 2, 日数回数 = 1 - 初診料
        ///     実日数=2, 日数回数=2 - 再診料、外来診療料自体、又は再診料、外来診療料が含まれる診療行為
        ///     実日数 = 3, 日数回数 = 3 - 入院基本料、特定入院料
        ///     実日数 = 4, 日数回数 = 0 - 外泊
        /// </summary>
        [Column("DAY_COUNT")]
        [CustomAttribute.DefaultValue(0)]
        public int DayCount { get; set; }

        /// <summary>
        /// 医薬品関連区分
        ///     医薬品の種類を算定要件とする診療行為であるか否かを表す。
        ///     0: 1～4以外の診療行為 
        ///     1: 麻薬加算、毒薬加算、覚醒剤加算、向精神薬加算、麻薬注射加算 
        ///     3: 神経ブロック（神経破壊剤使用） 
        ///     4: 生物学的製剤加算
        /// </summary>
        [Column("DRUG_KANREN_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int DrugKanrenKbn { get; set; }

        /// <summary>
        /// きざみ値計算識別
        ///     0: きざみ値により算定しない診療行為（項番１２「新又は現点数」により算定する。） 
        ///     1: きざみ値により算定する診療行為
        /// </summary>
        [Column("KIZAMI_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int KizamiId { get; set; }

        /// <summary>
        /// きざみ下限値
        ///     きざみ値により算定する診療行為において「数量データ」の下限値を表す。
        ///     下限値の制限がない場合は「0」である。
        /// </summary>
        [Column("KIZAMI_MIN")]
        [CustomAttribute.DefaultValue(0)]
        public int KizamiMin { get; set; }

        /// <summary>
        /// きざみ上限値
        ///     きざみ値により算定する診療行為において「数量データ」の上限値を表す。
        ///     上限値の制限がない場合は「99999999」である。
        /// </summary>
        [Column("KIZAMI_MAX")]
        [CustomAttribute.DefaultValue(0)]
        public int KizamiMax { get; set; }

        /// <summary>
        /// きざみ値
        ///     きざみ値により算定する診療行為において点数のきざみ単位を表す。
        /// </summary>
        [Column("KIZAMI_VAL")]
        [CustomAttribute.DefaultValue(0)]
        public int KizamiVal { get; set; }

        /// <summary>
        /// きざみ点数
        ///     きざみ値により算定する診療行為においてきざみ点数を表す。
        /// </summary>
        [Column("KIZAMI_TEN")]
        [CustomAttribute.DefaultValue(0)]
        public double KizamiTen { get; set; }

        /// <summary>
        /// きざみ値上下限エラー処理
        ///     当該診療行為に係る「数量データ」が「下限値－きざみ値」以下又は「上限値」を超えた場合の対処方法を表す。
        ///     上下限エラー処理は「0」～「3」の4つの値を持ち、「下限値－きざみ値」以下の場合の条件、
        ///     及び「上限値」を超えた場合の条件を両方共に満たす値を設定する。
        /// </summary>
        [Column("KIZAMI_ERR")]
        [CustomAttribute.DefaultValue(0)]
        public int KizamiErr { get; set; }

        /// <summary>
        /// 上限回数
        ///     0: 上限未設定
        /// </summary>
        [Column("MAX_COUNT")]
        [CustomAttribute.DefaultValue(0)]
        public int MaxCount { get; set; }

        /// <summary>
        /// 上限回数エラー処理
        ///     当該診療行為の算定可能回数が上限回数を超えた場合の処理方法を表す。
        ///     0: 上限回数を確認する。
        ///     1: 上限回数にて算定する。
        /// </summary>
        [Column("MAX_COUNT_ERR")]
        [CustomAttribute.DefaultValue(0)]
        public int MaxCountErr { get; set; }

        /// <summary>
        /// 注加算コード
        ///     注加算が算定可能な診療行為（基本項目、合成項目及び準用項目）と注加算を関連付ける任意の同一番号を設定する。
        ///    「告示等識別区分（１）」に「７：加算項目」を設定している診療行為のうち、
        ///     注加算コードを設定せずに専用の項目を設定して算定可否を判定する診療行為は「別紙７－８」のとおりである。 
        /// </summary>
        [Column("TYU_CD")]
        [MaxLength(4)]
        public string TyuCd { get; set; } = string.Empty;

        /// <summary>
        /// 注加算通番
        ///     １つの診療行為に対して同時に算定が可能な注加算に、異なる番号を設定する。 
        ///     注加算が算定可能な診療行為（基本項目、合成項目及び準用項目）に「0」を、
        ///     注加算である診療行為に「1」から「9」及び「A」から「Z」（昇順、アルファベット順）を設定する。 
        ///     注加算コードと注加算通番の関連は「別紙７－９」のとおりである。 
        /// </summary>
        [Column("TYU_SEQ")]
        [MaxLength(1)]
        public string TyuSeq { get; set; } = string.Empty;

        /// <summary>
        /// 通則年齢
        ///     当該診療行為が年齢の通則加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: １以外の診療行為
        ///     　1: 通則年齢加算が算定可能な診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: １以外の診療行為
        ///     　1: 通則年齢加算自体
        /// </summary>
        [Column("TUSOKU_AGE")]
        [CustomAttribute.DefaultValue(0)]
        public int TusokuAge { get; set; }

        /// <summary>
        /// 上下限年齢下限年齢
        ///     当該診療行為が算定可能な年齢の下限値を表す。
        ///     算定可能な年齢 ≧ 下限年齢
        ///     下限年齢に制限のない場合は「00」である。
        ///     数字２桁以外の取り扱いは以下のとおり
        ///     AA：生後２８日
        ///     B3：３歳に達した日の翌月の１日
        ///     B6：６歳に達した日の翌月の１日
        ///     BF：１５歳に達した日の翌月の１日
        ///     BK：２０歳に達した日の翌月の１日
        ///     MG：未就学
        /// </summary>
        [Column("MIN_AGE")]
        [MaxLength(2)]
        public string MinAge { get; set; } = string.Empty;

        /// <summary>
        /// 上下限年齢上限年齢
        ///     当該診療行為が算定可能な年齢の「上限値＋１」を表す。
        ///     算定可能な年齢 ＜ 上限年齢
        ///     上限年齢に制限のない場合は「00」である。
        ///     数字２桁以外の取り扱いは以下のとおり
        ///     AA：生後２８日
        ///     B3：３歳に達した日の翌月の１日
        ///     B6：６歳に達した日の翌月の１日
        ///     BF：１５歳に達した日の翌月の１日
        ///     BK：２０歳に達した日の翌月の１日
        ///     MG：未就学
        /// </summary>
        [Column("MAX_AGE")]
        [MaxLength(2)]
        public string MaxAge { get; set; } = string.Empty;

        /// <summary>
        /// 上下限年齢チェック
        ///     MIN_AGE, MAX_AGEの設定がある場合のみ有効
        ///     0: 年齢範囲外の時、算定不可にする
        ///     1: 年齢範囲外の時、警告扱いにする
        ///     2: チェックしない
        /// </summary>
        [Column("AGE_CHECK")]
        [CustomAttribute.DefaultValue(0)]
        public int AgeCheck { get; set; }

        /// <summary>
        /// 時間加算区分
        ///     当該診療行為が時間加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: 1, 3以外の診療行為
        ///     　1: 時間加算が算定可能な診療行為（含む合成項目）
        ///     　3: 初診料の休日加算に係る診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: 下記以外の診療行為
        ///     　1: 時間外加算自体
        ///     　2: 休日加算自体
        ///     　3: 初診料の休日加算自体
        ///     　4: 深夜加算自体
        ///     　5: 時間外特例加算自体
        ///     　6: 夜間・早朝加算自体
        ///     　7: 夜間加算自体
        ///     　8: 時間外、深夜、時間外特例加算（手術又は、1000 点以上の処置）（注加算又は通則加算）自体
        ///     　9: 休日加算（手術又は、1000 点以上の処置）（注加算又は通則加算）自体
        /// </summary>
        [Column("TIME_KASAN_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int TimeKasanKbn { get; set; }

        /// <summary>
        /// 基準不適合逓減区分
        ///     当該診療行為が施設基準不適合の場合点数を逓減して算定できる診療行為であるか否かを表す。
        ///     　0: 点数逓減して算定できる診療行為以外
        ///     　1: 逓減コード自体
        ///     　2: 点数逓減して算定できる診療行為
        ///     （削）3: 年齢が１歳未満のとき、点数逓減して算定できる診療行為
        /// </summary>
        [Column("FUTEKI_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int FutekiKbn { get; set; }

        /// <summary>
        /// 基準不適合逓減対象施設区分
        ///     当該診療行為が施設基準不適合の場合点数を逓減して算定できる診療行為について設定した施設基準コードを表す。
        ///     基準不適合逓減対象施設区分（施設基準コード）については「別紙５」を参照。
        /// </summary>
        [Column("FUTEKI_SISETU_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int FutekiSisetuKbn { get; set; }

        /// <summary>
        /// 処置乳幼児加算区分
        ///     当該診療行為が処置乳幼児加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: １～５以外の診療行為
        ///     　1: ３歳未満乳幼児加算（処置）（１００点）が算定できる診療行為
        ///     　2: ３歳未満乳幼児加算（処置）（５０点）が算定できる診療行為
        ///     　3: ６歳未満乳幼児加算（処置）（１００点）が算定できる診療行為
        ///     　4: ６歳未満乳幼児加算（処置）（７５点）が算定できる診療行為
        ///     　5: ６歳未満乳幼児加算（処置）（５０点）が算定できる診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: １～５以外の診療行為
        ///     　1: ３歳未満乳幼児加算（処置）（１００点）自体
        ///     　2: ３歳未満乳幼児加算（処置）（５０点）自体
        ///     　3: ６歳未満乳幼児加算（処置）（１００点）自体
        ///     　4: ６歳未満乳幼児加算（処置）（７５点）自体
        ///     　5: ６歳未満乳幼児加算（処置）（５０点）自体
        /// </summary>
        [Column("SYOTI_NYUYOJI_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int SyotiNyuyojiKbn { get; set; }

        /// <summary>
        /// 極低出生体重児加算区分
        ///     当該診療行為が極低出生体重児加算（手術）（４００％）又は新生児加算（手術）（３００％）が算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: １以外の診療行為
        ///     　1: 極低出生体重児加算（手術）（４００％）、新生児加算（手術）（３００％）が算定できる診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: １以外の診療行為
        ///     　1: 極低出生体重児加算（手術）（４００％）、新生児加算（手術）（３００％）自体
        /// </summary>
        [Column("LOW_WEIGHT_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int LowWeightKbn { get; set; }

        /// <summary>
        /// 検査等実施判断区分
        ///     当該診療行為が検査等の実施料又は判断料に関するものであるか否かを表す。
        ///     　0: 1, 2以外の診療行為
        ///     　1: 検体検査実施料、生体検査実施料、核医学撮影料、コンピューター断層撮影料、病理標本作製料に係る診療行為
        ///     　2: 検体検査判断料、生体検査判断料、核医学診断料、コンピューター断層診断料、病理診断料、病理判断料に係る診療行為
        /// </summary>
        [Column("HANDAN_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int HandanKbn { get; set; }


        /// <summary>
        /// 検査等実施判断グループ区分
        ///     当該診療行為が検査等の場合、判断料・診断料又は判断料･診断料を算定できるグループ区分を表す。
        ///     　0: 1～42以外の診療行為
        ///     (検体検査)
        ///     　　1: 尿・糞便等検査　2: 血液学的検査　3: 生化学的検査（Ⅰ）　4: 生化学的検査（Ⅱ）　5: 免疫学的検査
        ///     (細菌検査)
        ///     　　6: 微生物学的検査
        ///     (検体検査)
        ///     　　8: 基本的検体検査
        ///     (生理検査)
        ///     　　11: 呼吸機能検査　13: 脳波検査　14: 神経・筋検査　15: ラジオアイソトープ検査 16: 眼科学的検査
        ///     (その他検査)
        ///     　　31: 核医学診断（Ｅ１０１－２～Ｅ１０１－５）
        ///     　　32: 核医学診断（それ以外） 33: コンピューター断層診断
        ///     (病理検査)
        ///     　　40: 病理診断 ※
        ///     　　41: 病理診断（組織診断）
        ///     　　42: 病理診断（細胞診断）
        ///     　　※40: 病理診断は41: 病理診断（組織診断）、42: 病理診断（細胞診断）を含む。
        /// </summary>
        [Column("HANDAN_GRP_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int HandanGrpKbn { get; set; }

        /// <summary>
        /// 逓減対象区分
        ///     当該診療行為が算定回数による逓減計算の対象となるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: 1以外の診療行為
        ///     　1: 逓減計算の対象となる診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: 1以外の診療行為
        ///     　1: 逓減コード自体
        /// </summary>
        [Column("TEIGEN_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int TeigenKbn { get; set; }

        /// <summary>
        /// 脊髄誘発電位測定等加算区分
        ///     当該診療行為が脊髄誘発電位測定等加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: 1, 2以外の診療行為
        ///     　1: 脊髄誘発電位測定等加算（１ 脳、脊椎、脊髄又は大動脈瘤の手術に用いた場合）が算定可能な診療行為
        ///     　2: 脊髄誘発電位測定等加算（２ 甲状腺又は副甲状腺の手術に用いた場合）が算定可能な診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: 1, 2以外の診療行為
        ///     　1: 脊髄誘発電位測定等加算（１ 脳、脊椎、脊髄又は大動脈瘤の手術に用いた場合）自体
        ///     　2: 脊髄誘発電位測定等加算（２ 甲状腺又は副甲状腺の手術に用いた場合）自体
        /// </summary>
        [Column("SEKITUI_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int SekituiKbn { get; set; }

        /// <summary>
        /// 頸部郭清術併施加算区分
        ///     当該診療行為が頸部郭清術併施加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: 1以外の診療行為
        ///     　1: 頸部郭清術併施加算が算定可能な診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: 頸部郭清術併施加算が算定できない診療行為
        ///     　1: 頸部郭清術併施加算自体
        /// </summary>
        [Column("KEIBU_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int KeibuKbn { get; set; }

        /// <summary>
        /// 自動縫合器加算区分
        ///     当該診療行為が自動縫合器加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: １以外の診療行為
        ///     　1: 自動縫合器加算（2500点）が算定可能な診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: １以外の診療行為
        ///     　1: 自動縫合器加算（2500点）自体
        /// </summary>
        [Column("AUTO_HOUGOU_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int AutoHougouKbn { get; set; }

        /// <summary>
        /// 外来管理加算区分
        ///     当該診療行為が外来管理加算を算定できないものであるか否かを表す。
        ///     　0: １，２以外の診療行為
        ///     　1: 算定した場合に外来管理加算が算定できない診療行為
        ///     　2: 外来管理加算自体
        /// </summary>
        [Column("GAIRAI_KANRI_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int GairaiKanriKbn { get; set; }

        /// <summary>
        /// 通則加算所定点数対象区分
        ///     通則加算を行う場合において、所定点数として取扱うか否かを表す。
        ///     　0: 所定点数として取扱う診療行為及び通則加算
        ///     　1: 所定点数として取扱わない基本診療行為
        /// </summary>
        [Column("TUSOKU_TARGET_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int TusokuTargetKbn { get; set; }

        /// <summary>
        /// 包括逓減区分
        ///     逓減対象検査等のグループ区分を表す。
        /// </summary>
        [Column("HOKATU_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int HokatuKbn { get; set; }

        /// <summary>
        /// 超音波内視鏡加算区分
        ///     当該診療行為が超音波内視鏡加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: １以外の診療行為
        ///     　1: 超音波内視鏡加算が算定可能な診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: １以外の診療行為
        ///     　1: 超音波内視鏡加算自体
        /// </summary>
        [Column("TYOONPA_NAISI_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int TyoonpaNaisiKbn { get; set; }

        /// <summary>
        /// 自動吻合器加算区分
        ///     通則加算を行う場合において、所定点数として取扱うか否かを表す。
        ///     　0: 所定点数として取扱う診療行為及び通則加算
        ///     　1: 所定点数として取扱わない基本診療行為
        /// </summary>
        [Column("AUTO_FUNGO_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int AutoFungoKbn { get; set; }

        /// <summary>
        /// 超音波凝固切開装置等加算区分
        ///     当該診療行為が超音波凝固切開装置等加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: 1以外の診療行為
        ///     　1: 超音波凝固切開装置等加算が算定可能な診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: 1以外の診療行為
        ///     　1: 超音波凝固切開装置等加算自体
        /// </summary>
        [Column("TYOONPA_GYOKO_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int TyoonpaGyokoKbn { get; set; }

        /// <summary>
        /// 画像等手術支援加算
        ///     当該診療行為が画像等手術支援加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: 1～5以外の診療行為
        ///     　1: ナビゲーションによる支援加算（２０００点）が算定できる診療行為
        ///     　2: 実物大臓器立体モデルによる支援加算（２０００点）が算定できる診療行為
        ///     　3: ナビゲーション又は実物大臓器立体モデルによる支援加算（共に２０００点）が算定できる診療行為
        ///     　4: 患者適合型手術支援ガイドによる支援加算（２０００点）が算定できる診療行為
        ///     　5: ナビゲーション又は患者適合型手術支援ガイドによる支援加算（共に２０００点）が算定できる診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: 1,2,4以外の診療行為
        ///     　1: ナビゲーションによる支援加算自体
        ///     　2: 実物大臓器立体モデルによる支援加算自体
        ///     　4: 患者適合型手術支援ガイドによる支援加算自体
        /// </summary>
        [Column("GAZO_KASAN")]
        [CustomAttribute.DefaultValue(0)]
        public int GazoKasan { get; set; }

        /// <summary>
        /// 医療観察法対象区分
        ///     当該診療行為が医療観察法において算定可能であるか否かを表す。
        ///     　0: 1～4以外の診療行為
        ///     　1: 入院のみに出来高部分で算定可能な診療行為
        ///     　2: 外来（通院）のみに出来高部分で算定可能な診療行為
        ///     　3: 入院、外来（通院）共に出来高部分で算定可能な診療行為
        ///     　4: 医療観察法専用の診療行為
        /// </summary>
        [Column("KANSATU_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int KansatuKbn { get; set; }

        /// <summary>
        /// 麻酔識別区分
        ///     当該診療行為がマスク又は気管内挿管による閉鎖循環式全身麻酔であるか否かを表す。
        ///     　0: １～９以外の診療行為
        ///     　1: マスク又は気管内挿管による閉鎖循環式全身麻酔１
        ///     　2: マスク又は気管内挿管による閉鎖循環式全身麻酔２
        ///     　3: マスク又は気管内挿管による閉鎖循環式全身麻酔３
        ///     　4: マスク又は気管内挿管による閉鎖循環式全身麻酔４
        ///     　5: マスク又は気管内挿管による閉鎖循環式全身麻酔５
        ///     　8: マスク又は気管内挿管による閉鎖循環式全身麻酔の加算（硬膜外麻酔併施加算以外）
        ///     　9: 硬膜外麻酔併施加算
        /// </summary>
        [Column("MASUI_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int MasuiKbn { get; set; }

        /// <summary>
        /// 副鼻腔手術用内視鏡加算
        ///     当該診療行為が副鼻腔手術用内視鏡加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: 1以外の診療行為
        ///     　1: 副鼻腔手術用内視鏡加算が算定可能な診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: 1以外の診療行為
        ///     　1: 副鼻腔手術用内視鏡加算自体
        /// </summary>
        [Column("FUKUBIKU_NAISI_KASAN")]
        [CustomAttribute.DefaultValue(0)]
        public int FukubikuNaisiKasan { get; set; }

        /// <summary>
        /// 副鼻腔手術用骨軟部組織切除機器加算
        ///     当該診療行為が副鼻腔手術用骨軟部組織切除機器加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: 1以外の診療行為
        ///     　1: 副鼻腔手術用骨軟部組織切除機器加算が算定可能な診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: 1以外の診療行為
        ///     　1: 副鼻腔手術用骨軟部組織切除機器加算自体
        /// </summary>
        [Column("FUKUBIKU_KOTUNAN_KASAN")]
        [CustomAttribute.DefaultValue(0)]
        public int FukubikuKotunanKasan { get; set; }


        /// <summary>
        /// 長時間麻酔管理加算
        ///     当該診療行為が長時間麻酔管理加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: 1,2以外の診療行為
        ///     　1: 長時間麻酔管理加算が算定可能な診療行為
        ///     　2: L008に掲げるマスク又は気管内挿管による閉鎖循環式全身麻酔の実施時間が８時間を超え、長時間麻酔管理加算を算定する場合に実施している必要がある手術
        ///     ＜加算項目、通則加算項目＞
        ///     　0: 1以外の診療行為
        ///     　1: 長時間麻酔管理加算自体
        /// </summary>
        [Column("MASUI_KASAN")]
        [CustomAttribute.DefaultValue(0)]
        public int MasuiKasan { get; set; }

        /// <summary>
        /// 非侵襲的血行動態モニタリング加算
        ///     当該診療行為が非侵襲的血行動態モニタリング加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: １、２以外の診療行為
        ///     　1: 非侵襲的血行動態モニタリング加算が算定可能な診療行為
        ///     　2: 非侵襲的血行動態モニタリング加算を算定する場合に実施している必要がある手術
        ///     ＜加算項目、通則加算項目＞
        ///     　0: １以外の診療行為
        ///     　1: 非侵襲的血行動態モニタリング加算自体
        /// </summary>
        [Column("MONITER_KASAN")]
        [CustomAttribute.DefaultValue(0)]
        public int MoniterKasan { get; set; }

        /// <summary>
        /// 凍結保存同種組織加算
        ///     当該診療行為が凍結保存同種組織加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: １以外の診療行為
        ///     　1: 凍結保存同種組織加算が算定可能な診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: 1以外の診療行為
        ///     　1: 凍結保存同種組織加算自体
        /// </summary>
        [Column("TOKETU_KASAN")]
        [CustomAttribute.DefaultValue(0)]
        public int ToketuKasan { get; set; }

        /// <summary>
        /// 点数表区分番号
        ///     医科点数表の「第２章 特掲診療料」「第１０部 手術」に規定する診療行為（通則及び注に掲げる加算等を除く。）の区分番号及び項番等を表す。
        /// </summary>
        [Column("TEN_KBN_NO")]
        [MaxLength(30)]
        public string TenKbnNo { get; set; } = string.Empty;

        /// <summary>
        /// 短期滞在手術
        ///     当該診療行為が短期滞在手術等基本料を算定できるものであるか否かを表す。
        ///     　０：１～４以外の診療行為
        ///     　１：短期滞在手術等基本料１
        ///     　２：短期滞在手術等基本料２
        ///     　３：短期滞在手術等基本料１が算定可能な診療行為（手術）
        ///     　４：短期滞在手術等基本料２が算定可能な診療行為（手術）
        /// </summary>
        [Column("SHORTSTAY_OPE")]
        [CustomAttribute.DefaultValue(0)]
        public int ShortstayOpe { get; set; }

        /// <summary>
        /// 部位区分
        ///     画像診断撮影部位マスターにおいて撮影部位を表す。
        ///     　0: 部位以外
        ///     　1: 頭部
        ///     　2: 躯幹
        ///     　3: 四肢
        ///     　5: 胸部
        ///     　6: 腹部
        ///     　7: 脊椎
        ///     　8: 消化管
        ///      10: 指
        ///      99: その他部位（撮影部位マスターでない場合も含む。）
        /// </summary>
        [Column("BUI_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int BuiKbn { get; set; }

        /// <summary>
        /// 施設基準コード１
        ///     ＜診療行為＞
        ///        当該診療行為が施設基準に関するものであるか否かを表す。
        ///     　「施設基準コード１」から使用し最大１０項目（「施設基準コード１０」）まで使用可能である。
        ///     　施設基準コードについては「別紙５」を参照。
        ///     ＜医薬品＞
        ///     　当該医薬品について薬価基準の規格単位数を表す。
        ///     　ただし、規格単位数が１の場合は省略し０を収容する。
        /// </summary>
        [Column("SISETUCD1")]
        [CustomAttribute.DefaultValue(0)]
        public int Sisetucd1 { get; set; }

        /// <summary>
        /// 施設基準コード２
        ///     ＜診療行為＞
        ///     施設基準コード１を参照。
        ///     ＜医薬品＞
        ///     　当該医薬品が湿布薬で単位が「ｇ」の場合は膏体量を収容する。
        ///             /// </summary>
        [Column("SISETUCD2")]
        [CustomAttribute.DefaultValue(0)]
        public int Sisetucd2 { get; set; }

        /// <summary>
        /// 施設基準コード３
        /// </summary>
        [Column("SISETUCD3")]
        [CustomAttribute.DefaultValue(0)]
        public int Sisetucd3 { get; set; }

        /// <summary>
        /// 施設基準コード４
        /// </summary>
        [Column("SISETUCD4")]
        [CustomAttribute.DefaultValue(0)]
        public int Sisetucd4 { get; set; }

        /// <summary>
        /// 施設基準コード５
        /// </summary>
        [Column("SISETUCD5")]
        [CustomAttribute.DefaultValue(0)]
        public int Sisetucd5 { get; set; }

        /// <summary>
        /// 施設基準コード６
        /// </summary>
        [Column("SISETUCD6")]
        [CustomAttribute.DefaultValue(0)]
        public int Sisetucd6 { get; set; }

        /// <summary>
        /// 施設基準コード７
        /// </summary>
        [Column("SISETUCD7")]
        [CustomAttribute.DefaultValue(0)]
        public int Sisetucd7 { get; set; }

        /// <summary>
        /// 施設基準コード８
        /// </summary>
        [Column("SISETUCD8")]
        [CustomAttribute.DefaultValue(0)]
        public int Sisetucd8 { get; set; }

        /// <summary>
        /// 施設基準コード９
        /// </summary>
        [Column("SISETUCD9")]
        [CustomAttribute.DefaultValue(0)]
        public int Sisetucd9 { get; set; }

        /// <summary>
        /// 施設基準コード１０
        /// </summary>
        [Column("SISETUCD10")]
        [CustomAttribute.DefaultValue(0)]
        public int Sisetucd10 { get; set; }

        /// <summary>
        /// 年齢加算下限年齢１
        ///     当該診療行為に算定可能な年齢注加算の診療行為コードを表し、最大４つの年齢範囲まで記録する。
        ///     未使用部分には、下限年齢、上限年齢及び注加算診療行為コードに「ゼロ」を記録する。
        ///     数字２桁以外の取り扱いは以下のとおり
        ///     AA: 生後28日
        ///     B3: 3歳に達した日の翌月の１日
        ///     BF: 15歳に達した日の翌月の１日
        ///     BK: 20歳に達した日の翌月の１日
        ///     年齢加算下限年齢：
        ///     当該診療行為に注加算の算定が可能な場合、記録された注加算診療行為コードの下限年齢を表す。
        ///     年齢加算上限年齢：
        ///     　当該診療行為に注加算の算定が可能な場合、記録された注加算診療行為コードの上限年齢を表す。
        ///     注加算診療行為コード：
        ///     　年齢注加算の診療行為コードを表す。
        /// </summary>
        [Column("AGEKASAN_MIN1")]
        [MaxLength(2)]
        public string AgekasanMin1 { get; set; } = string.Empty;

        /// <summary>
        /// 年齢加算上限年齢１
        ///     年齢加算下限年齢１を参照。
        /// </summary>
        [Column("AGEKASAN_MAX1")]
        [MaxLength(2)]
        public string AgekasanMax1 { get; set; } = string.Empty;

        /// <summary>
        /// 年齢加算注加算診療行為コード１
        ///      年齢加算下限年齢１を参照。
        /// </summary>
        [Column("AGEKASAN_CD1")]
        [MaxLength(10)]
        public string AgekasanCd1 { get; set; } = string.Empty;

        /// <summary>
        /// 年齢加算下限年齢２
        ///      年齢加算下限年齢１を参照。
        /// </summary>
        [Column("AGEKASAN_MIN2")]
        [MaxLength(2)]
        public string AgekasanMin2 { get; set; } = string.Empty;

        /// <summary>
        /// 年齢加算上限年齢２
        ///      年齢加算下限年齢１を参照。
        /// </summary>
        [Column("AGEKASAN_MAX2")]
        [MaxLength(2)]
        public string AgekasanMax2 { get; set; } = string.Empty;

        /// <summary>
        /// 年齢加算注加算診療行為コード２
        ///      年齢加算下限年齢１を参照。
        /// </summary>
        [Column("AGEKASAN_CD2")]
        [MaxLength(10)]
        public string AgekasanCd2 { get; set; } = string.Empty;

        /// <summary>
        /// 年齢加算下限年齢３
        ///      年齢加算下限年齢１を参照。
        /// </summary>
        [Column("AGEKASAN_MIN3")]
        [MaxLength(2)]
        public string AgekasanMin3 { get; set; } = string.Empty;

        /// <summary>
        /// 年齢加算上限年齢３
        ///      年齢加算下限年齢１を参照。
        /// </summary>
        [Column("AGEKASAN_MAX3")]
        [MaxLength(2)]
        public string AgekasanMax3 { get; set; } = string.Empty;

        /// <summary>
        /// 年齢加算注加算診療行為コード３
        ///      年齢加算下限年齢１を参照。
        /// </summary>
        [Column("AGEKASAN_CD3")]
        [MaxLength(10)]
        public string AgekasanCd3 { get; set; } = string.Empty;

        /// <summary>
        /// 年齢加算下限年齢４
        ///      年齢加算下限年齢１を参照。
        /// </summary>
        [Column("AGEKASAN_MIN4")]
        [MaxLength(2)]
        public string AgekasanMin4 { get; set; } = string.Empty;

        /// <summary>
        /// 年齢加算上限年齢４
        ///      年齢加算下限年齢１を参照。
        /// </summary>
        [Column("AGEKASAN_MAX4")]
        [MaxLength(2)]
        public string AgekasanMax4 { get; set; } = string.Empty;

        /// <summary>
        /// 年齢加算注加算診療行為コード４
        ///      年齢加算下限年齢１を参照。
        /// </summary>
        [Column("AGEKASAN_CD4")]
        [MaxLength(10)]
        public string AgekasanCd4 { get; set; } = string.Empty;

        /// <summary>
        /// 検体検査コメント
        ///     当該診療行為が、検体検査の検体コメントであるか否かを表す。
        ///     　0: 検体コメント以外
        ///     　1: 検体コメント
        /// </summary>
        [Column("KENSA_CMT")]
        [CustomAttribute.DefaultValue(0)]
        public int KensaCmt { get; set; }

        /// <summary>
        /// 麻毒区分
        ///     当該医薬品が麻薬、毒薬、覚せい剤原料又は向精神薬であるか否かを表す。
        ///     　0: 麻薬、毒薬、覚せい剤原料又は向精神薬以外
        ///     　1: 麻薬
        ///     　2: 毒薬
        ///     　3: 覚せい剤原料
        ///     　5: 向精神薬
        /// </summary>
        [Column("MADOKU_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int MadokuKbn { get; set; }

        /// <summary>
        /// 神経破壊剤区分
        ///     当該医薬品が神経破壊剤であるか否かを表す。
        ///     　0: 神経破壊剤以外
        ///     　1: 神経破壊剤
        /// </summary>
        [Column("SINKEI_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int SinkeiKbn { get; set; }

        /// <summary>
        /// 生物学的製剤区分
        ///     当該医薬品が生物学的製剤加算対象品目であるか否かを表す。
        ///     　0: 生物学的製剤加算対象品目以外
        ///     　1: 生物学的製剤加算対象品目
        /// </summary>
        [Column("SEIBUTU_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int SeibutuKbn { get; set; }

        /// <summary>
        /// 造影剤区分
        ///     当該医薬品が造影剤又は造影補助剤であるか否かを表す。
        ///     　0: 造影剤、造影補助剤以外
        ///     　1: 造影剤
        ///     　2: 造影補助剤
        /// </summary>
        [Column("ZOUEI_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int ZoueiKbn { get; set; }

        /// <summary>
        /// 薬剤区分
        ///     当該医薬品の薬剤区分を表す。
        ///       0: 薬剤以外
        ///     　1: 内用薬
        ///     　3: その他
        ///     　4: 注射薬
        ///     　6: 外用薬
        ///     　8: 歯科用薬剤
        ///     （削）9: 歯科特定薬剤
        ///     ※レセプト電算マスターの項目「剤型」を収容する。
        /// </summary>
        [Column("DRUG_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int DrugKbn { get; set; }

        /// <summary>
        /// 剤型区分
        ///     当該医薬品の剤型区分を表す。
        ///     　0: 下記以外
        ///     　1: 散剤
        ///     　2: 顆粒剤（細粒剤）
        ///     　3: 液剤
        ///     ※レセプト電算マスターの項目「剤型」とは異なる。
        /// </summary>
        [Column("ZAI_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int ZaiKbn { get; set; }

        /// <summary>
        /// 注射容量
        ///     当該医薬品が注射薬の場合、その容量（単位はｍＬ）を表す。
        /// </summary>
        [Column("CAPACITY")]
        [CustomAttribute.DefaultValue(0)]
        public int Capacity { get; set; }

        /// <summary>
        /// 後発医薬品区分
        ///     当該医薬品が後発医薬品に該当するか否かを表す。
        ///     　0: 後発医薬品でない
        ///     　1: 先発医薬品がある後発医薬品である
        ///     ※基金マスタの設定、オーダー時はKOHATU_KBN_MSTを見るようにすること
        /// </summary>
        [Column("KOHATU_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int KohatuKbn { get; set; }

        /// <summary>
        /// 特定器材年齢加算区分
        ///     当該特定器材が年齢加算に関係があるか否かを表す。
        ///     　0: 年齢加算に関係のない特定器材
        ///     　1: 年齢加算又は年齢加算が算定可能な特定器材
        ///     　　　＊胸部又は腹部単純撮影の乳幼児加算、及びフィルム料
        /// </summary>
        [Column("TOKUZAI_AGE_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int TokuzaiAgeKbn { get; set; }

        /// <summary>
        /// 酸素等区分
        ///     当該特定器材が酸素又は窒素に関するものであるか否かを表す。
        ///     　0: 酸素、窒素、酸素補正率及び高気圧酸素加算以外
        ///     　1: 酸素補正率及び高気圧酸素加算
        ///     　2: 定置式液化酸素貯槽（ＣＥ）
        ///     　3: 可搬式液化酸素容器（ＬＧＣ）
        ///     　4: 大型ボンベ
        ///     　5: 小型ボンベ
        ///     　9: 窒素
        /// </summary>
        [Column("SANSO_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int SansoKbn { get; set; }

        /// <summary>
        /// 特定器材種別１
        ///     当該特定器材の点数算定方法の種別を表す。
        ///     　0:  ↑購入価格｜
        ///     　　　｜－－－－｜
        ///     　　　｜　１０円↓により算定する特定器材
        ///     　2:  ↑↑購入価格↓｜
        ///     　　　｜－－－－－－｜
        ///     　　　｜　　１０円　↓により算定する特定器材
        ///     （酸素、窒素）
        ///     　3:  ↑購入価格｜
        ///     　　　｜－－－－｜
        ///     　　　｜　５０円↓により算定する特定器材（高線量率イリジウム）
        ///     　4:  ↑購入価格　｜
        ///     　　　｜－－－－－｜
        ///     　　　｜１０００円↓により算定する特定器材（コバルト）
        ///     　　　↑↓：四捨五入
        /// </summary>
        [Column("TOKUZAI_SBT")]
        [CustomAttribute.DefaultValue(0)]
        public int TokuzaiSbt { get; set; }

        /// <summary>
        /// 上限価格
        ///     当該特定器材の金額に酸素の上限価格の設定がされていることを表す。
        ///     　0: 下記以外
        ///     　1: 上限価格の設定がされている場合
        /// </summary>
        [Column("MAX_PRICE")]
        [CustomAttribute.DefaultValue(0)]
        public int MaxPrice { get; set; }

        /// <summary>
        /// 上限点数
        ///     当該特定器材（眼底カメラ検査用インスタントフィルム）が算定可能な上限点数を表す。上限点数の設定されない場合は「０」である。
        /// </summary>
        [Column("MAX_TEN")]
        [CustomAttribute.DefaultValue(0)]
        public int MaxTen { get; set; }

        /// <summary>
        /// 点数欄集計先識別（外来）
        ///     当該診療行為の入院外レセプトにおける点数欄への集計先を表す。
        ///     点数欄集計先識別については「別紙９」を参照。
        ///     入院外レセプトで使用不可の診療行為は「０」である。
        /// </summary>
        [Column("SYUKEI_SAKI")]
        [MaxLength(3)]
        [CustomAttribute.DefaultValue(0)]
        public string SyukeiSaki { get; set; } = string.Empty;

        /// <summary>
        /// コード表用区分－区分
        ///     当該診療行為について医科点数表の章、部、区分番号及び項番を記録する。
        ///             区分（アルファベット部）：
        ///     　点数表の区分番号のアルファベット部を記録する。
        ///     　なお、介護老人保健施設入所者に係る診療料、医療観察法、入院時食事療養、入院時生活療養及び標準負担額については
        ///     　「－」（ハイホン）を、点数表に区分設定がないものは「＊」を記録する。
        ///     章：
        ///     部：
        ///     区分番号：
        ///     枝番：
        ///     項番：
        /// </summary>
        [Column("CD_KBN")]
        [MaxLength(1)]
        public string CdKbn { get; set; } = string.Empty;

        /// <summary>
        /// コード表用区分－章
        ///     コード表用区分－区分を参照。
        /// </summary>
        [Column("CD_SYO")]
        [CustomAttribute.DefaultValue(0)]
        public int CdSyo { get; set; }

        /// <summary>
        /// コード表用区分－部
        ///     コード表用区分－区分を参照。
        /// </summary>
        [Column("CD_BU")]
        [CustomAttribute.DefaultValue(0)]
        public int CdBu { get; set; }

        /// <summary>
        /// コード表用区分－区分番号
        ///     コード表用区分－区分を参照。
        /// </summary>
        [Column("CD_KBNNO")]
        [CustomAttribute.DefaultValue(0)]
        public int CdKbnno { get; set; }

        /// <summary>
        /// コード表用区分－区分番号－枝番
        ///     コード表用区分－区分を参照。
        /// </summary>
        [Column("CD_EDANO")]
        [CustomAttribute.DefaultValue(0)]
        public int CdEdano { get; set; }

        /// <summary>
        /// コード表用区分－項番
        ///     コード表用区分－区分を参照。
        /// </summary>
        [Column("CD_KOUNO")]
        [CustomAttribute.DefaultValue(0)]
        public int CdKouno { get; set; }

        /// <summary>
        /// 告知・通知関連番号－区分
        ///     当該診療行為が準用項目の場合、準用元の医科点数表の章、部、区分番号及び項番を記録する。
        ///             区分（アルファベット部）：
        ///     　点数表の区分番号のアルファベット部を記録する。準用項目以外は未使用。
        ///     章：
        ///     部：
        ///     区分番号：
        ///     枝番：
        ///     項番：
        /// </summary>
        [Column("KOKUJI_KBN")]
        [MaxLength(1)]
        public string KokujiKbn { get; set; } = string.Empty;

        /// <summary>
        /// 告知・通知関連番号－章
        ///     告知・通知関連番号－区分を参照。
        /// </summary>
        [Column("KOKUJI_SYO")]
        [CustomAttribute.DefaultValue(0)]
        public int KokujiSyo { get; set; }

        /// <summary>
        /// 告知・通知関連番号－部
        ///     告知・通知関連番号－区分を参照。
        /// </summary>
        [Column("KOKUJI_BU")]
        [CustomAttribute.DefaultValue(0)]
        public int KokujiBu { get; set; }

        /// <summary>
        /// 告知・通知関連番号－区分番号
        ///     告知・通知関連番号－区分を参照。
        /// </summary>
        [Column("KOKUJI_KBN_NO")]
        [CustomAttribute.DefaultValue(0)]
        public int KokujiKbnNo { get; set; }

        /// <summary>
        /// 告知・通知関連番号－区分番号－枝番
        ///     告知・通知関連番号－区分を参照。
        /// </summary>
        [Column("KOKUJI_EDA_NO")]
        [CustomAttribute.DefaultValue(0)]
        public int KokujiEdaNo { get; set; }

        /// <summary>
        /// 告知・通知関連番号－項番
        ///     告知・通知関連番号－区分を参照。
        /// </summary>
        [Column("KOKUJI_KOU_NO")]
        [CustomAttribute.DefaultValue(0)]
        public int KokujiKouNo { get; set; }

        /// <summary>
        /// 告示等識別区分（１）
        ///     当該診療行為についてコンピューター運用上の取扱い（磁気媒体に記録する際の取扱い）を表す。
        ///     　1: 基本項目（告示）　※基本項目
        ///     　3: 合成項目　　　　　※基本項目
        ///     　5: 準用項目（通知）　※基本項目
        ///     　7: 加算項目　　　　　※加算項目
        ///     　9: 通則加算項目　　　※加算項目
        ///       0: 診療行為以外（薬剤、特材等）
        ///       A: 入院基本料労災乗数項目又は四肢加算（手術）項目
        /// </summary>
        [Column("KOKUJI1")]
        [CustomAttribute.DefaultValue("0")]
        [MaxLength(1)]
        public string Kokuji1 { get; set; } = string.Empty;

        /// <summary>
        /// 告示等識別区分（２）
        ///     当該診療行為について点数表上の取扱いを表す。
        ///     　1: 基本項目（告示）
        ///     　3: 合成項目
        ///     （削）5: 準用項目（通知）
        ///     　7: 加算項目（告示）
        ///     （削）9: 通則加算項目
        ///       0: 診療行為以外（薬剤、特材等）
        /// </summary>
        [Column("KOKUJI2")]
        [CustomAttribute.DefaultValue("0")]
        [MaxLength(1)]
        public string Kokuji2 { get; set; } = string.Empty;

        /// <summary>
        /// 公表順序番号
        ///     コード表用番号による順序番号を記録する。
        /// </summary>
        [Column("KOHYO_JUN")]
        [CustomAttribute.DefaultValue(0)]
        public int KohyoJun { get; set; }

        /// <summary>
        /// 個別医薬品コード
        ///     薬価基準収載医薬品コードと同様に英数12桁のコードですが、統一名収載品目の個々の商品に対して別々のコードが付与されます。
        ///     銘柄別収載品目（商品名で官報に収載されるもの）については、薬価基準収載医薬品コードと同じコードです。
        /// </summary>
        [Column("YJ_CD")]
        [CustomAttribute.DefaultValue("0")]
        [MaxLength(12)]
        public string YjCd { get; set; } = string.Empty;

        /// <summary>
        /// 薬価基準収載医薬品コード
        ///     当該医薬品に係る薬価基準収載医薬品コードを表す。
        /// </summary>
        [Column("YAKKA_CD")]
        [CustomAttribute.DefaultValue("0")]
        [MaxLength(12)]
        public string YakkaCd { get; set; } = string.Empty;

        /// <summary>
        /// 収載方式等識別
        ///     当該医薬品の薬価基準収載方式の分類を表す。
        ///     　0: 1～8以外の医薬品
        ///     　1: 日本薬局方収載医薬品（局方品）
        ///     　2: 局方品で生物学的製剤基準医薬品
        ///     　3: 局方品で生薬
        ///     　6: 生物学的製剤基準医薬品
        ///     　7: 生薬
        ///     　8: 1～7以外の統一名収載品
        /// </summary>
        [Column("SYUSAI_SBT")]
        [CustomAttribute.DefaultValue(0)]
        public int SyusaiSbt { get; set; }

        /// <summary>
        /// 商品名等関連
        ///     当該医薬品が商品名医薬品(非告示品)の場合、その統一名収載品(告示品)の医薬品コードを記録する。
        ///     なお、商品名医薬品でない場合は「0000000000」である。
        /// </summary>
        [Column("SYOHIN_KANREN")]
        [MaxLength(9)]
        public string SyohinKanren { get; set; } = string.Empty;

        /// <summary>
        /// 変更年月日
        ///     yyyymmdd
        /// </summary>
        [Column("UPD_DATE")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdDate { get; set; }

        /// <summary>
        /// 廃止年月日
        ///     yyyymmdd
        /// </summary>
        [Column("DEL_DATE")]
        [CustomAttribute.DefaultValue(0)]
        public int DelDate { get; set; }

        /// <summary>
        /// 経過措置年月日
        ///     yyyymmdd
        /// </summary>
        [Column("KEIKA_DATE")]
        [CustomAttribute.DefaultValue(0)]
        public int KeikaDate { get; set; }

        /// <summary>
        /// 労災区分
        ///     当該診療行為が労災保険で算定可能かを表す。
        ///     　0: 健保・労災において算定可能
        ///     　1: 労災のみ算定可能
        ///     　2: 健保のみ算定可能
        /// </summary>
        [Column("ROUSAI_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int RousaiKbn { get; set; }


        /// <summary>
        /// 四肢加算区分（労災）
        ///     当該診療行為の四肢に対する特例の取扱い（1.5倍・2.0倍）を表す。
        ///     　0: 1～5以外の診療行為
        ///     　1: 1.5倍又は2.0倍の対象
        ///     　2: 1.5倍のみ対象
        ///     　3: 2.0倍のみ対象
        ///     　4: 1.5倍の加算自体
        ///     　5: 2.0倍の加算自体
        /// </summary>
        [Column("SISI_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int SisiKbn { get; set; }

        /// <summary>
        /// フィルム撮影回数
        ///     フィルム1枚あたりの撮影回数
        ///     0: フィルム以外
        /// </summary>
        [Column("SHOT_CNT")]
        [CustomAttribute.DefaultValue(0)]
        public int ShotCnt { get; set; }

        /// <summary>
        /// 検索不可区分
        ///     0: 検索可
        ///     1: 検索不可
        /// </summary>
        [Column("IS_NOSEARCH")]
        [CustomAttribute.DefaultValue(0)]
        public int IsNosearch { get; set; }

        /// <summary>
        /// 紙レセ非表示区分
        ///     0: 表示
        ///     1: 非表示（摘要欄に表示しない、点数欄には表示する）
        /// </summary>
        [Column("IS_NODSP_PAPER_RECE")]
        [CustomAttribute.DefaultValue(0)]
        public int IsNodspPaperRece { get; set; }

        /// <summary>
        /// レセ非表示区分
        ///     0: 表示
        ///     1: 非表示（レセプト自体に表示しない）
        /// </summary>
        [Column("IS_NODSP_RECE")]
        [CustomAttribute.DefaultValue(0)]
        public int IsNodspRece { get; set; }

        /// <summary>
        /// 領収証非表示区分
        ///     0: 表示
        ///     1: 非表示
        /// </summary>
        [Column("IS_NODSP_RYOSYU")]
        [CustomAttribute.DefaultValue(0)]
        public int IsNodspRyosyu { get; set; }

        /// <summary>
        /// カルテ非表示区分
        ///     0: 表示
        ///     1: 非表示
        /// </summary>
        [Column("IS_NODSP_KARTE")]
        [CustomAttribute.DefaultValue(0)]
        public int IsNodspKarte { get; set; }

        /// <summary>
        /// 自費種別コード
        ///     0: 自費項目以外
        ///     >0: 自費項目
        ///     JIHI_SBT_MST.自費種別
        /// </summary>
        [Column("JIHI_SBT")]
        [CustomAttribute.DefaultValue(0)]
        public int JihiSbt { get; set; }

        /// <summary>
        /// 課税区分
        ///     0: 非課税
        ///     1: 外税
        ///     2: 内税
        /// </summary>
        [Column("KAZEI_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int KazeiKbn { get; set; }

        /// <summary>
        /// 用法区分
        ///     0: 用法以外
        ///     1: 基本用法
        ///     2: 補助用法
        /// </summary>
        [Column("YOHO_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int YohoKbn { get; set; }

        /// <summary>
        /// 一般名コード
        ///     YJ_CDの頭9桁（例外あり）
        /// </summary>
        [Column("IPN_NAME_CD")]
        [MaxLength(12)]
        public string IpnNameCd { get; set; } = string.Empty;

        /// <summary>
        /// 服用時設定-起床時
        ///     0: 服用しない
        ///     1: 服用する
        /// </summary>
        [Column("FUKUYO_RISE")]
        [CustomAttribute.DefaultValue(0)]
        public int FukuyoRise { get; set; }

        /// <summary>
        /// 服用時設定-朝
        ///     0: 服用しない
        ///     1: 服用する
        /// </summary>
        [Column("FUKUYO_MORNING")]
        [CustomAttribute.DefaultValue(0)]
        public int FukuyoMorning { get; set; }

        /// <summary>
        /// 服用時設定-昼
        ///     0: 服用しない
        ///     1: 服用する
        /// </summary>
        [Column("FUKUYO_DAYTIME")]
        [CustomAttribute.DefaultValue(0)]
        public int FukuyoDaytime { get; set; }

        /// <summary>
        /// 服用時設定-夜
        ///     0: 服用しない
        ///     1: 服用する
        /// </summary>
        [Column("FUKUYO_NIGHT")]
        [CustomAttribute.DefaultValue(0)]
        public int FukuyoNight { get; set; }

        /// <summary>
        /// 服用時設定-眠前
        ///     0: 服用しない
        ///     1: 服用する
        /// </summary>
        [Column("FUKUYO_SLEEP")]
        [CustomAttribute.DefaultValue(0)]
        public int FukuyoSleep { get; set; }

        /// <summary>
        /// 数量端数切り上げ区分
        ///     1: 端数切り上げ
        /// </summary>
        [Column("SURYO_ROUNDUP_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int SuryoRoundupKbn { get; set; }

        /// <summary>
        /// 向精神薬区分
        ///     0: 向精神薬以外
        ///     1:抗不安薬
        ///     2:睡眠薬
        ///     3:抗うつ薬
        ///     4:抗精神病薬
        /// </summary>
        [Column("KOUSEISIN_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int KouseisinKbn { get; set; }

        /// <summary>
        /// 注射薬剤種別
        ///     ※1～5は、人工腎臓に包括される
        ///     0:1～5以外
        ///     1:透析液
        ///     2:血液凝固阻止剤
        ///     3:エリスロポエチン
        ///     4:ダルベポエチン
        ///     5:生理食塩水
        /// </summary>
        [Column("CHUSYA_DRUG_SBT")]
        [CustomAttribute.DefaultValue(0)]
        public int ChusyaDrugSbt { get; set; }

        /// <summary>
        /// 検査1来院複数回算定
        ///     1: 複数回算定可
        /// </summary>
        [Column("KENSA_FUKUSU_SANTEI")]
        [CustomAttribute.DefaultValue(0)]
        public int KensaFukusuSantei { get; set; }

        /// <summary>
        /// 算定診療行為コード
        ///     算定時の診療行為コード（自己結合でデータ取得）
        /// </summary>
        [Column("SANTEI_ITEM_CD")]
        [MaxLength(10)]
        public string SanteiItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 算定外区分
        ///     0: 算定する
        ///     1: 算定外
        /// </summary>
        [Column("SANTEIGAI_KBN")]
        public int SanteigaiKbn { get; set; }

        /// <summary>
        /// 検査項目コード
        ///     KENSA_MST.KENSA_ITEM_CD
        /// </summary>
        [Column("KENSA_ITEM_CD")]
        [MaxLength(20)]
        public string KensaItemCd { get; set; } = string.Empty;

        /// <summary>
        /// 検査項目コード連番
        ///     KENSA_MST.SEQ_NO
        /// </summary>
        [Column("KENSA_ITEM_SEQ_NO")]
        [CustomAttribute.DefaultValue(0)]
        public int KensaItemSeqNo { get; set; }

        /// <summary>
        /// 連携コード１
        ///     外部システムとの連携用コード
        /// </summary>
        [Column("RENKEI_CD1")]
        [MaxLength(20)]
        public string RenkeiCd1 { get; set; } = string.Empty;

        /// <summary>
        /// 連携コード２
        ///     外部システムとの連携用コード
        /// </summary>
        [Column("RENKEI_CD2")]
        [MaxLength(20)]
        public string RenkeiCd2 { get; set; } = string.Empty;

        /// <summary>
        /// 採血料区分
        ///     当該診療行為を算定した場合に血液採取料を自動発生させるか否かを表す。
        ///     　0: 下記以外
        ///     　1: 末梢採血料（6点）
        ///     　2: 静脈採血料（12点）
        ///     　3: 動脈血採取料（40点）
        /// </summary>
        [Column("SAIKETU_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int SaiketuKbn { get; set; }

        /// <summary>
        /// コメント区分
        ///     計算時、コメントを自動で付与する場合、付与するコメントの種類を設定
        ///     0: 自動付与なし
        ///     1: 実施日
        ///     2: 前回日（840000087 :前回実施 月日）
        ///     3: 初回日（840000085 :初回実施 月日）
        ///     4: 前回日 or 初回日
        ///     5: 初回算定日（840000085 :初回算定 月日）
        ///     6: 実施日（列挙）
        ///     7: 実施日（列挙：項目名あり）
        ///     8: 実施日数（840000096 :実施日数 日）
        ///     9: 前回日 or 初回日（項目名あり）
        /// </summary>
        [Column("CMT_KBN")]
        [CustomAttribute.DefaultValue(0)]
        public int CmtKbn { get; set; }

        /// <summary>
        /// カラム位置１
        ///     記録した数字情報の編集位置を表す。
        /// </summary>
        [Column("CMT_COL1")]
        [CustomAttribute.DefaultValue(0)]
        public int CmtCol1 { get; set; }

        /// <summary>
        /// 桁数１
        ///     カラム位置から編集する数字情報の文字数を表す。
        /// </summary>
        [Column("CMT_COL_KETA1")]
        [CustomAttribute.DefaultValue(0)]
        public int CmtColKeta1 { get; set; }

        /// <summary>
        /// カラム位置２
        ///     記録した数字情報の編集位置を表す。
        /// </summary>
        [Column("CMT_COL2")]
        [CustomAttribute.DefaultValue(0)]
        public int CmtCol2 { get; set; }

        /// <summary>
        /// 桁数２
        ///     カラム位置から編集する数字情報の文字数を表す。
        /// </summary>
        [Column("CMT_COL_KETA2")]
        [CustomAttribute.DefaultValue(0)]
        public int CmtColKeta2 { get; set; }

        /// <summary>
        /// カラム位置３
        ///     記録した数字情報の編集位置を表す。
        /// </summary>
        [Column("CMT_COL3")]
        [CustomAttribute.DefaultValue(0)]
        public int CmtCol3 { get; set; }

        /// <summary>
        /// 桁数３
        ///     カラム位置から編集する数字情報の文字数を表す。
        /// </summary>
        [Column("CMT_COL_KETA3")]
        [CustomAttribute.DefaultValue(0)]
        public int CmtColKeta3 { get; set; }

        /// <summary>
        /// カラム位置４
        ///     記録した数字情報の編集位置を表す。
        /// </summary>
        [Column("CMT_COL4")]
        [CustomAttribute.DefaultValue(0)]
        public int CmtCol4 { get; set; }

        /// <summary>
        /// 桁数４
        ///     カラム位置から編集する数字情報の文字数を表す。
        /// </summary>
        [Column("CMT_COL_KETA4")]
        [CustomAttribute.DefaultValue(0)]
        public int CmtColKeta4 { get; set; }

        /// <summary>
        /// 選択式コメント識別
        ///     選択式コメントであるか否かを表す。  
        ///     0: 選択式以外のコメント 
        ///     1: 選択式コメント
        /// </summary>
        [Column("SELECT_CMT_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int SelectCmtId { get; set; }

        /// <summary>
        /// 作成日時	
        /// </summary>
        [Column("CREATE_DATE")]
        [CustomAttribute.DefaultValueSql("current_timestamp")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 作成者
        /// </summary>
        [Column(name: "CREATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int CreateId { get; set; }

        /// <summary>
        /// 作成端末	
        /// </summary>
        [Column(name: "CREATE_MACHINE")]
        [MaxLength(60)]
        public string? CreateMachine { get; set; } = string.Empty;

        /// <summary>
        /// 更新日時	
        /// </summary>
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        [Column(name: "UPDATE_ID")]
        [CustomAttribute.DefaultValue(0)]
        public int UpdateId { get; set; }

        /// <summary>
        /// 更新端末	
        /// </summary>
        [Column(name: "UPDATE_MACHINE")]
        [MaxLength(60)]
        public string? UpdateMachine { get; set; }  = string.Empty;
    }
}