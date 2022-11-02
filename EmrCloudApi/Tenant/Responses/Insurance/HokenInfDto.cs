using Domain.Constant;
using Domain.Models.Insurance;

namespace EmrCloudApi.Tenant.Responses.Insurance
{
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
}
