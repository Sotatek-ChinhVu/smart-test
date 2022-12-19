using Domain.Models.InsuranceMst;

namespace EmrCloudApi.Responses.InsuranceMst
{
    public class HokenSyaMstDto
    {
        public HokenSyaMstDto(HokensyaMstModel hokenSyaMst)
        {
            HpId = hokenSyaMst.HpId;
            Name = hokenSyaMst.Name;
            KanaName = hokenSyaMst.KanaName;
            HoubetuKbn = hokenSyaMst.HoubetuKbn;
            Houbetu = hokenSyaMst.Houbetu;
            HokenKbn = hokenSyaMst.HokenKbn;
            PrefNo = hokenSyaMst.PrefNo;
            HokensyaNo = hokenSyaMst.HokensyaNo;
            Kigo = hokenSyaMst.Kigo;
            Bango = hokenSyaMst.Bango;
            RateHonnin = hokenSyaMst.RateHonnin;
            RateKazoku = hokenSyaMst.RateKazoku;
            PostCode = hokenSyaMst.PostCode;
            Address1 = hokenSyaMst.Address1;
            Address2 = hokenSyaMst.Address2;
            Tel1 = hokenSyaMst.Tel1;
            IsKigoNa = hokenSyaMst.IsKigoNa;
        }

        public int HpId { get; private set; }

        public string Name { get; private set; }

        public string KanaName { get; private set; }

        public string HoubetuKbn { get; private set; }

        public string Houbetu { get; private set; }

        public int HokenKbn { get; private set; }

        public int PrefNo { get; private set; }

        public string HokensyaNo { get; private set; }

        public string Kigo { get; private set; }

        public string Bango { get; private set; }

        public int RateHonnin { get; private set; }

        public int RateKazoku { get; private set; }

        public string PostCode { get; private set; }

        public string Address1 { get; private set; }

        public string Address2 { get; private set; }

        public string Tel1 { get; private set; }

        public int IsKigoNa { get; private set; }

        public bool IsReadOnlyHokenSyaNo
        {
            get { return !string.IsNullOrEmpty(HokensyaNo); }
        }

        public string PostCdDisplay
        {
            get
            {
                if (!string.IsNullOrEmpty(PostCode))
                {
                    if (PostCode.Length > 3)
                    {
                        return PostCode.Substring(0, 3) + "-" + PostCode.Substring(3);
                    }
                }
                return PostCode;
            }
        }
    }
}
