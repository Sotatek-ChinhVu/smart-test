using CommonChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Models;
using Entity.Tenant;
using Helper.Common;
using Helper.Extension;
using PostgreDataContext;

namespace CommonCheckers.OrderRealtimeChecker.DB
{
    public class RealtimeCheckerFinder : IRealtimeCheckerFinder
    {
        private readonly TenantNoTrackingDataContext _tenantNoTrackingDataContext;

        public RealtimeCheckerFinder(TenantNoTrackingDataContext tenantNoTrackingDataContext)
        {
            _tenantNoTrackingDataContext = tenantNoTrackingDataContext;
        }

        public Dictionary<string, string> GetYjCdListByItemCdList(int hpId, List<string> itemCdList, int sinDate)
        {
            return _tenantNoTrackingDataContext
                .TenMsts
                .Where(i => i.HpId == hpId && itemCdList.Contains(i.ItemCd) && i.StartDate <= sinDate && sinDate <= i.EndDate)
                .Select(t => new { t.ItemCd, t.YjCd })
                .ToDictionary(t => t.ItemCd ?? string.Empty, t => t.YjCd ?? string.Empty);
        }

        public List<PtAlrgyFoodModel> GetFoodAllergyByPtId(int hpId, long ptId, int sinDate)
        {
            var listPtAlrgyFood =
                _tenantNoTrackingDataContext.PtAlrgyFoods
                .Where(p => p.HpId == hpId && p.PtId == ptId && p.IsDeleted != 1)
                .AsEnumerable()
                .Select(p => new PtAlrgyFoodModel(p.HpId, p.PtId, p.SeqNo, p.SortNo, p.AlrgyKbn ?? string.Empty, p.StartDate, p.EndDate, p.Cmt ?? string.Empty, p.IsDeleted, string.Empty))
                .ToList();

            var listFilteredBySinData = listPtAlrgyFood
                .Where(p => CIUtil.FullStartDate(p.StartDate) <= sinDate && sinDate <= CIUtil.FullEndDate(p.EndDate))
                .Select(p => new PtAlrgyFoodModel(p.HpId, p.PtId, p.SeqNo, p.SortNo, p.AlrgyKbn ?? string.Empty, p.StartDate, p.EndDate, p.Cmt ?? string.Empty, p.IsDeleted, string.Empty))
                .ToList();
            return listFilteredBySinData;
        }

        public List<PtAlrgyDrugModel> GetDrugAllergyByPtId(int hpId, long ptId, int sinDate)
        {
            var listPtAlrgyDrug = _tenantNoTrackingDataContext.PtAlrgyDrugs
                .Where(p => p.HpId == hpId && p.PtId == ptId && p.IsDeleted != 1)
                .AsEnumerable()
                .Select(p => new PtAlrgyDrugModel(p.HpId, p.PtId, p.SeqNo, p.SortNo, p.ItemCd ?? string.Empty, p.DrugName ?? string.Empty, p.StartDate, p.EndDate, p.Cmt ?? string.Empty, p.IsDeleted))
                .ToList();


            var listFilteredBySinData = listPtAlrgyDrug
                .Where(p => CIUtil.FullStartDate(p.StartDate) <= sinDate && sinDate <= CIUtil.FullEndDate(p.EndDate))
                .Select(p => new PtAlrgyDrugModel(p.HpId, p.PtId, p.SeqNo, p.SortNo, p.ItemCd ?? string.Empty, p.DrugName ?? string.Empty, p.StartDate, p.EndDate, p.Cmt ?? string.Empty, p.IsDeleted))
                .ToList();
            return listFilteredBySinData;
        }

        public PtInf GetPatientInfo(int hpId, long ptId)
        {
            return _tenantNoTrackingDataContext.PtInfs.Where(p => p.HpId == hpId && p.PtId == ptId && p.IsDelete == 0).FirstOrDefault() ?? new PtInf();
        }

        public KensaInfDetail GetBodyInfo(int hpId, long ptId, int sinday, string kensaItemCode)
        {
            return _tenantNoTrackingDataContext.KensaInfDetails
                .Where(k => k.HpId == hpId && k.PtId == ptId && k.IraiDate <= sinday && k.KensaItemCd == kensaItemCode && !string.IsNullOrEmpty(k.ResultVal))
                .OrderByDescending(k => k.IraiDate).FirstOrDefault() ?? new KensaInfDetail();
        }

        public PhysicalAverage GetCommonBodyInfo(int birthDay, int sinday)
        {
            int ageY = 0;
            int ageM = 0;
            int ageD = 0;
            CIUtil.SDateToDecodeAge(birthDay, sinday, ref ageY, ref ageM, ref ageD);

            int sinYear = sinday / 10000;
            return _tenantNoTrackingDataContext.PhysicalAverage
                .Where
                (
                    p =>
                    p.JissiYear <= sinYear &&
                    p.AgeYear <= ageY &&
                    p.AgeMonth <= ageM &&
                    p.AgeDay <= ageD
                )
                .OrderByDescending(p => p.AgeYear)
                .ThenByDescending(p => p.AgeMonth)
                .ThenByDescending(p => p.AgeDay)
                .FirstOrDefault() ?? new PhysicalAverage();
        }

        private IQueryable<M56ExIngrdtMain> GetDrugTypeInfo(int haigouSetting)
        {
            return _tenantNoTrackingDataContext.M56ExIngrdtMain.Where
                                                                            (
                                                                                i => haigouSetting == 0 ||
                                                                                     haigouSetting == 1 && (i.HaigouFlg != "1" || i.YuekiFlg != "1") ||
                                                                                     haigouSetting == 2 && (i.HaigouFlg != "1" || i.KanpoFlg != "1") ||
                                                                                     haigouSetting == 3 && (i.HaigouFlg != "1" || (i.YuekiFlg != "1" && i.KanpoFlg != "1"))
                                                                            );
        }

        #region Check allergy

        public List<DrugAllergyResultModel> CheckDuplicatedComponent(int hpID, long ptID, int sinDate, List<string> listItemCode, List<string> listComparedItemCode)
        {
            bool IsNoMasterData()
            {
                return _tenantNoTrackingDataContext.M56ExEdIngredients.Count() == 0;
            }

            List<string> listDrugAllergyAsPatientCode = listComparedItemCode;
            List<DrugAllergyResultModel> checkedResult = new List<DrugAllergyResultModel>();

            if (!IsNoMasterData())
            {
                var listDrugAllergyAsPatientInfo =
                    (from drugMst in _tenantNoTrackingDataContext.TenMsts.Where(i => listDrugAllergyAsPatientCode.Contains(i.ItemCd) && i.StartDate <= sinDate && sinDate <= i.EndDate)
                     join componentInfo in _tenantNoTrackingDataContext.M56ExEdIngredients.Where(i => i.Sbt == 1 || i.Sbt == 2 && i.TenkabutuCheck == "1")
                     on drugMst.YjCd equals componentInfo.YjCd
                     select new
                     {
                         drugMst.ItemCd,
                         drugMst.YjCd,
                         componentInfo.SeibunCd
                     }).ToList();

                var listCheckingDrugInfo =
                    (from drugMst in _tenantNoTrackingDataContext.TenMsts.Where(i => listItemCode.Contains(i.ItemCd) && i.StartDate <= sinDate && sinDate <= i.EndDate)
                     join componentInfo in _tenantNoTrackingDataContext.M56ExEdIngredients.Where(i => i.Sbt == 1 || i.Sbt == 2 && i.TenkabutuCheck == "1")
                     on drugMst.YjCd equals componentInfo.YjCd
                     select new
                     {
                         drugMst.ItemCd,
                         drugMst.YjCd,
                         componentInfo.SeibunCd,
                         componentInfo.SeqNo,
                     }).ToList();

                List<DrugAllergyResultModel> tempResult = new List<DrugAllergyResultModel>();
                listCheckingDrugInfo.ToList().ForEach((checkingDrug) =>
                {
                    var checkUsage = listDrugAllergyAsPatientInfo.Where
                                                                 (
                                                                       a => a.SeibunCd == checkingDrug.SeibunCd
                                                                 )
                                                                 .Select
                                                                 (
                                                                       a => new DrugAllergyResultModel()
                                                                       {
                                                                           Level = 1,
                                                                           YjCd = checkingDrug.YjCd,
                                                                           ItemCd = checkingDrug.ItemCd,
                                                                           SeibunCd = checkingDrug.SeibunCd,
                                                                           SeqNo = checkingDrug.SeqNo,
                                                                           AllergyYjCd = a.YjCd,
                                                                           AllergyItemCd = a.ItemCd
                                                                       }
                                                                 )
                                                                 .ToList();
                    tempResult.AddRange(checkUsage);
                });

                var groupResult = tempResult.GroupBy(p => new { p.YjCd, p.AllergyYjCd })
                                            .Select(g => g.ToList())
                                            .ToList();

                groupResult.ForEach((group) =>
                {
                    var generalComponent = group.Where(a => a.SeqNo == "000").FirstOrDefault();
                    if (generalComponent != null)
                    {
                        group = new List<DrugAllergyResultModel>() { generalComponent };
                    }
                    checkedResult.AddRange(group);
                });
            }

            return checkedResult;
        }

        public List<DrugAllergyResultModel> CheckProDrug(int hpID, long ptID, int sinDate, List<string> listItemCode, List<string> listComparedItemCode)
        {
            List<DrugAllergyResultModel> result = new List<DrugAllergyResultModel>();
            List<string> listDrugAllergyAsPatientCode = listComparedItemCode;

            var listCheckingDrugInfo =
                (from drugMst in _tenantNoTrackingDataContext.TenMsts.Where(i => listItemCode.Contains(i.ItemCd) && i.StartDate <= sinDate && sinDate <= i.EndDate)
                 join componentInfo in _tenantNoTrackingDataContext.M56ExEdIngredients.Where(i => !string.IsNullOrEmpty(i.ProdrugCheck) && i.ProdrugCheck != "0")  //Filter ProDrug >= 1
                 on drugMst.YjCd equals componentInfo.YjCd
                 join drugPro in _tenantNoTrackingDataContext.M56ProdrugCd
                 on componentInfo.SeibunCd equals drugPro.SeibunCd
                 select new
                 {
                     drugMst.ItemCd,
                     drugMst.YjCd,
                     componentInfo.SeibunCd,
                     componentInfo.SeibunIndexCd,
                     componentInfo.ProdrugCheck,
                     componentInfo.Sbt,
                     drugPro.KasseitaiCd
                 }).ToList();

            var listDrugAllergyAsPatientInfo =
                (from drugMst in _tenantNoTrackingDataContext.TenMsts.Where(i => listDrugAllergyAsPatientCode.Contains(i.ItemCd) && i.StartDate <= sinDate && sinDate <= i.EndDate)
                 join componentInfo in _tenantNoTrackingDataContext.M56ExEdIngredients.Where(i => !string.IsNullOrEmpty(i.ProdrugCheck) && i.ProdrugCheck != "0")  //Filter ProDrug >= 1
                 on drugMst.YjCd equals componentInfo.YjCd
                 join drugPro in _tenantNoTrackingDataContext.M56ProdrugCd
                 on componentInfo.SeibunCd equals drugPro.SeibunCd
                 select new
                 {
                     drugMst.ItemCd,
                     drugMst.YjCd,
                     componentInfo.SeibunCd,
                     componentInfo.SeibunIndexCd,
                     componentInfo.ProdrugCheck,
                     componentInfo.Sbt,
                     drugPro.KasseitaiCd,
                 }).ToList();

            listCheckingDrugInfo.ToList().ForEach((checkingDrug) =>
            {
                var checkUsage = listDrugAllergyAsPatientInfo.Where
                                                             (
                                                                   a => a.KasseitaiCd == checkingDrug.KasseitaiCd
                                                             )
                                                             .Select
                                                             (
                                                                   item => new DrugAllergyResultModel()
                                                                   {
                                                                       Level = 2,
                                                                       YjCd = checkingDrug.YjCd,
                                                                       ItemCd = checkingDrug.ItemCd,
                                                                       SeibunCd = checkingDrug.SeibunCd,
                                                                       AllergyYjCd = item.YjCd,
                                                                       AllergyItemCd = item.ItemCd,
                                                                       AllergySeibunCd = item.SeibunCd,
                                                                       Tag = item.KasseitaiCd
                                                                   }
                                                             )
                                                             .ToList();
                result.AddRange(checkUsage);
            });

            return result;
        }

