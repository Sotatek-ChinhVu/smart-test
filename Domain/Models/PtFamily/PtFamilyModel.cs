namespace Domain.Models.PtFamily
{
    public class PtFamilyModel
    {
        public PtFamilyModel(long familyId, int hpId, long ptId, long seqNo, string zokugaraCd, int sortNo, int parentId, long familyPtId, string kanaName, string name, int sex, int birthday, int isDead, int isSeparated, string biko, int isDeleted, DateTime createDate, int createId, string createMachine, DateTime updateDate, int updateId, string updateMachine)
        {
            FamilyId = familyId;
            HpId = hpId;
            PtId = ptId;
            SeqNo = seqNo;
            ZokugaraCd = zokugaraCd;
            SortNo = sortNo;
            ParentId = parentId;
            FamilyPtId = familyPtId;
            KanaName = kanaName;
            Name = name;
            Sex = sex;
            Birthday = birthday;
            IsDead = isDead;
            IsSeparated = isSeparated;
            Biko = biko;
            IsDeleted = isDeleted;
            CreateDate = createDate;
            CreateId = createId;
            CreateMachine = createMachine;
            UpdateDate = updateDate;
            UpdateId = updateId;
            UpdateMachine = updateMachine;
        }

        public long FamilyId { get; private set; }
        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public long SeqNo { get; private set; }
        public string ZokugaraCd { get; private set; }
        public int SortNo { get; private set; }
        public int ParentId { get; private set; }
        public long FamilyPtId { get; private set; }
        public string KanaName { get; private set; }
        public string Name { get; private set; }
        public int Sex { get; private set; }
        public int Birthday { get; private set; }
        public int IsDead { get; private set; }
        public int IsSeparated { get; private set; }
        public string Biko { get; private set; }
        public int IsDeleted { get; private set; }
        public DateTime CreateDate { get; private set; }
        public int CreateId { get; private set; }
        public string CreateMachine { get; private set; }
        public DateTime UpdateDate { get; private set; }
        public int UpdateId { get; private set; }
        public string UpdateMachine { get; private set; }
    }
}
