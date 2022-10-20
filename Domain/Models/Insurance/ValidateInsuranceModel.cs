using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Insurance
{
    public class ValidateInsuranceModel
    {
        public ValidateInsuranceModel(SelectedHokenInf selectedHokenInf, SelectedHokenPattern selectedHokenPattern, SelectedHokenMst selectedHokenMst)
        {
            SelectedHokenInf = selectedHokenInf;
            SelectedHokenPattern = selectedHokenPattern;
            SelectedHokenMst = selectedHokenMst;
        }
        public SelectedHokenInf SelectedHokenInf { get; private set; }

        public SelectedHokenPattern SelectedHokenPattern { get; private set; }

        public SelectedHokenMst SelectedHokenMst { get; private set; }

        public bool IsHaveSelectedHokenPattern => SelectedHokenPattern != null;

        public bool IsHaveSelectedHokenInf => SelectedHokenInf != null;

        public bool IsHaveSelectedHokenMst => SelectedHokenMst != null;
    }

    public class SelectedHokenInf
    {
        public SelectedHokenInf(string hokensyaNo, string houbetu, int hokenNo, bool isAddNew, bool isJihi, int startDate, int endDate, int hokensyaMstIsKigoNa, string kigo, string bango, int honkeKbn, int tokureiYm1, int tokureiYm2, bool isShahoOrKokuho, bool isExpirated, bool isIsNoHoken, int confirmDate, bool isAddHokenCheck, string tokki1, string tokki2, string tokki3, string tokki4, string tokki5, string rodoBango, List<RousaiTenkiModel> listRousaiTenki, int rousaiSaigaiKbn, int rousaiSyobyoDate, string rousaiSyobyoCd, int ryoyoStartDate, int ryoyoEndDate, string nenkinBango, string kenkoKanriBango, int hokenId, int isDeleted)
        {
            HokensyaNo = hokensyaNo;
            Houbetu = houbetu;
            HokenNo = hokenNo;
            IsAddNew = isAddNew;
            IsJihi = isJihi;
            StartDate = startDate;
            EndDate = endDate;
            HokensyaMstIsKigoNa = hokensyaMstIsKigoNa;
            Kigo = kigo;
            Bango = bango;
            HonkeKbn = honkeKbn;
            TokureiYm1 = tokureiYm1;
            TokureiYm2 = tokureiYm2;
            IsShahoOrKokuho = isShahoOrKokuho;
            IsExpirated = isExpirated;
            IsIsNoHoken = isIsNoHoken;
            ConfirmDate = confirmDate;
            IsAddHokenCheck = isAddHokenCheck;
            Tokki1 = tokki1;
            Tokki2 = tokki2;
            Tokki3 = tokki3;
            Tokki4 = tokki4;
            Tokki5 = tokki5;
            RodoBango = rodoBango;
            ListRousaiTenki = listRousaiTenki;
            RousaiSaigaiKbn = rousaiSaigaiKbn;
            RousaiSyobyoDate = rousaiSyobyoDate;
            RousaiSyobyoCd = rousaiSyobyoCd;
            RyoyoStartDate = ryoyoStartDate;
            RyoyoEndDate = ryoyoEndDate;
            NenkinBango = nenkinBango;
            KenkoKanriBango = kenkoKanriBango;
            HokenId = hokenId;
            IsDeleted = isDeleted;
        }

        public string HokensyaNo { get; private set; }

        public string Houbetu { get; private set; }

        public int HokenNo { get; private set; }

        public bool IsAddNew { get; private set; }

        public bool IsJihi { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public int HokensyaMstIsKigoNa { get; private set; }

        public string Kigo { get; private set; }

        public string Bango { get; private set; }

        public int HonkeKbn { get; private set; }

        public int TokureiYm1 { get; private set; }

        public int TokureiYm2 { get; private set; }

        public bool IsShahoOrKokuho { get; private set; }

        public bool IsExpirated { get; private set; }

        public bool IsIsNoHoken { get; private set; }

        public int ConfirmDate { get; private set; }

        public bool IsAddHokenCheck { get; private set; }

        public string Tokki1 { get; private set; }

        public string Tokki2 { get; private set; }

        public string Tokki3 { get; private set; }

        public string Tokki4 { get; private set; }

        public string Tokki5 { get; private set; }

        public string RodoBango { get; private set; }

        public List<RousaiTenkiModel> ListRousaiTenki { get; private set; }

        public int RousaiSaigaiKbn { get; private set; }

        public int RousaiSyobyoDate { get; private set; }

        public string RousaiSyobyoCd { get; private set; }

        public int RyoyoStartDate { get; private set; }

        public int RyoyoEndDate { get; private set; }

        public string NenkinBango { get; private set; }

        public string KenkoKanriBango { get; private set; }

        public int HokenId { get; private set; }

        public int IsDeleted { get; private set; }
    }

    public class SelectedHokenPattern
    {
        public SelectedHokenPattern(int hokenKbn, SelectedKohiModel kohi1, SelectedKohiModel kohi2, SelectedKohiModel kohi3, SelectedKohiModel kohi4, bool isExpirated, bool isEmptyHoken, bool isAddNew, int isDeleted, int kohi1Id, int kohi2Id, int kohi3Id, int kohi4Id)
        {
            HokenKbn = hokenKbn;
            Kohi1 = kohi1;
            Kohi2 = kohi2;
            Kohi3 = kohi3;
            Kohi4 = kohi4;
            IsExpirated = isExpirated;
            IsEmptyHoken = isEmptyHoken;
            IsAddNew = isAddNew;
            IsDeleted = isDeleted;
            Kohi1Id = kohi1Id;
            Kohi2Id = kohi2Id;
            Kohi3Id = kohi3Id;
            Kohi4Id = kohi4Id;
        }

        public int HokenKbn { get; private set; }

        public SelectedKohiModel Kohi1 { get; private set; }

        public SelectedKohiModel Kohi2 { get; private set; }

        public SelectedKohiModel Kohi3 { get; private set; }

        public SelectedKohiModel Kohi4 { get; private set; }

        public bool IsExpirated { get; private set; }

        public bool IsEmptyHoken { get; private set; }

        public bool IsAddNew { get; private set; }

        public int IsDeleted { get; private set; }

        public int HokenId { get; private set; }

        public int Kohi1Id { get; set; }

        public int Kohi2Id { get; set; }

        public int Kohi3Id { get; set; }

        public int Kohi4Id { get; set; }

        public bool IsEmptyKohi1 => Kohi1 != null;

        public bool IsEmptyKohi2 => Kohi2 != null;

        public bool IsEmptyKohi3 => Kohi3 != null;

        public bool IsEmptyKohi4 => Kohi4 != null;
    }

    public class SelectedHokenMst
    {
        public SelectedHokenMst(string houbetu, int hokenNo, int checkDigit, int ageStart, int ageEnd, int isKigoNashi, int startDate, int endDate, string displayTextMaster, int futansyaCheckFlag, int jyukyusyaCheckFlag, int jyuKyuCheckDigit, int tokusyuCheckFlag, int hokenSbtKbn)
        {
            Houbetu = houbetu;
            HokenNo = hokenNo;
            CheckDigit = checkDigit;
            AgeStart = ageStart;
            AgeEnd = ageEnd;
            IsKigoNashi = isKigoNashi;
            StartDate = startDate;
            EndDate = endDate;
            DisplayTextMaster = displayTextMaster;
            FutansyaCheckFlag = futansyaCheckFlag;
            JyukyusyaCheckFlag = jyukyusyaCheckFlag;
            JyuKyuCheckDigit = jyuKyuCheckDigit;
            TokusyuCheckFlag = tokusyuCheckFlag;
            HokenSbtKbn = hokenSbtKbn;
        }

        public string Houbetu { get; private set; }

        public int HokenNo { get; private set; }

        public int CheckDigit { get; private set; }

        public int AgeStart { get; private set; }

        public int AgeEnd { get; private set; }

        public int IsKigoNashi { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public string DisplayTextMaster { get; private set; }

        public int FutansyaCheckFlag { get; private set; }

        public int JyukyusyaCheckFlag { get; private set; }

        public int JyuKyuCheckDigit { get; private set; }

        public int TokusyuCheckFlag { get; private set; }

        public int HokenSbtKbn { get; private set; }
    }

    public class SelectedKohiModel{
        public SelectedKohiModel(string futansyaNo, string jyukyusyaNo, string tokusyuNo, int startDate, int endDate, int confirmDate, int hokenNo, int hokenId, bool isAddNew, SelectedHokenMst kohiHokenMst)
        {
            FutansyaNo = futansyaNo;
            JyukyusyaNo = jyukyusyaNo;
            TokusyuNo = tokusyuNo;
            StartDate = startDate;
            EndDate = endDate;
            ConfirmDate = confirmDate;
            HokenNo = hokenNo;
            HokenId = hokenId;
            IsAddNew = isAddNew;
            KohiHokenMst = kohiHokenMst;
        }

        public string FutansyaNo { get; private set; }

        public string JyukyusyaNo { get; private set; }

        public string TokusyuNo { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public int ConfirmDate { get; private set; }

        public int HokenNo { get; private set; }

        public int HokenId { get; private set; }

        public bool IsAddNew { get; private set; }

        public SelectedHokenMst KohiHokenMst { get; private set; }

        public bool IsKohiMst => KohiHokenMst != null;
    }
}
