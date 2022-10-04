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

    public SaveSuperSetDetailStatus Validation()
    {
        if (Id < 0)
        {
            return SaveSuperSetDetailStatus.InvalidSetOrderInfId;
        }
        else if (RpNo < 0)
        {
            return SaveSuperSetDetailStatus.InvalidSetOrderInfRpNo;
        }
        else if (RpEdaNo < 0)
        {
            return SaveSuperSetDetailStatus.InvalidSetOrderInfRpEdaNo;
        }
        else if (OdrKouiKbn < 0)
        {
            return SaveSuperSetDetailStatus.InvalidSetOrderInfKouiKbn;
        }
        else if (RpName.Length > 240)
        {
            return SaveSuperSetDetailStatus.RpNameMaxLength240;
        }
        else if (InoutKbn < 0)
        {
            return SaveSuperSetDetailStatus.InvalidSetOrderInfInoutKbn;
        }
        else if (SikyuKbn < 0)
        {
            return SaveSuperSetDetailStatus.InvalidSetOrderInfSikyuKbn;
        }
        else if (SyohoSbt < 0)
        {
            return SaveSuperSetDetailStatus.InvalidSetOrderInfSyohoSbt;
        }
        else if (SanteiKbn < 0)
        {
            return SaveSuperSetDetailStatus.InvalidSetOrderInfSanteiKbn;
        }
        else if (TosekiKbn < 0)
        {
            return SaveSuperSetDetailStatus.InvalidSetOrderInfTosekiKbn;
        }
        else if (DaysCnt < 0)
        {
            return SaveSuperSetDetailStatus.InvalidSetOrderInfDaysCnt;
        }
        else if (SortNo < 0)
        {
            return SaveSuperSetDetailStatus.InvalidSetOrderInfSortNo;
        }
        return SaveSuperSetDetailStatus.ValidateOrderSuccess;
    }
}
