namespace UseCase.Document.GetListParamTemplate;

public class HokenParamReplaceModel
{
    public HokenParamReplaceModel()
    {

    }
    /// <summary>
    /// 労災/災害区分
    /// </summary>
    public string RousaiSaigaiKbn { get; set; } = string.Empty;
    /// <summary>
    /// 労災/労働保険番号
    /// </summary>
    public string RousaiRoudouHokenNo { get; set; } = string.Empty;
    /// <summary>
    /// 労災/年金証書番号
    /// </summary>
    public string RousaiNenkinNo { get; set; } = string.Empty;
    /// <summary>
    /// 労災/健康管理手帳番号
    /// </summary>
    public string RousaiKenkoKanriNo { get; set; } = string.Empty;
    /// <summary>
    /// 労災/労働局
    /// </summary>
    public string RousaiRoudouKyoku { get; set; } = string.Empty;
    /// <summary>
    /// 労災/監督署
    /// </summary>
    public string RousaiKantoku { get; set; } = string.Empty;
    /// <summary>
    /// 労災/傷病コード
    /// </summary>
    public string RousaiShyobyoCode { get; set; } = string.Empty;
    /// <summary>
    /// 労災/事業名称
    /// </summary>
    public string RousaiJigyouName { get; set; } = string.Empty;
    /// <summary>
    /// 労災/事業所住所
    /// </summary>
    public string RousaiJigyuoAddress { get; set; } = string.Empty;
    /// <summary>
    /// 労災/傷病年月日(西暦/)
    /// </summary>
    public string RousaiShyobyoDateWest { get; set; } = string.Empty;
    /// <summary>
    /// 労災/傷病年月日(西暦K)
    /// </summary>
    public string RousaiShyobyoDateWestK { get; set; } = string.Empty;
    /// <summary>
    /// 労災/傷病年月日(和暦/)
    /// </summary>
    public string RousaiShyobyoDateJapan { get; set; } = string.Empty;
    /// <summary>
    /// 労災/傷病年月日(和暦K)
    /// </summary>
    public string RousaiShyobyoDateJapanK { get; set; } = string.Empty;
    /// <summary>
    /// 労災/療養期間S(西暦/)
    /// </summary>
    public string RousaiRyouyouPeriodWestS { get; set; } = string.Empty;
    /// <summary>
    /// 労災/療養期間S(西暦K)
    /// </summary>
    public string RousaiRyouyouPeriodWestKS { get; set; } = string.Empty;
    /// <summary>
    /// 労災/療養期間S(和暦/)
    /// </summary>
    public string RousaiRyouyouPeriodJapanS { get; set; } = string.Empty;
    /// <summary>
    /// 労災/療養期間S(和暦K)
    /// </summary>
    public string RousaiRyouyouPeriodJapanKS { get; set; } = string.Empty;
    /// <summary>
    /// 労災/療養期間E(西暦/)
    /// </summary>
    public string RousaiRyouyouPeriodWestE { get; set; } = string.Empty;
    /// <summary>
    /// 労災/療養期間E(西暦K)
    /// </summary>
    public string RousaiRyouyouPeriodWestKE { get; set; } = string.Empty;
    /// <summary>
    /// 労災/療養期間E(和暦/)
    /// </summary>
    public string RousaiRyouyouPeriodJapanE { get; set; } = string.Empty;
    /// <summary>
    /// 労災/療養期間E(和暦K)
    /// </summary>
    public string RousaiRyouyouPeriodJapanKE { get; set; } = string.Empty;
    /// <summary>
    /// 労災/初診日(西暦/)
    /// </summary>
    public string RousaiSinDateWest { get; set; } = string.Empty;
    /// <summary>
    /// 労災/初診日(西暦K)
    /// </summary>
    public string RousaiSinDateWestK { get; set; } = string.Empty;
    /// <summary>
    /// 労災/初診日(和暦/)
    /// </summary>
    public string RousaiSinDateJapan { get; set; } = string.Empty;
    /// <summary>
    /// 労災/初診日(和暦K)
    /// </summary>
    public string RousaiSinDateJapanK { get; set; } = string.Empty;

