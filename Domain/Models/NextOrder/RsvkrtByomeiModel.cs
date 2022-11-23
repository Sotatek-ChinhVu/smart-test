using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using static Helper.Constants.RsvkrtByomeiConst;

namespace Domain.Models.NextOrder
{
    public class RsvkrtByomeiModel
    {
        public RsvkrtByomeiModel(long id, int hpId, long ptId, long rsvkrtNo, long seqNo, string byomeiCd, string byomei, int syobyoKbn, int sikkanKbn, int nanbyoCd, string hosokuCmt, int isNodspRece, int isNodspKarte, int isDeleted, List<string> prefixSuffixList, string icd10, string icd102013, string icd1012013, string icd1022013)
        {
            Id = id;
            HpId = hpId;
            PtId = ptId;
            RsvkrtNo = rsvkrtNo;
            SeqNo = seqNo;
            ByomeiCd = byomeiCd;
            Byomei = byomei;
            SyobyoKbn = syobyoKbn;
            SikkanKbn = sikkanKbn;
            NanbyoCd = nanbyoCd;
            HosokuCmt = hosokuCmt;
            IsNodspRece = isNodspRece;
            IsNodspKarte = isNodspKarte;
            IsDeleted = isDeleted;
            PrefixSuffixList = prefixSuffixList;
            Icd10 = icd10;
            Icd102013 = icd102013;
            Icd1012013 = icd1012013;
            Icd1022013 = icd1022013;
        }

        public RsvkrtByomeiStatus Validation()
        {
            #region common
            if (Id < 0)
            {
                return RsvkrtByomeiStatus.InvalidId;
            }
            if (RsvkrtNo <= 0)
            {
                return RsvkrtByomeiStatus.InvalidRsvkrtNo;
            }
            if (SeqNo < 0)
            {
                return RsvkrtByomeiStatus.InvalidSeqNo;
            }
            if (ByomeiCd.Length > 7)
            {
                return RsvkrtByomeiStatus.InvalidByomeiCd;
            }
            if (SyobyoKbn != 0 && SyobyoKbn != 1)
            {
                return RsvkrtByomeiStatus.InvalidSyubyoKbn;
            }
            if (HosokuCmt.Length > 80)
            {
                return RsvkrtByomeiStatus.InvalidHosokuCmt;
            }
            if (IsNodspRece != 0 && IsNodspRece != 1)
            {
                return RsvkrtByomeiStatus.InvalidIsNodspRece;
            }
            if (IsNodspKarte != 0 && IsNodspKarte != 1)
            {
                return RsvkrtByomeiStatus.InvalidIsNodspKarte;
            }
            if (IsDeleted != 0 && IsDeleted != 1)
            {
                return RsvkrtByomeiStatus.InvalidIsDeleted;
            }
            #endregion

            #region advance
            if (string.IsNullOrEmpty(Byomei) || Byomei.Length > 160)
            {
                return RsvkrtByomeiStatus.InvalidByomei;
            }
            if (!SikkanKbns.Values.Contains(SikkanKbn))
            {
                return RsvkrtByomeiStatus.InvalidSikkanKbn;
            }
            if (!NanByoCds.Values.Contains(NanbyoCd))
            {
                return RsvkrtByomeiStatus.InvalidNanByoCd;
            }
            if (CIUtil.GetByteCountFromString(Byomei) > 40 && ByomeiCd != null && ByomeiCd.Equals(PtDiseaseConst.FREE_WORD))
            {
                return RsvkrtByomeiStatus.InvalidFreeWord;
            }
            #endregion

            return RsvkrtByomeiStatus.Valid;
        }

        public long Id { get; private set; }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public long RsvkrtNo { get; private set; }

        public long SeqNo { get; private set; }

        public string ByomeiCd { get; private set; }

        public string Byomei { get; private set; }

        public int SyobyoKbn { get; private set; }

        public int SikkanKbn { get; private set; }

        public int NanbyoCd { get; private set; }

        public string HosokuCmt { get; private set; }

        public int IsNodspRece { get; private set; }

        public string Icd10 { get; private set; }

        public string Icd102013 { get; private set; }

        public string Icd1012013 { get; private set; }

        public string Icd1022013 { get; private set; }

        public int IsNodspKarte { get; private set; }

        public int IsDeleted { get; private set; }

        public bool IsFreeWord
        {
            get => ByomeiCd.AsString().Equals(ByomeiConstant.FreeWordCode);
            set { }
        }

        public bool IsNotFreeWord
        {
            get => !IsFreeWord;
        }

        public List<string> PrefixSuffixList { get; private set; }
    }
}
