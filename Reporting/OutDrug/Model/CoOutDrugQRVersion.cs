using Helper.Common;
using Helper.Constants;
using Reporting.CommonMasters.Constants;
using Reporting.OutDrug.Constants;
using System.Text;

namespace Reporting.OutDrug.Model;

/// <summary>
/// バージョンレコード
/// </summary>
public class CoOutDrugQRVersion
{
    /// <summary>
    /// バージョンレコード生成
    /// </summary>
    /// <param name="version">バージョン</param>
    public CoOutDrugQRVersion(string version)
    {
        Version = version;
    }

    public string Version { get; set; }
    /// <summary>
    /// QR用データ
    /// </summary>
    public string QRData
    {
        get
        {
            return $"{Version}";
        }
    }
}

/// <summary>
/// 001 医療機関レコード
/// </summary>
public class CoOutDrugQR001
{
    /// <summary>
    /// 医療機関レコード生成
    /// </summary>
    /// <param name="hpCd">医療機関コード</param>
    /// <param name="prefNo">都道府県番号</param>
    /// <param name="hpName">医療機関名称</param>
    public CoOutDrugQR001(string version, string hpCd, int prefNo, string hpName)
    {
        Version = version;
        HpSbt = 1;
        HpCd = hpCd;
        PrefNo = prefNo;
        HpName = hpName;

    }
    /// <summary>
    /// QRのバージョン
    /// </summary>
    public string Version { get; set; }
    /// <summary>
    /// 医療機関コード種別
    /// </summary>
    public int HpSbt { get; set; }
    /// <summary>
    /// 医療機関コード
    /// </summary>
    public string HpCd { get; set; }
    /// <summary>
    /// 都道府県番号
    /// </summary>
    public int PrefNo { get; set; }
    public string HpName { get; set; }
    /// <summary>
    /// QR用データ
    /// </summary>
    public string QRData
    {
        get
        {
            return $"1,{HpSbt},{HpCd},{PrefNo},{HpName}";
        }
    }
}

/// <summary>
/// 002 医療機関所在地レコード
/// </summary>
public class CoOutDrugQR002
{
    /// <summary>
    /// 医療機関所在地レコード生成
    /// </summary>
    /// <param name="postCd">医療期間郵便番号</param>
    /// <param name="hpAddress">医療機関住所</param>
    public CoOutDrugQR002(string version, string postCd, string hpAddress)
    {
        Version = version;
        PostCd = postCd;
        HpAddress = hpAddress;
    }

    /// <summary>
    /// QRのバージョン
    /// </summary>
    public string Version { get; set; }

    /// <summary>
    /// 医療機関郵便番号
    /// </summary>
    public string PostCd { get; set; }
    /// <summary>
    /// 医療機関所在地
    /// </summary>
    public string HpAddress { get; set; }
    /// <summary>
    /// QR用データ
    /// </summary>
    public string QRData
    {
        get
        {
            return $"2,{PostCd},{HpAddress}";
        }
    }
}

/// <summary>
/// 003 医療機関電話レコード
/// </summary>
public class CoOutDrugQR003
{
    /// <summary>
    /// 医療機関電話レコード生成
    /// </summary>
    /// <param name="tel">電話番号</param>
    /// <param name="faxNo">Fax番号</param>
    /// <param name="otherContacts">その他連絡先</param>
    public CoOutDrugQR003(string version, string tel, string faxNo, string otherContacts)
    {
        Version = version;
        Tel = tel;
        FaxNo = faxNo;
        OtherContacts = otherContacts;
    }

    /// <summary>
    /// QRのバージョン
    /// </summary>
    public string Version { get; set; }
    /// <summary>
    /// 医療機関電話番号
    /// </summary>
    public string Tel { get; set; }
    /// <summary>
    /// Fax番号
    /// </summary>
    public string FaxNo { get; set; }
    /// <summary>
    /// その他連絡先
    /// </summary>
    public string OtherContacts { get; set; }
    /// <summary>
    /// QR用データ
    /// </summary>
    public string QRData
    {
        get
        {
            return $"3,{Tel},{FaxNo},{OtherContacts}";
        }
    }
}
/// <summary>
/// 005 医師レコード
/// </summary>
public class CoOutDrugQR005
{
    /// <summary>
    /// 医師レコード生成
    /// </summary>
    /// <param name="doctorName">医師名</param>
    public CoOutDrugQR005(string version, string doctorName)
    {
        Version = version;
        DoctorCd = string.Empty;
        DoctorKanaName = string.Empty;
        DoctorName = doctorName;
    }

    /// <summary>
    /// QRのバージョン
    /// </summary>
    public string Version { get; set; }
    /// <summary>
    /// 医師コード
    /// </summary>
    public string DoctorCd { get; set; }
    /// <summary>
    /// 医師カナ氏名
    /// </summary>
    public string DoctorKanaName { get; set; }
    /// <summary>
    /// 医師名
    /// </summary>
    public string DoctorName { get; set; }
    /// <summary>
    /// QR用データ
    /// </summary>
    public string QRData
    {
        get
        {
            return $"5,{DoctorCd},{DoctorKanaName},{DoctorName}";
        }
    }
}
/// <summary>
/// 011 患者氏名レコード
/// </summary>
public class CoOutDrugQR011
{
    /// <summary>
    /// 患者氏名レコード生成
    /// </summary>
    /// <param name="ptNum">患者番号</param>
    /// <param name="ptName">患者氏名</param>
    /// <param name="ptKanaName">患者カナ氏名</param>
    public CoOutDrugQR011(string version, long ptNum, string ptName, string ptKanaName)
    {
        Version = version;
        PtNum = ptNum;
        PtName = ptName;
        PtKanaName = ptKanaName;
    }

