using Domain.Models.OrdInfs;

namespace UseCase.SuperSetDetail.SaveSuperSetDetail.SaveSetOrderInput;

public class SaveSetOrderInfInputItem
{
    public SaveSetOrderInfInputItem(long id, long rpNo, long rpEdaNo, int odrKouiKbn, string rpName, int inoutKbn, int sikyuKbn, int syohoSbt, int santeiKbn, int tosekiKbn, int daysCnt, int sortNo, int isDeleted, List<SetOrderInfDetailInputItem> ordInfDetails)
    {
        Id = id;
        RpNo = rpNo;
        RpEdaNo = rpEdaNo;
        OdrKouiKbn = odrKouiKbn;
        RpName = rpName;
        InoutKbn = inoutKbn;
        SikyuKbn = sikyuKbn;
        SyohoSbt = syohoSbt;
        SanteiKbn = santeiKbn;
        TosekiKbn = tosekiKbn;
        DaysCnt = daysCnt;
        SortNo = sortNo;
        IsDeleted = isDeleted;
        OrdInfDetails = ordInfDetails;
    }

    public long Id { get; private set; }

    public long RpNo { get; private set; }

    public long RpEdaNo { get; private set; }

    public int OdrKouiKbn { get; private set; }

    public string RpName { get; private set; }

    public int InoutKbn { get; private set; }

    public int SikyuKbn { get; private set; }

    public int SyohoSbt { get; private set; }

    public int SanteiKbn { get; private set; }

    public int TosekiKbn { get; private set; }

    public int DaysCnt { get; private set; }

    public int SortNo { get; private set; }

    public int IsDeleted { get; private set; }

    public List<SetOrderInfDetailInputItem> OrdInfDetails { get; private set; }
}
