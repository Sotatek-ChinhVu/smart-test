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
            List<string> syusyokuCdList, string byomei, int startDate, int tenkiKbn, int tenkiDate,
            int syubyoKbn, int sikkanKbn, int nanbyoCd, int isNodspRece, int isNodspKarte,
            int isDeleted, long id, int isImportant, int sinDate, string icd10, string icd102013, string icd1012013, string icd1022013, int hokenPid, string hosokuCmt)
        {
            HpId = hpId;
            PtId = ptId;
            SeqNo = seqNo;
            ByomeiCd = byomeiCd;
            SortNo = sortNo;
            SyusyokuCd = new Dictionary<string, string>()
            {
                {"SyusyokuCd1" , syusyokuCdList[0]},
                {"SyusyokuCd2" , syusyokuCdList[1]},
                {"SyusyokuCd3" , syusyokuCdList[2]},
                {"SyusyokuCd4" , syusyokuCdList[3]},
                {"SyusyokuCd5" , syusyokuCdList[4]},
                {"SyusyokuCd6" , syusyokuCdList[5]},
                {"SyusyokuCd7" , syusyokuCdList[6]},
                {"SyusyokuCd8" , syusyokuCdList[7]},
                {"SyusyokuCd9" , syusyokuCdList[8]},
                {"SyusyokuCd10" , syusyokuCdList[9]},
                {"SyusyokuCd11" , syusyokuCdList[10]},
                {"SyusyokuCd12" , syusyokuCdList[11]},
                {"SyusyokuCd13" , syusyokuCdList[12]},
                {"SyusyokuCd14" , syusyokuCdList[13]},
                {"SyusyokuCd15" , syusyokuCdList[14]},
                {"SyusyokuCd16" , syusyokuCdList[15]},
                {"SyusyokuCd17" , syusyokuCdList[16]},
                {"SyusyokuCd18" , syusyokuCdList[17]},
                {"SyusyokuCd19" , syusyokuCdList[18]},
                {"SyusyokuCd20" , syusyokuCdList[19]},
                {"SyusyokuCd21" , syusyokuCdList[20]}
            };
            Byomei = byomei;
            IsSuspect = 0;
            foreach (var item in SyusyokuCd.Select(e => e.Value))
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
        }

        public ValidationStatus Validation()
        {

            if (!PtDiseaseConst.TenkiKbns.Values.Contains(TenkiKbn))
            {
                return ValidationStatus.InvalidTenkiKbn;
            }
            if (!PtDiseaseConst.SikkanKbns.Values.Contains(SikkanKbn))
            {
                return ValidationStatus.InvalidSikkanKbn;
            }
            if (!PtDiseaseConst.NanByoCds.Values.Contains(NanbyoCd))
            {
                return ValidationStatus.InvalidNanByoCd;
            }
            if (CIUtil.GetByteCountFromString(Byomei) > 40 && ByomeiCd != null && ByomeiCd.Equals(PtDiseaseConst.FREE_WORD))
            {
                return ValidationStatus.InvalidFreeWord;
            }
            if (TenkiKbn == TenkiKbnConst.Continued && TenkiDate > 0)
            {
                return ValidationStatus.InvalidTenkiDateContinue;
            }
            if (TenkiKbn == TenkiKbnConst.Continued && TenkiDate < StartDate)
            {
                return ValidationStatus.InvalidTekiDateAndStartDate;
            }
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

        public string Icd10 { get; private set; }

        public string Icd102013 { get; private set; }

        public string Icd1012013 { get; private set; }

        public string Icd1022013 { get; private set; }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public long SeqNo { get; private set; }

        public string ByomeiCd { get; private set; }

        public int SortNo { get; private set; }

        public Dictionary<string, string>? SyusyokuCd { get; private set; }

        public string Byomei { get; private set; }

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
    }
}