        public List<DrugAllergyResultModel> CheckSameComponent(int hpID, long ptID, int sinDate, List<string> listItemCode, List<string> listComparedItemCode)
        {
            List<DrugAllergyResultModel> result = new List<DrugAllergyResultModel>();
            List<string> listDrugAllergyAsPatientCode = listComparedItemCode;

            var listCheckingDrugInfo =
                (from drugMst in _tenantNoTrackingDataContext.TenMsts.Where(i => listItemCode.Contains(i.ItemCd) && i.StartDate <= sinDate && sinDate <= i.EndDate)
                 join componentInfo in _tenantNoTrackingDataContext.M56ExEdIngredients.Where(i => i.AnalogueCheck == "1")
                 on drugMst.YjCd equals componentInfo.YjCd
                 join drugAnalogue in _tenantNoTrackingDataContext.M56ExAnalogue
                 on componentInfo.SeibunCd equals drugAnalogue.SeibunCd
                 select new
                 {
                     drugMst.ItemCd,
                     drugMst.YjCd,
                     componentInfo.SeibunCd,
                     componentInfo.SeibunIndexCd,
                     componentInfo.AnalogueCheck,
                     componentInfo.Sbt,
                     drugAnalogue.AnalogueCd
                 }).ToList();

            var listDrugAllergyAsPatientInfo =
                (from drugMst in _tenantNoTrackingDataContext.TenMsts.Where(i => listDrugAllergyAsPatientCode.Contains(i.ItemCd) && i.StartDate <= sinDate && sinDate <= i.EndDate)
                 join componentInfo in _tenantNoTrackingDataContext.M56ExEdIngredients.Where(i => i.AnalogueCheck == "1")
                 on drugMst.YjCd equals componentInfo.YjCd
                 join drugAnalogue in _tenantNoTrackingDataContext.M56ExAnalogue
                 on componentInfo.SeibunCd equals drugAnalogue.SeibunCd
                 select new
                 {
                     drugMst.ItemCd,
                     drugMst.YjCd,
                     componentInfo.SeibunCd,
                     componentInfo.SeibunIndexCd,
                     componentInfo.AnalogueCheck,
                     componentInfo.Sbt,
                     drugAnalogue.AnalogueCd
                 }).ToList();

            listCheckingDrugInfo.ToList().ForEach((checkingDrug) =>
            {
                var checkUsage = listDrugAllergyAsPatientInfo.Where
                                                             (
                                                                   a => a.AnalogueCd == checkingDrug.AnalogueCd
                                                             )
                                                             .Select
                                                             (
                                                                   item => new DrugAllergyResultModel()
                                                                   {
                                                                       Level = 3,
                                                                       YjCd = checkingDrug.YjCd,
                                                                       ItemCd = checkingDrug.ItemCd,
                                                                       SeibunCd = checkingDrug.SeibunCd,
                                                                       AllergyYjCd = item.YjCd,
                                                                       AllergyItemCd = item.ItemCd,
                                                                       AllergySeibunCd = item.SeibunCd,
                                                                       Tag = item.AnalogueCd
                                                                   }
                                                             )
                                                             .ToList();
                result.AddRange(checkUsage);
            });

            return result;
        }

        public List<DrugAllergyResultModel> CheckDuplicatedClass(int hpID, long ptID, int sinDate, List<string> listItemCode, List<string> listComparedItemCode)
        {
            List<DrugAllergyResultModel> result;
            List<string> listDrugAllergyAsPatientCode = listComparedItemCode;

            var listCheckingDrugInfo =
                 (from drugMst in _tenantNoTrackingDataContext.TenMsts.Where(i => listItemCode.Contains(i.ItemCd) && i.StartDate <= sinDate && sinDate <= i.EndDate)
                  join componentInfo in _tenantNoTrackingDataContext.M56AlrgyDerivatives
                  on drugMst.YjCd equals componentInfo.YjCd
                  join drvalrgyCode in _tenantNoTrackingDataContext.M56DrvalrgyCode
                  on componentInfo.DrvalrgyCd equals drvalrgyCode.DrvalrgyCd
                  select new
                  {
                      drugMst.ItemCd,
                      drugMst.YjCd,
                      componentInfo.SeibunCd,
                      componentInfo.DrvalrgyCd,
                      drvalrgyCode.RankNo
                  }).ToList();

            var listDrugAllergyAsPatientInfo =
                 (from drugMst in _tenantNoTrackingDataContext.TenMsts.Where(i => listDrugAllergyAsPatientCode.Contains(i.ItemCd) && i.StartDate <= sinDate && sinDate <= i.EndDate)
                  join componentInfo in _tenantNoTrackingDataContext.M56AlrgyDerivatives
                  on drugMst.YjCd equals componentInfo.YjCd
                  join drvalrgyCode in _tenantNoTrackingDataContext.M56DrvalrgyCode
                  on componentInfo.DrvalrgyCd equals drvalrgyCode.DrvalrgyCd
                  select new
                  {
                      drugMst.ItemCd,
                      drugMst.YjCd,
                      componentInfo.SeibunCd,
                      componentInfo.DrvalrgyCd,
                      drvalrgyCode.RankNo
                  }).ToList();

            var checkedResult =
                from allergyInfo in listDrugAllergyAsPatientInfo
                join checkingInfo in listCheckingDrugInfo
                on new { allergyInfo.DrvalrgyCd, allergyInfo.RankNo } equals new { checkingInfo.DrvalrgyCd, checkingInfo.RankNo }
                select new
                {
                    Level = 4,
                    checkingInfo.YjCd,
                    checkingInfo.ItemCd,
                    SeibunCd = checkingInfo.DrvalrgyCd,
                    AllergyYjCd = allergyInfo.YjCd,
                    AllergySeibunCd = allergyInfo.SeibunCd,
                    allergyInfo.DrvalrgyCd,
                    allergyInfo.RankNo
                };

            checkedResult = checkedResult
                .GroupBy(c => new { c.YjCd, c.AllergyYjCd })
                .Select(c => c.OrderBy(o => o.RankNo).FirstOrDefault());

            result =
                (from r in checkedResult.AsEnumerable()
                 select new DrugAllergyResultModel() { Level = 4, YjCd = r.YjCd, SeibunCd = r.SeibunCd, AllergyYjCd = r.AllergyYjCd, AllergySeibunCd = r.AllergySeibunCd, Tag = r.DrvalrgyCd, ItemCd = r.ItemCd }
                 ).ToList();

            return result;
        }

        #endregion

        #region Check duplicated component
        public List<DrugAllergyResultModel> CheckDuplicatedComponentForDuplication(int hpID, long ptID, int sinDate, List<string> listItemCode, List<string> listComparedItemCode, int haigouSetting)
        {
            List<DrugAllergyResultModel> checkedResult = new List<DrugAllergyResultModel>();

            var listDrugAllergyAsPatientInfo =
                    (from drugMst in _tenantNoTrackingDataContext.TenMsts.Where(i => listComparedItemCode.Contains(i.ItemCd) && i.StartDate <= sinDate && sinDate <= i.EndDate)
                     join componentInfo in _tenantNoTrackingDataContext.M56ExEdIngredients.Where(i => i.Sbt == 1)
                     on drugMst.YjCd equals componentInfo.YjCd
                     join drugTypeInfo in GetDrugTypeInfo(haigouSetting)
                     on drugMst.YjCd equals drugTypeInfo.YjCd
                     select new
                     {
                         drugMst.ItemCd,
                         drugMst.YjCd,
                         componentInfo.SeibunCd,
                         drugTypeInfo.ZensinsayoFlg,
                         drugTypeInfo.YohoCd,
                         drugTypeInfo.HaigouFlg,
                         drugTypeInfo.YuekiFlg,
                         drugTypeInfo.KanpoFlg,
                     }).ToList();

            var listCheckingDrugInfo =
                (from drugMst in _tenantNoTrackingDataContext.TenMsts.Where(i => listItemCode.Contains(i.ItemCd) && i.StartDate <= sinDate && sinDate <= i.EndDate)
                 join componentInfo in _tenantNoTrackingDataContext.M56ExEdIngredients.Where(i => i.Sbt == 1)
                 on drugMst.YjCd equals componentInfo.YjCd
                 join drugTypeInfo in GetDrugTypeInfo(haigouSetting)
                 on drugMst.YjCd equals drugTypeInfo.YjCd
                 select new
                 {
                     drugMst.ItemCd,
                     drugMst.YjCd,
                     componentInfo.SeibunCd,
                     componentInfo.SeqNo,
                     drugTypeInfo.ZensinsayoFlg,
                     drugTypeInfo.YohoCd,
                     drugTypeInfo.HaigouFlg,
                     drugTypeInfo.YuekiFlg,
                     drugTypeInfo.KanpoFlg,
                 }).ToList();

            List<DrugAllergyResultModel> tempResult = new List<DrugAllergyResultModel>();
            listCheckingDrugInfo.ToList().ForEach((checkingDrug) =>
            {
                var checkUsage = listDrugAllergyAsPatientInfo.Where
                                                             (
                                                                   a => a.SeibunCd == checkingDrug.SeibunCd &&
                                                                        a.ItemCd != checkingDrug.ItemCd &&
                                                                        (checkingDrug.ZensinsayoFlg == "1" || checkingDrug.YohoCd == a.YohoCd)
                                                             )
                                                             .Select
                                                             (
                                                                   a => new DrugAllergyResultModel()
                                                                   {
                                                                       Level = 1,
                                                                       YjCd = checkingDrug.YjCd,
                                                                       ItemCd = checkingDrug.ItemCd,
                                                                       SeibunCd = checkingDrug.SeibunCd,
                                                                       SeqNo = checkingDrug.SeqNo,
                                                                       AllergyYjCd = a.YjCd,
                                                                       AllergyItemCd = a.ItemCd
                                                                   }
                                                             )
                                                             .ToList();
                tempResult.AddRange(checkUsage);
            });

            var groupResult = tempResult.GroupBy(p => new { p.YjCd, p.AllergyYjCd })
                                        .Select(g => g.ToList())
                                        .ToList();

            groupResult.ForEach((group) =>
            {
                var generalComponent = group.Where(a => a.SeqNo == "000").FirstOrDefault();
                if (generalComponent != null)
                {
                    group = new List<DrugAllergyResultModel>() { generalComponent };
                }
                checkedResult.AddRange(group);
            });

            return checkedResult;
        }

        public List<DrugAllergyResultModel> CheckProDrugForDuplication(int hpID, long ptID, int sinDate, List<string> listItemCode, List<string> listComparedItemCode, int haigouSetting)
        {
            List<DrugAllergyResultModel> result = new List<DrugAllergyResultModel>();

            var listCheckingDrugInfo =
                (from drugMst in _tenantNoTrackingDataContext.TenMsts.Where(i => listItemCode.Contains(i.ItemCd) && i.StartDate <= sinDate && sinDate <= i.EndDate)
                 join componentInfo in _tenantNoTrackingDataContext.M56ExEdIngredients.Where(i => !string.IsNullOrEmpty(i.ProdrugCheck) && i.ProdrugCheck != "0") //Filter ProDrug >= 1
                 on drugMst.YjCd equals componentInfo.YjCd
                 join drugTypeInfo in GetDrugTypeInfo(haigouSetting)
                 on drugMst.YjCd equals drugTypeInfo.YjCd
                 join drugPro in _tenantNoTrackingDataContext.M56ProdrugCd
                 on componentInfo.SeibunCd equals drugPro.SeibunCd
                 select new
                 {
                     drugMst.ItemCd,
                     drugMst.YjCd,
                     componentInfo.SeibunCd,
                     componentInfo.SeibunIndexCd,
                     componentInfo.ProdrugCheck,
                     componentInfo.Sbt,
                     drugPro.KasseitaiCd,
                     drugTypeInfo.ZensinsayoFlg,
                     drugTypeInfo.YohoCd,
                     drugTypeInfo.HaigouFlg,
                     drugTypeInfo.YuekiFlg,
                     drugTypeInfo.KanpoFlg,
                 }).ToList();

            var listDrugAllergyAsPatientInfo =
                (from drugMst in _tenantNoTrackingDataContext.TenMsts.Where(i => listComparedItemCode.Contains(i.ItemCd) && i.StartDate <= sinDate && sinDate <= i.EndDate)
                 join componentInfo in _tenantNoTrackingDataContext.M56ExEdIngredients.Where(i => !string.IsNullOrEmpty(i.ProdrugCheck) && i.ProdrugCheck != "0") //Filter ProDrug >= 1
                 on drugMst.YjCd equals componentInfo.YjCd
                 join drugTypeInfo in GetDrugTypeInfo(haigouSetting)
                 on drugMst.YjCd equals drugTypeInfo.YjCd
                 join drugPro in _tenantNoTrackingDataContext.M56ProdrugCd
                 on componentInfo.SeibunCd equals drugPro.SeibunCd
                 select new
                 {
                     drugMst.ItemCd,
                     drugMst.YjCd,
                     componentInfo.SeibunCd,
                     componentInfo.SeibunIndexCd,
                     componentInfo.ProdrugCheck,
                     componentInfo.Sbt,
                     drugPro.KasseitaiCd,
                     drugTypeInfo.ZensinsayoFlg,
                     drugTypeInfo.YohoCd,
                     drugTypeInfo.HaigouFlg,
                     drugTypeInfo.YuekiFlg,
                     drugTypeInfo.KanpoFlg,
                 }).ToList();

            listCheckingDrugInfo.ToList().ForEach((checkingDrug) =>
            {
                var checkUsage = listDrugAllergyAsPatientInfo.Where
                                                             (
                                                                   a => a.KasseitaiCd == checkingDrug.KasseitaiCd &&
                                                                        a.ItemCd != checkingDrug.ItemCd &&
                                                                        (checkingDrug.ZensinsayoFlg == "1" || checkingDrug.YohoCd == a.YohoCd)
                                                             )
                                                             .Select
                                                             (
                                                                   item => new DrugAllergyResultModel()
                                                                   {
                                                                       Level = 2,
                                                                       YjCd = checkingDrug.YjCd,
                                                                       ItemCd = checkingDrug.ItemCd,
                                                                       SeibunCd = checkingDrug.SeibunCd,
                                                                       AllergyYjCd = item.YjCd,
                                                                       AllergyItemCd = item.ItemCd,
                                                                       AllergySeibunCd = item.SeibunCd,
                                                                       Tag = item.KasseitaiCd
                                                                   }
                                                             )
                                                             .ToList();
                result.AddRange(checkUsage);
            });

            return result;
        }

