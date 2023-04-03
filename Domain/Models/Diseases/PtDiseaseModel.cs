using Domain.Constant;
using Helper.Common;
using Helper.Constants;
using static Helper.Constants.PtDiseaseConst;

namespace Domain.Models.Diseases
{
    public class PtDiseaseModel
    {
        private const string FREE_WORD = "0000999";

        public PtDiseaseModel(int hpId, long ptId, long seqNo, string byomeiCd, int sortNo,
            List<PrefixSuffixModel> prefixSuffixList, string byomei, int startDate, int tenkiKbn, int tenkiDate,
            int syubyoKbn, int sikkanKbn, int nanbyoCd, int isNodspRece, int isNodspKarte,
            int isDeleted, long id, int isImportant, int sinDate, string icd10, string icd102013, string icd1012013, string icd1022013, int hokenPid, string hosokuCmt, int togetuByomei, int delDate)
        {
            HpId = hpId;
            PtId = ptId;
            SeqNo = seqNo;
            ByomeiCd = byomeiCd;
            SortNo = sortNo;
            PrefixSuffixList = prefixSuffixList;
            Byomei = byomei;
            IsSuspect = 0;
            foreach (var item in prefixSuffixList.Select(e => e.Code))
            {
                if (item != null && item.Equals("8002"))
                {
                    IsSuspect = 1;
                }
            }
            StartDate = startDate;
            TenkiKbn = tenkiKbn;
            TenkiDate = tenkiDate;
            SyubyoKbn = syubyoKbn;
            SikkanKbn = sikkanKbn;
            NanbyoCd = nanbyoCd;
            IsNodspRece = isNodspRece;
            IsNodspKarte = isNodspKarte;
            IsDeleted = isDeleted;
            Id = id;
            IsImportant = isImportant;
            SinDate = sinDate;
            Icd10 = icd10;
            Icd102013 = icd102013;
            Icd1012013 = icd1012013;
            Icd1022013 = icd1022013;
            HokenPid = hokenPid;
            HosokuCmt = hosokuCmt;
            TogetuByomei = togetuByomei;
            DelDate = delDate;
            ItemCd = string.Empty;
        }

        public PtDiseaseModel(string byomeiCd, string byomei, int sikkanKbn)
        {
            HpId = 0;
            PtId = 0;
            SeqNo = 0;
            ByomeiCd = byomeiCd;
            SortNo = 0;
            PrefixSuffixList = new List<PrefixSuffixModel>();
            Byomei = byomei;
            IsSuspect = 0;
            StartDate = 0;
            TenkiKbn = 0;
            TenkiDate = 0;
            SyubyoKbn = 0;
            SikkanKbn = sikkanKbn;
            NanbyoCd = 0;
            IsNodspRece = 0;
            IsNodspKarte = 0;
            IsDeleted = 0;
            Id = 0;
            IsImportant = 0;
            SinDate = 0;
            Icd10 = string.Empty;
            Icd102013 = string.Empty;
            Icd1012013 = string.Empty;
            Icd1022013 = string.Empty;
            HokenPid = 0;
            HosokuCmt = string.Empty;
            ItemCd = string.Empty;
        }

        public PtDiseaseModel(int hpId, long ptId, long seqNo, string byomeiCd, int sortNo,
            List<PrefixSuffixModel> prefixList, List<PrefixSuffixModel> suffixList, string byomei, int startDate, int tenkiKbn, int tenkiDate,
            int syubyoKbn, int sikkanKbn, int nanbyoCd, int isNodspRece, int isNodspKarte,
            int isDeleted, long id, int isImportant, int sinDate, string icd10, string icd102013, string icd1012013, string icd1022013, int hokenPid, string hosokuCmt)
        {
            HpId = hpId;
            PtId = ptId;
            SeqNo = seqNo;
            ByomeiCd = byomeiCd;
            SortNo = sortNo;
            PrefixSuffixList = new List<PrefixSuffixModel>();
            PrefixSuffixList.AddRange(prefixList);
            PrefixSuffixList.AddRange(suffixList);

            Byomei = byomei;
            IsSuspect = 0;
            foreach (var item in PrefixSuffixList.Select(e => e.Code))
            {
                if (item != null && item.Equals("8002"))
                {
                    IsSuspect = 1;
                }
            }
            StartDate = startDate;
            TenkiKbn = tenkiKbn;
            TenkiDate = tenkiDate;
            SyubyoKbn = syubyoKbn;
            SikkanKbn = sikkanKbn;
            NanbyoCd = nanbyoCd;
            IsNodspRece = isNodspRece;
            IsNodspKarte = isNodspKarte;
            IsDeleted = isDeleted;
            Id = id;
            IsImportant = isImportant;
            SinDate = sinDate;
            Icd10 = icd10;
            Icd102013 = icd102013;
            Icd1012013 = icd1012013;
            Icd1022013 = icd1022013;
            HokenPid = hokenPid;
            HosokuCmt = hosokuCmt;
            ItemCd = string.Empty;
        }

