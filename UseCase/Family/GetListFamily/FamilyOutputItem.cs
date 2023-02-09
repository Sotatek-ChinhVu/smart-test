using Domain.Models.Family;
using Helper.Common;

namespace UseCase.Family.GetListFamily;

public class FamilyOutputItem
{
    public FamilyOutputItem(FamilyModel model)
    {
        SeqNo = model.SeqNo;
        ZokugaraCd = model.ZokugaraCd;
        FamilyPtNum = model.FamilyPtNum;
        Name = model.Name;
        Sex = model.Sex;
        Birthday = model.Birthday;
        Age = model.Age;
        IsDead = model.IsDead;
        IsSeparated = model.IsSeparated;
        Biko = model.Biko;
        SortNo = model.SortNo;
        ListPtFamilyReki = model.ListPtFamilyRekis.Select(item => new PtFamilyRekiOutputItem(item)).ToList();
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

    public List<PtFamilyRekiOutputItem> ListPtFamilyReki { get; private set; }
}
