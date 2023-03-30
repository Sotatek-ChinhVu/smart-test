using Domain.Models.Receipt;
using Helper.Common;
using Helper.Constants;
using UseCase.Receipt.GetInsuranceReceInfList;

namespace Interactor.Receipt;

public class GetInsuranceReceInfListInteractor : IGetInsuranceReceInfListInputPort
{
    private readonly IReceiptRepository _receiptRepository;

    public GetInsuranceReceInfListInteractor(IReceiptRepository receiptRepository)
    {
        _receiptRepository = receiptRepository;
    }

    public GetInsuranceReceInfListOutputData Handle(GetInsuranceReceInfListInputData inputData)
    {
        try
        {
            var result = _receiptRepository.GetInsuranceReceInfList(inputData.HpId, inputData.SeikyuYm, inputData.HokenId, inputData.SinYm, inputData.PtId);
            bool haveKohi = result.Kohi1Id > 0 || result.Kohi2Id > 0 || result.Kohi3Id > 0 || result.Kohi4Id > 0;
            return new GetInsuranceReceInfListOutputData(result, GetInsuranceName(result.HokenKbn, result.ReceSbt, haveKohi), GetInsuranceReceInfListStatus.Successed);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
        }
    }

    private string GetInsuranceName(int hokenKbn, string receSbt, bool haveKohi)
    {
        string result = "ー";
        switch (hokenKbn)
        {
            case 0:
                if (!string.IsNullOrWhiteSpace(receSbt) && receSbt.Length == 4)
                {
                    if (receSbt[0] == '8')
                    {
                        result = "自費";
                    }
                    else if (receSbt[0] == '9')
                    {
                        result = "自費レセ";
                    }

                    if (haveKohi)
                    {
                        int kohiCount = receSbt[2];
                        string prefix = GetKohiCountName(kohiCount);
                        return result + prefix;
                    }
                }
                break;
            case 1:
                if (ReceiptListConstant.ShaHoDict.ContainsKey(receSbt))
                {
                    result = ReceiptListConstant.ShaHoDict[receSbt];
                }
                break;
            case 2:
                if (ReceiptListConstant.KokuHoDict.ContainsKey(receSbt))
                {
                    result = ReceiptListConstant.KokuHoDict[receSbt];
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

    private string GetKohiCountName(int kohicount)
    {
        if (kohicount <= 0)
        {
            return string.Empty;
        }
        if (kohicount == 1)
        {
            return "単独";
        }
        else
        {
            return HenkanJ.Instance.ToFullsize(kohicount.ToString()) + "併";
        }
    }
}
