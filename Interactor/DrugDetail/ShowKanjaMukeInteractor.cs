using Interactor.DrugInfor.CommonDrugInf;
using UseCase.DrugDetailData.ShowKanjaMuke;

namespace Interactor.DrugDetailData;

public class ShowKanjaMukeInteractor : IShowKanjaMukeInputPort
{
    private readonly IGetCommonDrugInf _getCommonDrugInf;

    public ShowKanjaMukeInteractor(IGetCommonDrugInf getCommonDrugInf)
    {
        _getCommonDrugInf = getCommonDrugInf;
    }

    public ShowKanjaMukeOutputData Handle(ShowKanjaMukeInputData inputData)
    {
        try
        {
            if (inputData.Level < 0)
            {
                return new ShowKanjaMukeOutputData(string.Empty, ShowKanjaMukeStatus.InvalidLevel);
            }
            string result = _getCommonDrugInf.ShowKanjaMuke(inputData.HpId, inputData.ItemCd, inputData.Level, inputData.DrugName, inputData.YJCode);
            return new ShowKanjaMukeOutputData(result, ShowKanjaMukeStatus.Successed);
        }
        finally
        {
            _getCommonDrugInf.ReleaseResources();
        }
    }
}