        public PtDiseaseModel()
        {
            HpId = 0;
            PtId = 0;
            SeqNo = 0;
            ByomeiCd = string.Empty;
            SortNo = 0;
            PrefixSuffixList = new List<PrefixSuffixModel>();
            Byomei = string.Empty;
            IsSuspect = 0;
            StartDate = 0;
            TenkiKbn = 0;
            TenkiDate = 0;
            SyubyoKbn = 0;
            SikkanKbn = 0;
            NanbyoCd = 0;
            IsNodspRece = 0;
            IsNodspKarte = 0;
            IsDeleted = 0;
            Id = 0;
            IsImportant = 0;
            SinDate = 0;
            Icd10 = string.Empty;
            Icd102013 = string.Empty;
            Icd1012013 = string.Empty;
            Icd1022013 = string.Empty;
            HokenPid = 0;
            HosokuCmt = string.Empty;
            ItemCd = string.Empty;
        }

        public PtDiseaseModel(int sikkanKbn, int hokenPid, int startDate, int tenkiKbn, int tenkiDate, int syubyoKbn)
        {
            HpId = 0;
            PtId = 0;
            SeqNo = 0;
            ByomeiCd = string.Empty;
            SortNo = 0;
            PrefixSuffixList = new List<PrefixSuffixModel>();
            Byomei = string.Empty;
            IsSuspect = 0;
            StartDate = startDate;
            TenkiKbn = tenkiKbn;
            TenkiDate = tenkiDate;
            SyubyoKbn = syubyoKbn;
            SikkanKbn = sikkanKbn;
            NanbyoCd = 0;
            IsNodspRece = 0;
            IsNodspKarte = 0;
            IsDeleted = 0;
            Id = 0;
            IsImportant = 0;
            SinDate = 0;
            Icd10 = string.Empty;
            Icd102013 = string.Empty;
            Icd1012013 = string.Empty;
            Icd1022013 = string.Empty;
            HokenPid = hokenPid;
            HosokuCmt = string.Empty;
            ItemCd = string.Empty;
        }

        public PtDiseaseModel(int sikkanKbn, int hokenPid, int startDate, int tenkiKbn, int tenkiDate, int syubyoKbn, string byomeiCd)
        {
            HpId = 0;
            PtId = 0;
            SeqNo = 0;
            ByomeiCd = byomeiCd;
            SortNo = 0;
            PrefixSuffixList = new List<PrefixSuffixModel>();
            Byomei = string.Empty;
            IsSuspect = 0;
            StartDate = startDate;
            TenkiKbn = tenkiKbn;
            TenkiDate = tenkiDate;
            SyubyoKbn = syubyoKbn;
            SikkanKbn = sikkanKbn;
            NanbyoCd = 0;
            IsNodspRece = 0;
            IsNodspKarte = 0;
            IsDeleted = 0;
            Id = 0;
            IsImportant = 0;
            SinDate = 0;
            Icd10 = string.Empty;
            Icd102013 = string.Empty;
            Icd1012013 = string.Empty;
            Icd1022013 = string.Empty;
            HokenPid = hokenPid;
            HosokuCmt = string.Empty;
            ItemCd = string.Empty;
        }