    /// <summary>
    /// 自賠/保険会社名
    /// </summary>
    public string JibaiHokenCompanyName { get; set; } = string.Empty;
    /// <summary>
    /// 自賠/保険担当者
    /// </summary>
    public string JibaiHokenTanto { get; set; } = string.Empty;
    /// <summary>
    /// 自賠/保険連絡先
    /// </summary>
    public string JibaiHokenContact { get; set; } = string.Empty;
    /// <summary>
    /// 自賠/障害年月日(西暦/)
    /// </summary>
    public string JibaiFailDateWest { get; set; } = string.Empty;
    /// <summary>
    /// 自賠/障害年月日(和暦/)
    /// </summary>
    public string JibaiFailDateJapan { get; set; } = string.Empty;
    /// <summary>
    /// 自賠/障害年月日(和暦K)
    /// </summary>
    public string JibaiFailDateJapanK { get; set; } = string.Empty;
    /// <summary>
    /// 自賠/療養期間S(西暦/)
    /// </summary>
    public string JibaiRyouyouPeriodWestS { get; set; } = string.Empty;
    /// <summary>
    /// 自賠/療養期間S(和歴/)
    /// </summary>
    public string JibaiRyouyouPeriodJapanS { get; set; } = string.Empty;
    /// <summary>
    /// 自賠/療養期間S(和暦K)
    /// </summary>
    public string JibaiiRyouyouPeriodJapanKS { get; set; } = string.Empty;
    /// <summary>
    /// 自賠/療養期間E(西暦/)
    /// </summary>
    public string JibaiRyouyouPeriodWestE { get; set; } = string.Empty;
    /// <summary>
    /// 自賠/療養期間E(和歴/)
    /// </summary>
    public string JibaiRyouyouPeriodJapanE { get; set; } = string.Empty;
    /// <summary>
    /// 自賠/療養期間E(和暦K)
    /// </summary>
    public string JibaiRyouyouPeriodJapanKE { get; set; } = string.Empty;
    /// <summary>
    /// 自賠/受傷日(西暦/)
    /// </summary>
    public string JibaiJushouDateWest { get; set; } = string.Empty;
    /// <summary>
    /// 自賠/受傷日(西暦K)
    /// </summary>
    public string JibaiJushouDateWestK { get; set; } = string.Empty;
    /// <summary>
    /// 自賠/受傷日(和暦/)
    /// </summary>
    public string JibaiJushouDateJapan { get; set; } = string.Empty;
    /// <summary>
    /// 自賠/受傷日(和暦K)
    /// </summary>
    public string JibaiJushouDateJapanK { get; set; } = string.Empty;
    /// <summary>
    /// 自賠/初診日(西暦/)
    /// </summary>
    public string JibaiSinDateWest { get; set; } = string.Empty;
    /// <summary>
    /// 自賠/初診日(西暦K)
    /// </summary>
    public string JibaiSinDateWestK { get; set; } = string.Empty;
    /// <summary>
    /// 自賠/初診日(和暦/)
    /// </summary>
    public string JibaiSinDateJapan { get; set; } = string.Empty;
    /// <summary>
    /// 自賠/初診日(和暦K)
    /// </summary>
    public string JibaiSinDateJapanK { get; set; } = string.Empty;

    /// <summary>
    /// 保険/保険者番号
    /// </summary>
    public string HokenNo { get; set; } = string.Empty;
    /// <summary>
    /// 保険/被保険者証記号
    /// </summary>
    public string HokenKigo { get; set; } = string.Empty;
    /// <summary>
    /// 保険/被保険者証番号
    /// </summary>
    public string HokenBango { get; set; } = string.Empty;
    /// <summary>
    /// 保険/被保険者証枝番
    /// </summary>
    public string EdaNo { get; set; } = string.Empty;
    /// <summary>
    /// 保険/本人家族
    /// </summary>
    public string HokenHonki { get; set; } = string.Empty;
    /// <summary>
    /// 保険/高額区分
    /// </summary>
    public string HokenKogakuKbn { get; set; } = string.Empty;
    /// <summary>
    /// 保険/マル長
    /// </summary>
    public string HokenMarucho { get; set; } = string.Empty;
    /// <summary>
    /// 保険/有効期限S(西暦/)
    /// </summary>
    public string HokenPeriodWestS { get; set; } = string.Empty;
    /// <summary>
    /// 保険/有効期限S(西暦/)
    /// </summary>
    public string HokenPeriodWestKS { get; set; } = string.Empty;
    /// <summary>
    /// 保険/有効期限S(和暦/)
    /// </summary>
    public string HokenPeriodJapanS { get; set; } = string.Empty;
    /// <summary>
    /// 保険/有効期限S(和暦K)
    /// </summary>
    public string HokenPeriodJapanKS { get; set; } = string.Empty;
    /// <summary>
    /// 保険/有効期限E(西暦/)
    /// </summary>
    public string HokenPeriodWestE { get; set; } = string.Empty;
    /// <summary>
    /// 保険/有効期限E(西暦K)
    /// </summary>
    public string HokenPeriodWestKE { get; set; } = string.Empty;
    /// <summary>
    /// 保険/有効期限E(和暦/)
    /// </summary>
    public string HokenPeriodJapanE { get; set; } = string.Empty;
    /// <summary>
    /// 保険/有効期限E(和暦K)
    /// </summary>
    public string HokenPeriodJapanKE { get; set; } = string.Empty;
    /// <summary>
    /// 保険/国保減免
    /// </summary>
    public string HokenKokuhoGenmen { get; set; } = string.Empty;
    /// <summary>
    /// 保険/職務上区分
    /// </summary>
    public string HokenSenin { get; set; } = string.Empty;
    /// <summary>
    /// 保険/特記事項１
    /// </summary>
    public string HokenSpecialNote1 { get; set; } = string.Empty;
    /// <summary>
    /// 保険/特記事項２
    /// </summary>
    public string HokenSpecialNote2 { get; set; } = string.Empty;
    /// <summary>
    /// 保険/特記事項３
    /// </summary>
    public string HokenSpecialNote3 { get; set; } = string.Empty;
    /// <summary>
    /// 保険/特記事項４
    /// </summary>
    public string HokenSpecialNote4 { get; set; } = string.Empty;
    /// <summary>
    /// 保険/特記事項５
    /// </summary>
    public string HokenSpecialNote5 { get; set; } = string.Empty;

