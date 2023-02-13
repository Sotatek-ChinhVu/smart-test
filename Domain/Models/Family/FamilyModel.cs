namespace Domain.Models.Family;

public class FamilyModel
{
    public FamilyModel(long seqNo, string zokugaraCd, long familyPtNum, string name, int sex, int birthday, int age, int isDead, int isSeparated, string biko, int sortNo, List<PtFamilyRekiModel> listPtFamilyRekis)
    {
        SeqNo = seqNo;
        ZokugaraCd = zokugaraCd;
        FamilyPtNum = familyPtNum;
        Name = name;
        Sex = sex;
        Birthday = birthday;
        Age = age;
        IsDead = isDead;
        IsSeparated = isSeparated;
        Biko = biko;
        SortNo = sortNo;
        ListPtFamilyRekis = listPtFamilyRekis;
        IsDeleted = false;
    }

    public long SeqNo { get; private set; }

    public string ZokugaraCd { get; private set; }

    public long FamilyPtNum { get; private set; }

    public string Name { get; private set; }

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
