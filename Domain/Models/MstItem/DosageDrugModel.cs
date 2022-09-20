namespace Domain.Models.MstItem;

public class DosageDrugModel
{
    public DosageDrugModel(string yjCd, string doeiCd, string drugKbn, string kikakiUnit, string yakkaiUnit, decimal rikikaRate, string rikikaUnit, string youkaiekiCd, string memoItem)
    {
        YjCd = yjCd;
        DoeiCd = doeiCd;
        DrugKbn = drugKbn;
        KikakiUnit = kikakiUnit;
        YakkaiUnit = yakkaiUnit;
        RikikaRate = rikikaRate;
        RikikaUnit = rikikaUnit;
        YoukaiekiCd = youkaiekiCd;
        MemoItem = memoItem;
    }

    public string YjCd { get; private set; }
    public string DoeiCd { get; private set; }
    public string DrugKbn { get; private set; }
    public string KikakiUnit { get; private set; }
    public string YakkaiUnit { get; private set; }
    public decimal RikikaRate { get; private set; }
    public string RikikaUnit { get; private set; }
    public string YoukaiekiCd { get; private set; }
    public string MemoItem { get; private set; }
}