    /// <summary>
    /// QRのバージョン
    /// </summary>
    public string Version { get; set; }
    /// <summary>
    /// 患者番号
    /// </summary>
    public long PtNum { get; set; }
    /// <summary>
    /// 患者氏名
    /// </summary>
    public string PtName { get; set; }
    /// <summary>
    /// 患者カナ氏名
    /// </summary>
    public string PtKanaName { get; set; }
    /// <summary>
    /// QR用データ
    /// </summary>
    public string QRData
    {
        get
        {
            return $"11,{PtNum},{PtName},{PtKanaName}";
        }
    }
}
/// <summary>
/// 012 患者性別レコード
/// </summary>
public class CoOutDrugQR012
{
    /// <summary>
    /// 患者性別レコード生成
    /// </summary>
    /// <param name="sex">
    /// 性別　1:男、2:女
    /// </param>
    public CoOutDrugQR012(string version, int sex)
    {
        Version = version;
        Sex = sex;
    }
    /// <summary>
    /// QRのバージョン
    /// </summary>
    public string Version { get; set; }
    /// <summary>
    /// 性別
    ///     1:男、2:女
    /// </summary>
    public int Sex { get; set; }

    /// <summary>
    /// QR用データ
    /// </summary>
    public string QRData
    {
        get
        {
            return $"12,{Sex}";
        }
    }
}
/// <summary>
/// 013 患者生年月日レコード
/// </summary>
public class CoOutDrugQR013
{
    /// <summary>
    /// 患者生年月日レコード生成
    /// </summary>
    /// <param name="birthDay">生年月日 YYYYMMDD</param>
    public CoOutDrugQR013(string version, int birthDay)
    {
        Version = version;
        BirthDay = birthDay;
    }
    /// <summary>
    /// QRのバージョン
    /// </summary>
    public string Version { get; set; }
    /// <summary>
    /// 生年月日　YYYYMMDD
    /// </summary>
    public int BirthDay { get; set; }

    /// <summary>
    /// QR用データ
    /// </summary>
    public string QRData
    {
        get
        {
            return $"13,{BirthDay}";
        }
    }
}

/// <summary>
/// 014 患者一部負担区分レコード
/// </summary>
public class CoOutDrugQR014
{
    /// <summary>
    /// 患者一部負担区分レコード生成
    /// </summary>
    /// <param name="ichibuFutanKbn">
    /// 一部負担区分
    ///     1:高齢者一般、2:高齢者７割、3:６歳未満、5:後期高齢者８割
    /// </param>
    public CoOutDrugQR014(string version, int? ichibuFutanKbn)
    {
        Version = version;
        IchibuFutanKbn = ichibuFutanKbn;
    }
    /// <summary>
    /// QRのバージョン
    /// </summary>
    public string Version { get; set; }
    /// <summary>
    /// 一部負担区分
    ///     1:高齢者一般、2:高齢者７割、3:６歳未満
    /// </summary>
    public int? IchibuFutanKbn { get; set; }

    /// <summary>
    /// QR用データ
    /// </summary>
    public string QRData
    {
        get
        {
            string ret = string.Empty;
            if (IchibuFutanKbn != null)
            {
                ret = $"14,{IchibuFutanKbn}";
            }

            return ret;
        }
    }
}
/// <summary>
/// 021 保険種別レコード
/// </summary>
public class CoOutDrugQR021
{
    /// <summary>
    /// 保険種別レコード生成
    /// </summary>
    /// <param name="hokenSbt">
    /// 保険種別
    ///     1:医保 or 公費、2:国保、3:労災、4:自賠、5:公害、6:自費、7：後期高齢者
    /// </param>
    public CoOutDrugQR021(string version, int? hokenSbt)
    {
        Version = version;
        HokenSbt = hokenSbt;
    }
    /// <summary>
    /// QRのバージョン
    /// </summary>
    public string Version { get; set; }
    /// <summary>
    /// 保険種別
    ///     1:医保 or 公費、2:国保、3:労災、4:自賠、5:公害、6:自費、7：後期高齢者
    /// </summary>
    public int? HokenSbt { get; set; }
    /// <summary>
    /// QR用データ
    /// </summary>
    public string QRData
    {
        get
        {
            string ret = string.Empty;
            if (HokenSbt != null)
            {
                ret = $"21,{HokenSbt}";
            }

            return ret;
        }
    }
}
/// <summary>
/// 022 保険者番号レコード
/// </summary>
public class CoOutDrugQR022
{
    /// <summary>
    /// 保険者番号レコード生成
    /// </summary>
    /// <param name="hokensyaNo">保険者番号</param>
    public CoOutDrugQR022(string version, string hokensyaNo)
    {
        Version = version;
        HokensyaNo = hokensyaNo;
    }
    /// <summary>
    /// QRのバージョン
    /// </summary>
    public string Version { get; set; }
    /// <summary>
    /// 保険者番号
    /// </summary>
    public string HokensyaNo { get; set; }
    /// <summary>
    /// QR用データ
    /// </summary>
    public string QRData
    {
        get
        {
            return $"22,{HokensyaNo ?? string.Empty}";
        }
    }
}
/// <summary>
/// 023 記号番号レコード
/// </summary>
public class CoOutDrugQR023
{
    /// <summary>
    /// 記号番号レコード生成
    /// </summary>
    /// <param name="kigo">記号</param>
    /// <param name="bango">番号</param>
    /// <param name="honke">
    /// 被保険者／被扶養者
    ///     1:被保険者,2:被扶養者
    /// </param>
    public CoOutDrugQR023(string version, string kigo, string bango, string edaNo, int honke)
    {
        Version = version;
        Kigo = kigo;
        Bango = bango;
        HonKe = honke == 0 ? 1 : honke;
        EdaNo = edaNo;
    }

    /// <summary>
    /// QRのバージョン
    /// </summary>
    public string Version { get; set; }
    /// <summary>
    /// 記号
    /// </summary>
    public string Kigo { get; set; }
    /// <summary>
    /// 番号
    /// </summary>
    public string Bango { get; set; }
    /// <summary>
    /// 枝番
    /// </summary>
    public string EdaNo { get; set; }
    /// <summary>
    /// 被保険者／被扶養者
    ///     1:被保険者,2:被扶養者
    /// </summary>
    public int HonKe { get; set; }
    /// <summary>
    /// QR用データ
    /// </summary>
    public string QRData
    {
        get
        {
            if (string.Compare(Version, QRVersion.Jahis7) >= 0)
            {
                return $"23,{Kigo ?? string.Empty},{Bango ?? string.Empty},{HonKe},{EdaNo}";
            }
            else
            {
                return $"23,{Kigo ?? string.Empty},{Bango ?? string.Empty},{HonKe}";
            }
        }
    }
}