        public List<DrugAllergyResultModel> CheckSameComponentForDuplication(int hpID, long ptID, int sinDate, List<string> listItemCode, List<string> listComparedItemCode, int haigouSetting)
        {
            List<DrugAllergyResultModel> result = new List<DrugAllergyResultModel>();

            var listCheckingDrugInfo =
                (from drugMst in _tenantNoTrackingDataContext.TenMsts.Where(i => listItemCode.Contains(i.ItemCd) && i.StartDate <= sinDate && sinDate <= i.EndDate)
                 join componentInfo in _tenantNoTrackingDataContext.M56ExEdIngredients.Where(i => i.AnalogueCheck == "1")
                 on drugMst.YjCd equals componentInfo.YjCd
                 join drugTypeInfo in GetDrugTypeInfo(haigouSetting)
                 on drugMst.YjCd equals drugTypeInfo.YjCd
                 join drugAnalogue in _tenantNoTrackingDataContext.M56ExAnalogue
                 on componentInfo.SeibunCd equals drugAnalogue.SeibunCd
                 select new
                 {
                     drugMst.ItemCd,
                     drugMst.YjCd,
                     componentInfo.SeibunCd,
                     componentInfo.SeibunIndexCd,
                     componentInfo.AnalogueCheck,
                     componentInfo.Sbt,
                     drugAnalogue.AnalogueCd,
                     drugTypeInfo.ZensinsayoFlg,
                     drugTypeInfo.YohoCd,
                     drugTypeInfo.HaigouFlg,
                     drugTypeInfo.YuekiFlg,
                     drugTypeInfo.KanpoFlg,
                 }).ToList();

            var listDrugAllergyAsPatientInfo =
                (from drugMst in _tenantNoTrackingDataContext.TenMsts.Where(i => listComparedItemCode.Contains(i.ItemCd) && i.StartDate <= sinDate && sinDate <= i.EndDate)
                 join componentInfo in _tenantNoTrackingDataContext.M56ExEdIngredients.Where(i => i.AnalogueCheck == "1")
                 on drugMst.YjCd equals componentInfo.YjCd
                 join drugTypeInfo in GetDrugTypeInfo(haigouSetting)
                 on drugMst.YjCd equals drugTypeInfo.YjCd
                 join drugAnalogue in _tenantNoTrackingDataContext.M56ExAnalogue
                 on componentInfo.SeibunCd equals drugAnalogue.SeibunCd
                 select new
                 {
                     drugMst.ItemCd,
                     drugMst.YjCd,
                     componentInfo.SeibunCd,
                     componentInfo.SeibunIndexCd,
                     componentInfo.AnalogueCheck,
                     componentInfo.Sbt,
                     drugAnalogue.AnalogueCd,
                     drugTypeInfo.ZensinsayoFlg,
                     drugTypeInfo.YohoCd,
                     drugTypeInfo.HaigouFlg,
                     drugTypeInfo.YuekiFlg,
                     drugTypeInfo.KanpoFlg,
                 }).ToList();

            listCheckingDrugInfo.ToList().ForEach((checkingDrug) =>
            {
                var checkUsage = listDrugAllergyAsPatientInfo.Where
                                                             (
                                                                   a => a.AnalogueCd == checkingDrug.AnalogueCd &&
                                                                        a.ItemCd != checkingDrug.ItemCd &&
                                                                        (checkingDrug.ZensinsayoFlg == "1" || checkingDrug.YohoCd == a.YohoCd)
                                                             )
                                                             .Select
                                                             (
                                                                   item => new DrugAllergyResultModel()
                                                                   {
                                                                       Level = 3,
                                                                       YjCd = checkingDrug.YjCd,
                                                                       ItemCd = checkingDrug.ItemCd,
                                                                       SeibunCd = checkingDrug.SeibunCd,
                                                                       AllergyYjCd = item.YjCd,
                                                                       AllergyItemCd = item.ItemCd,
                                                                       AllergySeibunCd = item.SeibunCd,
                                                                       Tag = item.AnalogueCd
                                                                   }
                                                             )
                                                             .ToList();
                result.AddRange(checkUsage);
            });

            return result;
        }

        public List<DrugAllergyResultModel> CheckDuplicatedClassForDuplication(int hpID, long ptID, int sinDate, List<string> listItemCode, List<string> listComparedItemCode, int haigouSetting)
        {
            List<DrugAllergyResultModel> result = new List<DrugAllergyResultModel>();

            var listCheckingDrugInfo =
                (from drugMst in _tenantNoTrackingDataContext.TenMsts.Where(i => listItemCode.Contains(i.ItemCd) && i.StartDate <= sinDate && sinDate <= i.EndDate)
                 join drugTypeInfo in GetDrugTypeInfo(haigouSetting)
                 on drugMst.YjCd equals drugTypeInfo.YjCd
                 join yjDrugClass in _tenantNoTrackingDataContext.M56YjDrugClass
                 on drugMst.YjCd equals yjDrugClass.YjCd
                 join drugClass in _tenantNoTrackingDataContext.M56DrugClass.Where(d => d.ClassDuplication == "1")
                 on yjDrugClass.ClassCd equals drugClass.ClassCd
                 select new
                 {
                     drugMst.ItemCd,
                     drugMst.YjCd,
                     drugClass.ClassCd,
                     drugTypeInfo.ZensinsayoFlg,
                     drugTypeInfo.YohoCd,
                     drugTypeInfo.HaigouFlg,
                     drugTypeInfo.YuekiFlg,
                     drugTypeInfo.KanpoFlg,
                 }).ToList();

            var listDrugAllergyAsPatientInfo =
                (from drugMst in _tenantNoTrackingDataContext.TenMsts.Where(i => listComparedItemCode.Contains(i.ItemCd) && i.StartDate <= sinDate && sinDate <= i.EndDate)
                 join drugTypeInfo in GetDrugTypeInfo(haigouSetting)
                 on drugMst.YjCd equals drugTypeInfo.YjCd
                 join yjDrugClass in _tenantNoTrackingDataContext.M56YjDrugClass
                 on drugMst.YjCd equals yjDrugClass.YjCd
                 join drugClass in _tenantNoTrackingDataContext.M56DrugClass.Where(d => d.ClassDuplication == "1")
                 on yjDrugClass.ClassCd equals drugClass.ClassCd
                 select new
                 {
                     drugMst.ItemCd,
                     drugMst.YjCd,
                     drugClass.ClassCd,
                     drugTypeInfo.ZensinsayoFlg,
                     drugTypeInfo.YohoCd,
                     drugTypeInfo.HaigouFlg,
                     drugTypeInfo.YuekiFlg,
                     drugTypeInfo.KanpoFlg,
                 }).ToList();

            listCheckingDrugInfo.ToList().ForEach((checkingDrug) =>
            {
                var checkUsage = listDrugAllergyAsPatientInfo.Where
                                                             (
                                                                   a => a.ClassCd == checkingDrug.ClassCd &&
                                                                        a.ItemCd != checkingDrug.ItemCd
                                                             )
                                                             .Select
                                                             (
                                                                   item => new DrugAllergyResultModel()
                                                                   {
                                                                       Level = 4,
                                                                       YjCd = checkingDrug.YjCd,
                                                                       ItemCd = checkingDrug.ItemCd,
                                                                       AllergyYjCd = item.YjCd,
                                                                       AllergyItemCd = item.ItemCd,
                                                                       Tag = item.ClassCd
                                                                   }
                                                             )
                                                             .ToList();
                result.AddRange(checkUsage);
            });

            return result;
        }
        #endregion

        public List<FoodAllergyResultModel> CheckFoodAllergy(int hpID, long ptID, int sinDate, List<string> listItemCode, int level, List<PtAlrgyFoodModel> listPtAlrgyFoods)
        {
            List<FoodAllergyResultModel> result = new List<FoodAllergyResultModel>();
            var allergyFoodAsPatient = listPtAlrgyFoods ?? GetFoodAllergyByPtId(hpID, ptID, sinDate);

            List<string> listAlrgyKbn = allergyFoodAsPatient.Where(a => a.AlrgyKbn != null).Select(a => a.AlrgyKbn).ToList();
            var checkedResult =
                _tenantNoTrackingDataContext.M12FoodAlrgy.
                Where(c => listItemCode.Contains(c.KikinCd) && listAlrgyKbn.Contains(c.FoodKbn))
                .Select(c => new
                {
                    c.FoodKbn,
                    c.KikinCd,
                    c.YjCd,
                    c.TenpuLevel,
                    c.AttentionCmt,
                    c.WorkingMechanism
                });

            if (checkedResult.Any())
            {
                foreach (var r in checkedResult)
                {
                    if (r.TenpuLevel.AsInteger() > level)
                    {
                        continue;
                    }

                    result.Add(new FoodAllergyResultModel()
                    {
                        PtId = ptID,
                        AlrgyKbn = r.FoodKbn,
                        ItemCd = r.KikinCd,
                        YjCd = r.YjCd,
                        TenpuLevel = r.TenpuLevel,
                        AttentionCmt = r.AttentionCmt,
                        WorkingMechanism = r.WorkingMechanism
                    });
                }
            }

            return result;
        }

        public List<AgeResultModel> CheckAge(int hpID, long ptID, int sinday, int level, int ageTypeCheckSetting, List<string> listItemCode)
        {
            //99: 収集又は作成中
            //00: 禁忌等の情報なし
            //10：安全性未確立
            //07：慎重投与[が望ましい]
            //06：有益性投与
            //05：投与回避[が望ましい]
            //04：原則投与禁忌が望ましい
            //03：投与禁忌が望ましい
            //02：原則投与禁忌
            //01：投与禁忌
            List<string> GetLevelRange()
            {
                List<string> listLevel = new List<string>();
                for (int i = 1; i <= level; i++)
                {
                    if (i < 8)
                    {
                        listLevel.Add("0" + i.AsString());
                    }
                    else if (i == 8)
                    {
                        listLevel.Add("10");
                    }
                    else if (i == 9)
                    {
                        listLevel.Add("00");
                    }
                    else if (i == 10)
                    {
                        listLevel.Add("99");
                    }
                }
                return listLevel;
            }

            PtInf patientInfo = GetPatientInfo(hpID, ptID);
            if (patientInfo == null)
            {
                return new List<AgeResultModel>();
            }

            int birthDay = patientInfo.Birthday;
            double age = CIUtil.SDateToAge(birthDay, sinday);
            string sex = patientInfo.Sex.AsString();
            double weight = -1;

            KensaInfDetail weightInfo = GetBodyInfo(hpID, ptID, sinday, "V0002");
            if (weightInfo != null)
            {
                weight = weightInfo.ResultVal?.AsDouble() ?? 0;
            }

            List<string> listSettingLevel = GetLevelRange();
            List<AgeResultModel> checkedResult;
            if (ageTypeCheckSetting == 0)
            {
                if (weight < 0.0 && age < 0.0)
                {
                    return new List<AgeResultModel>();
                }

                checkedResult =
                (from itemInfo in _tenantNoTrackingDataContext.TenMsts.Where(i => listItemCode.Contains(i.ItemCd) && i.StartDate <= sinday && sinday <= i.EndDate)
                 join ageCheck in _tenantNoTrackingDataContext.M14AgeCheck.Where
                 (
                     m =>
                     listSettingLevel.Contains(m.TenpuLevel) &&
                     (
                         (m.AgeKbn == "1" && m.AgeMin <= age && age < m.AgeMax && (string.IsNullOrEmpty(m.SexKbn) || m.SexKbn == sex)) ||
                         (m.WeightKbn == "1" && m.WeightMin <= weight && weight < m.WeightMax && (string.IsNullOrEmpty(m.SexKbn) || m.SexKbn == sex))
                     )
                 )
                 on itemInfo.YjCd equals ageCheck.YjCd
                 select new AgeResultModel()
                 {
                     ItemCd = itemInfo.ItemCd,
                     YjCd = itemInfo.YjCd ?? string.Empty,
                     TenpuLevel = ageCheck.TenpuLevel,
                     AttentionCmtCd = ageCheck.AttentionCmtCd,
                     WorkingMechanism = ageCheck.WorkingMechanism
                 }).ToList();
            }
            else
            {
                checkedResult =
                (from itemInfo in _tenantNoTrackingDataContext.TenMsts.Where(i => listItemCode.Contains(i.ItemCd) && i.StartDate <= sinday && sinday <= i.EndDate)
                 join ageCheck in _tenantNoTrackingDataContext.M14AgeCheck.Where
                 (
                     m =>
                     listSettingLevel.Contains(m.TenpuLevel) &&
                     (
                         (string.IsNullOrEmpty(m.AgeKbn) || (m.AgeKbn == "1" && m.AgeMin <= age && age < m.AgeMax && (string.IsNullOrEmpty(m.SexKbn) || m.SexKbn == sex))) &&
                         (string.IsNullOrEmpty(m.WeightKbn) || (m.WeightKbn == "1" && weight >= 0.0 && m.WeightMin <= weight && weight < m.WeightMax && (string.IsNullOrEmpty(m.SexKbn) || m.SexKbn == sex))) &&
                         (!string.IsNullOrEmpty(m.AgeKbn) || !string.IsNullOrEmpty(m.WeightKbn))
                     )
                 )
                 on itemInfo.YjCd equals ageCheck.YjCd
                 select new AgeResultModel()
                 {
                     ItemCd = itemInfo.ItemCd,
                     YjCd = itemInfo.YjCd ?? string.Empty,
                     TenpuLevel = ageCheck.TenpuLevel,
                     AttentionCmtCd = ageCheck.AttentionCmtCd,
                     WorkingMechanism = ageCheck.WorkingMechanism
                 }).ToList();
            }

            return checkedResult;
        }

