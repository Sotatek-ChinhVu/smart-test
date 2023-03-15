namespace Helper.Common;

public class ReceSyukeisaki
{
    #region 初診

    /// <summary>
    /// 初診
    /// </summary>
    public const string Syosin = "1100";
    /// <summary>
    /// 初診-時間内
    /// </summary>
    public const string SyosinJikanNai = "1110";
    /// <summary>
    /// 初診-時間外
    /// </summary>
    public const string SyosinJikanGai = "1120";
    /// <summary>
    /// 初診-休日
    /// </summary>
    public const string SyosinKyujitu = "1130";
    /// <summary>
    /// 初診-深夜
    /// </summary>
    public const string SyosinSinya = "1140";
    /// <summary>
    /// 初診-夜間早朝
    /// </summary>
    public const string SyosinYasou = "1150";
    /// <summary>
    /// 初診-その他加算等
    /// </summary>
    public const string SyosinSonota = "1189";
    /// <summary>
    /// 初診-コメント
    /// </summary>
    public const string SyosinComment = "1190";
    #endregion

    #region 再診
    /// <summary>
    /// 再診
    /// </summary>
    public const string Saisin = "1200";
    /// <summary>
    /// 再診-外来管理加算
    /// </summary>
    public const string SaisinGairai = "1220";
    public const string SaisinGairaiRousai = "1221";
    /// <summary>
    /// 再診-時間外
    /// </summary>
    public const string SaisinJikangai = "1230";
    /// <summary>
    /// 再診-休日
    /// </summary>
    public const string SaisinKyujitu = "1240";
    /// <summary>
    /// 再診-深夜
    /// </summary>
    public const string SaisinSinya = "1250";
    /// <summary>
    /// 再診-コメント
    /// </summary>
    public const string SaisinComment = "1290";
    #endregion

    /// <summary>
    /// 初再診その他
    /// </summary>
    public const string SyosaiSonota = "1900";

    #region 医学管理
    /// <summary>
    /// 医学管理
    /// </summary>
    public const string Igaku = "1300";
    /// <summary>
    /// 保険指導（アフターケア）
    /// </summary>
    public const string HokenSido = "1300";
    /// <summary>
    /// 診察その他（自賠）
    /// </summary>
    public const string IgakuSonota = "1900";
    #endregion

    #region 在宅
    /// <summary>
    /// 在宅-往診
    /// </summary>
    public const string ZaiOusin = "1400";
    /// <summary>
    /// 在宅-夜間
    /// </summary>
    public const string ZaiYakan = "1410";
    /// <summary>
    /// 在宅-深夜・緊急
    /// </summary>
    public const string ZaiSinya = "1420";
    /// <summary>
    /// 在宅-在宅患者訪問診療料
    /// </summary>
    public const string ZaiHoumon = "1430";
    /// <summary>
    /// 在宅-その他
    /// </summary>
    public const string ZaiSonota = "1440";
    /// <summary>
    /// 在宅調整項目
    /// </summary>
    public const string ZaiCyosei = "1441";
    /// <summary>
    /// 在宅-薬剤
    /// </summary>
    public const string ZaiYakuzai = "1450";
    #endregion

    #region 投薬
    /// <summary>
    /// 投薬-内服薬剤
    /// </summary>
    public const string TouyakuNaiYakuzai = "2100";
    /// <summary>
    /// 投薬-内服調剤
    /// </summary>
    public const string TouyakuNaiCyozai = "2110";
    /// <summary>
    /// 投薬-頓服
    /// </summary>
    public const string TouyakuTon = "2200";
    /// <summary>
    /// 投薬-外用薬剤
    /// </summary>
    public const string TouyakuGaiYakuzai = "2300";
    /// <summary>
    /// 投薬-外用調剤
    /// </summary>
    public const string TouyakuGaiCyozai = "2310";
    /// <summary>
    /// 投薬-処方
    /// </summary>
    public const string TouyakuSyoho = "2500";
    /// <summary>
    /// 投薬-麻毒
    /// </summary>
    public const string TouyakuMadoku = "2600";
    /// <summary>
    /// 投薬-調基
    /// </summary>
    public const string TouyakuChoKi = "2700";
    /// <summary>
    /// 投薬-自己注射
    /// </summary>
    public const string TouyakuJikoCyu = "2900";
    #endregion

    #region 注射
    /// <summary>
    /// 注射-皮下筋肉
    /// </summary>
    public const string ChusyaHikakin = "3110";
    /// <summary>
    /// 注射-静脈
    /// </summary>
    public const string ChusyaJyoumyaku = "3210";
    /// <summary>
    /// 注射-その他
    /// </summary>
    public const string ChusyaSonota = "3310";
    /// <summary>
    /// 注射-薬剤（自賠労災準拠）
    /// </summary>
    public const string ChusyaYakuzai = "3900";
    #endregion

    #region 処置
    /// <summary>
    /// 処置
    /// </summary>
    public const string Syoti = "4000";
    /// <summary>
    /// 処置-薬剤
    /// </summary>
    public const string SyotiYakuzai = "4010";
    #endregion

    #region 手術
    /// <summary>
    /// 手術・麻酔
    /// </summary>
    public const string OpeMasui = "5000";
    /// <summary>
    /// 手術・麻酔-薬剤
    /// </summary>
    public const string OpeYakuzai = "5010";
    #endregion

    #region 検査・病理
    /// <summary>
    /// 検査・病理
    /// </summary>
    public const string Kensa = "6000";
    /// <summary>
    /// 検査・病理-薬剤
    /// </summary>
    public const string Kensayakuzai = "6010";
    #endregion

    #region 画像診断
    /// <summary>
    /// 画像
    /// </summary>
    public const string Gazo = "7000";
    /// <summary>
    /// 画像-薬剤
    /// </summary>
    public const string GazoYakuzai = "7010";
    #endregion

    #region その他
    /// <summary>
    /// その他-処方箋
    /// </summary>
    public const string SonotaSyohoSen = "8000";
    /// <summary>
    /// その他-処方箋コメント
    /// </summary>
    public const string SonotaSyohoSenComment = "8001";
    /// <summary>
    /// その他（アフターケア）
    /// </summary>
    public const string Sonota = "8000";
    /// <summary>
    /// その他-その他
    /// </summary>
    public const string SonotaSonota = "8010";
    /// <summary>
    /// その他-リハビリテーション等（自賠労災準拠）
    /// </summary>
    public const string SonotaRiha = "8010";
    /// <summary>
    /// その他-薬剤
    /// </summary>
    public const string SonotaYakuzai = "8020";
    #endregion

    #region 金額
    /// <summary>
    /// （金額）初診
    /// </summary>
    public const string EnSyosin = "A110";
    /// <summary>
    /// （金額）再診
    /// </summary>
    public const string EnSaisin = "A120";

    /// <summary>
    /// （金額）指導
    /// </summary>
    public const string EnSido = "A130";
    /// <summary>
    /// （金額）救急医療管理加算
    /// </summary>
    public const string EnKyukyu = "A131";

    /// <summary>
    /// （金額）その他
    /// </summary>
    public const string EnSonota = "A180";

    /// <summary>
    /// （金額）診断書料
    /// </summary>
    public const string SindanSyo = "ZZZ0";
    /// <summary>
    /// （金額）明細書料
    /// </summary>
    public const string MeisaiSyo = "ZZZ1";
    #endregion

    /// <summary>
    /// 自費
    /// </summary>
    public const string Jihi = "9999";
}
