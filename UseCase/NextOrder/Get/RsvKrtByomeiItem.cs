using Domain.Models.NextOrder;

namespace UseCase.NextOrder.Get
{
    public class RsvKrtByomeiItem
    {
        public RsvKrtByomeiItem(RsvkrtByomeiModel rsvkrtByomeiModel)
        {
            Id = rsvkrtByomeiModel.Id;
            HpId = rsvkrtByomeiModel.HpId;
            PtId = rsvkrtByomeiModel.PtId;
            RsvkrtNo = rsvkrtByomeiModel.RsvkrtNo;
            SeqNo = rsvkrtByomeiModel.SeqNo;
            ByomeiCd = rsvkrtByomeiModel.ByomeiCd;
            Byomei = rsvkrtByomeiModel.Byomei;
            SyobyoKbn = rsvkrtByomeiModel.SyobyoKbn;
            SikkanKbn = rsvkrtByomeiModel.SikkanKbn;
            NanbyoCd = rsvkrtByomeiModel.NanbyoCd;
            HosokuCmt = rsvkrtByomeiModel.HosokuCmt;
            IsNodspRece = rsvkrtByomeiModel.IsNodspRece;
            IsNodspKarte = rsvkrtByomeiModel.IsNodspKarte;
            IsDeleted = rsvkrtByomeiModel.IsDeleted;
            Icd10 = rsvkrtByomeiModel.Icd10;
            Icd102013 = rsvkrtByomeiModel.Icd102013;
            Icd1012013 = rsvkrtByomeiModel.Icd1012013;
            Icd1022013 = rsvkrtByomeiModel.Icd1022013;
            PrefixSuffixList = rsvkrtByomeiModel.PrefixSuffixList;
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
