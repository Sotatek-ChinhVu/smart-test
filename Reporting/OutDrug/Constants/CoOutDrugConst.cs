namespace Reporting.OutDrug.Constants;

public class ItemTypeConst
{
    public const int Item = 0;
    public const int Yoho = 1;
    public const int Hojyo = 2;
    public const int Comment = 90;
    public const int NoAstComment = 91;
    public const int Bunkatu = 100;
}

public class QRVersion
{
    public const string Jahis5 = "JAHIS5";
    public const string Jahis7 = "JAHIS7";
    public const string Jahis8 = "JAHIS8";
    public const string Jahis9 = "JAHIS9";
}

public class EpsCsvVersion
{
    public const string SJ1 = "SJ1";
}

/// <summary>
/// 電子処方箋ファイル種別
/// 1: 電子処方箋ファイル
/// 2: 処方箋情報提供ファイル
/// 3: 確定前処方箋情報ファイル
/// </summary>
public class EpsCsvType
{
    public const int Electronic = 1;
    public const int Paper = 2;
    public const int Unfinished = 3;
}

public class MaxLength
{
    public const int KohiFutansya = 8;
    public const int KohiJyukyusya = 7;
}

/// <summary>
/// 電子処方箋公費記録区分
/// 0: 備考
/// 1: 第１公費
/// 2: 第２公費
/// 3: 第３公費
/// 4: 特殊公費
/// </summary>
public class EpsKohiRecKbn
{
    public const int Biko = 0;
    public const int Kohi1 = 1;
    public const int Kohi2 = 2;
    public const int Kohi3 = 3;
    public const int KohiSp = 4;
}

public class DrugInfKbn
{
    public const int Drug = 1;
    public const int Zairyo = 1;
}

/// <summary>
/// 薬品コード種別
/// 2: レセプト電算処理システム用コード
/// 4: YJコード
/// 7: 一般名コード
/// </summary>
public class DrugCdSbt
{
    public const int RezedenCd = 2;
    public const int YJCd = 4;
    public const int IpnNameCd = 7;
}

public class DrugCdConst
{
    public const string DrugDummy = "666660000";
    public const string ZairyoDummy = "777770000";
    public const string YJDummy = "2000000X0000";
}

public class YohoCdConst
{
    public const string Dummy = "0X0XXXXXXXXX0000";
}

public class NewLineCd
{
    public const string QR = "\r\n";
    public const string EpsCsv = "\n";
}
