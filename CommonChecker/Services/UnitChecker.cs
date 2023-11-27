using CommonChecker;
using CommonChecker.Caches.Interface;
using CommonChecker.Models;
using CommonChecker.Types;
using CommonCheckers.OrderRealtimeChecker.DB;
using CommonCheckers.OrderRealtimeChecker.Enums;
using CommonCheckers.OrderRealtimeChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Services.Interface;
using Domain.Models.Diseases;
using Domain.Models.Family;
using PostgreDataContext;
using SpecialNoteFull = Domain.Models.SpecialNote.SpecialNoteModel;

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

        public void InitFinder(TenantNoTrackingDataContext _dataContext, IMasterDataCacheService _tenMstCacheService)
        {
            Finder = new RealtimeCheckerFinder(_dataContext, _tenMstCacheService);
            MasterFinder = new MasterFinder(_dataContext);
            SystemConfig = _tenMstCacheService.GetSystemConfig();
        }

        public UnitCheckerResult<TOdrInf, TOdrDetail> CheckOrder(TOdrInf checkingOrder)
        {
            UnitCheckerResult<TOdrInf, TOdrDetail> unitCheckResult = new UnitCheckerResult<TOdrInf, TOdrDetail>(CheckType, checkingOrder, Sinday, PtID);
            unitCheckResult = HandleCheckOrder(unitCheckResult);
            return unitCheckResult;
        }

        public UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> CheckOrderList(List<TOdrInf> checkingOrderList, SpecialNoteFull specialNoteModel, List<PtDiseaseModel> ptDiseaseModels, List<FamilyModel> familyModels, bool isDataOfDb)
        {

            UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> unitCheckForOrderListResult = new UnitCheckerForOrderListResult<TOdrInf, TOdrDetail>(CheckType, checkingOrderList, Sinday, PtID, specialNoteModel, ptDiseaseModels, familyModels, isDataOfDb);
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
            List<ItemCodeModel> result = new();

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
            ///DataContext.Dispose();
        }
    }
}
