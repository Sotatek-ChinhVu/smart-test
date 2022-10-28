using Domain.Constant;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using System.Linq.Expressions;
using HokenMstModel = Domain.Models.Insurance.HokenMstModel;
using HokensyaMstModel = Domain.Models.Insurance.HokensyaMstModel;

namespace EmrCloudApi.Tenant.Responses.InsuranceList
{
    public class GetInsuranceListResponse
    {
        public PatientInsuranceDto Data { get; set; } = new PatientInsuranceDto();
    }

    public class PatientInsuranceDto
    {
        public PatientInsuranceDto(List<InsuranceModel> listInsurance, List<HokenInfModel> listHokenInf, List<KohiInfModel> listKohi)
        {
            ListInsurance = listInsurance.Select(i => new PatternDto(i)).ToList();
            ListHokenInf = listHokenInf.Select(h => new HokenInfDto(h)).ToList();
            ListKohi = listKohi.Select(k => new KohiInfDto(k)).ToList();
        }

        public PatientInsuranceDto()
        {
            ListInsurance = new List<PatternDto>();
            ListHokenInf = new List<HokenInfDto>();
            ListKohi = new List<KohiInfDto>();
        }

        public List<PatternDto> ListInsurance { get; private set; }

        public List<HokenInfDto> ListHokenInf { get; private set; }

        public List<KohiInfDto> ListKohi { get; private set; }
    }

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

    public class HokenInfDto
    {
        public HokenInfDto(HokenInfModel hokenInfModel)
        {
            HpId = hokenInfModel.HpId;
            PtId = hokenInfModel.PtId;
            HokenId = hokenInfModel.HokenId;
            SeqNo = hokenInfModel.SeqNo;
            HokenNo = hokenInfModel.HokenNo;
            HokenEdaNo = hokenInfModel.HokenEdaNo;
            HokenKbn = hokenInfModel.HokenKbn;
            HokensyaNo = hokenInfModel.HokensyaNo;
            Kigo = hokenInfModel.Kigo;
            Bango = hokenInfModel.Bango;
            EdaNo = hokenInfModel.EdaNo;
            HonkeKbn = hokenInfModel.HonkeKbn;
            StartDate = hokenInfModel.StartDate;
            EndDate = hokenInfModel.EndDate;
            SikakuDate = hokenInfModel.SikakuDate;
            KofuDate = hokenInfModel.KofuDate;
            ConfirmDate = hokenInfModel.ConfirmDate;
            KogakuKbn = hokenInfModel.KogakuKbn;
            TasukaiYm = hokenInfModel.TasukaiYm;
            TokureiYm1 = hokenInfModel.TokureiYm1;
            TokureiYm2 = hokenInfModel.TokureiYm2;
            GenmenKbn = hokenInfModel.GenmenKbn;
            GenmenRate = hokenInfModel.GenmenRate;
            GenmenGaku = hokenInfModel.GenmenGaku;
            SyokumuKbn = hokenInfModel.SyokumuKbn;
            KeizokuKbn = hokenInfModel.KeizokuKbn;
            Tokki1 = hokenInfModel.Tokki1;
            Tokki2 = hokenInfModel.Tokki2;
            Tokki3 = hokenInfModel.Tokki3;
            Tokki4 = hokenInfModel.Tokki4;
            Tokki5 = hokenInfModel.Tokki5;
            RousaiKofuNo = hokenInfModel.RousaiKofuNo;
            NenkinBango = hokenInfModel.NenkinBango;
            RousaiRoudouCd = hokenInfModel.RousaiRoudouCd;
            KenkoKanriBango = hokenInfModel.KenkoKanriBango;
            RousaiSaigaiKbn = hokenInfModel.RousaiSaigaiKbn;
            RousaiKantokuCd = hokenInfModel.RousaiKantokuCd;
            RousaiSyobyoDate = hokenInfModel.RousaiSyobyoDate;
            RyoyoStartDate = hokenInfModel.RyoyoStartDate;
            RyoyoEndDate = hokenInfModel.RyoyoEndDate;
            RousaiSyobyoCd = hokenInfModel.RousaiSyobyoCd;
            RousaiJigyosyoName = hokenInfModel.RousaiJigyosyoName;
            RousaiPrefName = hokenInfModel.RousaiPrefName;
            RousaiCityName = hokenInfModel.RousaiCityName;
            RousaiReceCount = hokenInfModel.RousaiReceCount;
            HokensyaName = hokenInfModel.HokensyaName;
            HokensyaAddress = hokenInfModel.HokensyaAddress;
            HokensyaTel = hokenInfModel.HokensyaTel;
            SinDate = hokenInfModel.SinDate;
            JibaiHokenName = hokenInfModel.JibaiHokenName;
            JibaiHokenTanto = hokenInfModel.JibaiHokenTanto;
            JibaiHokenTel = hokenInfModel.JibaiHokenTel;
            JibaiJyusyouDate = hokenInfModel.JibaiJyusyouDate;
            Houbetu = hokenInfModel.Houbetu;
            ConfirmDateList = hokenInfModel.ConfirmDateList.Select(c => new ConfirmDateDto(c)).ToList();
            ListRousaiTenki = hokenInfModel.ListRousaiTenki.Select(r => new RousaiTenkiDto(r)).ToList();
            IsReceKisaiOrNoHoken = hokenInfModel.IsReceKisaiOrNoHoken;
            IsDeleted = hokenInfModel.IsDeleted;
            HokenMst = new HokenMstDto(hokenInfModel.HokenMst);
            IsAddNew = hokenInfModel.IsAddNew;
            IsAddHokenCheck = hokenInfModel.IsAddHokenCheck;
            RodoBango = hokenInfModel.RodoBango;
            HokensyaMst = new HokensyaMstDto(hokenInfModel.HokensyaMst);
        }

