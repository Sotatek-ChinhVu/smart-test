namespace Helper.Constants
{
    public static class HokenConstant
    {
        public const string HOUBETU_NASHI = "0";
        public const string HOUBETU_KOKUHO = "100";
        public const string HOUBETU_JIHI_108 = "108";
        public const string HOUBETU_JIHI_109 = "109";
        public const string HOUBETU_MARUCHO = "102";
        public const string HOUBETU_ROUSAI = "103";
        public const string HOUBETU_JIBAI = "104";
        public const string HOUBETU_39 = "39";
        public const string HOUBETU_67 = "67";
        public const string HOUBETU_27 = "27";
        public const string HOUBETU_01 = "01";
    }
    /// <summary>
    /// 医科円点レート
    /// </summary>
    public static class EntenRate
    {
        public const int Val = 10;
    }

    /// <summary>
    /// 保険種別区分
    /// </summary>
    public static class HokenSbtKbn
    {
        public const int None = 0;
        public const int Hoken = 1;
        public const int Choki = 2;
        public const int Seiho = 5;
        public const int Bunten = 6;
        public const int Ippan = 7;
    }

    /// <summary>
    /// 保険区分
    /// </summary>
    public static class HokenKbn
    {
        public const int Jihi = 0;
        public const int Syaho = 1;
        public const int Kokho = 2;
        public const int RousaiShort = 11;
        public const int RousaiLong = 12;
        public const int AfterCare = 13;
        public const int Jibai = 14;
    }

    /// <summary>
    /// 本人家族区分
    ///  1:本人
    ///  2:家族
    /// </summary>
    public static class HonkeKbn
    {
        public const int Mine = 1;
        public const int Family = 2;
    }

    /// <summary>
    /// 上限管理区分
    /// </summary>
    public static class IsLimitList
    {
        public const int Yes = 1;
    }

    /// <summary>
    /// 上限管理総額表示区分
    /// </summary>
    public static class IsLimitListSum
    {
        public const int Yes = 1;
    }

    /// <summary>
    /// 国保減免区分
    /// </summary>
    public static class GenmenKbn
    {
        public const int Gengaku = 1;
        public const int Menjyo = 2;
        public const int Yuyo = 3;
        public const int Jiritusien = 4;
    }

    /// <summary>
    /// 高額療養費超過区分(0:なし 1:あり 2:あり[1円単位])
    /// </summary>
    public static class KogakuOverStatus
    {
        public const int None = 0;
        public const int Over = 1;
        public const int OverOneYen = 2;
    }

    /// <summary>
    /// 高額療養費処理区分
    ///  1:高額委任払い 
    ///  2:適用区分一般
    /// </summary>
    public static class KogakuType
    {
        public const int TekiyoIppan = 2;
    }

    /// <summary>
    /// レセプト記載   
    ///  0:記載あり
    ///  1:上限未満記載なし
    ///  2:上限以下記載なし
    ///  3:記載なし
    /// </summary>
    public static class ReceKisai
    {
        public const int None = 3;
    }

    /// <summary>
    /// 公費法別
    /// </summary>
    public static class KohiHoubetu
    {
        public const string Choki = "102";
    }

    /// <summary>
    /// 高額療養費の合算対象となる一部負担金
    /// </summary>
    public static class KogakuIchibu
    {
        public const int BorderVal = 21000;
    }
}