/// <summary>
/// 025 職務上の事由
/// </summary>
public class CoOutDrugQR025
{
    /// <summary>
    /// 職務上の事由レコード生成
    /// </summary>
    /// <param name="syokumuKbn">
    /// 職務上の事由
    ///     1:職務上、2:下船後３ヶ月以内、3:通勤災害
    /// </param>
    public CoOutDrugQR025(string version, int syokumuKbn)
    {
        Version = version;
        SyokumuKbn = syokumuKbn;
    }
    /// <summary>
    /// QRのバージョン
    /// </summary>
    public string Version { get; set; }
    /// <summary>
    /// 職務上の事由
    ///     1:職務上、2:下船後３ヶ月以内、3:通勤災害
    /// </summary>
    public int SyokumuKbn { get; set; }
    /// <summary>
    /// QR用データ
    /// </summary>
    public string QRData
    {
        get
        {
            string ret = string.Empty;

            if (SyokumuKbn > 0)
            {
                ret = $"25,{SyokumuKbn}";
            }
            return ret;
        }
    }
}
/// <summary>
/// 027 第一公費レコード
/// </summary>
public class CoOutDrugQR027
{
    /// <summary>
    /// 第一公費レコード生成
    /// </summary>
    /// <param name="futansyaNo">公費負担者番号</param>
    /// <param name="jyukyusyaNo">公費受給者番号</param>
    public CoOutDrugQR027(string version, string futansyaNo, string jyukyusyaNo)
    {
        Version = version;
        FutansyaNo = futansyaNo;
        JyukyusyaNo = jyukyusyaNo;
    }
    /// <summary>
    /// QRのバージョン
    /// </summary>
    public string Version { get; set; }
    /// <summary>
    /// 公費負担者番号
    /// </summary>
    public string FutansyaNo { get; set; }
    /// <summary>
    /// 公費受給者番号
    /// </summary>
    public string JyukyusyaNo { get; set; }
    /// <summary>
    /// QR用データ
    /// </summary>
    public string QRData
    {
        get
        {
            string ret = string.Empty;

            if ((FutansyaNo != null && FutansyaNo != string.Empty) || (JyukyusyaNo != null && JyukyusyaNo != string.Empty))
            {
                ret = $"27,{FutansyaNo},{JyukyusyaNo}";
            }
            return ret;
        }
    }
}
/// <summary>
/// 028 第二公費レコード
/// </summary>
public class CoOutDrugQR028
{
    /// <summary>
    /// 第二公費レコード
    /// </summary>
    /// <param name="futansyaNo">公費負担者番号</param>
    /// <param name="jyukyusyaNo">公費受給者番号</param>
    public CoOutDrugQR028(string version, string futansyaNo, string jyukyusyaNo)
    {
        Version = version;
        FutansyaNo = futansyaNo;
        JyukyusyaNo = jyukyusyaNo;
    }
    /// <summary>
    /// QRのバージョン
    /// </summary>
    public string Version { get; set; }
    /// <summary>
    /// 公費負担者番号
    /// </summary>
    public string FutansyaNo { get; set; }
    /// <summary>
    /// 公費受給者番号
    /// </summary>
    public string JyukyusyaNo { get; set; }
    /// <summary>
    /// QR用データ
    /// </summary>
    public string QRData
    {
        get
        {
            string ret = string.Empty;

            if ((FutansyaNo != null && FutansyaNo != string.Empty) || (JyukyusyaNo != null && JyukyusyaNo != string.Empty))
            {
                ret = $"28,{FutansyaNo},{JyukyusyaNo}";
            }
            return ret;
        }
    }
}
/// <summary>
/// 029 第三公費レコード
/// </summary>
public class CoOutDrugQR029
{
    /// <summary>
    /// 第三公費レコード
    /// </summary>
    /// <param name="futansyaNo">公費負担者番号</param>
    /// <param name="jyukyusyaNo">公費受給者番号</param>
    public CoOutDrugQR029(string version, string futansyaNo, string jyukyusyaNo)
    {
        Version = version;
        FutansyaNo = futansyaNo;
        JyukyusyaNo = jyukyusyaNo;
    }
    /// <summary>
    /// QRのバージョン
    /// </summary>
    public string Version { get; set; }
    /// <summary>
    /// 公費負担者番号
    /// </summary>
    public string FutansyaNo { get; set; }
    /// <summary>
    /// 公費受給者番号
    /// </summary>
    public string JyukyusyaNo { get; set; }
    /// <summary>
    /// QR用データ
    /// </summary>
    public string QRData
    {
        get
        {
            string ret = string.Empty;

            if ((FutansyaNo != null && FutansyaNo != string.Empty) || (JyukyusyaNo != null && JyukyusyaNo != string.Empty))
            {
                ret = $"29,{FutansyaNo},{JyukyusyaNo}";
            }
            return ret;
        }
    }
}
/// <summary>
/// 030 特殊公費レコード
/// </summary>
public class CoOutDrugQR030
{
    /// <summary>
    /// 特殊公費レコード
    /// </summary>
    /// <param name="futansyaNo">公費負担者番号</param>
    /// <param name="jyukyusyaNo">公費受給者番号</param>
    public CoOutDrugQR030(string version, string futansyaNo, string jyukyusyaNo)
    {
        Version = version;
        FutansyaNo = futansyaNo;
        JyukyusyaNo = jyukyusyaNo;
    }
    /// <summary>
    /// QRのバージョン
    /// </summary>
    public string Version { get; set; }
    /// <summary>
    /// 公費負担者番号
    /// </summary>
    public string FutansyaNo { get; set; }
    /// <summary>
    /// 公費受給者番号
    /// </summary>
    public string JyukyusyaNo { get; set; }
    /// <summary>
    /// QR用データ
    /// </summary>
    public string QRData
    {
        get
        {
            string ret = string.Empty;

            if ((FutansyaNo != null && FutansyaNo != string.Empty) || (JyukyusyaNo != null && JyukyusyaNo != string.Empty))
            {
                ret = $"30,{FutansyaNo},{JyukyusyaNo}";
            }
            return ret;
        }
    }
}
/// <summary>
/// 051 処方箋交付年月日レコード
/// </summary>
public class CoOutDrugQR051
{
    /// <summary>
    /// 処方箋交付年月日レコード生成
    /// </summary>
    /// <param name="kofuDate">交付日 YYYYMMDD</param>
    public CoOutDrugQR051(string version, int kofuDate)
    {
        Version = version;
        KofuDate = kofuDate;
    }
    /// <summary>
    /// QRのバージョン
    /// </summary>
    public string Version { get; set; }
    /// <summary>
    /// 交付日
    /// </summary>
    public int KofuDate { get; set; }
    /// <summary>
    /// QR用データ
    /// </summary>
    public string QRData
    {
        get
        {
            return $"51,{KofuDate}";

        }
    }
}
/// <summary>
/// 062 残薬確認欄レコード
/// </summary>
public class CoOutDrugQR062
{
    /// <summary>
    /// 残薬確認欄レコード生成
    /// </summary>
    /// <param name="zanyakuFlg">
    /// 残薬確認対応フラグ
    ///     1:保険医療機関へ疑義照会した上で調剤
    ///     2:保険医療機関へ情報提供
    /// </param>
    public CoOutDrugQR062(string version, int zanyakuFlg)
    {
        Version = version;
        ZanyakuFlg = zanyakuFlg;
    }
    /// <summary>
    /// QRのバージョン
    /// </summary>
    public string Version { get; set; }
    /// <summary>
    /// 残薬確認対応フラグ
    ///     1:保険医療機関へ疑義照会した上で調剤
    ///     2:保険医療機関へ情報提供
    /// </summary>
    public int ZanyakuFlg { get; set; }
    /// <summary>
    /// QR用データ
    /// </summary>
    public string QRData
    {
        get
        {
            return $"62,{ZanyakuFlg}";

        }
    }
}
/// <summary>
/// 063 分割指示
/// </summary>
public class CoOutDrugQR063
{
    /// <summary>
    /// 分割指示レコード生成
    /// </summary>
    /// <param name="bunkatuKaisu">分割回数</param>
    /// <param name="bunkatuKai">分割回</param>
    public CoOutDrugQR063(string version, int bunkatuKaisu, int bunkatuKai)
    {
        Version = version;
        BunkatuKaisu = bunkatuKaisu;
        BunkatuKai = bunkatuKai;
    }
    /// <summary>
    /// QRのバージョン
    /// </summary>
    public string Version { get; set; }
    /// <summary>
    /// 分割回数
    /// </summary>
    public int BunkatuKaisu { get; set; }
    /// <summary>
    /// 分割回
    /// </summary>
    public int BunkatuKai { get; set; }