        public List<ConfirmDateDto> ConfirmDateList { get; private set; }

        public List<RousaiTenkiDto> ListRousaiTenki { get; private set; }

        public HokenMstDto HokenMst { get; private set; }

        public HokensyaMstDto HokensyaMst { get; private set; }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int HokenId { get; private set; }

        public long SeqNo { get; private set; }

        public int HokenNo { get; private set; }

        public int HokenEdaNo { get; private set; }

        public int HokenKbn { get; private set; }

        public string HokensyaNo { get; private set; }

        public string Kigo { get; private set; }

        public string Bango { get; private set; }

        public string EdaNo { get; private set; }

        public int HonkeKbn { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public int SikakuDate { get; private set; }

        public int KofuDate { get; private set; }

        public int ConfirmDate { get; private set; }

        public int KogakuKbn { get; private set; }

        public int TasukaiYm { get; private set; }

        public int TokureiYm1 { get; private set; }

        public int TokureiYm2 { get; private set; }

        public int GenmenKbn { get; private set; }

        public int GenmenRate { get; private set; }

        public int GenmenGaku { get; private set; }

        public int SyokumuKbn { get; private set; }

        public int KeizokuKbn { get; private set; }

        public string Tokki1 { get; private set; }

        public string Tokki2 { get; private set; }

        public string Tokki3 { get; private set; }

        public string Tokki4 { get; private set; }

        public string Tokki5 { get; private set; }

        public string RousaiKofuNo { get; private set; }

        public string NenkinBango { get; private set; }

        public string RousaiRoudouCd { get; private set; }

        public string KenkoKanriBango { get; private set; }

        public int RousaiSaigaiKbn { get; private set; }

        public string RousaiKantokuCd { get; private set; }

        public int RousaiSyobyoDate { get; private set; }

        public int RyoyoStartDate { get; private set; }

        public int RyoyoEndDate { get; private set; }

        public string RousaiSyobyoCd { get; private set; }

        public string RousaiJigyosyoName { get; private set; }

        public string RousaiPrefName { get; private set; }

        public string RousaiCityName { get; private set; }

        public int RousaiReceCount { get; private set; }

        public int SinDate { get; private set; }

        public string JibaiHokenName { get; private set; }

        public string JibaiHokenTanto { get; private set; }

        public string JibaiHokenTel { get; private set; }

        public int JibaiJyusyouDate { get; private set; }

        public string Houbetu { get; private set; }

        public string HokensyaName { get; private set; }

        public string HokensyaAddress { get; private set; }

        public string HokensyaTel { get; private set; }

        public bool IsReceKisaiOrNoHoken { get; private set; }

        public int IsDeleted { get; private set; }

        public bool IsAddHokenCheck { get; private set; }

        public string RodoBango { get; private set; }

        public bool IsAddNew { get; private set; }

        public bool IsHaveHokenMst { get => HokenMst != null; }

        public string HokenMstHoubetu => HokenMst != null ? HokenMst.Houbetu : string.Empty;

        public int HokenMstFutanRate => HokenMst != null ? HokenMst.FutanRate : 0;

        public int HokenMstFutanKbn => HokenMst != null ? HokenMst.FutanKbn : 0;

        public int HokenMstSbtKbn => HokenMst != null ? HokenMst.HokenSbtKbn : 0;

        public int HokenMstStartDate => HokenMst != null ? HokenMst.StartDate : 0;

        public int HokenMstEndDate => HokenMst != null ? HokenMst.EndDate : 0;

        public string HokenMstDisplayTextMaster => HokenMst != null ? HokenMst.DisplayTextMaster : string.Empty;

        public bool IsJihi
        {
            get
            {
                if (IsHaveHokenMst)
                {
                    return HokenMst.HokenSbtKbn == 8;
                }
                return HokenKbn == 0 && (Houbetu == HokenConstant.HOUBETU_JIHI_108 || Houbetu == HokenConstant.HOUBETU_JIHI_109);
            }
        }

        public bool IsExpirated
        {
            get
            {
                return !(StartDate <= SinDate && EndDate >= SinDate);
            }
        }

        public bool IsShaho
        {
            get => HokenKbn == 1 && Houbetu != HokenConstant.HOUBETU_NASHI;
        }

        public bool IsKokuho
        {
            get => HokenKbn == 2;
        }

        public bool IsShahoOrKokuho => IsShaho || IsKokuho;

        public bool IsNoHoken
        {
            get
            {
                if (HokenMst != null)
                {
                    return HokenMst.HokenSbtKbn == 0;
                }
                return HokenKbn == 1 && Houbetu == HokenConstant.HOUBETU_NASHI;
            }
        }
    }

