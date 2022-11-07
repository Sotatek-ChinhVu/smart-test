namespace EmrCloudApi.Tenant.Requests.InsuranceMst
{
    public class SaveHokenSyaMstRequest
    {
        public string Name { get; set; } = string.Empty;

        public string KanaName { get; set; } = string.Empty;

        public string HoubetuKbn { get; set; } = string.Empty;

        public string Houbetu { get; set; } = string.Empty;

        public int HokenKbn { get; set; }

        public int PrefNo { get; set; }

        public string HokensyaNo { get; set; } = string.Empty;

        public string Kigo { get; set; } = string.Empty;

        public string Bango { get; set; } = string.Empty;

        public int RateHonnin { get; set; }

        public int RateKazoku { get; set; }

        public string PostCode { get; set; } = string.Empty;

        public string Address1 { get; set; } = string.Empty;

        public string Address2 { get; set; } = string.Empty;

        public string Tel1 { get; set; } = string.Empty;

        public int IsKigoNa { get; set; }
    }
}