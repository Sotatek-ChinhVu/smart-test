namespace UseCase.Family.SaveListFamily;

public class FamilyInputItem
{
    public FamilyInputItem(long familyId, long ptId, string zokugaraCd, long familyPtId, string name, string kanaName, int sex, int birthday, int isDead, int isSeparated, string biko, int sortNo, bool isDeleted, List<FamilyRekiInputItem> listPtFamilyReki)
    {
        FamilyId = familyId;
        PtId = ptId;
        ZokugaraCd = zokugaraCd;
        FamilyPtId = familyPtId;
        Name = name;
        KanaName = kanaName;
        Sex = sex;
        Birthday = birthday;
        IsDead = isDead;
        IsSeparated = isSeparated;
        Biko = biko;
        SortNo = sortNo;
        IsDeleted = isDeleted;
        ListPtFamilyReki = listPtFamilyReki;
    }

    public long FamilyId { get; private set; }

    public long PtId { get; private set; }

    public string ZokugaraCd { get; private set; }

    public long FamilyPtId { get; private set; }

    public string Name { get; private set; }

    public string KanaName { get; private set; }

    public int Sex { get; private set; }

    public int Birthday { get; private set; }

    public int IsDead { get; private set; }

    public int IsSeparated { get; private set; }

    public string Biko { get; private set; }

    public int SortNo { get; private set; }

    public bool IsDeleted { get; private set; }

    public List<FamilyRekiInputItem> ListPtFamilyReki { get; private set; }
}