    public class KohiInfDto
    {
        public KohiInfDto(KohiInfModel kohiInfModel)
        {
            FutansyaNo = kohiInfModel.FutansyaNo;
            JyukyusyaNo = kohiInfModel.JyukyusyaNo;
            HokenId = kohiInfModel.HokenId;
            StartDate = kohiInfModel.StartDate;
            EndDate = kohiInfModel.EndDate;
            ConfirmDate = kohiInfModel.ConfirmDate;
            Rate = kohiInfModel.Rate;
            GendoGaku = kohiInfModel.GendoGaku;
            SikakuDate = kohiInfModel.SikakuDate;
            KofuDate = kohiInfModel.KofuDate;
            TokusyuNo = kohiInfModel.TokusyuNo;
            HokenSbtKbn = kohiInfModel.HokenSbtKbn;
            Houbetu = kohiInfModel.Houbetu;
            HokenMstModel = new HokenMstDto(kohiInfModel.HokenMstModel);
            HokenNo = kohiInfModel.HokenNo;
            HokenEdaNo = kohiInfModel.HokenEdaNo;
            PrefNo = kohiInfModel.PrefNo;
            SinDate = kohiInfModel.SinDate;
            ConfirmDateList = kohiInfModel.ConfirmDateList.Select(c => new ConfirmDateDto(c)).ToList();
            IsHaveKohiMst = kohiInfModel.IsHaveKohiMst;
            IsDeleted = kohiInfModel.IsDeleted;
            IsAddNew = kohiInfModel.IsAddNew;
        }

        public List<ConfirmDateDto> ConfirmDateList { get; private set; }

        public HokenMstDto HokenMstModel { get; private set; }

        public string FutansyaNo { get; private set; }

        public string JyukyusyaNo { get; private set; }

        public int HokenId { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public int ConfirmDate { get; private set; }

        public int Rate { get; private set; }

        public int GendoGaku { get; private set; }

        public int SikakuDate { get; private set; }

        public int KofuDate { get; private set; }

        public string TokusyuNo { get; private set; }

        public int HokenSbtKbn { get; private set; }

        public string Houbetu { get; private set; }

        public int HokenNo { get; private set; }

        public int HokenEdaNo { get; private set; }

        public int PrefNo { get; private set; }

        public int SinDate { get; private set; }

        public bool IsHaveKohiMst { get; private set; }

        public int IsDeleted { get; private set; }

        public bool IsAddNew { get; private set; }

        public bool IsEmptyModel => HokenId == 0;

        public bool IsExpirated
        {
            get
            {
                return !(StartDate <= SinDate && EndDate >= SinDate);
            }
        }
    }

