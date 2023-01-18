using UseCase.Family.GetListFamilyReverser;

namespace EmrCloudApi.Responses.Family;

public class FamilyReverserDto
{
    public FamilyReverserDto(FamilyReverserOutputItem ouputItem)
    {
        PtId = ouputItem.PtId;
        PtNum = ouputItem.PtNum;
        Name = ouputItem.Name;
        KanaName = ouputItem.KanaName;
        Sex = ouputItem.Sex;
        Birthday = ouputItem.Birthday;
        IsDead = ouputItem.IsDead;
        ZokugaraCd = ouputItem.ZokugaraCd;
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