    /// <summary>
    /// 公１/負担者番号
    /// </summary>
    public string Kohi1FutanshaNo { get; set; } = string.Empty;
    /// <summary>
    /// 公１/受給者番号
    /// </summary>
    public string Kohi1JukyuushaNo { get; set; } = string.Empty;
    /// <summary>
    /// 公１/交付番号
    /// </summary>
    public string Kohi1KoufuNo { get; set; } = string.Empty;
    /// <summary>
    /// 公１/有効期限S(西暦/)
    /// </summary>
    public string Kohi1PeriodWestS { get; set; } = string.Empty;
    /// <summary>
    /// 公１/有効期限S(西暦K)
    /// </summary>
    public string Kohi1PeriodWestKS { get; set; } = string.Empty;
    /// <summary>
    /// 公１/有効期限S(和暦/)
    /// </summary>
    public string Kohi1PeriodJapanS { get; set; } = string.Empty;
    /// <summary>
    /// 公１/有効期限S(和暦K)
    /// </summary>
    public string Kohi1PeriodJapanKS { get; set; } = string.Empty;
    /// <summary>
    /// 公１/有効期限E(西暦/)
    /// </summary>
    public string Kohi1PeriodWestE { get; set; } = string.Empty;
    /// <summary>
    /// 公１/有効期限E(西暦K)
    /// </summary>
    public string Kohi1PeriodWestKE { get; set; } = string.Empty;
    /// <summary>
    /// 公１/有効期限E(和暦/)
    /// </summary>
    public string Kohi1PeriodJapanE { get; set; } = string.Empty;
    /// <summary>
    /// 公１/有効期限E(和暦K)
    /// </summary>
    public string Kohi1PeriodJapanKE { get; set; } = string.Empty;
    /// <summary>
    /// 公１/取得日(西暦/)
    /// </summary>
    public string Kohi1ShutokuWest { get; set; } = string.Empty;
    /// <summary>
    /// 公１/取得日(西暦K)
    /// </summary>
    public string Kohi1ShutokuWestK { get; set; } = string.Empty;
    /// <summary>
    /// 公１/取得日(和暦/)
    /// </summary>
    public string Kohi1ShutokuJapan { get; set; } = string.Empty;
    /// <summary>
    /// 公１/取得日(和暦K)
    /// </summary>
    public string Kohi1ShutokuJapanK { get; set; } = string.Empty;
    /// <summary>
    /// 公１/交付日(西暦/)
    /// </summary>
    public string Kohi1KoufuWest { get; set; } = string.Empty;
    /// <summary>
    /// 公１/交付日(西暦K)
    /// </summary>
    public string Kohi1KoufuWestK { get; set; } = string.Empty;
    /// <summary>
    /// 公１/交付日(和暦/)
    /// </summary>
    public string Kohi1KoufuJapan { get; set; } = string.Empty;
    /// <summary>
    /// 公１/交付日(和暦K)
    /// </summary>
    public string Kohi1KoufuJapanK { get; set; } = string.Empty;