    public string QRData
    {
        get
        {
            return $"63,{BunkatuKaisu},{BunkatuKai}";
        }
    }
}
/// <summary>
/// 064 リフィル処方箋
/// </summary>
public class CoOutDrugQR064
{
    /// <summary>
    /// リフィル処方箋情報レコード生成
    /// </summary>
    /// <param name="bunkatuKaisu">分割回数</param>
    /// <param name="bunkatuKai">分割回</param>
    public CoOutDrugQR064(string version, int totalKaisu)
    {
        Version = version;
        TotalKaisu = totalKaisu;
    }
    /// <summary>
    /// QRのバージョン
    /// </summary>
    public string Version { get; set; }
    /// <summary>
    /// 総使用回数回数
    /// </summary>
    public int TotalKaisu { get; set; }

    public string QRData
    {
        get
        {
            return $"64,{TotalKaisu}";

        }
    }
}
/// <summary>
/// 081 備考レコード
/// </summary>
public class CoOutDrugQR081
{
    readonly List<(int seqNo, int? Sbt, string biko)> _qr081s = new();
    int index = 0;

    /// <summary>
    /// 備考レコード生成(Addメソッドで追加)
    /// </summary>
    public CoOutDrugQR081(string version)
    {
        Version = version;
        index = 0;
    }
    /// <summary>
    /// QRのバージョン
    /// </summary>
    public string Version { get; set; }
    /// <summary>
    /// 備考レコード追加
    /// </summary>
    /// <param name="sbt">
    /// 備考種別
    ///     1:一包化、2:粉砕、3:分割、4～99:予備、省略:不明
    /// </param>
    /// <param name="biko">
    ///     処方箋全体に掛かる補足情報を出力（漢字半角混在可）
    /// </param>
    public void Add(string biko)
    {
        index++;
        _qr081s.Add((index, null, biko));
    }
    /// <summary>
    /// QR用データ
    /// </summary>
    public string QRData
    {
        get
        {
            StringBuilder ret = new();

            foreach ((int seqNo, int? sbt, string biko) qr081 in _qr081s)
            {
                if (ret.Length > 0)
                {
                    ret.Append("\r\n");
                }
                ret.Append($"81,{qr081.seqNo},{CIUtil.ToStringIgnoreNull(qr081.sbt)},{qr081.biko}");
            }
            return ret.ToString();
        }
    }

}

/// <summary>
/// QR Rp情報管理クラス
/// 101~181 + 薬剤情報のリスト
/// </summary>
public class CoOutDrugQRRp
{
    string _appendStr(string source, string addStr)
    {
        string ret = source;
        string tmp = addStr;
        if (tmp != string.Empty)
        {
            if (ret != string.Empty) ret += "\r\n";
            ret += tmp;
        }

        return ret;
    }

    readonly CoOutDrugQR101 _qr101;
    readonly CoOutDrugQR102 _qr102;
    readonly CoOutDrugQR111 _qr111;
    readonly CoOutDrugQR181 _qr181;
    readonly List<CoOutDrugQRDrug> _qrDrugs;

    /// <summary>
    /// QR Rp情報管理クラス生成
    /// </summary>
    /// <param name="qr101">剤型レコード</param>
    /// <param name="qr111">用法レコード</param>
    /// <param name="qr181">用法補足レコード</param>
    /// <param name="qrDrugs">QR薬剤情報管理クラスのリスト</param>
    public CoOutDrugQRRp(CoOutDrugQR101 qr101, CoOutDrugQR102 qr102, CoOutDrugQR111 qr111, CoOutDrugQR181 qr181, List<CoOutDrugQRDrug> qrDrugs)
    {
        _qr101 = qr101;
        _qr102 = qr102;
        _qr111 = qr111;
        _qr181 = qr181;
        _qrDrugs = qrDrugs;
    }