        public List<DiseaseResultModel> CheckContraindicationForCurrentDisease(int hpID, int level, int sinDate, List<string> listItemCode, List<string> listDiseaseCode)
        {

            List<string> listBYCode =
                _tenantNoTrackingDataContext.M42ContraindiDisCon
                .Where(m => listDiseaseCode.Contains(m.ReceCd))
                .Select(m => m.ByotaiCd)
                .ToList();

            List<DiseaseResultModel> checkedResult =
                (from itemMst in _tenantNoTrackingDataContext.TenMsts.Where(i => listItemCode.Contains(i.ItemCd) && i.StartDate <= sinDate && sinDate <= i.EndDate)
                 join contraindication in _tenantNoTrackingDataContext.M42ContraindiDrugMainEx.Where(c => c.TenpuLevel <= level && listBYCode.Contains(c.ByotaiCd) && (string.IsNullOrEmpty(c.KioCd) || c.KioCd == "1") && (string.IsNullOrEmpty(c.FamilyCd) || c.FamilyCd == "1"))
                 on itemMst.YjCd equals contraindication.YjCd
                 select new DiseaseResultModel()
                 {
                     DiseaseType = 0,
                     ItemCd = itemMst.ItemCd,
                     YjCd = itemMst.YjCd ?? string.Empty,
                     TenpuLevel = contraindication.TenpuLevel,
                     ByotaiCd = contraindication.ByotaiCd,
                     CmtCd = contraindication.CmtCd,
                     KijyoCd = contraindication.KijyoCd
                 }).ToList();

            return checkedResult;
        }

        public List<DiseaseResultModel> CheckContraindicationForHistoryDisease(int hpID, long ptID, int level, int sinday, List<string> listItemCode, List<PtKioRekiModel> listPtKioReki)
        {
            List<string> listByomeiCd = new List<string>();
            if (listPtKioReki != null)
            {
                //Get newest data from SpecialNote
                listByomeiCd = listPtKioReki.Where(item => !string.IsNullOrEmpty(item.ByomeiCd)).Select(p => p.ByomeiCd).ToList();
            }
            else
            {
                listByomeiCd = _tenantNoTrackingDataContext.PtKioRekis
                    .Where(p => p.HpId == hpID && p.PtId == ptID && p.IsDeleted == 0 && !string.IsNullOrEmpty(p.ByomeiCd))
                    .AsEnumerable()
                    .Select(p =>
                    new PtKioRekiModel(
                        p.HpId, p.PtId, p.SeqNo, p.SortNo,
                        p.ByomeiCd ?? string.Empty, p.ByotaiCd ?? string.Empty,
                        p.Byomei ?? string.Empty, p.StartDate, p.Cmt ?? string.Empty, p.IsDeleted))
                    .Where(p => CIUtil.FullStartDate(p.StartDate) <= sinday)
                    .Select(p => p.ByomeiCd)
                    .ToList();
            }

            List<DiseaseResultModel> checkedResult =
                (from itemMst in _tenantNoTrackingDataContext.TenMsts.Where(i => listItemCode.Contains(i.ItemCd) && i.StartDate <= sinday && sinday <= i.EndDate)
                 join contraindication in _tenantNoTrackingDataContext.M42ContraindiDrugMainEx.Where(c => c.TenpuLevel <= level && (c.KioCd == "1" || c.KioCd == "2"))
                 on itemMst.YjCd equals contraindication.YjCd
                 join contraindiDisCon in _tenantNoTrackingDataContext.M42ContraindiDisCon.Where(c => listByomeiCd.Contains(c.ReceCd))
                 on contraindication.ByotaiCd equals contraindiDisCon.ByotaiCd
                 select new DiseaseResultModel()
                 {
                     DiseaseType = 1,
                     ItemCd = itemMst.ItemCd,
                     YjCd = itemMst.YjCd ?? string.Empty,
                     TenpuLevel = contraindication.TenpuLevel,
                     ByotaiCd = contraindication.ByotaiCd,
                     CmtCd = contraindication.CmtCd,
                     KijyoCd = contraindication.KijyoCd
                 }).ToList();

            return checkedResult;
        }

        public List<DiseaseResultModel> CheckContraindicationForFamilyDisease(int hpID, long ptID, int level, int sinday, List<string> listItemCode)
        {
            List<DiseaseResultModel> checkedResult =
                (from itemMst in _tenantNoTrackingDataContext.TenMsts.Where(i => listItemCode.Contains(i.ItemCd) && i.StartDate <= sinday && sinday <= i.EndDate)
                 join contraindication in _tenantNoTrackingDataContext.M42ContraindiDrugMainEx.Where(c => c.TenpuLevel <= level && (c.FamilyCd == "1" || c.FamilyCd == "2"))
                 on itemMst.YjCd equals contraindication.YjCd
                 join contraindiDisCon in _tenantNoTrackingDataContext.M42ContraindiDisCon
                 on contraindication.ByotaiCd equals contraindiDisCon.ByotaiCd
                 join historyDisease in _tenantNoTrackingDataContext.PtFamilyRekis.Where(p => p.HpId == hpID && p.IsDeleted == 0)
                 on contraindiDisCon.ReceCd equals historyDisease.ByomeiCd
                 join familyInfo in _tenantNoTrackingDataContext.PtFamilys.Where(p => p.HpId == hpID && p.PtId == ptID && p.IsDeleted == 0 && p.ZokugaraCd != "OT")
                 on historyDisease.FamilyId equals familyInfo.FamilyId
                 select new DiseaseResultModel()
                 {
                     DiseaseType = 2,
                     ItemCd = itemMst.ItemCd,
                     YjCd = itemMst.YjCd ?? string.Empty,
                     TenpuLevel = contraindication.TenpuLevel,
                     ByotaiCd = contraindication.ByotaiCd,
                     CmtCd = contraindication.CmtCd,
                     KijyoCd = contraindication.KijyoCd
                 }).ToList();

            return checkedResult;
        }

        public List<KinkiResultModel> CheckKinki(int hpID, int level, int sinday, List<string> listCurrentOrderCode, List<string> listAddedOrderCode)
        {
            var listCurrentOrderSubYjCode = _tenantNoTrackingDataContext.TenMsts
                .Where(m => listCurrentOrderCode.Contains(m.ItemCd) && m.StartDate <= sinday && sinday <= m.EndDate)
                .Select(m => new
                {
                    m.YjCd,
                    m.ItemCd,
                    YjCd4 = m.YjCd ?? string.Empty.Substring(0, 4),
                    YjCd7 = m.YjCd ?? string.Empty.Substring(0, 7),
                    YjCd8 = m.YjCd ?? string.Empty.Substring(0, 8),
                    YjCd9 = m.YjCd ?? string.Empty.Substring(0, 9),
                    YjCd12 = m.YjCd ?? string.Empty.Substring(0, 12),
                })
                .ToList();

            var listAddedOrderSubYjCode = _tenantNoTrackingDataContext.TenMsts
                .Where(m => listAddedOrderCode.Contains(m.ItemCd) && m.StartDate <= sinday && sinday <= m.EndDate)
                .Select(m => new
                {
                    m.YjCd,
                    m.ItemCd,
                    YjCd4 = m.YjCd ?? string.Empty.Substring(0, 4),
                    YjCd7 = m.YjCd ?? string.Empty.Substring(0, 7),
                    YjCd8 = m.YjCd ?? string.Empty.Substring(0, 8),
                    YjCd9 = m.YjCd ?? string.Empty.Substring(0, 9),
                    YjCd12 = m.YjCd ?? string.Empty.Substring(0, 12),
                })
                .ToList();

            #region filter master data to improve performance
            var listAddedOrderSubYj4Code = listAddedOrderSubYjCode.Select(o => o.YjCd4).ToList();
            var listAddedOrderSubYj7Code = listAddedOrderSubYjCode.Select(o => o.YjCd7).ToList();
            var listAddedOrderSubYj8Code = listAddedOrderSubYjCode.Select(o => o.YjCd8).ToList();
            var listAddedOrderSubYj9Code = listAddedOrderSubYjCode.Select(o => o.YjCd9).ToList();
            var listAddedOrderSubYj12Code = listAddedOrderSubYjCode.Select(o => o.YjCd12).ToList();
            var listCurrentOrderSubYj4Code = listCurrentOrderSubYjCode.Select(o => o.YjCd4).ToList();
            var listCurrentOrderSubYj7Code = listCurrentOrderSubYjCode.Select(o => o.YjCd7).ToList();
            var listCurrentOrderSubYj8Code = listCurrentOrderSubYjCode.Select(o => o.YjCd8).ToList();
            var listCurrentOrderSubYj9Code = listCurrentOrderSubYjCode.Select(o => o.YjCd9).ToList();
            var listCurrentOrderSubYj12Code = listCurrentOrderSubYjCode.Select(o => o.YjCd12).ToList();
            var filteredMasterData = _tenantNoTrackingDataContext.M01Kinki
                        .Where
                        (
                            k =>
                            ((
                                listAddedOrderSubYj7Code.Contains(k.ACd) ||
                                listAddedOrderSubYj8Code.Contains(k.ACd) ||
                                listAddedOrderSubYj9Code.Contains(k.ACd) ||
                                listAddedOrderSubYj12Code.Contains(k.ACd)
                            )
                            &&
                            (
                                listCurrentOrderSubYj4Code.Contains(k.BCd) ||
                                listCurrentOrderSubYj7Code.Contains(k.BCd) ||
                                listCurrentOrderSubYj8Code.Contains(k.BCd) ||
                                listCurrentOrderSubYj9Code.Contains(k.BCd) ||
                                listCurrentOrderSubYj12Code.Contains(k.BCd)
                            ))
                            ||
                            ((
                                listCurrentOrderSubYj7Code.Contains(k.ACd) ||
                                listCurrentOrderSubYj8Code.Contains(k.ACd) ||
                                listCurrentOrderSubYj9Code.Contains(k.ACd) ||
                                listCurrentOrderSubYj12Code.Contains(k.ACd)
                            )
                            &&
                            (
                                listAddedOrderSubYj4Code.Contains(k.BCd) ||
                                listAddedOrderSubYj7Code.Contains(k.BCd) ||
                                listAddedOrderSubYj8Code.Contains(k.BCd) ||
                                listAddedOrderSubYj9Code.Contains(k.BCd) ||
                                listAddedOrderSubYj12Code.Contains(k.BCd)
                            ))
                        )
                        .ToList();

            #endregion

            List<KinkiResultModel> result = new List<KinkiResultModel>();

            foreach (string addedOrderItemCode in listAddedOrderCode)
            {
                var addedOrderSubYjCode = listAddedOrderSubYjCode.FirstOrDefault(s => s.ItemCd == addedOrderItemCode);

                if (addedOrderSubYjCode == null)
                {
                    continue;
                }

                foreach (string currentOrderCode in listCurrentOrderCode)
                {
                    var currentOrderSubYjCode = listCurrentOrderSubYjCode.FirstOrDefault(s => s.ItemCd == currentOrderCode);
                    if (currentOrderSubYjCode == null)
                    {
                        continue;
                    }

                    var checkedResult = filteredMasterData
                        .Where
                        (
                            k =>
                            ((
                                k.ACd == addedOrderSubYjCode.YjCd7 ||
                                k.ACd == addedOrderSubYjCode.YjCd8 ||
                                k.ACd == addedOrderSubYjCode.YjCd9 ||
                                k.ACd == addedOrderSubYjCode.YjCd12
                            )
                            &&
                            (
                                k.BCd == currentOrderSubYjCode.YjCd4 ||
                                k.BCd == currentOrderSubYjCode.YjCd7 ||
                                k.BCd == currentOrderSubYjCode.YjCd8 ||
                                k.BCd == currentOrderSubYjCode.YjCd9 ||
                                k.BCd == currentOrderSubYjCode.YjCd12
                            ))
                            ||
                            ((
                                k.ACd == currentOrderSubYjCode.YjCd7 ||
                                k.ACd == currentOrderSubYjCode.YjCd8 ||
                                k.ACd == currentOrderSubYjCode.YjCd9 ||
                                k.ACd == currentOrderSubYjCode.YjCd12
                            )
                            &&
                            (
                                k.BCd == addedOrderSubYjCode.YjCd4 ||
                                k.BCd == addedOrderSubYjCode.YjCd7 ||
                                k.BCd == addedOrderSubYjCode.YjCd8 ||
                                k.BCd == addedOrderSubYjCode.YjCd9 ||
                                k.BCd == addedOrderSubYjCode.YjCd12
                            ))
                        )
                        .Select
                        (
                            m => new KinkiResultModel()
                            {
                                AYjCd = addedOrderSubYjCode.YjCd ?? string.Empty,
                                BYjCd = currentOrderSubYjCode.YjCd ?? string.Empty,
                                SubAYjCd = m.ACd,
                                SubBYjCd = m.BCd,
                                CommentCode = m.CmtCd,
                                SayokijyoCode = m.SayokijyoCd,
                                Kyodo = m.Kyodo,
                                IsNeedToReplace =
                                (
                                    m.ACd == currentOrderSubYjCode.YjCd7 ||
                                    m.ACd == currentOrderSubYjCode.YjCd8 ||
                                    m.ACd == currentOrderSubYjCode.YjCd9 ||
                                    m.ACd == currentOrderSubYjCode.YjCd12
                                ),
                                ItemCd = addedOrderItemCode
                            }
                        )
                        .ToList();

                    result.AddRange(checkedResult);
                }
            }

            List<KinkiResultModel> filteredResultAsLevel = new List<KinkiResultModel>();
            foreach (KinkiResultModel kinkiResultModel in result)
            {
                if (kinkiResultModel.Kyodo.AsInteger() <= level)
                {
                    filteredResultAsLevel.Add(kinkiResultModel);
                }
            }

            return filteredResultAsLevel;
        }

