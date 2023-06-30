using CommonChecker.Caches.Interface;
using CommonCheckers;
using Entity.Tenant;
using Helper.Common;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace CommonChecker.Caches
{
    public class MasterDataCacheService : RepositoryBase, IMasterDataCacheService
    {
        private readonly List<string> _itemCodeCacheList = new List<string>();
        private readonly List<TenMst> _tenMstCacheList = new List<TenMst>();
        private readonly List<M56ExEdIngredients> _m56ExEdIngredientList = new List<M56ExEdIngredients>();
        private readonly List<M56ExIngrdtMain> _m56ExIngrdtMainList = new List<M56ExIngrdtMain>();
        private readonly List<M56ProdrugCd> _m56ProdrugCdList = new List<M56ProdrugCd>();
        private readonly List<M56ExAnalogue> _m56ExAnalogueList = new List<M56ExAnalogue>();
        private readonly List<M56YjDrugClass> _m56YjDrugClassList = new List<M56YjDrugClass>();
        private readonly List<M56DrugClass> _m56DrugClassList = new List<M56DrugClass>();
        private readonly List<KinkiMst> _kinkiMstList = new List<KinkiMst>();
        private readonly SystemConfig _systemConfig;
        
        private PtInf _ptInf = new PtInf();
        private int _sinday;

        public MasterDataCacheService(ITenantProvider tenantProvider) : base(tenantProvider)
        {
            _systemConfig = new SystemConfig(tenantProvider.GetNoTrackingDataContext());
        }

        public void InitCache(List<string> itemCodeList, int sinday, long ptId)
        {
            _sinday = sinday;
            _ptInf = NoTrackingDataContext.PtInfs.FirstOrDefault(p => p.PtId == ptId && p.IsDelete == 0) ?? new PtInf();
            AddCacheList(itemCodeList);
        }

        private void AddCacheList(List<string> itemCodeList)
        {
            if (itemCodeList == null || itemCodeList.Count == 0)
            {
                return;
            }

            _itemCodeCacheList.AddRange(itemCodeList);

            var tenMstList = NoTrackingDataContext.TenMsts.Where(t => itemCodeList.Contains(t.ItemCd) && t.IsDeleted == 0 && t.StartDate <= _sinday && _sinday <= t.EndDate).ToList();
            _tenMstCacheList.AddRange(tenMstList);

            var yjCodeList = tenMstList.Select(t => t.YjCd).Distinct().ToList();

            #region Cache for duplication
            var componentList = NoTrackingDataContext.M56ExEdIngredients.Where(i => yjCodeList.Contains(i.YjCd)).ToList();
            var seibunCdList = componentList.Select(s => s.SeibunCd).ToList();

            _m56ExEdIngredientList.AddRange(componentList);
            _m56ExIngrdtMainList.AddRange(NoTrackingDataContext.M56ExIngrdtMain.Where(i => yjCodeList.Contains(i.YjCd)).ToList());

            var yjDrugList = NoTrackingDataContext.M56YjDrugClass.Where(i => yjCodeList.Contains(i.YjCd)).ToList();

            _m56YjDrugClassList.AddRange(yjDrugList);
            _m56ProdrugCdList.AddRange(NoTrackingDataContext.M56ProdrugCd.Where(m => seibunCdList.Contains(m.SeibunCd)).ToList());
            _m56ExAnalogueList.AddRange(NoTrackingDataContext.M56ExAnalogue.Where(m => seibunCdList.Contains(m.SeibunCd)).ToList());

            var classCdList = yjDrugList.Select(y => y.ClassCd).Distinct().ToList();

            _m56DrugClassList.AddRange(NoTrackingDataContext.M56DrugClass.Where(d => classCdList.Contains(d.ClassCd)).ToList());
            #endregion

            #region Cache for kinki

            _kinkiMstList.AddRange(NoTrackingDataContext.KinkiMsts.Where(k => k.IsDeleted == 0 &&
                                                                            k.BCd != null &&
                                                                            (
                                                                                 itemCodeList.Contains(k.ACd) ||
                                                                                 itemCodeList.Contains(k.BCd)
                                                                            )).ToList());
            #endregion
        }

        public SystemConfig GetSystemConfig()
        {
            return _systemConfig;
        }

        private void AddCacheIfNeed(List<string> itemCodeList)
        {
            List<string> itemCodeListNotCache = itemCodeList.Where(i => !_itemCodeCacheList.Contains(i)).ToList();
            if (itemCodeListNotCache == null || 
                itemCodeListNotCache.Count == 0)
            {
                return;
            }
            AddCacheList(itemCodeListNotCache);
        }

        public TenMst? GetTenMst(string itemCode, int sinday)
        {
            AddCacheIfNeed(new List<string>() { itemCode });

            return _tenMstCacheList.FirstOrDefault(t => itemCode == t.ItemCd && t.StartDate <= sinday && sinday <= t.EndDate);
        }

        public List<TenMst> GetTenMstList(List<string> itemCodeList, int sinday)
        {
            AddCacheIfNeed(itemCodeList);

            return _tenMstCacheList.Where(t => itemCodeList.Contains(t.ItemCd) && t.StartDate <= sinday && sinday <= t.EndDate).ToList();
        }

        public List<M56ExIngrdtMain> GetM56ExIngrdtMainList(List<string> itemCodeList)
        {
            AddCacheIfNeed(itemCodeList);

            var yjCdList = _tenMstCacheList.Where(t => itemCodeList.Contains(t.ItemCd)).Select(t => t.YjCd).ToList();

            return _m56ExIngrdtMainList.Where(t => yjCdList.Contains(t.YjCd)).ToList();
        }

        public List<M56YjDrugClass> GetM56YjDrugClassList(List<string> itemCodeList)
        {
            AddCacheIfNeed(itemCodeList);

            var yjCdList = _tenMstCacheList.Where(t => itemCodeList.Contains(t.ItemCd)).Select(t => t.YjCd).ToList();

            return _m56YjDrugClassList.Where(t => yjCdList.Contains(t.YjCd)).ToList();
        }

        public List<M56ExEdIngredients> GetM56ExEdIngredientList(List<string> itemCodeList)
        {
            AddCacheIfNeed(itemCodeList);

            var yjCdList = _tenMstCacheList.Where(t => itemCodeList.Contains(t.ItemCd)).Select(t => t.YjCd).ToList();

            return _m56ExEdIngredientList.Where(t => yjCdList.Contains(t.YjCd)).ToList();
        }

        public List<M56ProdrugCd> GetM56ProdrugCdList(List<string> itemCodeList)
        {
            AddCacheIfNeed(itemCodeList);

            var yjCdList = _tenMstCacheList.Where(t => itemCodeList.Contains(t.ItemCd)).Select(t => t.YjCd).ToList();
            var componentList = _m56ExEdIngredientList.Where(t => yjCdList.Contains(t.YjCd)).ToList();
            var seibunCdList = componentList.Select(s => s.SeibunCd).ToList();

            return _m56ProdrugCdList.Where(m => seibunCdList.Contains(m.SeibunCd)).ToList();
        }

        public List<M56DrugClass> GetM56DrugClassList(List<string> itemCodeList)
        {
            AddCacheIfNeed(itemCodeList);

            var classCdList = GetM56YjDrugClassList(itemCodeList).Select(y => y.ClassCd).Distinct().ToList();

            return _m56DrugClassList.Where(d => classCdList.Contains(d.ClassCd)).ToList();
        }

        public List<M56ExAnalogue> GetM56ExAnalogueList(List<string> itemCodeList)
        {
            AddCacheIfNeed(itemCodeList);

            var yjCdList = _tenMstCacheList.Where(t => itemCodeList.Contains(t.ItemCd)).Select(t => t.YjCd).ToList();
            var componentList = _m56ExEdIngredientList.Where(t => yjCdList.Contains(t.YjCd)).ToList();
            var seibunCdList = componentList.Select(s => s.SeibunCd).ToList();

            return _m56ExAnalogueList.Where(m => seibunCdList.Contains(m.SeibunCd)).ToList();
        }

        public List<KinkiMst> GetKinkiMstList(List<string> itemCodeList)
        {
            AddCacheIfNeed(itemCodeList);

            return _kinkiMstList;
        }

        public PtInf GetPtInf()
        {
            return _ptInf;
        }
    }
}