    /// <summary>
    /// QR用データ
    /// </summary>
    public string QRData
    {
        get
        {
            string ret = string.Empty;

            if (_qr101 != null)
            {
                ret = _appendStr(ret, _qr101.QRData);
            }
            if (_qr102 != null)
            {
                ret = _appendStr(ret, _qr102.QRData);
            }
            if (_qr111 != null)
            {
                ret = _appendStr(ret, _qr111.QRData);
            }
            if (_qr181 != null)
            {
                ret = _appendStr(ret, _qr181.QRData);
            }
            if (_qrDrugs != null && _qrDrugs.Any())
            {
                foreach (CoOutDrugQRDrug qrDrug in _qrDrugs)
                {
                    if (qrDrug != null)
                    {
                        ret = _appendStr(ret, qrDrug.QRData);
                    }
                }
            }

            return ret;
        }
    }
}

/// <summary>
/// 101 剤型レコード
/// </summary>
public class CoOutDrugQR101
{
    /// <summary>
    /// 101 剤型レコード生成
    /// </summary>
    /// <param name="rpNo">Rp番号</param>
    /// <param name="zaikeiKbn">
    /// 剤型区分
    ///     1:内服、2:頓服、3:外用、4:内服滴剤、5:注射、6:医療材料、9:不明
    /// </param>
    /// <param name="cyozaiSuryo">調剤数量</param>
    public CoOutDrugQR101(string version, int rpNo, int zaikeiKbn, int cyozaiSuryo)
    {
        Version = version;
        RpNo = rpNo;
        ZaikeiKbn = zaikeiKbn;
        ZaikeiName = string.Empty;
        CyozaiSuryo = cyozaiSuryo;
    }

    /// <summary>
    /// <summary>
    /// QRのバージョン
    /// </summary>
    public string Version { get; set; }/// Rp番号
                                       /// </summary>
    public int RpNo { get; set; }
    /// <summary>
    /// 剤型区分
    ///     1:内服、2:頓服、3:外用、4:内服滴剤、5:注射、6:医療材料、9:不明
    /// </summary>
    public int ZaikeiKbn { get; set; }
    /// <summary>
    /// 剤型名称
    /// </summary>
    public string ZaikeiName { get; set; }
    /// <summary>
    /// 剤型数量
    /// </summary>
    public int CyozaiSuryo { get; set; }
    /// <summary>
    /// QR用データ
    /// </summary>
    public string QRData
    {
        get
        {
            return $"101,{RpNo},{ZaikeiKbn},{ZaikeiName},{CyozaiSuryo}";

        }
    }
}

/// <summary>
/// 102 分割指示調剤数量レコード
/// </summary>
public class CoOutDrugQR102
{
    public CoOutDrugQR102(string version, int rpNo, int cyozaiSuryo, int totalSuryo)
    {
        Version = version;
        RpNo = rpNo;
        CyozaiSuryo = cyozaiSuryo;
        TotalSuryo = totalSuryo;
    }

    public CoOutDrugQR102()
    {
        Version = string.Empty;
    }

    /// <summary>
    /// QRのバージョン
    /// </summary>
    public string Version { get; set; }
    /// <summary>
    /// Rp番号
    /// </summary>
    public int RpNo { get; set; }
    /// <summary>
    /// 分割数量
    /// </summary>
    public int CyozaiSuryo { get; set; }
    /// <summary>
    /// 総数量
    /// </summary>
    public int TotalSuryo { get; set; }

    /// <summary>
    /// QR用データ
    /// </summary>
    public string QRData
    {
        get
        {
            return $"102,{RpNo},{CyozaiSuryo},{TotalSuryo}";

        }
    }
}
/// <summary>
/// 111 用法レコード
/// </summary>
public class CoOutDrugQR111
{
    /// <summary>
    /// 111 用法レコード生成
    /// </summary>
    /// <param name="rpNo">Rp番号</param>
    /// <param name="yohoName">用法名</param>
    public CoOutDrugQR111(string version, int rpNo, string yohoName)
    {
        Version = version;
        RpNo = rpNo;
        YohoSbt = 1;
        YohoCd = string.Empty;
        YohoName = yohoName;
        OneDayKaisu = null;
    }
    /// <summary>
    /// QRのバージョン
    /// </summary>
    public string Version { get; set; }
    /// <summary>
    /// Rp番号
    /// </summary>
    public int RpNo { get; set; }
    /// <summary>
    /// 用法種別
    /// 1:コードなし,2：JAMI 用法コード
    /// </summary>
    public int YohoSbt { get; set; }
    /// <summary>
    /// 用法コード
    /// </summary>
    public string YohoCd { get; set; }
    /// <summary>
    /// 用法名称
    /// </summary>
    public string YohoName { get; set; }
    /// <summary>
    /// 1日回数
    /// </summary>
    public int? OneDayKaisu { get; set; }

    /// <summary>
    /// QR用データ
    /// </summary>
    public string QRData
    {
        get
        {
            return $"111,{RpNo},{YohoSbt},{YohoCd},{YohoName},{CIUtil.ToStringIgnoreNull(OneDayKaisu)}";

        }
    }
}
/// <summary>
/// 181 用法補足レコード
/// </summary>
public class CoOutDrugQR181
{
    readonly List<(int rpNo, int seqNo, int? hosokuKbn, string hosokuInf, int? buiCd)> _qr181s = new();
    readonly int _rpNo = 0;
    int _seqNo = 0;

    /// <summary>
    /// 181 用法補足レコード生成
    /// </summary>
    /// <param name="rpNo">Rp番号</param>
    public CoOutDrugQR181(string version, int rpNo)
    {
        Version = version;
        _rpNo = rpNo;
        _seqNo = 0;
    }
    /// <summary>
    /// QRのバージョン
    /// </summary>
    public string Version { get; set; }
    public void Add(string hosokuInf)
    {
        _seqNo++;

        _qr181s.Add((_rpNo, _seqNo, null, hosokuInf, null));
    }