    /// <summary>
    /// 公２/負担者番号
    /// </summary>
    public string Kohi2FutanshaNo { get; set; } = string.Empty;
    /// <summary>
    /// 公２/受給者番号
    /// </summary>
    public string Kohi2JukyuushaNo { get; set; } = string.Empty;
    /// <summary>
    /// 公２/交付番号
    /// </summary>
    public string Kohi2KoufuNo { get; set; } = string.Empty;
    /// <summary>
    /// 公２/有効期限S(西暦/)
    /// </summary>
    public string Kohi2PeriodWestS { get; set; } = string.Empty;
    /// <summary>
    /// 公２/有効期限S(西暦K)
    /// </summary>
    public string Kohi2PeriodWestKS { get; set; } = string.Empty;
    /// <summary>
    /// 公２/有効期限S(和暦/)
    /// </summary>
    public string Kohi2PeriodJapanS { get; set; } = string.Empty;
    /// <summary>
    /// 公２/有効期限S(和暦K)
    /// </summary>
    public string Kohi2PeriodJapanKS { get; set; } = string.Empty;
    /// <summary>
    /// 公２/有効期限E(西暦/)
    /// </summary>
    public string Kohi2PeriodWestE { get; set; } = string.Empty;
    /// <summary>
    /// 公２/有効期限E(西暦K)
    /// </summary>
    public string Kohi2PeriodWestKE { get; set; } = string.Empty;
    /// <summary>
    /// 公２/有効期限E(和暦/)
    /// </summary>
    public string Kohi2PeriodJapanE { get; set; } = string.Empty;
    /// <summary>
    /// 公２/有効期限E(和暦K)
    /// </summary>
    public string Kohi2PeriodJapanKE { get; set; } = string.Empty;
    /// <summary>
    /// 公２/取得日(西暦/)
    /// </summary>
    public string Kohi2ShutokuWest { get; set; } = string.Empty;
    /// <summary>
    /// 公２/取得日(西暦K)
    /// </summary>
    public string Kohi2ShutokuWestK { get; set; } = string.Empty;
    /// <summary>
    /// 公２/取得日(和暦/)
    /// </summary>
    public string Kohi2ShutokuJapan { get; set; } = string.Empty;
    /// <summary>
    /// 公２/取得日(和暦K)
    /// </summary>
    public string Kohi2ShutokuJapanK { get; set; } = string.Empty;
    /// <summary>
    /// 公２/交付日(西暦/)
    /// </summary>
    public string Kohi2KoufuWest { get; set; } = string.Empty;
    /// <summary>
    /// 公２/交付日(西暦K)
    /// </summary>
    public string Kohi2KoufuWestK { get; set; } = string.Empty;
    /// <summary>
    /// 公２/交付日(和暦/)
    /// </summary>
    public string Kohi2KoufuJapan { get; set; } = string.Empty;
    /// <summary>
    /// 公２/交付日(和暦K)
    /// </summary>
    public string Kohi2KoufuJapanK { get; set; } = string.Empty;