        public PtDiseaseModel(int sikkanKbn, int hokenPid, int tenkiKbn, int tenkiDate, int syubyoKbn)
        {
            HpId = 0;
            PtId = 0;
            SeqNo = 0;
            ByomeiCd = string.Empty;
            SortNo = 0;
            PrefixSuffixList = new List<PrefixSuffixModel>();
            Byomei = string.Empty;
            IsSuspect = 0;
            StartDate = 0;
            TenkiKbn = tenkiKbn;
            TenkiDate = tenkiDate;
            SyubyoKbn = syubyoKbn;
            SikkanKbn = sikkanKbn;
            NanbyoCd = 0;
            IsNodspRece = 0;
            IsNodspKarte = 0;
            IsDeleted = 0;
            Id = 0;
            IsImportant = 0;
            SinDate = 0;
            Icd10 = string.Empty;
            Icd102013 = string.Empty;
            Icd1012013 = string.Empty;
            Icd1022013 = string.Empty;
            HokenPid = hokenPid;
            HosokuCmt = string.Empty;
            ItemCd = string.Empty;
        }

        public PtDiseaseModel(int sikkanKbn, int hokenPid, int tenkiKbn, int tenkiDate, int syubyoKbn, string icd1012013)
        {
            HpId = 0;
            PtId = 0;
            SeqNo = 0;
            ByomeiCd = string.Empty;
            SortNo = 0;
            PrefixSuffixList = new List<PrefixSuffixModel>();
            Byomei = string.Empty;
            IsSuspect = 0;
            StartDate = 0;
            TenkiKbn = tenkiKbn;
            TenkiDate = tenkiDate;
            SyubyoKbn = syubyoKbn;
            SikkanKbn = sikkanKbn;
            NanbyoCd = 0;
            IsNodspRece = 0;
            IsNodspKarte = 0;
            IsDeleted = 0;
            Id = 0;
            IsImportant = 0;
            SinDate = 0;
            Icd10 = string.Empty;
            Icd102013 = string.Empty;
            Icd1012013 = icd1012013;
            Icd1022013 = string.Empty;
            HokenPid = hokenPid;
            HosokuCmt = string.Empty;
            ItemCd = string.Empty;
        }

        public PtDiseaseModel(int sikkanKbn, int hokenPid, int tenkiKbn, int tenkiDate, int syubyoKbn, string icd1012013, int nanbyoCd)
        {
            HpId = 0;
            PtId = 0;
            SeqNo = 0;
            ByomeiCd = string.Empty;
            SortNo = 0;
            PrefixSuffixList = new List<PrefixSuffixModel>();
            Byomei = string.Empty;
            IsSuspect = 0;
            StartDate = 0;
            TenkiKbn = tenkiKbn;
            TenkiDate = tenkiDate;
            SyubyoKbn = syubyoKbn;
            SikkanKbn = sikkanKbn;
            NanbyoCd = nanbyoCd;
            IsNodspRece = 0;
            IsNodspKarte = 0;
            IsDeleted = 0;
            Id = 0;
            IsImportant = 0;
            SinDate = 0;
            Icd10 = string.Empty;
            Icd102013 = string.Empty;
            Icd1012013 = icd1012013;
            Icd1022013 = string.Empty;
            HokenPid = hokenPid;
            HosokuCmt = string.Empty;
            ItemCd = string.Empty;
        }

        public PtDiseaseModel(long ptId, string byomeiCd, long seqNo, int sortNo, int syubyoKbn, int sikkanKbn, string byomei, int startDate, int tenkiDate, string hosokuCmt, int togetuByomei, List<PrefixSuffixModel> prefixList)
        {
            PtId = ptId;
            ByomeiCd = byomeiCd;
            SeqNo = seqNo;
            SyubyoKbn = syubyoKbn;
            SikkanKbn = sikkanKbn;
            SortNo = sortNo;
            Byomei = byomei;
            StartDate = startDate;
            TenkiDate = tenkiDate;
            HosokuCmt = hosokuCmt;
            TogetuByomei = togetuByomei;
            PrefixSuffixList = new List<PrefixSuffixModel>();
            PrefixSuffixList.AddRange(prefixList);
            Icd10 = string.Empty;
            Icd102013 = string.Empty;
            Icd1012013 = string.Empty;
            Icd1022013 = string.Empty;
            ItemCd = string.Empty;
        }