    public class HokenMstDto
    {
        public HokenMstDto(HokenMstModel hokenMstModel)
        {
            FutanKbn = hokenMstModel.FutanKbn;
            FutanRate = hokenMstModel.FutanRate;
            StartDate = hokenMstModel.StartDate;
            EndDate = hokenMstModel.EndDate;
            HokenNo = hokenMstModel.HokenNo;
            HokenEdaNo = hokenMstModel.HokenEdaNo;
            HokenSName = hokenMstModel.HokenSName;
            Houbetu = hokenMstModel.Houbetu;
            HokenSbtKbn = hokenMstModel.HokenSbtKbn;
            CheckDigit = hokenMstModel.CheckDigit;
            AgeStart = hokenMstModel.AgeStart;
            AgeEnd = hokenMstModel.AgeEnd;
            JyuKyuCheckDigit = hokenMstModel.JyuKyuCheckDigit;
            FutansyaCheckFlag = hokenMstModel.FutansyaCheckFlag;
            JyukyusyaCheckFlag = hokenMstModel.JyukyusyaCheckFlag;
            TokusyuCheckFlag = hokenMstModel.TokusyuCheckFlag;
        }

        public int FutanKbn { get; private set; }

        public int FutanRate { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public int HokenNo { get; private set; }

        public int HokenEdaNo { get; private set; }

        public string HokenSName { get; private set; }

        public string Houbetu { get; private set; }

        public int HokenSbtKbn { get; private set; }

        public int CheckDigit { get; private set; }

        public int AgeStart { get; private set; }

        public int AgeEnd { get; private set; }

        public int FutansyaCheckFlag { get; private set; }

        public int JyukyusyaCheckFlag { get; private set; }

        public int JyuKyuCheckDigit { get; private set; }

        public int TokusyuCheckFlag { get; private set; }

        public string SelectedValueMaster
        {
            get
            {
                string result = string.Empty;
                if (HokenEdaNo == 0)
                {
                    result = HokenNo.ToString().PadLeft(3, '0');
                }
                else
                {
                    result = HokenNo.ToString().PadLeft(3, '0') + HokenEdaNo;
                }

                return result;
            }
        }

        public string DisplayTextMaster
        {
            get
            {
                string DisplayText = SelectedValueMaster + " " + HokenSName;
                return DisplayText;
            }
        }
    }

    public class HokensyaMstDto
    {
        public HokensyaMstDto(HokensyaMstModel hokensyaMstModel)
        {
            IsKigoNa = hokensyaMstModel.IsKigoNa;
        }

        public int IsKigoNa { get; private set; }
    }

    public class RousaiTenkiDto
    {
        public RousaiTenkiDto(RousaiTenkiModel rousaiTenkiModel)
        {
            RousaiTenkiSinkei = rousaiTenkiModel.RousaiTenkiSinkei;
            RousaiTenkiTenki = rousaiTenkiModel.RousaiTenkiTenki;
            RousaiTenkiEndDate = rousaiTenkiModel.RousaiTenkiEndDate;
            RousaiTenkiIsDeleted = rousaiTenkiModel.RousaiTenkiIsDeleted;
            SeqNo = rousaiTenkiModel.SeqNo;
        }

        public int RousaiTenkiSinkei { get; private set; }

        public int RousaiTenkiTenki { get; private set; }

        public int RousaiTenkiEndDate { get; private set; }

        public int RousaiTenkiIsDeleted { get; private set; }

        public long SeqNo { get; private set; }
    }

    public class ConfirmDateDto
    {
        public int HokenGrp { get; private set; }

        public int HokenId { get; private set; }

        public long SeqNo { get; private set; }

        public int CheckId { get; private set; }

        public string CheckName { get; private set; }

        public string CheckComment { get; private set; }

        public int ConfirmDate { get; private set; }

        public ConfirmDateDto(ConfirmDateModel confirmDateModel)
        {
            HokenGrp = confirmDateModel.HokenGrp;
            HokenId = confirmDateModel.HokenId;
            SeqNo = confirmDateModel.SeqNo;
            CheckId = confirmDateModel.CheckId;
            CheckName = confirmDateModel.CheckName;
            CheckComment = confirmDateModel.CheckComment;
            ConfirmDate = confirmDateModel.ConfirmDate;
        }
    }
}