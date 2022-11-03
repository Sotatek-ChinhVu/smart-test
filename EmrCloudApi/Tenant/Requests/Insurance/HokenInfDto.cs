using EmrCloudApi.Tenant.Requests.ConfirmDate;
using EmrCloudApi.Tenant.Requests.RousaiTenki;

namespace EmrCloudApi.Tenant.Requests.Insurance
{
    public class HokenInfDto
    {
        public HokenInfDto(int hpId, long ptId, int hokenId, long seqNo, int hokenNo, string edaNo, int hokenEdaNo, string hokensyaNo, string kigo, string bango, int honkeKbn, int hokenKbn, string houbetu, int keizokuKbn, int sikakuDate, int kofuDate, int startDate, int endDate, int kogakuKbn, int tokureiYm1, int tokureiYm2, int tasukaiYm, int syokumuKbn, int genmenKbn, int genmenRate, int genmenGaku, string tokki1, string tokki2, string tokki3, string tokki4, string tokki5, string rousaiKofuNo, int rousaiSaigaiKbn, string rousaiJigyosyoName, string rousaiPrefName, string rousaiCityName, int rousaiSyobyoDate, string rousaiSyobyoCd, string rousaiRoudouCd, string rousaiKantokuCd, int rousaiReceCount, int ryoyoStartDate, int ryoyoEndDate, string jibaiHokenName, string jibaiHokenTanto, string jibaiHokenTel, int jibaiJyusyouDate, int isDeleted, List<ConfirmDateDto> confirmDates, List<RousaiTenkiDto> rousaiTenkis, bool isAddNew)
        {
            HpId = hpId;
            PtId = ptId;
            HokenId = hokenId;
            SeqNo = seqNo;
            HokenNo = hokenNo;
            EdaNo = edaNo;
            HokenEdaNo = hokenEdaNo;
            HokensyaNo = hokensyaNo;
            Kigo = kigo;
            Bango = bango;
            HonkeKbn = honkeKbn;
            HokenKbn = hokenKbn;
            Houbetu = houbetu;
            KeizokuKbn = keizokuKbn;
            SikakuDate = sikakuDate;
            KofuDate = kofuDate;
            StartDate = startDate;
            EndDate = endDate;
            KogakuKbn = kogakuKbn;
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
            RyoyoStartDate = ryoyoStartDate;
            RyoyoEndDate = ryoyoEndDate;
            JibaiHokenName = jibaiHokenName;
            JibaiHokenTanto = jibaiHokenTanto;
            JibaiHokenTel = jibaiHokenTel;
            JibaiJyusyouDate = jibaiJyusyouDate;
            IsDeleted = isDeleted;
            ConfirmDates = confirmDates;
            RousaiTenkis = rousaiTenkis;
            IsAddNew = isAddNew;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int HokenId { get; private set; }

        public long SeqNo { get; private set; }

        public int HokenNo { get; private set; }

        public string EdaNo { get; private set; }

        public int HokenEdaNo { get; private set; }

        public string HokensyaNo { get; private set; }

        public string Kigo { get; private set; }

        public string Bango { get; private set; }

        public int HonkeKbn { get; private set; }

        public int HokenKbn { get; private set; }

        public string Houbetu { get; private set; }

        public int KeizokuKbn { get; private set; }

        public int SikakuDate { get; private set; }

        public int KofuDate { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public int KogakuKbn { get; private set; }

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

        public int RyoyoStartDate { get; private set; }

        public int RyoyoEndDate { get; private set; }

        public string JibaiHokenName { get; private set; }

        public string JibaiHokenTanto { get; private set; }

        public string JibaiHokenTel { get; private set; }

        public int JibaiJyusyouDate { get; private set; }

        public int IsDeleted { get; private set; }

        public List<ConfirmDateDto> ConfirmDates { get; private set; }

        public List<RousaiTenkiDto> RousaiTenkis { get; private set; }

        public bool IsAddNew { get; private set; }
    }
}
