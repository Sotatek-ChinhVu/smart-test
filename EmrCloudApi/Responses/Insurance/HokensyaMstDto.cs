using Domain.Models.Insurance;
using Domain.Models.InsuranceMst;

namespace EmrCloudApi.Responses.Insurance
{
    public class HokensyaMstDto
    {
        public HokensyaMstDto(HokensyaMstModel hokensyaMstModel)
        {
            HpId = hokensyaMstModel.HpId;
            Name = hokensyaMstModel.Name;
            KanaName = hokensyaMstModel.KanaName;
            HoubetuKbn = hokensyaMstModel.HoubetuKbn;
            Houbetu = hokensyaMstModel.Houbetu;
            HokenKbn = hokensyaMstModel.HokenKbn;
            PrefNo = hokensyaMstModel.PrefNo;
            HokensyaNo = hokensyaMstModel.HokensyaNo;
            Kigo = hokensyaMstModel.Kigo;
            Bango = hokensyaMstModel.Bango;
            RateHonnin = hokensyaMstModel.RateHonnin;
            RateKazoku = hokensyaMstModel.RateKazoku;
            PostCode = hokensyaMstModel.PostCode;
            Address1 = hokensyaMstModel.Address1;
            Address2 = hokensyaMstModel.Address2;
            Tel1 = hokensyaMstModel.Tel1;
            IsKigoNa = hokensyaMstModel.IsKigoNa;
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
    }
}