        public List<KinkiResultModel> CheckKinkiUser(int hpID, int level, int sinday, List<string> listCurrentOrderCode, List<string> listAddedOrderCode)
        {
            if (level <= 0) return new List<KinkiResultModel>();
            var listYjCd = _tenantNoTrackingDataContext.TenMsts
                .Where(m => (listCurrentOrderCode.Contains(m.ItemCd) || listAddedOrderCode.Contains(m.ItemCd)) && m.StartDate <= sinday && sinday <= m.EndDate)
                .Select(m => new
                {
                    m.ItemCd,
                    m.YjCd
                }
                );

            var listChecked = _tenantNoTrackingDataContext.KinkiMsts.Where(k => k.HpId == hpID &&
                                                                                         k.IsDeleted == 0 &&
                                                                                         (
                                                                                              listCurrentOrderCode.Contains(k.ACd) && listAddedOrderCode.Contains(k.BCd) ||
                                                                                              listCurrentOrderCode.Contains(k.BCd) && listAddedOrderCode.Contains(k.ACd)
                                                                                         )
                                                                                    )
                                                          .ToList();

            List<KinkiResultModel> result = new List<KinkiResultModel>();
            listChecked.ForEach((c) =>
            {
                string addedYjCd = listYjCd.Where(y => y.ItemCd == c.ACd).Select(y => y.YjCd).FirstOrDefault() ?? string.Empty;
                string currentYjCd = listYjCd.Where(y => y.ItemCd == c.BCd).Select(y => y.YjCd).FirstOrDefault() ?? string.Empty;
                result.Add(new KinkiResultModel()
                {
                    AYjCd = addedYjCd,
                    BYjCd = currentYjCd,
                    IsNeedToReplace = false,
                });
            });
            return result;
        }

        public List<KinkiResultModel> CheckKinkiTain(int hpID, long ptId, int sinday, int level, List<string> addedOrderItemCodeList, List<PtOtherDrugModel> listPtOtherDrug)
        {
            List<string> listTainCode = new List<string>();
            if (listPtOtherDrug == null)
            {
                var listPtOtherDrugModel = _tenantNoTrackingDataContext.PtOtherDrug
                    .Where(o => o.HpId == hpID && o.PtId == ptId && o.IsDeleted == 0)
                    .AsEnumerable()
                    .Select(p => new PtOtherDrugModel(p.HpId, p.PtId, p.SeqNo, p.SortNo, p.ItemCd ?? string.Empty, p.DrugName ?? string.Empty, p.StartDate, p.EndDate, p.Cmt ?? string.Empty, p.IsDeleted))
                    .ToList();

                listTainCode = listPtOtherDrugModel
                    .Where(p => CIUtil.FullStartDate(p.StartDate) <= sinday && sinday <= CIUtil.FullEndDate(p.EndDate))
                    .Select(p => p.ItemCd)
                    .ToList();
            }
            else
            {
                listTainCode = listPtOtherDrug.Select(t => t.ItemCd).ToList();
            }

            var listCurrentOrderSubYjCode = _tenantNoTrackingDataContext.TenMsts
                .Where(m => listTainCode.Contains(m.ItemCd) && m.StartDate <= sinday && sinday <= m.EndDate)
                .Select(m => new
                {
                    m.YjCd,
                    m.ItemCd,
                    YjCd4 = m.YjCd ?? string.Empty.Substring(0, 4),
                    YjCd7 = m.YjCd ?? string.Empty.Substring(0, 7),
                    YjCd8 = m.YjCd ?? string.Empty.Substring(0, 8),
                    YjCd9 = m.YjCd ?? string.Empty.Substring(0, 9),
                    YjCd12 = m.YjCd ?? string.Empty.Substring(0, 12),
                })
                .ToList();

            var listAddedOrderSubYjCode = _tenantNoTrackingDataContext.TenMsts
                .Where(m => addedOrderItemCodeList.Contains(m.ItemCd) && m.StartDate <= sinday && sinday <= m.EndDate)
                .Select(m => new
                {
                    m.YjCd,
                    m.ItemCd,
                    YjCd4 = m.YjCd ?? string.Empty.Substring(0, 4),
                    YjCd7 = m.YjCd ?? string.Empty.Substring(0, 7),
                    YjCd8 = m.YjCd ?? string.Empty.Substring(0, 8),
                    YjCd9 = m.YjCd ?? string.Empty.Substring(0, 9),
                    YjCd12 = m.YjCd ?? string.Empty.Substring(0, 12),
                })
                .ToList();

            List<KinkiResultModel> result = new List<KinkiResultModel>();

            foreach (string addedOrderItemCode in addedOrderItemCodeList)
            {
                var addedOrderSubYjCode = listAddedOrderSubYjCode.FirstOrDefault(s => s.ItemCd == addedOrderItemCode);
                if (addedOrderSubYjCode == null)
                {
                    continue;
                }

                foreach (string currentOrderCode in listTainCode)
                {
                    var currentOrderSubYjCode = listCurrentOrderSubYjCode.FirstOrDefault(s => s.ItemCd == currentOrderCode);
                    if (currentOrderSubYjCode == null)
                    {
                        continue;
                    }

                    var checkedResult =
                        _tenantNoTrackingDataContext.M01Kinki
                        .Where
                        (
                            k =>
                            ((
                                k.ACd == addedOrderSubYjCode.YjCd7 ||
                                k.ACd == addedOrderSubYjCode.YjCd8 ||
                                k.ACd == addedOrderSubYjCode.YjCd9 ||
                                k.ACd == addedOrderSubYjCode.YjCd12
                            )
                            &&
                            (
                                k.BCd == currentOrderSubYjCode.YjCd4 ||
                                k.BCd == currentOrderSubYjCode.YjCd7 ||
                                k.BCd == currentOrderSubYjCode.YjCd8 ||
                                k.BCd == currentOrderSubYjCode.YjCd9 ||
                                k.BCd == currentOrderSubYjCode.YjCd12
                            ))
                            ||
                            ((
                                k.ACd == currentOrderSubYjCode.YjCd7 ||
                                k.ACd == currentOrderSubYjCode.YjCd8 ||
                                k.ACd == currentOrderSubYjCode.YjCd9 ||
                                k.ACd == currentOrderSubYjCode.YjCd12
                            )
                            &&
                            (
                                k.BCd == addedOrderSubYjCode.YjCd4 ||
                                k.BCd == addedOrderSubYjCode.YjCd7 ||
                                k.BCd == addedOrderSubYjCode.YjCd8 ||
                                k.BCd == addedOrderSubYjCode.YjCd9 ||
                                k.BCd == addedOrderSubYjCode.YjCd12
                            ))
                        )
                        .Select
                        (
                            m => new KinkiResultModel()
                            {
                                AYjCd = addedOrderSubYjCode.YjCd ?? string.Empty,
                                BYjCd = currentOrderSubYjCode.YjCd ?? string.Empty,
                                SubAYjCd = m.ACd,
                                SubBYjCd = m.BCd,
                                CommentCode = m.CmtCd,
                                SayokijyoCode = m.SayokijyoCd,
                                Kyodo = m.Kyodo,
                                IsNeedToReplace =
                                (
                                    m.ACd == currentOrderSubYjCode.YjCd7 ||
                                    m.ACd == currentOrderSubYjCode.YjCd8 ||
                                    m.ACd == currentOrderSubYjCode.YjCd9 ||
                                    m.ACd == currentOrderSubYjCode.YjCd12
                                ),
                                ItemCd = addedOrderItemCode
                            }
                        )
                        .ToList();

                    result.AddRange(checkedResult);
                }
            }

            List<KinkiResultModel> filteredResultAsLevel = new List<KinkiResultModel>();
            foreach (KinkiResultModel kinkiResultModel in result)
            {
                if (kinkiResultModel.Kyodo.AsInteger() <= level)
                {
                    filteredResultAsLevel.Add(kinkiResultModel);
                }
            }

            return filteredResultAsLevel;
        }

        public List<KinkiResultModel> CheckKinkiOTC(int hpID, long ptId, int sinday, int level, List<string> addedOrderItemCodeList, List<PtOtcDrugModel> listPtOtcDrug)
        {
            List<int> listSerialNum = new List<int>();
            if (listPtOtcDrug == null)
            {
                listSerialNum = _tenantNoTrackingDataContext.PtOtcDrug
                    .Where(o => o.HpId == hpID && o.PtId == ptId && o.IsDeleted == 0)
                    .AsEnumerable()
                    .Select(p => new PtOtcDrugModel(p.HpId, p.PtId, p.SeqNo, p.SortNo, p.SerialNum, p.TradeName ?? string.Empty, p.StartDate, p.EndDate, p.Cmt ?? string.Empty, p.IsDeleted))
                    .Where(p => CIUtil.FullStartDate(p.StartDate) <= sinday && sinday <= CIUtil.FullEndDate(p.EndDate))
                    .Select(p => p.SerialNum)
                    .ToList();
            }
            else
            {
                listSerialNum = listPtOtcDrug.Select(t => t.SerialNum).ToList();
            }

            var listSubOTCCode = _tenantNoTrackingDataContext.M38Ingredients
                .Where(m => listSerialNum.Contains(m.SerialNum))
                .Select(m => new
                {
                    m.SerialNum,
                    m.SeibunCd,
                    m.Sbt,
                    Otc7 = m.SeibunCd.Substring(0, 7),
                })
                .ToList();

            var listAddedOrderSubYjCode = _tenantNoTrackingDataContext.TenMsts
                .Where(m => addedOrderItemCodeList.Contains(m.ItemCd) && m.StartDate <= sinday && sinday <= m.EndDate)
                .Select(m => new
                {
                    m.YjCd,
                    m.ItemCd,
                    YjCd4 = m.YjCd ?? string.Empty.Substring(0, 4),
                    YjCd7 = m.YjCd ?? string.Empty.Substring(0, 7),
                    YjCd8 = m.YjCd ?? string.Empty.Substring(0, 8),
                    YjCd9 = m.YjCd ?? string.Empty.Substring(0, 9),
                    YjCd12 = m.YjCd ?? string.Empty.Substring(0, 12),
                })
                .ToList();

            List<KinkiResultModel> result = new List<KinkiResultModel>();

            foreach (string addedOrderItemCode in addedOrderItemCodeList)
            {
                var addedOrderSubYjCode = listAddedOrderSubYjCode.FirstOrDefault(s => s.ItemCd == addedOrderItemCode);
                if (addedOrderSubYjCode == null)
                {
                    continue;
                }

                foreach (int oTCSerialNum in listSerialNum)
                {
                    List<string> subOTCCode = listSubOTCCode.Where(s => s.SerialNum == oTCSerialNum).Select(s => s.Otc7).ToList();
                    var checkedResult =
                        _tenantNoTrackingDataContext.M01Kinki
                        .Where
                        (
                            k =>
                            ((
                                k.ACd == addedOrderSubYjCode.YjCd4 ||
                                k.ACd == addedOrderSubYjCode.YjCd7 ||
                                k.ACd == addedOrderSubYjCode.YjCd8 ||
                                k.ACd == addedOrderSubYjCode.YjCd9 ||
                                k.ACd == addedOrderSubYjCode.YjCd12
                            )
                            &&
                            (
                                subOTCCode.Contains(k.BCd)
                            ))
                            ||
                            ((
                                subOTCCode.Contains(k.ACd)
                            )
                            &&
                            (
                                k.BCd == addedOrderSubYjCode.YjCd4 ||
                                k.BCd == addedOrderSubYjCode.YjCd7 ||
                                k.BCd == addedOrderSubYjCode.YjCd8 ||
                                k.BCd == addedOrderSubYjCode.YjCd9 ||
                                k.BCd == addedOrderSubYjCode.YjCd12
                            ))
                        )
                        .Select
                        (
                            m => new KinkiResultModel()
                            {
                                ItemCd = addedOrderItemCode,
                                AYjCd = addedOrderSubYjCode.YjCd ?? string.Empty,
                                BYjCd = oTCSerialNum.ToString(),
                                SubAYjCd = m.ACd,
                                SubBYjCd = m.BCd,
                                CommentCode = m.CmtCd,
                                SayokijyoCode = m.SayokijyoCd,
                                Kyodo = m.Kyodo,
                                IsNeedToReplace = subOTCCode.Contains(m.ACd)
                            }
                        )
                        .ToList();

                    checkedResult.ForEach((c) =>
                    {
                        //Follow comment 3704
                        //Get subOtcInfo to update Sbt and SeibunCd of KinkiResultModel
                        var subOtc = listSubOTCCode
                        .FirstOrDefault(s => s.SerialNum == oTCSerialNum && (s.Otc7 == c.SubAYjCd && c.IsNeedToReplace || !c.IsNeedToReplace && s.Otc7 == c.SubBYjCd));
                        if (subOtc != null)
                        {
                            c.Sbt = subOtc.Sbt;
                            c.SeibunCd = subOtc.SeibunCd;
                        }
                    });

                    result.AddRange(checkedResult);
                }
            }

            List<KinkiResultModel> filteredResultAsLevel = new List<KinkiResultModel>();
            foreach (KinkiResultModel kinkiResultModel in result)
            {
                if (kinkiResultModel.Kyodo.AsInteger() <= level)
                {
                    filteredResultAsLevel.Add(kinkiResultModel);
                }
            }

            return filteredResultAsLevel;
        }

