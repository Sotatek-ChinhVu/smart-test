using Domain.Models.Family;

namespace UseCase.Family;

public class FamilyItem
{
    public FamilyItem(long familyId, long ptId, string zokugaraCd, long familyPtId, string name, string kanaName, int sex, int birthday, int isDead, int isSeparated, string biko, int sortNo, bool isDeleted, List<FamilyRekiItem> ptFamilyRekiList)
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
        PtFamilyRekiList = ptFamilyRekiList;
    }

    public FamilyItem(FamilyModel model)
    {
        FamilyId = model.FamilyId;
        ZokugaraCd = model.ZokugaraCd;
        FamilyPtNum = model.FamilyPtNum;
        FamilyPtId = model.FamilyPtId;
        Name = model.Name;
        KanaName = model.KanaName;
        Sex = model.Sex;
        Birthday = model.Birthday;
        Age = model.Age;
        IsDead = model.IsDead;
        IsSeparated = model.IsSeparated;
        Biko = model.Biko;
        SortNo = model.SortNo;
        PtFamilyRekiList = model.ListPtFamilyRekis.Select(item => new FamilyRekiItem(item)).ToList();
    }

    public long FamilyId { get; private set; }

    public long PtId { get; private set; }

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

    public bool IsDeleted { get; private set; }

    public List<FamilyRekiItem> PtFamilyRekiList { get; private set; }
}
