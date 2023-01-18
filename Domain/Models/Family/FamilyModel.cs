namespace Domain.Models.Family;

public class FamilyModel
{
    public FamilyModel(long familyId, long seqNo, string zokugaraCd, long familyPtId, long familyPtNum, string name, string kanaName, int sex, int birthday, int age, int isDead, int isSeparated, string biko, int sortNo, List<PtFamilyRekiModel> listPtFamilyRekis)
    {
        FamilyId = familyId;
        SeqNo = seqNo;
        ZokugaraCd = zokugaraCd;
        FamilyPtId = familyPtId;
        FamilyPtNum = familyPtNum;
        Name = name;
        KanaName = kanaName;
        Sex = sex;
        Birthday = birthday;
        Age = age;
        IsDead = isDead;
        IsSeparated = isSeparated;
        Biko = biko;
        SortNo = sortNo;
        ListPtFamilyRekis = listPtFamilyRekis;
        PtId = 0;
        IsDeleted = false;
    }

    public FamilyModel(long familyId, long ptId, string zokugaraCd, long familyPtId)
    {
        FamilyId = familyId;
        ZokugaraCd = zokugaraCd;
        FamilyPtId = familyPtId;
        SeqNo = 0;
        PtId = ptId;
        FamilyPtNum = 0;
        Name = string.Empty;
        KanaName = string.Empty;
        Sex = 0;
        Birthday = 0;
        Age = 0;
        IsDead = 0;
        IsSeparated = 0;
        Biko = string.Empty;
        SortNo = 0;
        ListPtFamilyRekis = new();
        IsDeleted = false;
    }

    public FamilyModel(long familyId, long ptId, string zokugaraCd, long familyPtId, string name, string kanaName, int sex, int birthday, int isDead, int isSeparated, string biko, int sortNo, bool isDeleted, List<PtFamilyRekiModel> listPtFamilyRekis)
    {
        FamilyId = familyId;
        PtId = ptId;
        SeqNo = 0;
        ZokugaraCd = zokugaraCd;
        FamilyPtId = familyPtId;
        FamilyPtNum = 0;
        Name = name;
        KanaName = kanaName;
        Sex = sex;
        Birthday = birthday;
        Age = 0;
        IsDead = isDead;
        IsSeparated = isSeparated;
        Biko = biko;
        SortNo = sortNo;
        ListPtFamilyRekis = listPtFamilyRekis;
        IsDeleted = isDeleted;
    }

    public long FamilyId { get; private set; }

    public long PtId { get; private set; }

    public long SeqNo { get; private set; }

    public string ZokugaraCd { get; private set; }

    public long FamilyPtId { get; private set; }

    public long FamilyPtNum { get; private set; }

    public string Name { get; private set; }

    public string KanaName { get; private set; }

    public int Sex { get; private set; }

    public int Birthday { get; private set; }

    public int Age { get; private set; }

    public int IsDead { get; private set; }

    public int IsSeparated { get; private set; }

    public string Biko { get; private set; }

    public int SortNo { get; private set; }

    public List<PtFamilyRekiModel> ListPtFamilyRekis { get; private set; }

    public bool IsDeleted { get; private set; }
}
