using Interactor.DrugInfor.CommonDrugInf;
using UseCase.DrugInfor.GetDataPrintDrugInfo;

namespace Interactor.DrugInfor;

public class GetDataPrintDrugInfoInteractor : IGetDataPrintDrugInfoInputPort
{
    private readonly IGetCommonDrugInf _getCommonDrugInf;

    public GetDataPrintDrugInfoInteractor(IGetCommonDrugInf getCommonDrugInf)
    {
        _getCommonDrugInf = getCommonDrugInf;
    }

    public GetDataPrintDrugInfoOutputData Handle(GetDataPrintDrugInfoInputData inputData)
    {
        try
        {
            var drugInfo = _getCommonDrugInf.GetDrugInforModel(inputData.HpId, inputData.SinDate, inputData.ItemCd);
            string htmlData = string.Empty;
            switch (inputData.Type)
            {
                case TypeHTMLEnum.ShowProductInf:
                    htmlData = _getCommonDrugInf.ShowProductInf(inputData.HpId, inputData.SinDate, inputData.ItemCd, inputData.Level, inputData.DrugName, inputData.YJCode);
                    break;
                case TypeHTMLEnum.ShowKanjaMuke:
                    htmlData = _getCommonDrugInf.ShowKanjaMuke(inputData.ItemCd, inputData.Level, inputData.DrugName, inputData.YJCode);
                    break;
                case TypeHTMLEnum.ShowMdbByomei:
                    htmlData = _getCommonDrugInf.ShowMdbByomei(inputData.ItemCd, inputData.Level, inputData.DrugName, inputData.YJCode);
                    break;
            }
            return new GetDataPrintDrugInfoOutputData(drugInfo, htmlData, (int)inputData.Type);
        }
        finally
        {
            _getCommonDrugInf.ReleaseResources();
        }
    }
}
