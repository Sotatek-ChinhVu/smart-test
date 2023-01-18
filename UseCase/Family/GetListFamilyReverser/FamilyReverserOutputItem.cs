using Domain.Models.Family;

namespace UseCase.Family.GetListFamilyReverser;

public class FamilyReverserOutputItem
{
    public FamilyReverserOutputItem(FamilyModel model, string zokugaraCd)
    {
        PtId = model.PtId;
        PtNum = model.FamilyPtNum;
        Name = model.Name;
        KanaName = model.KanaName;
        Sex = model.Sex;
        Birthday = model.Birthday;
        IsDead = model.IsDead;
        ZokugaraCd = zokugaraCd;
    }

    public long PtId { get; private set; }

    public long PtNum { get; private set; }

    public string Name { get; private set; }

    public string KanaName { get; private set; }

    public int Sex { get; private set; }

    public int Birthday { get; private set; }

    public int IsDead { get; private set; }

    public string ZokugaraCd { get; private set; }
}
