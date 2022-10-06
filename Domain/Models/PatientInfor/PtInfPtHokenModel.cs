namespace Domain.Models.PatientInfor
{
    public class PtInfPtHokenModel
    {
        public PtInfPtHokenModel(long seqNo, int hokenId, int hokenNo, int hokenEdaNo, string hokensyaNo, string kigo, string bango, int honkeKbn, int hokenKbn, string houbetu, string hokensyaName, string hokensyaPost, string hokensyaAddress, string hokensyaTel, int sikakuDate, int kofuDate, int startDate, int endDate, int rate, int gendogaku, int keizokuKbn, int kogakuKbn, int kogakuType, int tokureiYm1, int tokureiYm2, int tasukaiYm, int syokumuKbn, int genmenKbn, int genmenRate, int genmenGaku, string tokki1, string tokki2, string tokki3, string tokki4, string tokki5, string rousaiKofuNo, int rousaiSaigaiKbn, string rousaiJigyosyoName, string rousaiPrefName, string rousaiCityName, int rousaiSyobyoDate, string rousaiSyobyoCd, string rousaiRoudouCd, string rousaiKantokuCd, int rousaiReceCount, string jibaiHokenName, string jibaiHokenTanto, string jibaiHokenTel, int jibaiJyusyouDate, int ryoyoStartDate, int ryoyoEndDate, string edaNo, List<PtHokenCheckModel> hokenChecks)
        {
            SeqNo = seqNo;
            HokenId = hokenId;
            HokenNo = hokenNo;
            HokenEdaNo = hokenEdaNo;
            HokensyaNo = hokensyaNo;
            Kigo = kigo;
            Bango = bango;
            HonkeKbn = honkeKbn;
            HokenKbn = hokenKbn;
            Houbetu = houbetu;
            HokensyaName = hokensyaName;
            HokensyaPost = hokensyaPost;
            HokensyaAddress = hokensyaAddress;
            HokensyaTel = hokensyaTel;
            SikakuDate = sikakuDate;
            KofuDate = kofuDate;
            StartDate = startDate;
            EndDate = endDate;
            Rate = rate;
            Gendogaku = gendogaku;
            KeizokuKbn = keizokuKbn;
            KogakuKbn = kogakuKbn;
            KogakuType = kogakuType;
            TokureiYm1 = tokureiYm1;
            TokureiYm2 = tokureiYm2;
            TasukaiYm = tasukaiYm;
            SyokumuKbn = syokumuKbn;
            GenmenKbn = genmenKbn;
            GenmenRate = genmenRate;
            GenmenGaku = genmenGaku;
            Tokki1 = tokki1;
            Tokki2 = tokki2;
            Tokki3 = tokki3;
            Tokki4 = tokki4;
            Tokki5 = tokki5;
            RousaiKofuNo = rousaiKofuNo;
            RousaiSaigaiKbn = rousaiSaigaiKbn;
            RousaiJigyosyoName = rousaiJigyosyoName;
            RousaiPrefName = rousaiPrefName;
            RousaiCityName = rousaiCityName;
            RousaiSyobyoDate = rousaiSyobyoDate;
            RousaiSyobyoCd = rousaiSyobyoCd;
            RousaiRoudouCd = rousaiRoudouCd;
            RousaiKantokuCd = rousaiKantokuCd;
            RousaiReceCount = rousaiReceCount;
            JibaiHokenName = jibaiHokenName;
            JibaiHokenTanto = jibaiHokenTanto;
            JibaiHokenTel = jibaiHokenTel;
            JibaiJyusyouDate = jibaiJyusyouDate;
            RyoyoStartDate = ryoyoStartDate;
            RyoyoEndDate = ryoyoEndDate;
            EdaNo = edaNo;
            HokenChecks = hokenChecks;
        }

        public long SeqNo { get;private set; }
        public int HokenId { get; private set; }
        public int HokenNo { get; private set; }
        public int HokenEdaNo { get; private set; }
        public string HokensyaNo { get; private set; }
        public string Kigo { get; private set; }
        public string Bango { get; private set; }
        public int HonkeKbn { get; private set; }
        public int HokenKbn { get; private set; }
        public string Houbetu { get; private set; }
        public string HokensyaName { get; private set; }
        public string HokensyaPost { get; private set; }
        public string HokensyaAddress { get; private set; }
        public string HokensyaTel { get; private set; }
        public int SikakuDate { get; private set; }
        public int KofuDate { get; private set; }
        public int StartDate { get; private set; }
        public int EndDate { get; private set; }
        public int Rate { get; private set; }
        public int Gendogaku { get; private set; }
        public int KeizokuKbn { get; private set; }
        public int KogakuKbn { get; private set; }
        public int KogakuType { get; private set; }
        public int TokureiYm1 { get; private set; }
        public int TokureiYm2 { get; private set; }
        public int TasukaiYm { get; private set; }
        public int SyokumuKbn { get; private set; }
        public int GenmenKbn { get; private set; }
        public int GenmenRate { get; private set; }
        public int GenmenGaku { get; private set; }
        public string Tokki1 { get; private set; }
        public string Tokki2 { get; private set; }
        public string Tokki3 { get; private set; }
        public string Tokki4 { get; private set; }
        public string Tokki5 { get; private set; }
        public string RousaiKofuNo { get; private set; }
        public int RousaiSaigaiKbn { get; private set; }
        public string RousaiJigyosyoName { get; private set; }
        public string RousaiPrefName { get; private set; }
        public string RousaiCityName { get; private set; }
        public int RousaiSyobyoDate { get; private set; }
        public string RousaiSyobyoCd { get; private set; }
        public string RousaiRoudouCd { get; private set; }
        public string RousaiKantokuCd { get; private set; }
        public int RousaiReceCount { get; private set; }
        public string JibaiHokenName { get; private set; }
        public string JibaiHokenTanto { get; private set; }
        public string JibaiHokenTel { get; private set; }
        public int JibaiJyusyouDate { get; private set; }
        public int RyoyoStartDate { get; private set; }
        public int RyoyoEndDate { get; private set; }
        public string EdaNo { get; private set; }
        public List<PtHokenCheckModel> HokenChecks { get; private set; } = new List<PtHokenCheckModel>();
    }
}
