﻿namespace UseCase.NextOrder.Get
{
    public class RsvKrtByomeiItem
    {
        public RsvKrtByomeiItem(long id, int hpId, long ptId, long rsvkrtNo, long seqNo, string byomeiCd, string byomei, int syobyoKbn, int sikkanKbn, int nanbyoCd, string hosokuCmt, int isNodspRece, int isNodspKarte, int isDeleted, string icd10, string icd102013, string icd1012013, string icd1022013, List<string> prefixSuffixList)
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
            Icd10 = icd10;
            Icd102013 = icd102013;
            Icd1012013 = icd1012013;
            Icd1022013 = icd1022013;
            PrefixSuffixList = prefixSuffixList;
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

        public int IsNodspKarte { get; private set; }

        public int IsDeleted { get; private set; }

        public string Icd10 { get; private set; }

        public string Icd102013 { get; private set; }

        public string Icd1012013 { get; private set; }

        public string Icd1022013 { get; private set; }

        public List<string> PrefixSuffixList { get; private set; }
    }
}
