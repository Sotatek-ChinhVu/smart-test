using CommonChecker.Models;
using CommonChecker.Models.OrdInf;
using CommonChecker.Models.OrdInfDetailModel;
using CommonCheckers.OrderRealtimeChecker.Models;

namespace Interactor.CommonChecker.CommonMedicalCheck;

public interface ICommonMedicalCheck
{
    List<UnitCheckInfoModel> CheckListOrder(int hpId, long ptId, int sinday, List<OrdInfoModel> currentListOdr, List<OrdInfoModel> listCheckingOrder);

    List<ErrorInfoModel> GetErrorDetails(int hpId, long ptId, int sinday, List<UnitCheckInfoModel> listErrorInfo);

    UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel> CheckDayLimit(List<OrdInfoModel> checkingOrderList);
}
