namespace EmrCloudApi.Requests.ReceSeikyu
{
    public class ReceSeikyuDto
    {
        public ReceSeikyuDto(int hpId, long ptId, int sinYm, int hokenId, int seqNo, int seikyuYm, int seikyuKbn, int preHokenId, string cmt, bool isModified, int originSeikyuYm, int originSinYm, bool isAddNew, int isDeleted, bool isChecked)
        {
            HpId = hpId;
            PtId = ptId;
            SinYm = sinYm;
            HokenId = hokenId;
            SeqNo = seqNo;
            SeikyuYm = seikyuYm;
            SeikyuKbn = seikyuKbn;
            PreHokenId = preHokenId;
            Cmt = cmt;
            IsModified = isModified;
            OriginSeikyuYm = originSeikyuYm;
            OriginSinYm = originSinYm;
            IsAddNew = isAddNew;
            IsDeleted = isDeleted;
            IsChecked = isChecked;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int SinYm { get; private set; }

        public int HokenId { get; private set; }

        public int SeqNo { get; private set; }

        public int SeikyuYm { get; private set; }

        public int SeikyuKbn { get; private set; }

        public int PreHokenId { get; private set; }

        public string Cmt { get; private set; } = string.Empty;

        public bool IsModified { get; private set; }

        public int OriginSeikyuYm { get; private set; }

        public int OriginSinYm { get; private set; }

        public bool IsAddNew { get; private set; }

        public int IsDeleted { get; private set; }

        public bool IsChecked { get; private set; }
    }
}
