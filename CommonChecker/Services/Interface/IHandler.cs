using CommonCheckers.OrderRealtimeChecker.Enums;
using CommonCheckers.OrderRealtimeChecker.Models;
using Domain.Types;

namespace CommonCheckers.OrderRealtimeChecker.Services.Interface
{
    public interface IHandler<TOdrInf, TOdrDetail>
        where TOdrInf : class, IOdrInfModel<TOdrDetail>
        where TOdrDetail : class, IOdrInfDetailModel
    {
        ActionResultType ShowOrderErrorInfo(List<UnitCheckInfoModel> listError, bool allowEditOrder);

        ActionResultType ShowOrderErrorInfo(UnitCheckerResult<TOdrInf, TOdrDetail> error);

        TOdrInf EditOrder(TOdrInf currentOrder);
    }
}
