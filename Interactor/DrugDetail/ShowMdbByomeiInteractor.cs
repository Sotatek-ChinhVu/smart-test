using Interactor.DrugInfor.CommonDrugInf;
using UseCase.DrugDetailData.ShowMdbByomei;

namespace Interactor.DrugDetailData;

public class ShowMdbByomeiInteractor : IShowMdbByomeiInputPort
{
    private readonly IGetCommonDrugInf _getCommonDrugInf;

    public ShowMdbByomeiInteractor(IGetCommonDrugInf getCommonDrugInf)
    {
        _getCommonDrugInf = getCommonDrugInf;
    }

    public ShowMdbByomeiOutputData Handle(ShowMdbByomeiInputData inputData)
    {
        try
        {
            if (inputData.Level < 0)
            {
                return new ShowMdbByomeiOutputData(string.Empty, ShowMdbByomeiStatus.InvalidLevel);
            }
            var result = _getCommonDrugInf.ShowMdbByomei(inputData.HpId, inputData.ItemCd, inputData.Level, inputData.DrugName, inputData.YJCode);
            return new ShowMdbByomeiOutputData(result, ShowMdbByomeiStatus.Successed);
        }
        finally
        {
            _getCommonDrugInf.ReleaseResources();
        }
    }
}
