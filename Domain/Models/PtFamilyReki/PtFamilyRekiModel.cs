namespace Domain.Models.PtFamilyReki
{
    public class PtFamilyRekiModel
    {
        public PtFamilyRekiModel(long id, int hpId, long ptId, long familyId, long seqNo, int sortNo, string byomeiCd, string byotaiCd, string byomei, string cmt, int isDeleted, DateTime createDate, int createId, string createMachine, DateTime updateDate, int updateId, string updateMachine)
        {
            Id = id;
            HpId = hpId;
            PtId = ptId;
            FamilyId = familyId;
            SeqNo = seqNo;
            SortNo = sortNo;
            ByomeiCd = byomeiCd;
            ByotaiCd = byotaiCd;
            Byomei = byomei;
            Cmt = cmt;
            IsDeleted = isDeleted;
            CreateDate = createDate;
            CreateId = createId;
            CreateMachine = createMachine;
            UpdateDate = updateDate;
            UpdateId = updateId;
            UpdateMachine = updateMachine;
        }

        public long Id { get; private set; }
        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public long FamilyId { get; private set; }
        public long SeqNo { get; private set; }
        public int SortNo { get; private set; }
        public string ByomeiCd { get; private set; }
        public string ByotaiCd { get; private set; }
        public string Byomei { get; private set; }
        public string Cmt { get; private set; }
        public int IsDeleted { get; private set; }
        public DateTime CreateDate { get; private set; }
        public int CreateId { get; private set; }
        public string CreateMachine { get; private set; }
        public DateTime UpdateDate { get; private set; }
        public int UpdateId { get; private set; }
        public string UpdateMachine { get; private set; }
    }
}
