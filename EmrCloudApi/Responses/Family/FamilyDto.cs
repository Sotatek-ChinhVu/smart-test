using UseCase.Family.GetListFamily;

namespace EmrCloudApi.Responses.Family;

public class FamilyDto
{
    public FamilyDto(FamilyOutputItem model)
    {
        FamilyId = model.FamilyId;
        ZokugaraCd = model.ZokugaraCd;
        FamilyPtId = model.FamilyPtId;
        FamilyPtNum = model.FamilyPtNum;
        Name = model.Name;
        KanaName = model.KanaName;
        Sex = model.Sex;
        Birthday = model.Birthday;
        Age = model.Age;
        IsDead = model.IsDead;
        IsSeparated = model.IsSeparated;
        Biko = model.Biko;
        SortNo = model.SortNo;
        ListPtFamilyReki = model.ListPtFamilyReki.Select(item => new PtFamilyRekiDto(item)).ToList();
    }

    public long FamilyId { get; private set; }

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

    public List<PtFamilyRekiDto> ListPtFamilyReki { get; private set; }
}