        public List<KinkiResultModel> CheckKinkiSupple(int hpID, long ptId, int sinday, int level, List<string> addedOrderItemCodeList, List<PtSuppleModel> listPtSupple)
        {
            List<string> listIndexWord = new List<string>();

            if (listPtSupple == null)
            {
                listIndexWord = _tenantNoTrackingDataContext.PtSupples
                    .Where(o => o.HpId == hpID && o.PtId == ptId && o.IsDeleted == 0)
                    .AsEnumerable()
                    .Select(p => new PtSuppleModel(p.HpId, p.PtId, p.SeqNo, p.SortNo, p.IndexCd ?? string.Empty, p.IndexWord ?? string.Empty, p.StartDate, p.EndDate, p.Cmt ?? string.Empty, p.IsDeleted))
                    .Where(p => p.StartDate <= sinday && sinday <= p.EndDate)
                    .Select(p => p.IndexWord)
                    .ToList();
            }
            else
            {
                listIndexWord = listPtSupple.Select(s => s.IndexWord).ToList();

            }

            List<SeibunInfo> listSeibunInfo =
                    (
                        from indexdef in _tenantNoTrackingDataContext.M41SuppleIndexdefs.Where(s => listIndexWord.Contains(s.IndexWord))
                        join indexCode in _tenantNoTrackingDataContext.M41SuppleIndexcodes
                        on indexdef.SeibunCd equals indexCode.IndexCd
                        select new SeibunInfo
                        {
                            IndexWord = indexdef.IndexWord,
                            SeibunCd = indexCode.SeibunCd,
                            IndexCd = indexCode.IndexCd
                        }
                    ).ToList();

            var listAddedOrderSubYjCode = _tenantNoTrackingDataContext.TenMsts
                .Where(m => addedOrderItemCodeList.Contains(m.ItemCd) && m.StartDate <= sinday && sinday <= m.EndDate)
                .Select(m => new
                {
                    m.YjCd,
                    m.ItemCd,
                    YjCd4 = m.YjCd ?? string.Empty.Substring(0, 4),
                    YjCd7 = m.YjCd ?? string.Empty.Substring(0, 7),
                    YjCd8 = m.YjCd ?? string.Empty.Substring(0, 8),
                    YjCd9 = m.YjCd ?? string.Empty.Substring(0, 9),
                    YjCd12 = m.YjCd ?? string.Empty.Substring(0, 12),
                })
                .ToList();

            List<KinkiResultModel> result = new List<KinkiResultModel>();

            foreach (string addedOrderItemCode in addedOrderItemCodeList)
            {
                var addedOrderSubYjCode = listAddedOrderSubYjCode.FirstOrDefault(s => s.ItemCd == addedOrderItemCode);
                if (addedOrderSubYjCode == null)
                {
                    continue;
                }

                foreach (var seibunInfo in listSeibunInfo)
                {
                    string seibunCd = seibunInfo.SeibunCd;
                    var checkedResult =
                        _tenantNoTrackingDataContext.M01Kinki
                        .Where
                        (
                            k =>
                            (
                                k.ACd == seibunCd
                            )
                            &&
                            (
                                k.BCd == addedOrderSubYjCode.YjCd4 ||
                                k.BCd == addedOrderSubYjCode.YjCd7 ||
                                k.BCd == addedOrderSubYjCode.YjCd8 ||
                                k.BCd == addedOrderSubYjCode.YjCd9 ||
                                k.BCd == addedOrderSubYjCode.YjCd12
                            )
                        )
                        .Select
                        (
                            m => new KinkiResultModel()
                            {
                                AYjCd = addedOrderSubYjCode.YjCd ?? string.Empty,
                                BYjCd = seibunInfo.IndexCd,
                                SubAYjCd = m.ACd,
                                SubBYjCd = m.BCd,
                                CommentCode = m.CmtCd,
                                SayokijyoCode = m.SayokijyoCd,
                                Kyodo = m.Kyodo,
                                IsNeedToReplace = true,
                                IndexWord = seibunInfo.IndexWord,
                                SeibunCd = seibunInfo.SeibunCd,
                                ItemCd = addedOrderItemCode
                            }
                        )
                        .ToList();

                    result.AddRange(checkedResult);
                }
            }

            List<KinkiResultModel> filteredResultAsLevel = new List<KinkiResultModel>();
            foreach (KinkiResultModel kinkiResultModel in result)
            {
                if (kinkiResultModel.Kyodo.AsInteger() <= level)
                {
                    filteredResultAsLevel.Add(kinkiResultModel);
                }
            }

            return filteredResultAsLevel;
        }

