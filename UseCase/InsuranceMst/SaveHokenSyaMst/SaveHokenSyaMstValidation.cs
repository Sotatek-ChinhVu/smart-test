using System.ComponentModel;

namespace UseCase.InsuranceMst.SaveHokenSyaMst
{
    public enum SaveHokenSyaMstValidation
    {
        [Description("HpId property is valid")]
        InvalidHpId,
        [Description("Name property is valid")]
        InvalidName,
        [Description("KanaName property is valid")]
        InvalidKanaName,
        [Description("HoubetuKbn property is valid")]
        InvalidHoubetuKbn,
        [Description("Houbetu property is valid")]
        InvalidHoubetu,
        [Description("HokenKbn property is valid")]
        InvalidHokenKbn,
        [Description("PrefNo property is valid")]
        InvalidPrefNo,
        [Description("HokensyaNo property is valid")]
        InvalidHokensyaNo,
        [Description("Kigo property is valid")]
        InvalidKigo,
        [Description("Bango property is valid")]
        InvalidBango,
        [Description("RateHonnin property is valid")]
        InvalidRateHonnin,
        [Description("RateKazoku property is valid")]
        InvalidRateKazoku,
        [Description("PostCode property is valid")]
        InvalidPostCode,
        [Description("Address1 property is valid")]
        InvalidAddress1,
        [Description("Address2 property is valid")]
        InvalidAddress2,
        [Description("Tel1 property is valid")]
        InvalidTel1
    }
}