    /// <summary>
    /// 公３/負担者番号
    /// </summary>
    public string Kohi3FutanshaNo { get; set; } = string.Empty;
    /// <summary>
    /// 公３/受給者番号
    /// </summary>
    public string Kohi3JukyuushaNo { get; set; } = string.Empty;
    /// <summary>
    /// 公３/交付番号
    /// </summary>
    public string Kohi3KoufuNo { get; set; } = string.Empty;
    /// <summary>
    /// 公３/有効期限S(西暦/)
    /// </summary>
    public string Kohi3PeriodWestS { get; set; } = string.Empty;
    /// <summary>
    /// 公３/有効期限S(西暦K)
    /// </summary>
    public string Kohi3PeriodWestKS { get; set; } = string.Empty;
    /// <summary>
    /// 公３/有効期限S(和暦/)
    /// </summary>
    public string Kohi3PeriodJapanS { get; set; } = string.Empty;
    /// <summary>
    /// 公３/有効期限S(和暦K)
    /// </summary>
    public string Kohi3PeriodJapanKS { get; set; } = string.Empty;
    /// <summary>
    /// 公３/有効期限E(西暦/)
    /// </summary>
    public string Kohi3PeriodWestE { get; set; } = string.Empty;
    /// <summary>
    /// 公３/有効期限E(西暦K)
    /// </summary>
    public string Kohi3PeriodWestKE { get; set; } = string.Empty;
    /// <summary>
    /// 公３/有効期限E(和暦/)
    /// </summary>
    public string Kohi3PeriodJapanE { get; set; } = string.Empty;
    /// <summary>
    /// 公３/有効期限E(和暦K)
    /// </summary>
    public string Kohi3PeriodJapanKE { get; set; } = string.Empty;
    /// <summary>
    /// 公３/取得日(西暦/)
    /// </summary>
    public string Kohi3ShutokuWest { get; set; } = string.Empty;
    /// <summary>
    /// 公３/取得日(西暦K)
    /// </summary>
    public string Kohi3ShutokuWestK { get; set; } = string.Empty;
    /// <summary>
    /// 公３/取得日(和暦/)
    /// </summary>
    public string Kohi3ShutokuJapan { get; set; } = string.Empty;
    /// <summary>
    /// 公３/取得日(和暦K)
    /// </summary>
    public string Kohi3ShutokuJapanK { get; set; } = string.Empty;
    /// <summary>
    /// 公３/交付日(西暦/)
    /// </summary>
    public string Kohi3KoufuWest { get; set; } = string.Empty;
    /// <summary>
    /// 公３/交付日(西暦K)
    /// </summary>
    public string Kohi3KoufuWestK { get; set; } = string.Empty;
    /// <summary>
    /// 公３/交付日(和暦/)
    /// </summary>
    public string Kohi3KoufuJapan { get; set; } = string.Empty;
    /// <summary>
    /// 公３/交付日(和暦K)
    /// </summary>
    public string Kohi3KoufuJapanK { get; set; } = string.Empty;

    /// <summary>
    /// 公４/負担者番号
    /// </summary>
    public string Kohi4FutanshaNo { get; set; } = string.Empty;
    /// <summary>
    /// 公４/受給者番号
    /// </summary>
    public string Kohi4JukyuushaNo { get; set; } = string.Empty;
    /// <summary>
    /// 公４/交付番号
    /// </summary>
    public string Kohi4KoufuNo { get; set; } = string.Empty;
    /// <summary>
    /// 公４/有効期限S(西暦/)
    /// </summary>
    public string Kohi4PeriodWestS { get; set; } = string.Empty;
    /// <summary>
    /// 公４/有効期限S(西暦K)
    /// </summary>
    public string Kohi4PeriodWestKS { get; set; } = string.Empty;
    /// <summary>
    /// 公４/有効期限S(和暦/)
    /// </summary>
    public string Kohi4PeriodJapanS { get; set; } = string.Empty;
    /// <summary>
    /// 公４/有効期限S(和暦K)
    /// </summary>
    public string Kohi4PeriodJapanKS { get; set; } = string.Empty;
    /// <summary>
    /// 公４/有効期限E(西暦/)
    /// </summary>
    public string Kohi4PeriodWestE { get; set; } = string.Empty;
    /// <summary>
    /// 公４/有効期限E(西暦K)
    /// </summary>
    public string Kohi4PeriodWestKE { get; set; } = string.Empty;
    /// <summary>
    /// 公４/有効期限E(和暦/)
    /// </summary>
    public string Kohi4PeriodJapanE { get; set; } = string.Empty;
    /// <summary>
    /// 公４/有効期限E(和暦K)
    /// </summary>
    public string Kohi4PeriodJapanKE { get; set; } = string.Empty;
    /// <summary>
    /// 公４/取得日(西暦/)
    /// </summary>
    public string Kohi4ShutokuWest { get; set; } = string.Empty;
    /// <summary>
    /// 公４/取得日(西暦K)
    /// </summary>
    public string Kohi4ShutokuWestK { get; set; } = string.Empty;
    /// <summary>
    /// 公４/取得日(和暦/)
    /// </summary>
    public string Kohi4ShutokuJapan { get; set; } = string.Empty;
    /// <summary>
    /// 公４/取得日(和暦K)
    /// </summary>
    public string Kohi4ShutokuJapanK { get; set; } = string.Empty;
    /// <summary>
    /// 公４/交付日(西暦/)
    /// </summary>
    public string Kohi4KoufuWest { get; set; } = string.Empty;
    /// <summary>
    /// 公４/交付日(西暦K)
    /// </summary>
    public string Kohi4KoufuWestK { get; set; } = string.Empty;
    /// <summary>
    /// 公４/交付日(和暦/)
    /// </summary>
    public string Kohi4KoufuJapan { get; set; } = string.Empty;
    /// <summary>
    /// 公４/交付日(和暦K)
    /// </summary>
    public string Kohi4KoufuJapanK { get; set; } = string.Empty;
}
