using CommonChecker;
using CommonChecker.Models;
using CommonChecker.Types;
using CommonCheckers.OrderRealtimeChecker.DB;
using CommonCheckers.OrderRealtimeChecker.Enums;
using CommonCheckers.OrderRealtimeChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Services.Interface;
using PostgreDataContext;

namespace CommonCheckers.OrderRealtimeChecker.Services
{
    public abstract class UnitChecker<TOdrInf, TOdrDetail> : IDisposable, IUnitChecker<TOdrInf, TOdrDetail>
        where TOdrInf : class, IOdrInfoModel<TOdrDetail>
        where TOdrDetail : class, IOdrInfoDetailModel
    {
        public RealtimeCheckerType CheckType { get; set; }

        public int HpID { get; set; }

        public long PtID { get; set; }

        public int Sinday { get; set; }

        public IRealtimeCheckerFinder? Finder { get; private set; }

        public IMasterFinder? MasterFinder { get; private set; }

        public ISystemConfig? SystemConfig { get; private set; }

        private TenantNoTrackingDataContext? _dataContext;
        public TenantNoTrackingDataContext DataContext
        {
            get => _dataContext!;
            set
            {
                _dataContext = value;
                Finder = new RealtimeCheckerFinder(value);
                MasterFinder = new MasterFinder(value);
                SystemConfig = new SystemConfig(value);
            }
        }

        public UnitCheckerResult<TOdrInf, TOdrDetail> CheckOrder(TOdrInf checkingOrder)
        {
            UnitCheckerResult<TOdrInf, TOdrDetail> unitCheckResult = new UnitCheckerResult<TOdrInf, TOdrDetail>(CheckType, checkingOrder, Sinday, PtID);
            unitCheckResult = HandleCheckOrder(unitCheckResult);
            return unitCheckResult;
        }

        public UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> CheckOrderList(List<TOdrInf> checkingOrderList)
        {
            UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> unitCheckForOrderListResult = new UnitCheckerForOrderListResult<TOdrInf, TOdrDetail>(CheckType, checkingOrderList, Sinday, PtID);
            unitCheckForOrderListResult = HandleCheckOrderList(unitCheckForOrderListResult);
            return unitCheckForOrderListResult;
        }

        // For this checking, dont need to show error message
        public UnitCheckerResult<TOdrInf, TOdrDetail> CheckOnlyOrder(TOdrInf checkingOrder)
        {
            UnitCheckerResult<TOdrInf, TOdrDetail> unitCheckResult = new UnitCheckerResult<TOdrInf, TOdrDetail>(CheckType, checkingOrder, Sinday, PtID);
            return HandleCheckOrder(unitCheckResult);
        }

        public abstract UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> HandleCheckOrderList(UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> unitCheckerForOrderListResult);

        public abstract UnitCheckerResult<TOdrInf, TOdrDetail> HandleCheckOrder(UnitCheckerResult<TOdrInf, TOdrDetail> unitCheckerResult);

        public List<ItemCodeModel> GetAllOdrDetailCodeByOrderList(List<TOdrInf> orderList)
        {
            List<ItemCodeModel> result = new List<ItemCodeModel>();

            foreach (var order in orderList)
            {
                result.AddRange(GetAllOdrDetailCodeByOrder(order));
            }
            return result;
        }

        public List<ItemCodeModel> GetAllOdrDetailCodeByOrder(TOdrInf order)
        {
            List<TOdrDetail> odrDetailList = order.OdrInfDetailModelsIgnoreEmpty.Where(o => o.YohoKbn == 0 && o.DrugKbn > 0).ToList();
            return odrDetailList.Select(o => new ItemCodeModel(o.ItemCd, o.Id)).ToList();
        }

        public void Dispose()
        {
            //DataContext.Dispose();
        }
    }
}