    /// <summary>
    /// QR用データ
    /// </summary>
    public string QRData
    {
        get
        {
            StringBuilder ret = new();

            foreach ((int rpNo, int seqNo, int? hosokuKbn, string hosokuInf, int? buiCd) qr181 in _qr181s)
            {
                if (ret.Length > 0)
                {
                    ret.Append("\r\n");
                }
                ret.Append($"181,{qr181.rpNo},{qr181.seqNo},{CIUtil.ToStringIgnoreNull(qr181.hosokuKbn)},{qr181.hosokuInf},{CIUtil.ToStringIgnoreNull(qr181.buiCd)}");
            }
            return ret.ToString();
        }
    }
}
/// <summary>
/// QR薬剤情報管理クラス
/// 201~281
/// </summary>
public class CoOutDrugQRDrug
{
    string _appendStr(string source, string addStr)
    {
        string ret = source;
        string tmp = addStr;
        if (tmp != string.Empty)
        {
            if (ret != string.Empty) ret += "\r\n";
            ret += tmp;
        }

        return ret;
    }

    readonly CoOutDrugQR201 _qr201;
    readonly CoOutDrugQR211 _qr211;
    readonly CoOutDrugQR231 _qr231;
    readonly CoOutDrugQR281 _qr281;

    public CoOutDrugQRDrug(CoOutDrugQR201 qr201, CoOutDrugQR211 qr211, CoOutDrugQR231 qr231, CoOutDrugQR281 qr281)
    {
        _qr201 = qr201;
        _qr211 = qr211;
        _qr231 = qr231;
        _qr281 = qr281;
    }

    public string QRData
    {
        get
        {
            string ret = string.Empty;

            if (_qr201 != null)
            {
                ret = _appendStr(ret, _qr201.QRData);
            }
            if (_qr211 != null)
            {
                ret = _appendStr(ret, _qr211.QRData);
            }
            if (_qr231 != null)
            {
                ret = _appendStr(ret, _qr231.QRData);
            }
            if (_qr281 != null)
            {
                ret = _appendStr(ret, _qr281.QRData);
            }

            return ret;
        }
    }
}

/// <summary>
/// 201 薬品レコード
/// </summary>
public class CoOutDrugQR201
{
    public CoOutDrugQR201(string version, int rpNo, int seqNo, int infKbn, int cdSbt, string itemCd, string drugName, double yoryo, int rikika, string unitName)
    {
        Version = version;
        RpNo = rpNo;
        SeqNo = seqNo;
        InfKbn = infKbn;
        CdSbt = cdSbt;
        ItemCd = itemCd;
        DrugName = drugName;
        Yoryo = yoryo;
        Rikika = rikika;
        UnitName = unitName;
    }

    public CoOutDrugQR201()
    {
        Version = string.Empty;
        ItemCd = string.Empty;
        DrugName = string.Empty;
        UnitName = string.Empty;
    }

    /// <summary>
    /// QRのバージョン
    /// </summary>
    public string Version { get; set; }
    /// <summary>
    /// Rp番号
    /// </summary>
    public int RpNo { get; set; }
    /// <summary>
    /// Rp内連番
    /// </summary>
    public int SeqNo { get; set; }
    /// <summary>
    /// 情報区分
    /// 1:医薬品、2:医療材料、3:非保険薬、省略:不明
    /// </summary>
    public int InfKbn { get; set; }
    /// <summary>
    /// 薬品コード種別
    /// 1:ｺｰﾄﾞなし, 2:ﾚｾﾌﾟﾄ電算ｺｰﾄﾞ,3:厚生省ｺｰﾄﾞ,4:ＹＪｺｰﾄﾞ,6:HOT ｺｰﾄﾞ,7:一般名ｺｰﾄﾞ(厚労省), 5 及び8:予備
    /// </summary>
    public int CdSbt { get; set; }
    /// <summary>
    /// 薬品コード
    /// </summary>
    public string ItemCd { get; set; }
    /// <summary>
    /// 薬品名称
    /// </summary>
    public string DrugName { get; set; }
    /// <summary>
    /// 用量 整数 6 桁+小数点+小数5 桁
    /// </summary>
    public double Yoryo { get; set; }
    /// <summary>
    /// 力価フラグ
    /// 1:薬価単位、2:力価単位
    /// </summary>
    public int Rikika { get; set; }
    /// <summary>
    /// 単位名称
    /// </summary>
    public string UnitName { get; set; }
    /// <summary>
    /// QR用データ
    /// </summary>
    public string QRData
    {
        get
        {
            return $"201,{RpNo},{SeqNo},{(InfKbn == 0 ? string.Empty : InfKbn.ToString())},{CdSbt},{ItemCd},{DrugName},{Yoryo},{Rikika},{UnitName}";

        }
    }
}
/// <summary>
/// 211 単位変換レコード
/// </summary>
public class CoOutDrugQR211
{
    public CoOutDrugQR211(string version, int rpNo, int seqNo, double termVal)
    {
        Version = version;
        RpNo = rpNo;
        SeqNo = seqNo;
        TermVal = termVal;
    }

    public CoOutDrugQR211()
    {
        Version = string.Empty;
    }

    /// <summary>
    /// QRのバージョン
    /// </summary>
    public string Version { get; set; }
    /// <summary>
    /// Rp番号
    /// </summary>
    public int RpNo { get; set; }
    /// <summary>
    /// Rp内連番
    /// </summary>
    public int SeqNo { get; set; }
    /// <summary>
    /// 単位換算係数
    /// </summary>
    public double TermVal { get; set; }
    /// <summary>
    /// QR用データ
    /// </summary>
    public string QRData
    {
        get
        {
            return $"211,{RpNo},{SeqNo},{TermVal}";

        }
    }
}
/// <summary>
/// 231 負担区分レコード
/// </summary>
public class CoOutDrugQR231
{
    /// <summary>
    /// 231 負担区分レコード生成
    /// </summary>
    /// <param name="rpNo">Rp番号</param>
    /// <param name="seqNo">Rp内連番</param>
    /// <param name="k1FutanKbn">公１負担区分</param>
    /// <param name="k2FutanKbn">公２負担区分</param>
    /// <param name="k3FutanKbn">公３負担区分</param>
    /// <param name="k4FutanKbn">特殊公費負担区分</param>
    public CoOutDrugQR231(string version, int rpNo, int seqNo, int k1FutanKbn, int k2FutanKbn, int k3FutanKbn, int k4FutanKbn)
    {
        Version = version;
        RpNo = rpNo;
        SeqNo = seqNo;
        K1FutanKbn = k1FutanKbn;
        K2FutanKbn = k2FutanKbn;
        K3FutanKbn = k3FutanKbn;
        K4FutanKbn = k4FutanKbn;
    }