        public PtDiseaseModel(string itemCd, string byomeiCd, string byomei, int sikkanCd, bool isAdopted, int nanbyoCd)
        {
            ItemCd = itemCd;
            ByomeiCd = byomeiCd;
            Byomei = byomei;
            SikkanKbn = sikkanCd;
            SikkanCd = sikkanCd;
            IsAdopted = isAdopted;
            NanbyoCd = nanbyoCd;
            PrefixSuffixList = new();
            Icd10 = string.Empty;
            Icd102013 = string.Empty;
            Icd1012013 = string.Empty;
            Icd1022013 = string.Empty;
            HosokuCmt = string.Empty;
        }

        public PtDiseaseModel(long ptId, string byomeiCd, long seqNo, int sortNo, int syubyoKbn, int sikkanKbn, string byomei, int startDate, int tenkiDate, string hosokuCmt, int togetuByomei, int isNodspRece, int tenkiKbn, List<PrefixSuffixModel> prefixList)
        {
            PtId = ptId;
            ByomeiCd = byomeiCd;
            SeqNo = seqNo;
            SyubyoKbn = syubyoKbn;
            SikkanKbn = sikkanKbn;
            SortNo = sortNo;
            Byomei = byomei;
            StartDate = startDate;
            TenkiDate = tenkiDate;
            HosokuCmt = hosokuCmt;
            TogetuByomei = togetuByomei;
            IsNodspRece = isNodspRece;
            TenkiKbn = tenkiKbn;
            PrefixSuffixList = new List<PrefixSuffixModel>();
            PrefixSuffixList.AddRange(prefixList);
            Icd10 = string.Empty;
            Icd102013 = string.Empty;
            Icd1012013 = string.Empty;
            Icd1022013 = string.Empty;
            ItemCd = string.Empty;
        }

        public ValidationStatus Validation()
        {
            #region common

            if (IsImportant != 0 && IsImportant != 1)
            {
                return ValidationStatus.InvalidIsImportant;
            }
            if (SyubyoKbn != 0 && SyubyoKbn != 1)
            {
                return ValidationStatus.InvalidSyubyoKbn;
            }
            if (string.IsNullOrEmpty(Byomei) || Byomei.Length > 160)
            {
                return ValidationStatus.InvalidByomei;
            }
            if (ByomeiCd.Length > 7)
            {
                return ValidationStatus.InvalidByomeiCd;
            }
            if (StartDate <= 0)
            {
                return ValidationStatus.InvalidStartDate;
            }
            if (!TenkiKbns.ContainsValue(TenkiKbn))
            {
                return ValidationStatus.InvalidTenkiKbn;
            }
            if (TenkiDate < 0)
            {
                return ValidationStatus.InvalidTenkiDate;
            }
            if (HosokuCmt.Length > 80)
            {
                return ValidationStatus.InvalidHosokuCmt;
            }
            if (!SikkanKbns.ContainsValue(SikkanKbn))
            {
                return ValidationStatus.InvalidSikkanKbn;
            }
            if (!NanByoCds.ContainsValue(NanbyoCd))
            {
                return ValidationStatus.InvalidNanByoCd;
            }
            if (IsNodspRece != 0 && IsNodspRece != 1)
            {
                return ValidationStatus.InvalidIsNodspRece;
            }
            if (IsNodspKarte != 0 && IsNodspKarte != 1)
            {
                return ValidationStatus.InvalidIsNodspKarte;
            }
            if (HokenPid < 0)
            {
                return ValidationStatus.InvalidHokenPid;
            }
            if (Id < 0)
            {
                return ValidationStatus.InvalidId;
            }
            if (PtId <= 0)
            {
                return ValidationStatus.InvalidPtId;
            }
            if (HpId <= 0)
            {
                return ValidationStatus.InvalidHpId;
            }
            if (SortNo < 0)
            {
                return ValidationStatus.InvalidSortNo;
            }
            if (SeqNo < 0)
            {
                return ValidationStatus.InvalidSeqNo;
            }

            if (IsDeleted != 0 && IsDeleted != 1)
            {
                return ValidationStatus.InvalidIsDeleted;
            }
            #endregion

            #region advance
            if (CIUtil.GetByteCountFromString(Byomei) > 40 && ByomeiCd != null && ByomeiCd.Equals(PtDiseaseConst.FREE_WORD))
            {
                return ValidationStatus.InvalidFreeWord;
            }
            if (TenkiKbn == TenkiKbnConst.Continued && TenkiDate > 0)
            {
                return ValidationStatus.InvalidTenkiDateContinue;
            }
            if (TenkiKbn > TenkiKbnConst.Continued && TenkiDate == 0)
            {
                return ValidationStatus.InvalidTenkiDateCommon;
            }
            if (TenkiKbn > TenkiKbnConst.Continued && TenkiDate < StartDate)
            {
                return ValidationStatus.InvalidTekiDateAndStartDate;
            }
            #endregion

            return ValidationStatus.Valid;
        }

