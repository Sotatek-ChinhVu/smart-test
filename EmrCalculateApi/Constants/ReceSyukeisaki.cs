namespace EmrCalculateApi.Constants
{
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

    /// <summary>
    /// レセプト電算レコード識別
    /// </summary>
    public class ReceRecId
    {
        /// <summary>
        /// 診療行為
        /// </summary>
        public const string Sinryo = "SI";
        public const string SinryoRo = "RI";  // 労災よう
        /// <summary>
        /// 薬剤
        /// </summary>
        public const string Yakuzai = "IY";
        /// <summary>
        /// 特材
        /// </summary>
        public const string Tokutei = "TO";
        /// <summary>
        /// コメント
        /// </summary>
        public const string Comment = "CO";
        /// <summary>
        /// 自費
        /// </summary>
        public const string Jihi = "JI";
    }

    /// <summary>
    /// レセプト電算診療識別
    /// </summary>
    public class ReceSinId
    {
        /// <summary>
        /// 全体に係る識別コード（ヘッダー）
        /// </summary>
        public const int Header = 1;

        /// <summary>
        /// 初再診
        /// </summary>
        public const int SyosaiMin = 11;
        public const int SyosaiMax = 12;

        /// <summary>
        /// 初診
        /// </summary>
        public const int Syosin = 11;

        /// <summary>
        /// 再診
        /// </summary>
        public const int Saisin = 12;

        /// <summary>
        /// 初再診その他
        /// </summary>
        public const int SyosaiSonota = 19;

        /// <summary>
        /// 医学管理
        /// </summary>
        public const int IgakuMin = 13;
        public const int IgakuMax = 13;

        public const int Igaku = 13;

        /// <summary>
        /// 在宅
        /// </summary>
        public const int ZaitakuMin = 14;
        public const int ZaitakuMax = 14;

        public const int Zaitaku = 14;

        /// <summary>
        /// 投薬
        /// </summary>
        public const int TouyakuMin = 21;
        public const int TouyakuMax = 29;

        /// <summary>
        /// 投薬-内服
        /// </summary>
        public const int Naifuku = 21;
        /// <summary>
        /// 投薬-頓服
        /// </summary>
        public const int Tonpuku = 22;
        /// <summary>
        /// 投薬-外用
        /// </summary>
        public const int Gaiyo = 23;
        /// <summary>
        /// 投薬-調剤
        /// </summary>
        public const int Cyozai = 24;
        /// <summary>
        /// 投薬-処方
        /// </summary>
        public const int Syoho = 25;
        /// <summary>
        /// 投薬-麻毒
        /// </summary>
        public const int Madoku = 26;
        /// <summary>
        /// 投薬-調基
        /// </summary>
        public const int Cyoki = 27;
        /// <summary>
        /// 投薬-処方
        /// </summary>
        public const int SyohoSonota = 28;
        /// <summary>
        /// 自己注射
        /// </summary>
        public const int JikoCyu = 29;

        public const int ChusyaMin = 30;
        public const int ChusyaMax = 39;

        /// <summary>
        /// 注射-皮下筋肉内
        /// </summary>
        public const int Hikakin = 31;
        /// <summary>
        /// 注射-静脈内
        /// </summary>
        public const int Jyomyaku = 32;
        /// <summary>
        /// 注射-その他
        /// </summary>
        public const int ChusyaSonota = 33;

        public const int SyotiMin = 40;
        public const int SyotiMax = 49;

        /// <summary>
        /// 処置
        /// </summary>
        public const int Syoti = 40;

        public const int SyujyutuMin = 50;
        public const int SyujyutuMax = 59;

        /// <summary>
        /// 手術
        /// </summary>
        public const int Syujyutu = 50;

        /// <summary>
        /// 麻酔
        /// </summary>
        public const int Masui = 54;

        public const int KensaMin = 60;
        public const int KensaMax = 69;

        /// <summary>
        /// 検査
        /// </summary>
        public const int Kensa = 60;

        public const int GazoMin = 70;
        public const int GazoMax = 79;
        /// <summary>
        /// 画像診断
        /// </summary>
        public const int Gazo = 70;

        public const int SonotaMin = 80;
        public const int SonotaMax = 89;

        /// <summary>
        /// その他
        /// </summary>
        public const int Sonota = 80;

        /// <summary>
        /// 全体に係る識別コード（フッター）
        /// </summary>
        public const int Footer = 99;

        /// <summary>
        /// 自費
        /// </summary>
        public const int Jihi = 96;
    }

    /// <summary>
    /// 診療行為コード
    /// </summary>
    public class ReceKouiKbn
    {
        /// <summary>
        /// 初診
        /// </summary>
        public const int Syosin = 110;

        /// <summary>
        /// 再診
        /// </summary>
        public const int Saisin = 120;

        /// <summary>
        /// 医学管理
        /// </summary>
        public const int Igaku = 130;

        /// <summary>
        /// 在宅
        /// </summary>
        public const int Zaitaku = 140;

        /// <summary>
        /// 初再診その他
        /// </summary>
        public const int SyosaiSonota = 190;

        /// <summary>
        /// 投薬
        /// </summary>
        public const int Touyaku = 200;
        public const int TouyakuMin = 200;
        public const int TouyakuMax = 299;

        /// <summary>
        /// 内服
        /// </summary>
        public const int Naifuku = 210;
        /// <summary>
        /// 頓服
        /// </summary>
        public const int Tonpuku = 220;
        /// <summary>
        /// 外用
        /// </summary>
        public const int Gaiyo = 230;
        /// <summary>
        /// 自己注射
        /// </summary>
        public const int JikoCyu = 240;

        /// <summary>
        /// 注射
        /// </summary>
        public const int Cyusya = 300;
        /// <summary>
        /// 皮下筋肉内
        /// </summary>
        public const int Hikakin = 310;
        /// <summary>
        /// 静脈内
        /// </summary>
        public const int Jyomyaku = 320;
        /// <summary>
        /// 点滴
        /// </summary>
        public const int Tenteki = 330;
        /// <summary>
        /// 注射その他
        /// </summary>
        public const int ChusyaSonota = 340;

        /// <summary>
        /// 処置
        /// </summary>
        public const int Syoti = 400;

        /// <summary>
        /// 手術
        /// </summary>
        public const int Syujyutu = 500;

        /// <summary>
        /// 検査
        /// </summary>
        public const int Kensa = 600;

        /// <summary>
        /// 画像
        /// </summary>
        public const int Gazo = 700;

        /// <summary>
        /// その他
        /// </summary>
        public const int Sonota = 800;

        /// <summary>
        /// 自費
        /// </summary>
        public const int Jihi = 900;
    }

    /// <summary>
    /// 診療行為コード
    /// </summary>
    public class OdrKouiKbnConst
    {
        /// <summary>
        /// 未設定
        /// </summary>
        public const int None = 0;
        /// <summary>
        /// 初再診
        /// </summary>
        public const int Syosai = 10;
        /// <summary>
        /// 初診料
        /// </summary>
        public const int Syosin = 11;
        /// <summary>
        /// 再診料
        /// </summary>
        public const int Saisin = 12;
        /// <summary>
        /// 初再診その他
        /// </summary>
        public const int SyosaiSonota = 19;
        /// <summary>
        /// 医学管理
        /// </summary>
        public const int Igaku = 13;
        /// <summary>
        /// 在宅
        /// </summary>
        public const int Zaitaku = 14;
        /// <summary>
        /// 投薬
        /// </summary>
        public const int Touyaku = 20;
        public const int TouyakuMin = 20;
        public const int TouyakuMax = 29;

        /// <summary>
        /// 内服
        /// </summary>
        public const int Naifuku = 21;
        /// <summary>
        /// 頓服
        /// </summary>
        public const int Tonpuku = 22;
        /// <summary>
        /// 外用
        /// </summary>
        public const int Gaiyo = 23;
        /// <summary>
        /// 調剤料
        /// </summary>
        public const int Cyozai = 24;
        /// <summary>
        /// 処方料
        /// </summary>
        public const int Syoho = 25;
        /// <summary>
        /// 麻毒加算
        /// </summary>
        public const int Madoku = 26;
        /// <summary>
        /// 調剤技術基本料
        /// </summary>
        public const int Cyoki = 27;
        /// <summary>
        /// 自己注射
        /// </summary>
        public const int JikoCyu = 28;
        /// <summary>
        /// 注射
        /// </summary>
        public const int Chusya = 30;
        public const int ChusyaMin = 30;
        public const int ChusyaMax = 39;

        /// <summary>
        /// 皮下筋肉注射
        /// </summary>
        public const int Hikakin = 31;
        /// <summary>
        /// 静脈注射
        /// </summary>
        public const int Jyoumyaku = 32;
        /// <summary>
        /// 点滴注射
        /// </summary>
        public const int Tenteki = 33;
        /// <summary>
        /// その他注射
        /// </summary>
        public const int ChusyaSonota = 34;
        /// <summary>
        /// 処置
        /// </summary>
        public const int Syoti = 40;
        public const int SyotiMin = 40;
        public const int SyotiMax = 49;

        /// <summary>
        /// 手術
        /// </summary>
        public const int Syujyutu = 50;
        public const int SyujyutuMin = 50;
        public const int SyujyutuMax = 59;
        /// <summary>
        /// 輸血
        /// </summary>
        public const int Yuketu = 52;
        /// <summary>
        /// 麻酔
        /// </summary>
        public const int Masui = 54;
        /// <summary>
        /// 検査
        /// </summary>
        public const int Kensa = 60;
        public const int KensaMin = 60;
        public const int KensaMax = 69;
        /// <summary>
        /// 検体検査
        /// </summary>
        public const int Kentai = 61;
        /// <summary>
        /// 生体検査
        /// </summary>
        public const int Seitai = 62;
        /// <summary>
        /// 病理診断
        /// </summary>
        public const int Byouri = 64;
        /// <summary>
        /// 画像診断
        /// </summary>
        public const int Gazo = 70;
        public const int GazoMin = 70;
        public const int GazoMax = 79;
        /// <summary>
        /// その他
        /// </summary>
        public const int Sonota = 80;
        public const int SonotaMin = 80;
        public const int SonotaMax = 89;
        /// <summary>
        /// リハビリ
        /// </summary>
        public const int Riha = 81;
        /// <summary>
        /// 精神
        /// </summary>
        public const int Seisin = 82;
        /// <summary>
        /// 処方箋料
        /// </summary>
        public const int Syohosen = 83;
        /// <summary>
        /// 放射線
        /// </summary>
        public const int Housya = 84;
        /// <summary>
        /// <summary>
        /// 保険外医療
        /// </summary>
        public const int Jihi = 96;
        /// <summary>
        /// 処方箋コメント
        /// </summary>
        public const int SyohoCmt = 100;
        /// <summary>
        /// 処方箋備考
        /// </summary>
        public const int SyohoBiko = 101;

    }

    /// <summary>
    /// 請求区分
    ///  1:月遅れ 2:返戻 3:オンライン返戻
    /// </summary>
    public static class SeikyuKbn
    {
        public const int Normal = 0;
        public const int Delay = 1;
        public const int Henrei = 2;
        public const int Online = 3;
    }
}