    public CoOutDrugQR231()
    {
        Version = string.Empty;
    }

    /// <summary>
    /// QRのバージョン
    /// </summary>
    public string Version { get; set; }
    /// <summary>
    /// Rp番号
    /// </summary>
    public int RpNo { get; set; }
    /// <summary>
    /// Rp内連番
    /// </summary>
    public int SeqNo { get; set; }
    /// <summary>
    /// 公１公費負担区分
    /// </summary>
    public int K1FutanKbn { get; set; }
    /// <summary>
    /// 公２公費負担区分
    /// </summary>
    public int K2FutanKbn { get; set; }
    /// <summary>
    /// 公３公費負担区分
    /// </summary>
    public int K3FutanKbn { get; set; }
    /// <summary>
    /// 特殊公費負担区分
    /// </summary>
    public int K4FutanKbn { get; set; }
    /// <summary>
    /// QR用データ
    /// </summary>
    public string QRData
    {
        get
        {
            return $"231,{RpNo},{SeqNo},{K1FutanKbn},{K2FutanKbn},{K3FutanKbn},{K4FutanKbn}";

        }
    }
}
/// <summary>
/// 281 薬品補足レコード
/// </summary>
public class CoOutDrugQR281
{
    readonly List<(int rpNo, int seqNo, int hosokuSeqNo, int hosokuKbn, string hosokuInf, string yohoCd)> _qr281s = new();
    readonly int _rpNo;
    readonly int _seqNo;
    int _hosokuSeqNo;

    /// <summary>
    /// 281 薬品補足レコード生成
    /// </summary>
    /// <param name="rpNo">Rp番号</param>
    /// <param name="seqNo">Rp内連番</param>
    public CoOutDrugQR281(string version, int rpNo, int seqNo)
    {
        Version = version;
        _rpNo = rpNo;
        _seqNo = seqNo;
        _hosokuSeqNo = 0;
    }
    /// <summary>
    /// QRのバージョン
    /// </summary>
    public string Version { get; set; }
    public void Add(int hosokuKbn, string hosokuInf)
    {
        _hosokuSeqNo++;

        _qr281s.Add((_rpNo, _seqNo, _hosokuSeqNo, hosokuKbn, hosokuInf, string.Empty));
    }

    /// <summary>
    /// QR用データ
    /// </summary>
    public string QRData
    {
        get
        {
            StringBuilder ret = new();

            foreach ((int rpNo, int seqNo, int hosokuSeqNo, int hosokuKbn, string hosokuInf, string yohoCd) qr281 in _qr281s)
            {
                if (ret.Length > 0)
                {
                    ret.Append("\r\n");
                }
                ret.Append($"281,{qr281.rpNo},{qr281.seqNo},{qr281.hosokuSeqNo},{qr281.hosokuKbn},{qr281.hosokuInf},{qr281.yohoCd}");
            }
            return ret.ToString();
        }
    }
}

/// <summary>
/// 処方箋QRデータ
/// </summary>
public class CoOutDrugQRData
{
    string _appendStr(string source, string addStr)
    {
        string ret = source;
        string tmp = addStr;
        if (tmp != string.Empty)
        {
            if (ret != string.Empty) ret += "\r\n";
            ret += tmp;
        }

        return ret;
    }

