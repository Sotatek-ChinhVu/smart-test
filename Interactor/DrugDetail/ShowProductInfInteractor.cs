using Interactor.DrugInfor.CommonDrugInf;
using UseCase.DrugDetailData.ShowProductInf;

namespace Interactor.DrugDetailData;

public class ShowProductInfInteractor : IShowProductInfInputPort
{
    private readonly IGetCommonDrugInf _getCommonDrugInf;

    public ShowProductInfInteractor(IGetCommonDrugInf getCommonDrugInf)
    {
        _getCommonDrugInf = getCommonDrugInf;
    }

    public ShowProductInfOutputData Handle(ShowProductInfInputData inputData)
    {
        try
        {
            if (inputData.HpId <= 0)
            {
                return new ShowProductInfOutputData(string.Empty, ShowProductInfStatus.InvalidHpId);
            }

            if (inputData.SinDate <= 0)
            {
                return new ShowProductInfOutputData(string.Empty, ShowProductInfStatus.InvalidSinDate);
            }
            if (inputData.Level < 0)
            {
                return new ShowProductInfOutputData(string.Empty, ShowProductInfStatus.InvalidLevel);
            }

            var result = _getCommonDrugInf.ShowProductInf(inputData.HpId, inputData.SinDate, inputData.ItemCd, inputData.Level, inputData.DrugName, inputData.YJCode);
            return new ShowProductInfOutputData(result, ShowProductInfStatus.Successed);
        }
        finally
        {
            _getCommonDrugInf.ReleaseResources();
        }
    }
}