        public List<DosageResultModel> CheckDosage(int hpId, long ptId, int sinday, List<DrugInfo> listItem, bool minCheck, double ratioSetting, double currentHeight, double currentWeight)
        {
            PtInf patientInfo = GetPatientInfo(hpId, ptId);
            if (patientInfo == null)
            {
                return new List<DosageResultModel>();
            }

            List<DosageResultModel> checkedResult = new List<DosageResultModel>();

            double age = CIUtil.SDateToAge(patientInfo.Birthday, sinday);
            int sex = patientInfo.Sex;

            double weight = 0;
            if (currentWeight <= -1)
            {
                // Get new data from SpecialNote but have no WeightInfo
                weight = GetCommonWeight(hpId, ptId, patientInfo.Birthday, sinday, sex);
            }
            else if (currentWeight == 0)
            {
                // Can't get newData from SpecialNote
                weight = GetPatientWeight(hpId, ptId, patientInfo.Birthday, sinday, sex);
            }
            else
            {
                weight = currentWeight;
            }

            double height = 0;
            if (currentHeight <= -1)
            {
                // Get new data from SpecialNote but have no HeightInfo
                height = GetCommonHeight(hpId, ptId, patientInfo.Birthday, sinday, sex);
            }
            else if (currentHeight == 0)
            {
                // Can't get newData from SpecialNote
                height = GetPatientHeight(hpId, ptId, patientInfo.Birthday, sinday, sex);
            }
            else
            {
                height = currentHeight;
            }

            double ratioAsAge = GetRatio(patientInfo.Birthday, sinday);
            double bodySize = GetBodySize(weight, height, age);


            List<string> listDrugCode = listItem.Select(i => i.ItemCD).ToList();

            #region Check by UserData

            var listDosageInfoByUser =
                (
                    from tenMst in _tenantNoTrackingDataContext.TenMsts.Where(t => listDrugCode.Contains(t.ItemCd) && t.StartDate <= sinday && sinday <= t.EndDate)
                    join dosageDrug in _tenantNoTrackingDataContext.DosageDrugs.Where(d => d.RikikaUnit != null)
                    on tenMst.YjCd equals dosageDrug.YjCd
                    join dosageDMst in _tenantNoTrackingDataContext.DosageMsts.Where(d => d.IsDeleted == 0)
                    on tenMst.ItemCd equals dosageDMst.ItemCd
                    select new
                    {
                        tenMst.ItemCd,
                        tenMst.YjCd,
                        tenMst.ReceUnitName,
                        tenMst.OdrTermVal,
                        tenMst.CnvTermVal,
                        dosageDrug.YakkaiUnit,
                        dosageDrug.RikikaRate,
                        dosageDrug.RikikaUnit,
                        dosageDMst.OnceMin,
                        dosageDMst.OnceMax,
                        dosageDMst.OnceLimit,
                        dosageDMst.OnceUnit,
                        dosageDMst.DayMin,
                        dosageDMst.DayMax,
                        dosageDMst.DayLimit,
                        dosageDMst.DayUnit
                    }
                ).ToList();

            listDosageInfoByUser.ForEach((d) =>
            {
                var itemInfo = listItem.FirstOrDefault(i => i.ItemCD == d.ItemCd) ?? new DrugInfo();
                // Caculate dosage
                double factor = 0;
                double odrCnv = 1;
                double dosage = -1;

                string rikikaUnit = d.RikikaUnit;
                if (!string.IsNullOrEmpty(rikikaUnit))
                {
                    string yakkaUnit = d.YakkaiUnit;
                    string unitName = itemInfo.UnitName;

                    if (unitName == rikikaUnit ||
                        unitName == yakkaUnit)
                    {
                        dosage = itemInfo.Suryo;
                        factor = 1;
                        odrCnv = 1;
                    }
                    else if (itemInfo.TermVal > 0)
                    {
                        //ODR_INF_DETAIL.数量 * ODR_INF_DETAIL.単位換算値)
                        dosage = itemInfo.Suryo * itemInfo.TermVal;
                        factor = 1;
                        odrCnv = itemInfo.TermVal;
                    }
                }

                if (dosage > 0)
                {
                    // Caculate the limited points
                    double minOnce = -1;
                    double maxOnce = -1;
                    double limitOnce = -1;
                    double minDay = -1;
                    double maxDay = -1;
                    double limitDay = -1;

                    if (d.OnceMin > 0)
                    {
                        if (d.OnceUnit == 1)
                        {
                            minOnce = d.OnceMin * weight;
                        }
                        else if (d.OnceUnit == 2)
                        {
                            minOnce = d.OnceMin * bodySize;
                        }
                        else
                        {
                            minOnce = d.OnceMin;
                        }
                    }

                    if (d.OnceMax > 0)
                    {
                        if (d.OnceUnit == 1)
                        {
                            maxOnce = d.OnceMax * weight;
                        }
                        else if (d.OnceUnit == 2)
                        {
                            maxOnce = d.OnceMax * bodySize;
                        }
                        else
                        {
                            maxOnce = d.OnceMax;
                        }
                    }

                    if (d.OnceLimit > 0)
                    {
                        limitOnce = d.OnceLimit;
                    }

                    if (d.DayMin > 0)
                    {
                        if (d.DayUnit == 1)
                        {
                            minDay = d.DayMin * weight;
                        }
                        else if (d.DayUnit == 2)
                        {
                            minDay = d.DayMin * bodySize;
                        }
                        else
                        {
                            minDay = d.DayMin;
                        }
                    }

                    if (d.DayMax > 0)
                    {
                        if (d.DayUnit == 1)
                        {
                            maxDay = d.DayMax * weight;
                        }
                        else if (d.DayUnit == 2)
                        {
                            maxDay = d.DayMax * bodySize;
                        }
                        else
                        {
                            maxDay = d.DayMax;
                        }
                    }

                    if (d.DayLimit > 0)
                    {
                        limitDay = d.DayLimit;
                    }

                    double maxByDayToCheck = 0;
                    DosageLabelChecking checkingLabelByDay = DosageLabelChecking.DayMax;
                    if (0 < limitDay && 0 < maxDay)
                    {
                        maxByDayToCheck = Math.Min(limitDay, maxDay);
                        checkingLabelByDay = limitDay < maxDay ? DosageLabelChecking.DayLimit : DosageLabelChecking.DayMax;
                    }
                    else if (0 < limitDay)
                    {
                        maxByDayToCheck = limitDay;
                        checkingLabelByDay = DosageLabelChecking.DayLimit;
                    }
                    else if (0 < maxDay)
                    {
                        maxByDayToCheck = maxDay;
                        checkingLabelByDay = DosageLabelChecking.DayMax;
                    }

                    double maxByOnceToCheck = 0;
                    DosageLabelChecking checkingLabelByOnce = DosageLabelChecking.OneMax;
                    if (0 < limitOnce && 0 < maxOnce)
                    {
                        maxByOnceToCheck = Math.Min(limitOnce, maxOnce);
                        checkingLabelByOnce = limitOnce < maxOnce ? DosageLabelChecking.OneLimit : DosageLabelChecking.OneMax;
                    }
                    else if (0 < limitOnce)
                    {
                        maxByOnceToCheck = limitOnce;
                        checkingLabelByOnce = DosageLabelChecking.OneLimit;
                    }
                    else if (0 < maxOnce)
                    {
                        maxByOnceToCheck = maxOnce;
                        checkingLabelByOnce = DosageLabelChecking.OneMax;
                    }

                    // Execute checking
                    DosageResultModel? dosageResultModel = null;
                    if (itemInfo?.SinKouiKbn == 21)
                    {
                        // 内服
                        if (minCheck && minDay > dosage && minDay != -1)
                        {
                            dosageResultModel = new DosageResultModel() { LabelChecking = DosageLabelChecking.DayMin };
                            dosageResultModel.SuggestedValue = GetSuggestedValue(minDay, factor, odrCnv, true);
                        }
                        else if (0 < maxByDayToCheck && maxByDayToCheck < dosage)
                        {
                            dosageResultModel = new DosageResultModel() { LabelChecking = checkingLabelByDay };
                            dosageResultModel.SuggestedValue = GetSuggestedValue(maxByDayToCheck, factor, odrCnv, false);
                        }
                    }
                    else if (itemInfo?.SinKouiKbn == 22)
                    {
                        // 頓服
                        if (minCheck && minOnce > dosage && minOnce != -1)
                        {
                            dosageResultModel = new DosageResultModel() { LabelChecking = DosageLabelChecking.OneMin };
                            dosageResultModel.SuggestedValue = GetSuggestedValue(minOnce, factor, odrCnv, true);
                        }
                        else if (0 < maxByOnceToCheck && maxByOnceToCheck < dosage)
                        {
                            dosageResultModel = new DosageResultModel() { LabelChecking = checkingLabelByOnce };
                            dosageResultModel.SuggestedValue = GetSuggestedValue(maxByOnceToCheck, factor, odrCnv, false);
                        }
                    }
                    else
                    {
                        // その他
                        if (minCheck && minOnce > dosage && minOnce != -1)
                        {
                            dosageResultModel = new DosageResultModel() { LabelChecking = DosageLabelChecking.OneMin };
                            dosageResultModel.SuggestedValue = GetSuggestedValue(minOnce, factor, odrCnv, true);
                        }
                        else if (0 < maxByOnceToCheck && maxByOnceToCheck < dosage)
                        {
                            dosageResultModel = new DosageResultModel() { LabelChecking = checkingLabelByOnce };
                            dosageResultModel.SuggestedValue = GetSuggestedValue(maxByOnceToCheck, factor, odrCnv, false);
                        }
                        else if (minCheck && minDay > dosage && minDay != -1)
                        {
                            dosageResultModel = new DosageResultModel() { LabelChecking = DosageLabelChecking.DayMin };
                            dosageResultModel.SuggestedValue = GetSuggestedValue(minDay, factor, odrCnv, true);
                        }
                        else if (0 < maxByDayToCheck && maxByDayToCheck < dosage)
                        {
                            dosageResultModel = new DosageResultModel() { LabelChecking = checkingLabelByDay };
                            dosageResultModel.SuggestedValue = GetSuggestedValue(maxByDayToCheck, factor, odrCnv, false);
                        }
                    }

                    if (dosageResultModel != null)
                    {
                        dosageResultModel.ItemCd = d.ItemCd;
                        dosageResultModel.YjCd = d.YjCd;
                        dosageResultModel.CurrentValue = itemInfo?.Suryo ?? 0;
                        dosageResultModel.UnitName = itemInfo?.UnitName ?? string.Empty;
                        dosageResultModel.ItemName = itemInfo?.ItemName ?? string.Empty;
                        dosageResultModel.IsFromUserDefined = true;
                        checkedResult.Add(dosageResultModel);
                    }
                }
            });
            #endregion

            #region Check by MasterData

            List<string> listCheckedCode = listDosageInfoByUser.Select(d => d.ItemCd).ToList();
            List<string> listRestCode = listDrugCode.Where(d => !listCheckedCode.Contains(d)).ToList();

            var listDosageInfo =
                (from tenMst in _tenantNoTrackingDataContext.TenMsts.Where(t => listRestCode.Contains(t.ItemCd) && t.StartDate <= sinday && sinday <= t.EndDate)
                 join dosageDrug in _tenantNoTrackingDataContext.DosageDrugs.Where(d => d.RikikaUnit != null)
                 on tenMst.YjCd equals dosageDrug.YjCd
                 join dosageDosage in _tenantNoTrackingDataContext.DosageDosages.Where(d => string.IsNullOrEmpty(d.KyugenCd)
                                                                                                     && d.DosageCheckFlg == "1"
                                                                                                     && (string.IsNullOrEmpty(d.AgeCd) || (d.AgeOver <= age && d.AgeUnder > age) || (d.AgeOver == 0 && d.AgeUnder == 0))
                                                                                                     && ((d.WeightOver <= weight && d.WeightUnder > weight) || (d.WeightOver == 0 && d.WeightUnder == 0))
                                                                                                     && ((d.BodyOver <= bodySize && d.BodyUnder > bodySize) || (d.BodyOver == 0 && d.BodyUnder == 0))
                                                                                                )
                 on dosageDrug.DoeiCd equals dosageDosage.DoeiCd
                 select new
                 {
                     tenMst.ItemCd,
                     tenMst.YjCd,
                     tenMst.OdrTermVal,
                     tenMst.CnvTermVal,
                     dosageDrug.YakkaiUnit,
                     dosageDrug.RikikaRate,
                     dosageDrug.RikikaUnit,
                     dosageDosage.AgeUnder,
                     dosageDosage.AgeOver,
                     dosageDosage.OnceMin,
                     dosageDosage.OnceMax,
                     dosageDosage.OnceLimit,
                     dosageDosage.OnceUnit,
                     dosageDosage.OnceLimitUnit,
                     dosageDosage.DosageLimitTerm,
                     dosageDosage.UnittermLimit,
                     dosageDosage.UnittermUnit,
                     dosageDosage.DosageLimitUnit,
                     dosageDosage.IntervalWarningFlg,
                     dosageDosage.DayMin,
                     dosageDosage.DayMax,
                     dosageDosage.DayLimit,
                     dosageDosage.DayUnit,
                     dosageDosage.DayLimitUnit
                 }).ToList();

            foreach (var itemInfo in listItem)
            {
                // 年齢_以上が -1 以外で年齢が範囲内のもの
                double ratio = 1;
                var listFilteredDosageInfo = listDosageInfo.Where(i => i.ItemCd == itemInfo.ItemCD && (i.AgeOver != 0 || i.AgeUnder != 0)).ToList();
                if (listFilteredDosageInfo == null || listFilteredDosageInfo.Count == 0)
                {
                    // Eofなら、年齢_以上が-1のものだけでチェックする
                    listFilteredDosageInfo = listDosageInfo.Where(i => i.ItemCd == itemInfo.ItemCD && i.AgeOver == 0 && i.AgeUnder == 0).ToList();
                    ratio = ratioAsAge;
                }

                if (listFilteredDosageInfo == null || listFilteredDosageInfo.Count == 0)
                {
                    continue;
                }

                // Caculate dosage
                var firstDosageInfo = listFilteredDosageInfo[0];
                double factor = 0;
                double odrCnv = 0;
                double dosage = -1;

                string rikikaUnit = firstDosageInfo.RikikaUnit;
                if (!string.IsNullOrEmpty(rikikaUnit))
                {
                    double rikikaRate = (double)firstDosageInfo.RikikaRate;
                    string yakkaUnit = firstDosageInfo.YakkaiUnit;
                    string unitName = itemInfo.UnitName;

                    if (unitName == rikikaUnit)
                    {
                        dosage = itemInfo.Suryo;
                        factor = 1;
                        odrCnv = 1;
                    }
                    else if (unitName == yakkaUnit)
                    {
                        dosage = itemInfo.Suryo * rikikaRate;
                        factor = rikikaRate;
                        odrCnv = 1;
                    }
                    else if (itemInfo.TermVal > 0)
                    {
                        //ODR_INF_DETAIL.数量 * ODR_INF_DETAIL.単位換算値)  × M46_DOSAGE_DRUG.力価係数
                        dosage = itemInfo.Suryo * itemInfo.TermVal * rikikaRate;
                        factor = rikikaRate;
                        odrCnv = itemInfo.TermVal;
                    }
                }

                if (dosage <= 0)
                {
                    continue;
                }

                // Caculate the limited points
                double minPerOnce = -1;
                double maxPerOnce = -1;
                double limitByOnce = -1;
                double minPerDay = -1;
                double maxPerDay = -1;
                double limitByDay = -1;

                double limitByTerm = -1;
                double dosageLimitByTerm = -1;

                bool _isTermCheck(string IntervalWarningFlg, int DosageLimitTerm, string DosageLimitUnit, double UnittermLimit)
                {
                    return string.IsNullOrEmpty(IntervalWarningFlg) // ※投与間隔警告フラグ がNullのレコード（連日投与）に限定してチェックする。
                        && DosageLimitTerm != 0
                        && !string.IsNullOrEmpty(DosageLimitUnit) // 上限投与量定義期間と単位期間投与量上限値を取得します。
                        && UnittermLimit != 0;
                }

                foreach (var dosageInfo in listFilteredDosageInfo)
                {
                    double tempMinPerOnce = -1;
                    double tempMaxPerOnce = -1;
                    double tempLimitByOnce = -1;
                    double tempMinPerDay = -1;
                    double tempMaxPerDay = -1;
                    double tempLimitByDay = -1;
                    double tempLimitByTerm = -1;

                    if (string.IsNullOrEmpty(dosageInfo.OnceUnit))
                    {
                        tempMinPerOnce = -1;
                        tempMaxPerOnce = -1;
                    }
                    else if (dosageInfo.OnceUnit.EndsWith("/kg"))
                    {
                        tempMinPerOnce = dosageInfo.OnceMin * weight * ratio;
                        tempMaxPerOnce = dosageInfo.OnceMax * weight * ratio * ratioSetting;
                    }
                    else if (dosageInfo.OnceUnit.EndsWith("/m2"))
                    {
                        tempMinPerOnce = dosageInfo.OnceMin * bodySize * ratio;
                        tempMaxPerOnce = dosageInfo.OnceMax * bodySize * ratio * ratioSetting;
                    }
                    else
                    {
                        tempMinPerOnce = dosageInfo.OnceMin * ratio;
                        tempMaxPerOnce = dosageInfo.OnceMax * ratio * ratioSetting;
                    }

                    if (string.IsNullOrEmpty(dosageInfo.OnceLimitUnit))
                    {
                        tempLimitByOnce = -1;
                    }
                    else if (dosageInfo.OnceLimitUnit.EndsWith("/kg"))
                    {
                        tempLimitByOnce = dosageInfo.OnceLimit * weight * ratio;
                    }
                    else if (dosageInfo.OnceLimitUnit.EndsWith("/m2"))
                    {
                        tempLimitByOnce = dosageInfo.OnceLimit * bodySize * ratio;
                    }
                    else
                    {
                        tempLimitByOnce = dosageInfo.OnceLimit * ratio;
                    }

                    if (string.IsNullOrEmpty(dosageInfo.DayUnit))
                    {
                        tempMinPerDay = -1;
                        tempMaxPerDay = -1;
                    }
                    else if (dosageInfo.DayUnit.EndsWith("/kg"))
                    {
                        tempMinPerDay = dosageInfo.DayMin * weight * ratio;
                        tempMaxPerDay = dosageInfo.DayMax * weight * ratio * ratioSetting;
                    }
                    else if (dosageInfo.DayUnit.EndsWith("/m2"))
                    {
                        tempMinPerDay = dosageInfo.DayMin * bodySize * ratio;
                        tempMaxPerDay = dosageInfo.DayMax * bodySize * ratio * ratioSetting;
                    }
                    else
                    {
                        tempMinPerDay = dosageInfo.DayMin * ratio;
                        tempMaxPerDay = dosageInfo.DayMax * ratio * ratioSetting;
                    }

                    if (string.IsNullOrEmpty(dosageInfo.DayLimitUnit))
                    {
                        tempLimitByDay = -1;
                    }
                    else if (dosageInfo.DayLimitUnit.EndsWith("/kg"))
                    {
                        tempLimitByDay = dosageInfo.DayLimit * weight * ratio;
                    }
                    else if (dosageInfo.DayLimitUnit.EndsWith("/m2"))
                    {
                        tempLimitByDay = dosageInfo.DayLimit * bodySize * ratio;
                    }
                    else
                    {
                        tempLimitByDay = dosageInfo.DayLimit * ratio;
                    }

                    if (_isTermCheck(dosageInfo.IntervalWarningFlg, dosageInfo.DosageLimitTerm, dosageInfo.DosageLimitUnit, dosageInfo.UnittermLimit))
                    {
                        // 投与日数 ＞ 上限投与量定義期間 × 上限投与量定義期間単位 の場合
                        //上限値 = 単位期間投与量上限値 / (上限投与量定義期間 × 上限投与量定義期間単位) × 投与日数
                        // 上限投与量定義期間単位:
                        int upperLimitPeriod = -1;
                        switch (dosageInfo.DosageLimitUnit)
                        {
                            case "d":
                                upperLimitPeriod = 1;
                                break;
                            case "w":
                                upperLimitPeriod = 7;
                                break;
                            case "m":
                                upperLimitPeriod = 31;
                                break;
                            case "y":
                                upperLimitPeriod = 365;
                                break;
                        }

                        if (string.IsNullOrEmpty(dosageInfo.UnittermUnit))
                        {
                            tempLimitByTerm = -1;
                        }
                        else if (dosageInfo.UnittermUnit.EndsWith("/kg"))
                        {
                            tempLimitByTerm = dosageInfo.UnittermLimit * weight * ratio;
                        }
                        else if (dosageInfo.UnittermUnit.EndsWith("/m2"))
                        {
                            tempLimitByTerm = dosageInfo.UnittermLimit * bodySize * ratio;
                        }
                        else
                        {
                            tempLimitByTerm = dosageInfo.UnittermLimit * ratio;
                        }

                        if (tempLimitByTerm != -1 && upperLimitPeriod != -1)
                        {
                            if (itemInfo.UsageQuantity > dosageInfo.DosageLimitTerm * upperLimitPeriod)
                            {
                                tempLimitByTerm = tempLimitByTerm / (dosageInfo.DosageLimitTerm * upperLimitPeriod) * itemInfo.UsageQuantity;
                            }
                        }
                    }

                    if ((tempMinPerOnce < minPerOnce || minPerOnce == -1) && tempMinPerOnce != -1) minPerOnce = tempMinPerOnce;
                    if ((tempMaxPerOnce > maxPerOnce || maxPerOnce == -1) && tempMaxPerOnce != -1) maxPerOnce = tempMaxPerOnce;
                    if ((tempLimitByOnce > limitByOnce || limitByOnce == -1) && tempLimitByOnce != -1) limitByOnce = tempLimitByOnce;

                    if ((tempMinPerDay < minPerDay || minPerDay == -1) && tempMinPerDay != -1) minPerDay = tempMinPerDay;
                    if ((tempMaxPerDay > maxPerDay || maxPerDay == -1) && tempMaxPerDay != -1) maxPerDay = tempMaxPerDay;
                    if ((tempLimitByDay > limitByDay || limitByDay == -1) && tempLimitByDay != -1) limitByDay = tempLimitByDay;

                    if ((tempLimitByTerm > limitByTerm || limitByTerm == -1) && tempLimitByTerm != -1)
                    {
                        limitByTerm = tempLimitByTerm;
                        dosageLimitByTerm = itemInfo.UsageQuantity * dosage;
                    }
                }

                void _additionDosageResult(ref DosageResultModel dosageResult)
                {
                    dosageResult.ItemCd = firstDosageInfo.ItemCd;
                    dosageResult.YjCd = firstDosageInfo.YjCd;
                    dosageResult.CurrentValue = itemInfo.Suryo;
                    dosageResult.UnitName = itemInfo.UnitName;
                    dosageResult.ItemName = itemInfo.ItemName;
                }
                // Execute checking
                DosageResultModel? dosageResultModel = null;

                //Follow this document: https://wiki.sotatek.com/pages/viewpage.action?pageId=29130796
                bool isOnceLimitFoundIntoAllOfRecord = listFilteredDosageInfo.Where(f => string.IsNullOrEmpty(f.OnceLimitUnit)).Count() == 0;
                bool isNotOnceLimitFoundIntoAnyRecord = listFilteredDosageInfo.Where(f => !string.IsNullOrEmpty(f.OnceLimitUnit)).Count() == 0;
                bool isDayLimitFoundIntoAllOfRecord = listFilteredDosageInfo.Where(f => string.IsNullOrEmpty(f.DayLimitUnit)).Count() == 0;
                bool isNotDayLimitFoundIntoAnyRecord = listFilteredDosageInfo.Where(f => !string.IsNullOrEmpty(f.DayLimitUnit)).Count() == 0;

                double maxByDayToCheck = 0;
                DosageLabelChecking checkingLabelByDay = DosageLabelChecking.DayMax;

                if (isDayLimitFoundIntoAllOfRecord && 0 < limitByDay)
                {
                    maxByDayToCheck = limitByDay;
                    checkingLabelByDay = DosageLabelChecking.DayLimit;
                }
                else if (isNotDayLimitFoundIntoAnyRecord && 0 < maxPerDay)
                {
                    maxByDayToCheck = maxPerDay;
                    checkingLabelByDay = DosageLabelChecking.DayMax;
                }
                else if (0 < limitByDay && 0 < maxPerDay)
                {
                    maxByDayToCheck = Math.Max(limitByDay, maxPerDay);
                    checkingLabelByDay = maxPerDay < limitByDay ? DosageLabelChecking.DayLimit : DosageLabelChecking.DayMax;
                }

                double maxByOnceToCheck = 0;
                DosageLabelChecking checkingLabelByOnce = DosageLabelChecking.OneMax;
                if (isOnceLimitFoundIntoAllOfRecord && 0 < limitByOnce)
                {
                    maxByOnceToCheck = limitByOnce;
                    checkingLabelByOnce = DosageLabelChecking.OneLimit;
                }
                else if (isNotOnceLimitFoundIntoAnyRecord && 0 < maxPerOnce)
                {
                    maxByOnceToCheck = maxPerOnce;
                    checkingLabelByOnce = DosageLabelChecking.OneMax;
                }
                else if (0 < limitByOnce && 0 < maxPerOnce)
                {
                    maxByOnceToCheck = Math.Max(limitByOnce, maxPerOnce);
                    checkingLabelByOnce = maxPerOnce < limitByOnce ? DosageLabelChecking.OneLimit : DosageLabelChecking.OneMax;
                }

                if (itemInfo.SinKouiKbn == 21) // 内服
                {
                    // 一日量
                    if (minCheck && minPerDay > dosage && minPerDay != -1)
                    {
                        dosageResultModel = new DosageResultModel() { LabelChecking = DosageLabelChecking.DayMin };
                        dosageResultModel.SuggestedValue = GetSuggestedValue(minPerDay, factor, odrCnv, true);
                    }
                    else if (0 < maxByDayToCheck && maxByDayToCheck < dosage)
                    {
                        dosageResultModel = new DosageResultModel() { LabelChecking = checkingLabelByDay };
                        dosageResultModel.SuggestedValue = GetSuggestedValue(maxByDayToCheck, factor, odrCnv, false);
                    }

                    if (dosageResultModel != null)
                    {
                        _additionDosageResult(ref dosageResultModel);
                        checkedResult.Add(dosageResultModel);
                    }

                    // 期間投与量
                    if (limitByTerm < dosageLimitByTerm && limitByTerm != -1)
                    {
                        dosageResultModel = new DosageResultModel() { LabelChecking = DosageLabelChecking.TermLimit };
                        dosageResultModel.SuggestedValue = GetSuggestedValue(limitByTerm, factor, odrCnv, false);
                        _additionDosageResult(ref dosageResultModel);
                        dosageResultModel.CurrentValue = itemInfo.Suryo * itemInfo.UsageQuantity;
                        checkedResult.Add(dosageResultModel);
                    }
                }
                else if (itemInfo.SinKouiKbn == 22) // 頓服
                {
                    // 一回量
                    if (minCheck && minPerOnce > dosage && minPerOnce != -1)
                    {
                        dosageResultModel = new DosageResultModel() { LabelChecking = DosageLabelChecking.OneMin };
                        dosageResultModel.SuggestedValue = GetSuggestedValue(minPerOnce, factor, odrCnv, true);
                    }
                    else if (0 < maxByOnceToCheck && maxByOnceToCheck < dosage)
                    {
                        dosageResultModel = new DosageResultModel() { LabelChecking = checkingLabelByOnce };
                        dosageResultModel.SuggestedValue = GetSuggestedValue(maxByOnceToCheck, factor, odrCnv, false);
                    }

                    if (dosageResultModel != null)
                    {
                        _additionDosageResult(ref dosageResultModel);
                        checkedResult.Add(dosageResultModel);
                    }
                }
                else // その他
                {
                    // 一回量
                    if (minCheck && minPerOnce > dosage && minPerOnce != -1)
                    {
                        dosageResultModel = new DosageResultModel() { LabelChecking = DosageLabelChecking.OneMin };
                        dosageResultModel.SuggestedValue = GetSuggestedValue(minPerOnce, factor, odrCnv, true);
                    }
                    else if (0 < maxByOnceToCheck && maxByOnceToCheck < dosage)
                    {
                        dosageResultModel = new DosageResultModel() { LabelChecking = checkingLabelByOnce };
                        dosageResultModel.SuggestedValue = GetSuggestedValue(maxByOnceToCheck, factor, odrCnv, false);
                    }
                    // 一日量
                    else if (minCheck && minPerDay > dosage && minPerDay != -1)
                    {
                        dosageResultModel = new DosageResultModel() { LabelChecking = DosageLabelChecking.DayMin };
                        dosageResultModel.SuggestedValue = GetSuggestedValue(minPerDay, factor, odrCnv, true);
                    }
                    else if (0 < maxByDayToCheck && maxByDayToCheck < dosage)
                    {
                        dosageResultModel = new DosageResultModel() { LabelChecking = checkingLabelByDay };
                        dosageResultModel.SuggestedValue = GetSuggestedValue(maxByDayToCheck, factor, odrCnv, false);
                    }

                    if (dosageResultModel != null)
                    {
                        _additionDosageResult(ref dosageResultModel);
                        checkedResult.Add(dosageResultModel);
                    }
                }
            }

            #endregion

            return checkedResult;
        }