    public CoOutDrugQRData(
        string version, int sinDate, CoHpInfModel hpInf, CoRaiinInfModel raiinInf, CoPtInfModel ptInf, CoPtHokenInfModel ptHoken,
        List<CoPtKohiModel> filteredPtKohis, int bunkatuMax, int bunkatuKai, int refillCount)
    {
        QRVersion = new CoOutDrugQRVersion(version);
        QR001 = new CoOutDrugQR001(version, hpInf.HpCd, hpInf.PrefNo, hpInf.HpName);
        QR002 = new CoOutDrugQR002(version, hpInf.PostCdDsp, hpInf.Address1 + hpInf.Address2);
        QR003 = new CoOutDrugQR003(version, hpInf.Tel, hpInf.FaxNo, hpInf.OtherContacts);
        QR005 = new CoOutDrugQR005(version, raiinInf.TantoName);
        QR011 = new CoOutDrugQR011(version, ptInf.PtNum, ptInf.Name, ptInf.KanaName);
        QR012 = new CoOutDrugQR012(version, ptInf.Sex);
        QR013 = new CoOutDrugQR013(version, ptInf.Birthday);

        int? ichibuFutanKbn = null;
        if (sinDate >= KaiseiDate.d20221001 &&
            ptHoken != null &&
            ptHoken.IsKouki &&
            ptHoken.KogakuKbn == 41 &&
            string.Compare(version, Constants.QRVersion.Jahis9) >= 0
            )
        {
            ichibuFutanKbn = 5;
        }
        else if (CIUtil.AgeChk(ptInf.Birthday, sinDate, 70))
        {
            if (ptHoken != null)
            {
                if (!(new int[] { 3, 26, 27, 28 }.Contains(ptHoken.KogakuKbn)))
                {
                    ichibuFutanKbn = 1;
                }
                else
                {
                    ichibuFutanKbn = 2;
                }
            }
        }
        else if (!CIUtil.IsStudent(ptInf.Birthday, sinDate))
        {
            ichibuFutanKbn = 3;
        }

        QR014 = new CoOutDrugQR014(version, ichibuFutanKbn);

        int? hokenKbn = null;
        if (ptHoken != null)
        {
            switch (ptHoken.HokenKbn)
            {
                case 0: // 自費
                    hokenKbn = 6;
                    break;
                case 1: // 社保
                    hokenKbn = 1;
                    break;
                case 2: // 国保
                    if (ptHoken.Houbetu == "39")
                    {
                        // 後期高齢者
                        hokenKbn = 7;
                    }
                    else
                    {
                        hokenKbn = 2;
                    }
                    break;
                case 11:    // 労災
                case 12:
                case 13:
                    hokenKbn = 3;
                    break;
                case 14:    // 自賠
                    hokenKbn = 4;
                    break;
            }
        }
        else
        {
            hokenKbn = 6;
        }
        QR021 = new CoOutDrugQR021(version, hokenKbn);

        QR022 = new CoOutDrugQR022(version, ptHoken?.HokensyaNo ?? string.Empty);

        QR023 = new CoOutDrugQR023(version, CIUtil.ToWide(ptHoken?.Kigo ?? string.Empty), CIUtil.ToWide(ptHoken?.Bango ?? string.Empty), ptHoken?.EdaNo ?? string.Empty, ptHoken?.HonkeKbn ?? 1);

        QR025 = new CoOutDrugQR025(version, ptHoken?.SyokumuKbn ?? 0);

        IsHeiyo = (filteredPtKohis.Count >= 1);

        QR027 = null;
        if (filteredPtKohis.Count >= 1)
        {
            QR027 = new CoOutDrugQR027(version, filteredPtKohis[0].FutansyaNo, filteredPtKohis[0].JyukyusyaNo);
        }

        QR028 = null;
        if (filteredPtKohis.Count >= 2)
        {
            QR028 = new CoOutDrugQR028(version, filteredPtKohis[1].FutansyaNo, filteredPtKohis[1].JyukyusyaNo);
        }

        QR029 = null;
        if (filteredPtKohis.Count >= 3)
        {
            QR029 = new CoOutDrugQR029(version, filteredPtKohis[2].FutansyaNo, filteredPtKohis[2].JyukyusyaNo);
        }

        QR030 = null;
        if (filteredPtKohis.Count >= 4)
        {
            QR030 = new CoOutDrugQR030(version, filteredPtKohis[3].FutansyaNo, filteredPtKohis[3].JyukyusyaNo);
        }

        QR051 = new CoOutDrugQR051(version, sinDate);

        QR063 = null;
        if (bunkatuMax > 1)
        {
            QR063 = new CoOutDrugQR063(version, bunkatuMax, bunkatuKai);
        }

        QR064 = null;
        if (refillCount > 0)
        {
            QR064 = new CoOutDrugQR064(version, refillCount);
        }
    }

    public bool IsHeiyo { get; set; }

    public CoOutDrugQRVersion QRVersion { get; set; }
    public CoOutDrugQR001 QR001 { get; set; }
    public CoOutDrugQR002 QR002 { get; set; }
    public CoOutDrugQR003 QR003 { get; set; }
    public CoOutDrugQR005 QR005 { get; set; }
    public CoOutDrugQR011 QR011 { get; set; }
    public CoOutDrugQR012 QR012 { get; set; }
    public CoOutDrugQR013 QR013 { get; set; }
    public CoOutDrugQR014 QR014 { get; set; }
    public CoOutDrugQR021 QR021 { get; set; }
    public CoOutDrugQR022 QR022 { get; set; }
    public CoOutDrugQR023 QR023 { get; set; }
    public CoOutDrugQR025 QR025 { get; set; }
    public CoOutDrugQR027? QR027 { get; set; }
    public CoOutDrugQR028? QR028 { get; set; }
    public CoOutDrugQR029? QR029 { get; set; }
    public CoOutDrugQR030? QR030 { get; set; }
    public CoOutDrugQR051 QR051 { get; set; }
    public CoOutDrugQR062? QR062 { get; set; }
    public CoOutDrugQR063? QR063 { get; set; }
    public CoOutDrugQR064? QR064 { get; set; }
    public CoOutDrugQR081? QR081 { get; set; }
    public List<CoOutDrugQRRp> QRRps { get; set; } = new();

    public string QRData
    {
        get
        {
            string ret = string.Empty;
            if (QRVersion != null) ret = _appendStr(ret, QRVersion.QRData);
            if (QR001 != null) ret = _appendStr(ret, QR001.QRData);
            if (QR002 != null) ret = _appendStr(ret, QR002.QRData);
            if (QR003 != null) ret = _appendStr(ret, QR003.QRData);
            if (QR005 != null) ret = _appendStr(ret, QR005.QRData);
            if (QR011 != null) ret = _appendStr(ret, QR011.QRData);
            if (QR012 != null) ret = _appendStr(ret, QR012.QRData);
            if (QR013 != null) ret = _appendStr(ret, QR013.QRData);
            if (QR014 != null) ret = _appendStr(ret, QR014.QRData);
            if (QR021 != null) ret = _appendStr(ret, QR021.QRData);
            if (QR022 != null) ret = _appendStr(ret, QR022.QRData);
            if (QR023 != null) ret = _appendStr(ret, QR023.QRData);
            if (QR025 != null) ret = _appendStr(ret, QR025.QRData);
            if (QR027 != null) ret = _appendStr(ret, QR027.QRData);
            if (QR028 != null) ret = _appendStr(ret, QR028.QRData);
            if (QR029 != null) ret = _appendStr(ret, QR029.QRData);
            if (QR030 != null) ret = _appendStr(ret, QR030.QRData);
            if (QR051 != null) ret = _appendStr(ret, QR051.QRData);
            if (QR062 != null) ret = _appendStr(ret, QR062.QRData);
            if (QR063 != null) ret = _appendStr(ret, QR063.QRData);
            if (QR064 != null && (string.Compare(QRVersion?.Version, Constants.QRVersion.Jahis8) >= 0)) ret = _appendStr(ret, QR064.QRData);
            if (QR081 != null) ret = _appendStr(ret, QR081.QRData);
            if (QRRps != null && QRRps.Any())
            {
                foreach (CoOutDrugQRRp qrRp in QRRps)
                {
                    if (qrRp != null)
                    {
                        ret = _appendStr(ret, qrRp.QRData);
                    }
                }
            }

            ret = _appendStr(ret, '\x1A'.ToString());

            return ret;
        }
    }
}
