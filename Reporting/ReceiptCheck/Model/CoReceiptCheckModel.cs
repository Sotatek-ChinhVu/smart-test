using Entity.Tenant;
using Helper.Constants;

namespace Reporting.ReceiptCheck.Model;

public class CoReceiptCheckModel
{
    private readonly PtInf _ptInf;
    public PtInf PtInf
    {
        get => _ptInf;
    }

    private readonly ReceInf _receInf;
    public ReceInf ReceInf
    {
        get => _receInf;
    }

    private readonly ReceCheckErr _receCheckErr;
    public ReceCheckErr ReceCheckErr
    {
        get => _receCheckErr;
    }

    public CoReceiptCheckModel(PtInf ptInf, ReceInf receInf, ReceCheckErr receCheckErr)
    {
        _ptInf = ptInf;
        _receInf = receInf;
        _receCheckErr = receCheckErr;
    }

    public CoReceiptCheckModel()
    {
        _ptInf = new();
        _receInf = new();
        _receCheckErr = new();
    }

    public int SeikyuYm
    {
        get => ReceInf != null ? ReceInf.SeikyuYm : 0;
    }

    public int SinYm
    {
        get => ReceInf != null ? ReceInf.SinYm : 0;
    }

    public int HokenId
    {
        get => ReceInf != null ? ReceInf.HokenId : 0;
    }

    public int HokenId2
    {
        get => ReceInf != null ? ReceInf.HokenId2 : 0;
    }

    public int HokenKbn
    {
        get => ReceInf != null ? ReceInf.HokenKbn : 0;
    }

    public string ReceSbt
    {
        get => ReceInf.ReceSbt ?? string.Empty;
    }

    public long PtNum
    {
        get => PtInf != null ? PtInf.PtNum : 0;
    }

    public long PtId
    {
        get => PtInf != null ? PtInf.PtId : 0;
    }

    public string PtName
    {
        get => PtInf.Name ?? string.Empty;
    }

    public string HokenName
    {
        get => GetHokenName();
    }

    public string ErrorMessage
    {
        get => ReceCheckErr != null ? ReceCheckErr.Message1 + ReceCheckErr.Message2 : string.Empty;
    }

    private string GetHokenName()
    {
        string result = string.Empty;
        switch (HokenKbn)
        {
            case 0:
                if (ReceSbt.Substring(0, 1) == "9")
                {
                    result = "自レ";
                }
                break;
            case 1:
                if (ReceiptListConstant.ShaHoDict.ContainsKey(ReceSbt))
                {
                    result = ReceiptListConstant.ShaHoDict[ReceSbt];
                }
                break;
            case 2:
                if (ReceiptListConstant.KokuHoDict.ContainsKey(ReceSbt))
                {
                    result = ReceiptListConstant.KokuHoDict[ReceSbt];
                }
                break;
            case 11:
                result = "労災(短期給付)";
                break;
            case 12:
                result = "労災(傷病年金)";
                break;
            case 13:
                result = "アフターケア";
                break;
            case 14:
                result = "自賠責";
                break;
        }

        return result;
    }
}
