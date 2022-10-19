using CommonCheckers.OrderRealtimeChecker.Models;

namespace CommonCheckers.OrderRealtimeChecker.Services.Interface
{
    public interface IUnitChecker<TOdrInf, TOdrDetail>
        where TOdrInf : class, IOdrInfModel<TOdrDetail>
        where TOdrDetail : class, IOdrInfDetailModel
    {
        UnitCheckerResult<TOdrInf, TOdrDetail> HandleCheckOrder(UnitCheckerResult<TOdrInf, TOdrDetail> unitCheckerResult);
    }
}
