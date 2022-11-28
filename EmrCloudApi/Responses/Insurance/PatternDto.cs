using Domain.Constant;
using Domain.Models.InsuranceInfor;

namespace EmrCloudApi.Responses.Insurance
{
    public class PatternDto
    {
        public PatternDto(InsuranceModel insuranceModel)
        {
            HpId = insuranceModel.HpId;
            PtId = insuranceModel.PtId;
            PtBirthday = insuranceModel.PtBirthday;
            SeqNo = insuranceModel.SeqNo;
            HokenSbtCd = insuranceModel.HokenSbtCd;
            HokenPid = insuranceModel.HokenPid;
            HokenKbn = insuranceModel.HokenKbn;
            SinDate = insuranceModel.SinDate;
            HokenMemo = insuranceModel.HokenMemo;
            HokenInf = new HokenInfDto(insuranceModel.HokenInf);
            Kohi1 = new KohiInfDto(insuranceModel.Kohi1);
            Kohi2 = new KohiInfDto(insuranceModel.Kohi2);
            Kohi3 = new KohiInfDto(insuranceModel.Kohi3);
            Kohi4 = new KohiInfDto(insuranceModel.Kohi4);
            IsDeleted = insuranceModel.IsDeleted;
            StartDate = insuranceModel.StartDate;
            EndDate = insuranceModel.EndDate;
            IsAddNew = insuranceModel.IsAddNew;

            DisplayRateOnly = insuranceModel.DisplayRateOnly;
            HokenName = insuranceModel.HokenName;
            PatternRate = insuranceModel.PatternRate;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int PtBirthday { get; private set; }

        public long SeqNo { get; private set; }

        public int HokenSbtCd { get; private set; }

        public int HokenPid { get; private set; }

        public int HokenKbn { get; private set; }

        public string HokenMemo { get; private set; }

        public int SinDate { get; private set; }

        public int IsDeleted { get; private set; }

        public HokenInfDto HokenInf { get; private set; }

        public KohiInfDto Kohi1 { get; private set; }

        public KohiInfDto Kohi2 { get; private set; }

        public KohiInfDto Kohi3 { get; private set; }

        public KohiInfDto Kohi4 { get; private set; }

        public int HoubetuPoint(List<string> houbetuList)
        {
            int point = 0;
            if (!IsEmptyHoken && !HokenInf.IsNoHoken) point++;
            if (!IsEmptyKohi1 && houbetuList.Contains(Kohi1.Houbetu)) point++;
            if (!IsEmptyKohi2 && houbetuList.Contains(Kohi2.Houbetu)) point++;
            if (!IsEmptyKohi3 && houbetuList.Contains(Kohi3.Houbetu)) point++;
            if (!IsEmptyKohi4 && houbetuList.Contains(Kohi4.Houbetu)) point++;
            return point;
        }

        public int KohiCount
        {
            get
            {
                int count = 0;
                if (!IsEmptyKohi1) count++;
                if (!IsEmptyKohi2) count++;
                if (!IsEmptyKohi3) count++;
                if (!IsEmptyKohi4) count++;
                return count;
            }
        }

        public List<KohiInfDto> BuntenKohis
        {
            get
            {
                var result = new List<KohiInfDto>();
                if (!IsEmptyKohi1 && Kohi1.HokenSbtKbn == 6) result.Add(Kohi1);
                if (!IsEmptyKohi2 && Kohi2.HokenSbtKbn == 6) result.Add(Kohi2);
                if (!IsEmptyKohi3 && Kohi3.HokenSbtKbn == 6) result.Add(Kohi3);
                if (!IsEmptyKohi4 && Kohi4.HokenSbtKbn == 6) result.Add(Kohi4);
                return result;
            }
        }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public string DisplayRateOnly { get; private set; }

        public string HokenName { get; private set; }

        public bool IsEmptyHoken => (HokenInf.HokenId == 0);

        public bool IsEmptyKohi1 => (Kohi1 == null || Kohi1.HokenId == 0);

        public bool IsEmptyKohi2 => (Kohi2 == null || Kohi2.HokenId == 0);

        public bool IsEmptyKohi3 => (Kohi3 == null || Kohi3.HokenId == 0);

        public bool IsEmptyKohi4 => (Kohi4 == null || Kohi4.HokenId == 0);

        public string PatternRate { get; private set; }

        public bool IsShaho
        {
            get => HokenKbn == 1 && HokenInf.Houbetu != HokenConstant.HOUBETU_NASHI;
        }

        public bool IsKokuho
        {
            get => HokenKbn == 2;
        }

        public bool IsNoHoken
        {
            get
            {
                if (HokenInf != null)
                {
                    return HokenInf.HokenMst.HokenSbtKbn == 0;
                }
                return HokenKbn == 1 && HokenInf?.Houbetu == HokenConstant.HOUBETU_NASHI;
            }
        }

        public bool IsJibaiOrRosai
        {
            get { return HokenKbn >= 11 && HokenKbn <= 14; }
        }

        public bool IsAddNew { get; private set; }

        public bool IsExpirated => !(StartDate <= SinDate && EndDate >= SinDate);
    }
}