        public List<DayLimitResultModel> CheckDayLimit(int hpID, int sinday, List<string> listAddedOrderCode, double usingDay)
        {
            var dayLimitInfoByUser =
               (
                   from drugMst in _tenantNoTrackingDataContext.TenMsts.Where(m => listAddedOrderCode.Contains(m.ItemCd) && m.StartDate <= sinday && sinday <= m.EndDate)
                   join dayLimit in _tenantNoTrackingDataContext.DrugDayLimits.Where(d => 0 < d.LimitDay &&
                                                                                                   d.LimitDay < 999 &&
                                                                                                   d.StartDate <= sinday &&
                                                                                                   sinday <= d.EndDate &&
                                                                                                   d.IsDeleted == 0)
                   on drugMst.ItemCd equals dayLimit.ItemCd
                   select new
                   {
                       drugMst.Name,
                       drugMst.ItemCd,
                       drugMst.YjCd,
                       dayLimit.LimitDay,
                       dayLimit.StartDate,
                       dayLimit.EndDate
                   }
               ).ToList();

            List<string> listItemCodeByUserSetting = dayLimitInfoByUser.Select(d => d.ItemCd).ToList();
            List<string> listRestedItemCode = listAddedOrderCode.Where(c => !listItemCodeByUserSetting.Contains(c)).ToList();

            var dayLimitInfo =
                (
                from drugMst in _tenantNoTrackingDataContext.TenMsts.Where(m => listRestedItemCode.Contains(m.ItemCd) && m.StartDate <= sinday && sinday <= m.EndDate)
                join dayLimit in _tenantNoTrackingDataContext.M10DayLimit.Where(d => 0 < d.LimitDay && d.LimitDay < 999)
                on drugMst.YjCd equals dayLimit.YjCd
                select new
                {
                    drugMst.Name,
                    drugMst.ItemCd,
                    drugMst.YjCd,
                    dayLimit.LimitDay,
                    StartDate = dayLimit.StDate,
                    EndDate = dayLimit.EdDate
                }
                ).ToList();



            List<DayLimitResultModel> result = new List<DayLimitResultModel>();

            foreach (var item in dayLimitInfoByUser)
            {
                if (usingDay <= item.LimitDay)
                {
                    continue;
                }
                result.Add(new DayLimitResultModel()
                {
                    ItemCd = item.ItemCd,
                    ItemName = item.Name,
                    YjCd = item.YjCd,
                    LimitDay = item.LimitDay,
                    UsingDay = usingDay
                });
            }

            foreach (var item in dayLimitInfo)
            {
                if (!string.IsNullOrEmpty(item.StartDate) && sinday < item.StartDate.AsInteger() ||
                    !string.IsNullOrEmpty(item.EndDate) && item.EndDate.AsInteger() < sinday ||
                    usingDay <= item.LimitDay)
                {
                    continue;
                }
                result.Add(new DayLimitResultModel()
                {
                    ItemCd = item.ItemCd,
                    ItemName = item.Name,
                    YjCd = item.YjCd,
                    LimitDay = item.LimitDay,
                    UsingDay = usingDay
                });
            }
            return result;
        }

        #region private method

        private double GetRatio(int fromDay, int today)
        {
            double result = 0;
            int yyyy = 0;
            int mm = 0;
            int dd = 0;

            CIUtil.SDateToDecodeAge(fromDay, today, ref yyyy, ref mm, ref dd);
            if (yyyy == 0 && mm == 0)
            {
                // 年齢 1ヶ月未満 1/8 (von Harnackの換算表)
                result = 1.0 / 8;
            }
            else if (yyyy == 0 && mm < 4)
            {
                // 年齢 1ヶ月～4ヶ月 1/6 (von Harnackの換算表)
                result = 1.0 / 6;
            }
            else if (yyyy == 0)
            {
                // 年齢 4ヶ月以上1歳未満 1/5 (von Harnackの換算表)
                result = 1.0 / 5;
            }
            else if (yyyy < 15)
            {
                // 年齢 1歳以上 15歳未満 (((年齢 * 4) + 20) / 100) * 成人量 (Augsbergerの式 - Ⅱ)
                result = ((double)(yyyy * 4 + 20)) / 100;
            }
            else
            {
                // 15歳以上は成人量
                result = 1;
            }
            return result;
        }

        private double GetCommonWeight(int hpId, long ptID, int birdthDay, int sinday, int sex)
        {
            PhysicalAverage commonBodyInfo = GetCommonBodyInfo(birdthDay, sinday);
            double weight = 0;
            if (commonBodyInfo != null)
            {
                if (sex == 1)
                {
                    weight = commonBodyInfo.MaleWeight;
                }
                else
                {
                    weight = commonBodyInfo.FemaleWeight;
                }
            }
            return weight;
        }

        private double GetCommonHeight(int hpId, long ptID, int birdthDay, int sinday, int sex)
        {
            PhysicalAverage commonBodyInfo = GetCommonBodyInfo(birdthDay, sinday);
            double height = 0;
            if (commonBodyInfo != null)
            {
                if (sex == 1)
                {
                    height = commonBodyInfo.MaleHeight;
                }
                else
                {
                    height = commonBodyInfo.FemaleHeight;
                }
            }
            return height;
        }

        private double GetPatientWeight(int hpId, long ptID, int birdthDay, int sinday, int sex)
        {
            KensaInfDetail weightInfo = GetBodyInfo(hpId, ptID, sinday, "V0002");

            if (weightInfo != null && CIUtil.IsDigitsOnly(weightInfo?.ResultVal ?? string.Empty))
            {
                return weightInfo?.ResultVal?.AsDouble() ?? 0;
            }

            return GetCommonWeight(hpId, ptID, birdthDay, sinday, sex);
        }

        private double GetPatientHeight(int hpId, long ptID, int birdthDay, int sinday, int sex)
        {
            KensaInfDetail heightInfo = GetBodyInfo(hpId, ptID, sinday, "V0001");

            if (heightInfo != null && CIUtil.IsDigitsOnly(heightInfo.ResultVal ?? string.Empty))
            {
                var value = heightInfo.ResultVal ?? string.Empty;
                return value.AsDouble();
            }

            return GetCommonHeight(hpId, ptID, birdthDay, sinday, sex);
        }

        private double GetBodySize(double weight, double height, double age)
        {
            double bodySize;
            if (age >= 6)
            {
                // ①6歳以上用一般式（高齢者含む）
                // 体表面積(㎡)＝体重(kg)^0.444 × 身長(cm)^0.663 × 0.008883
                bodySize = Math.Pow(weight, 0.444) * Math.Pow(height, 0.663) * 0.008883;
            }
            else if (age >= 1)
            {
                // ②1歳以上6歳未満用乳幼児式
                // 体表面積(㎡)＝体重(kg)^0.423 × 身長(cm)^0.362 × 0.038189
                bodySize = Math.Pow(weight, 0.423) * Math.Pow(height, 0.362) * 0.038189;
            }
            else
            {
                // ③1歳未満用乳幼児式
                // 体表面積(㎡)＝体重(kg)^0.473 × 身長(cm)^0.655 × 0.009568
                bodySize = Math.Pow(weight, 0.473) * Math.Pow(height, 0.655) * 0.009568;
            }
            return bodySize;
        }

        private double GetSuggestedValue(double range, double factor, double odrCnv, bool up)
        {
            // 範囲値を換算し、文字列で返す
            if (up)
            {
                // 切り上げ（下限に対して使用)
                return CIUtil.RoundUp(range * (1 / factor) * (1 / odrCnv), 4);
            }
            else
            {
                // 切捨て（上限に対して使用)
                return CIUtil.RoundDown(range * (1 / factor) * (1 / odrCnv), 4);
            }
        }
        #endregion
    }

    class SeibunInfo
    {
        public string IndexWord { get; set; } = string.Empty;

        public string SeibunCd { get; set; } = string.Empty;

        public string IndexCd { get; set; } = string.Empty;
    }
}