        public bool IsFreeWord
        {
            get => ByomeiCd != null && ByomeiCd.Equals(FREE_WORD);
        }

        public bool IsTenki
        {
            get => TenkiKbn > TenkiKbnConst.Continued;
        }

        public bool IsContinous
        {
            get => !IsTenki;
        }

        public bool IsInMonth
        {
            get => IsContinous || (StartDate <= (SinDate / 100 * 100 + 31) && TenkiDate >= (SinDate / 100 * 100 + 1));
        }

        public int HokenPid { get; private set; }

        public string Icd10 { get; set; }

        public string Icd102013 { get; set; }

        public string Icd1012013 { get; set; }

        public string Icd1022013 { get; set; }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public long SeqNo { get; private set; }

        public string ByomeiCd { get; private set; }

        public int SortNo { get; private set; }

        public List<PrefixSuffixModel> PrefixSuffixList { get; private set; }

        public string Byomei { get; set; }

        public int IsSuspect { get; private set; }

        public int StartDate { get; private set; }

        public int TenkiKbn { get; private set; }

        public int TenkiDate { get; private set; }

        public int SyubyoKbn { get; private set; }

        public int SikkanKbn { get; private set; }

        public int NanbyoCd { get; private set; }

        public int IsNodspRece { get; private set; }

        public int IsNodspKarte { get; private set; }

        public int IsDeleted { get; private set; }

        public long Id { get; private set; }

        public int IsImportant { get; private set; }

        public int SinDate { get; private set; }

        public string HosokuCmt { get; private set; }

        public int TogetuByomei { get; private set; }

        public int SikkanCd { get; private set; }

        public int DelDate { get; private set; }

        public string ItemCd { get; private set; }

        public bool IsAdopted { get; private set; }

        public string FullByomei { get => string.Concat(Byomei, HosokuCmt); }

        public bool IsDspRece { get => IsNodspRece == 0; }

        public int ReceTenkiKbn
        {
            get
            {
                int ret = ReceTenkiKbnConst.Continued;
                switch (TenkiKbn)
                {
                    case TenkiKbnConst.Cured:
                        ret = ReceTenkiKbnConst.Cured;
                        break;
                    case TenkiKbnConst.Canceled:
                        ret = ReceTenkiKbnConst.Canceled;
                        break;
                    case TenkiKbnConst.Dead:
                        ret = ReceTenkiKbnConst.Dead;
                        break;
                }
                return ret;
            }
        }

        public string TenKiBinding
        {
            get
            {
                return TenkiKbnConst.DisplayedTenkiKbnDict[TenkiKbn];
            }
        }

        public bool IsFree => ByomeiCd == FREE_WORD;

        public bool IsMain => SyubyoKbn == 1;

        public string ByomeiHankToZen => HenkanJ.Instance.ToFullsize(Byomei);

        public PtDiseaseModel ChangeSikkanCd(int sikkanCd)
        {
            SikkanCd = sikkanCd;
            return this;
        }
    }
}
