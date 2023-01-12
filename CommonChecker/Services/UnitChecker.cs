using CommonChecker;
using CommonChecker.Types;
using CommonCheckers.OrderRealtimeChecker.DB;
using CommonCheckers.OrderRealtimeChecker.Enums;
using CommonCheckers.OrderRealtimeChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Services.Interface;

namespace CommonCheckers.OrderRealtimeChecker.Services
{
    public abstract class UnitChecker<TOdrInf, TOdrDetail> : IUnitChecker<TOdrInf, TOdrDetail>
        where TOdrInf : class, IOdrInfoModel<TOdrDetail>
        where TOdrDetail : class, IOdrInfoDetailModel
    {
        public int Id;
        public RealtimeCheckerType CheckType;
        public IRealtimeCheckerFinder Finder = null!;
        public IMasterFinder? MasterFinder;
        public int HpID;
        public long PtID;
        public int Sinday;
        public ISystemConfig SystemConfig;

        public UnitCheckerResult<TOdrInf, TOdrDetail> CheckOrder(TOdrInf checkingOrder)
        {
            UnitCheckerResult<TOdrInf, TOdrDetail> unitCheckResult = new UnitCheckerResult<TOdrInf, TOdrDetail>(Id,  CheckType, checkingOrder, Sinday, PtID);
            unitCheckResult = HandleCheckOrder(unitCheckResult);
            return unitCheckResult;
        }

        public UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> CheckOrderList(List<TOdrInf> checkingOrderList)
        {
            UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> unitCheckForOrderListResult = new UnitCheckerForOrderListResult<TOdrInf, TOdrDetail>(Id, CheckType, checkingOrderList, Sinday, PtID);
            unitCheckForOrderListResult = HandleCheckOrderList(unitCheckForOrderListResult);
            return unitCheckForOrderListResult;
        }

        // For this checking, dont need to show error message
        public UnitCheckerResult<TOdrInf, TOdrDetail> CheckOnlyOrder(TOdrInf checkingOrder)
        {
            UnitCheckerResult<TOdrInf, TOdrDetail> unitCheckResult = new UnitCheckerResult<TOdrInf, TOdrDetail>(Id, CheckType, checkingOrder, Sinday, PtID);
            return HandleCheckOrder(unitCheckResult);
        }

        public abstract UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> HandleCheckOrderList(UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> unitCheckerForOrderListResult);

        public abstract UnitCheckerResult<TOdrInf, TOdrDetail> HandleCheckOrder(UnitCheckerResult<TOdrInf, TOdrDetail> unitCheckerResult);

        public List<string> GetAllOdrDetailCodeByOrderList(List<TOdrInf> orderList)
        {
            List<string> result = new List<string>();

            foreach (var order in orderList)
            {
                result.AddRange(GetAllOdrDetailCodeByOrder(order));
            }
            return result;
        }

        public List<string> GetAllOdrDetailCodeByOrder(TOdrInf order)
        {
            List<TOdrDetail> odrDetailList = order.OdrInfDetailModelsIgnoreEmpty.Where(o => o.YohoKbn == 0 && o.DrugKbn > 0).ToList();
            return odrDetailList.Select(o => o.ItemCd.Trim()).ToList();
        }
    }
}
