using CommonChecker.Models;
using CommonChecker.Models.OrdInf;
using CommonCheckers.OrderRealtimeChecker.Models;

namespace Interactor.CommonChecker.CommonMedicalCheck;

public interface ICommonMedicalCheck
{
    List<UnitCheckInfoModel> CheckListOrder(int hpId, long ptId, int sinday, List<OrdInfoModel> currentListOdr, List<OrdInfoModel> listCheckingOrder);

    List<UnitCheckInfoModel> CheckListOrder(int hpId, long ptId, int sinday, List<OrdInfoModel> listCheckingOrder, RealTimeCheckerCondition checkerCondition);

    List<ErrorInfoModel> GetErrorDetails(int hpId, long ptId, int sinday, List<UnitCheckInfoModel> listErrorInfo);
}
