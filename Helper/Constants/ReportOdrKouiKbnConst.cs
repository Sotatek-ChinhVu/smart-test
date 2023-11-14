namespace Helper.Constants;

/// <summary>
/// 診療行為コード
/// </summary>
public static class ReportOdrKouiKbnConst
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
