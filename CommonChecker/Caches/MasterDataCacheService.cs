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
        private readonly List<DosageDrug> _dosageDrugList = new List<DosageDrug>();
        private readonly List<DosageMst> _dosageMstList = new List<DosageMst>();
        private readonly List<DosageDosage> _dosageDosageList = new List<DosageDosage>();
        private readonly SystemConfig _systemConfig;

        private PtInf? _ptInf;
        private int _sinday;

        public MasterDataCacheService(ITenantProvider tenantProvider) : base(tenantProvider)
        {
            _systemConfig = new SystemConfig(tenantProvider.GetNoTrackingDataContext());
        }

        public void InitCache(int hpId, List<string> itemCodeList, int sinday, long ptId)
        {
            _sinday = sinday;
            _ptInf = NoTrackingDataContext.PtInfs.FirstOrDefault(p => p.PtId == ptId && p.IsDelete == 0);
            AddCacheList(hpId, itemCodeList);
        }

        private void AddCacheList(int hpId, List<string> itemCodeList)
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
            var componentList = NoTrackingDataContext.M56ExEdIngredients.Where(i => i.HpId == hpId && yjCodeList.Contains(i.YjCd)).ToList();
            var seibunCdList = componentList.Select(s => s.SeibunCd).ToList();

            _m56ExEdIngredientList.AddRange(componentList);
            _m56ExIngrdtMainList.AddRange(NoTrackingDataContext.M56ExIngrdtMain.Where(i => i.HpId == hpId && yjCodeList.Contains(i.YjCd)).ToList());

            var yjDrugList = NoTrackingDataContext.M56YjDrugClass.Where(i => i.HpId == hpId && yjCodeList.Contains(i.YjCd)).ToList();

            _m56YjDrugClassList.AddRange(yjDrugList);
            _m56ProdrugCdList.AddRange(NoTrackingDataContext.M56ProdrugCd.Where(m => m.HpId == hpId && seibunCdList.Contains(m.SeibunCd)).ToList());
            _m56ExAnalogueList.AddRange(NoTrackingDataContext.M56ExAnalogue.Where(m => m.HpId == hpId && seibunCdList.Contains(m.SeibunCd)).ToList());

            var classCdList = yjDrugList.Select(y => y.ClassCd).Distinct().ToList();

            _m56DrugClassList.AddRange(NoTrackingDataContext.M56DrugClass.Where(d => d.HpId == hpId && classCdList.Contains(d.ClassCd)).ToList());
            #endregion

            #region Cache for kinki

            _kinkiMstList.AddRange(NoTrackingDataContext.KinkiMsts.Where(k => k.IsDeleted == 0 &&
                                                                            k.BCd != null &&
                                                                            (
                                                                                 itemCodeList.Contains(k.ACd) ||
                                                                                 itemCodeList.Contains(k.BCd)
                                                                            )).ToList());
            #endregion

            #region Cache for Dosage

            //on dosageDrug.DoeiCd equals dosageDosage.DoeiCd
            var dosageDrugListTemp = NoTrackingDataContext.DosageDrugs.Where(d => d.HpId == hpId && yjCodeList.Contains(d.YjCd) && d.RikikaUnit != null).ToList();
            var doeiCdList = dosageDrugListTemp.Select(d => d.DoeiCd).ToList();

            _dosageDrugList.AddRange(dosageDrugListTemp);
            _dosageMstList.AddRange(NoTrackingDataContext.DosageMsts.Where(d => d.IsDeleted == 0 && itemCodeList.Contains(d.ItemCd)).ToList());
            _dosageDosageList.AddRange(NoTrackingDataContext.DosageDosages.Where(d => d.HpId == hpId && string.IsNullOrEmpty(d.KyugenCd) && 
                                                                                      d.DosageCheckFlg == "1" &&
                                                                                      doeiCdList.Contains(d.DoeiCd)).ToList());
            #endregion
        }

        public List<DosageDrug> GetDosageDrugList(int hpId, List<string> itemCodeList)
        {
            AddCacheIfNeed(hpId, itemCodeList);

            return _dosageDrugList;
        }
        
        public List<DosageMst> GetDosageMstList(int hpId, List<string> itemCodeList)
        {
            AddCacheIfNeed(hpId, itemCodeList);

            return _dosageMstList;
        }
        
        public List<DosageDosage> GetDosageDosageList(int hpId, List<string> itemCodeList)
        {
            AddCacheIfNeed(hpId, itemCodeList);

            return _dosageDosageList;
        }

        public SystemConfig GetSystemConfig()
        {
            return _systemConfig;
        }

        private void AddCacheIfNeed(int hpId, List<string> itemCodeList)
        {
            List<string> itemCodeListNotCache = itemCodeList.Where(i => !_itemCodeCacheList.Contains(i)).ToList();
            if (itemCodeListNotCache == null ||
                itemCodeListNotCache.Count == 0)
            {
                return;
            }
            AddCacheList(hpId, itemCodeListNotCache);
        }

        public TenMst? GetTenMst(int hpId, string itemCode)
        {
            AddCacheIfNeed(hpId, new List<string>() { itemCode });

            return _tenMstCacheList.FirstOrDefault(t => itemCode == t.ItemCd);
        }

        public List<TenMst> GetTenMstList(int hpId, List<string> itemCodeList)
        {
            AddCacheIfNeed(hpId, itemCodeList);

            return _tenMstCacheList.Where(t => itemCodeList.Contains(t.ItemCd)).ToList();
        }

        public List<M56ExIngrdtMain> GetM56ExIngrdtMainList(int hpId, List<string> itemCodeList)
        {
            AddCacheIfNeed(hpId, itemCodeList);

            var yjCdList = _tenMstCacheList.Where(t => itemCodeList.Contains(t.ItemCd)).Select(t => t.YjCd).ToList();

            return _m56ExIngrdtMainList.Where(t => yjCdList.Contains(t.YjCd)).ToList();
        }

        public List<M56YjDrugClass> GetM56YjDrugClassList(int hpId, List<string> itemCodeList)
        {
            AddCacheIfNeed(hpId, itemCodeList);

            var yjCdList = _tenMstCacheList.Where(t => itemCodeList.Contains(t.ItemCd)).Select(t => t.YjCd).ToList();

            return _m56YjDrugClassList.Where(t => yjCdList.Contains(t.YjCd)).ToList();
        }

        public List<M56ExEdIngredients> GetM56ExEdIngredientList(int hpId, List<string> itemCodeList)
        {
            AddCacheIfNeed(hpId, itemCodeList);

            var yjCdList = _tenMstCacheList.Where(t => itemCodeList.Contains(t.ItemCd)).Select(t => t.YjCd).ToList();

            return _m56ExEdIngredientList.Where(t => yjCdList.Contains(t.YjCd)).ToList();
        }

        public List<M56ProdrugCd> GetM56ProdrugCdList(int hpId, List<string> itemCodeList)
        {
            AddCacheIfNeed(hpId, itemCodeList);

            var yjCdList = _tenMstCacheList.Where(t => itemCodeList.Contains(t.ItemCd)).Select(t => t.YjCd).ToList();
            var componentList = _m56ExEdIngredientList.Where(t => yjCdList.Contains(t.YjCd)).ToList();
            var seibunCdList = componentList.Select(s => s.SeibunCd).ToList();

            return _m56ProdrugCdList.Where(m => seibunCdList.Contains(m.SeibunCd)).ToList();
        }

        public List<M56DrugClass> GetM56DrugClassList(int hpId, List<string> itemCodeList)
        {
            AddCacheIfNeed(hpId, itemCodeList);

            var classCdList = GetM56YjDrugClassList(hpId, itemCodeList).Select(y => y.ClassCd).Distinct().ToList();

            return _m56DrugClassList.Where(d => classCdList.Contains(d.ClassCd)).ToList();
        }

        public List<M56ExAnalogue> GetM56ExAnalogueList(int hpId, List<string> itemCodeList)
        {
            AddCacheIfNeed(hpId, itemCodeList);

            var yjCdList = _tenMstCacheList.Where(t => itemCodeList.Contains(t.ItemCd)).Select(t => t.YjCd).ToList();
            var componentList = _m56ExEdIngredientList.Where(t => yjCdList.Contains(t.YjCd)).ToList();
            var seibunCdList = componentList.Select(s => s.SeibunCd).ToList();

            return _m56ExAnalogueList.Where(m => seibunCdList.Contains(m.SeibunCd)).ToList();
        }

        public List<KinkiMst> GetKinkiMstList(int hpId, List<string> itemCodeList)
        {
            AddCacheIfNeed(hpId, itemCodeList);

            return _kinkiMstList;
        }

        /// <summary>
        /// PtInf
        /// </summary>
        /// <returns></returns>
        public PtInf? GetPtInf()
        {
            return _ptInf;
        }
    }
}
