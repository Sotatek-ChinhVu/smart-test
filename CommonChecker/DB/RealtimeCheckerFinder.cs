﻿using CommonChecker.Caches;
using CommonChecker.Caches.Interface;
using CommonChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Models;
using Domain.Constant;
using Domain.Models.Diseases;
using Domain.Models.Family;
using Domain.Models.SpecialNote.PatientInfo;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using PostgreDataContext;
using PtAlrgyDrugModelStandard = Domain.Models.SpecialNote.ImportantNote.PtAlrgyDrugModel;
using PtAlrgyFoodModelStandard = Domain.Models.SpecialNote.ImportantNote.PtAlrgyFoodModel;
using PtKioRekiModelStandard = Domain.Models.SpecialNote.ImportantNote.PtKioRekiModel;
using PtOtcDrugModelStandard = Domain.Models.SpecialNote.ImportantNote.PtOtcDrugModel;
using PtOtherDrugModelStandard = Domain.Models.SpecialNote.ImportantNote.PtOtherDrugModel;
using PtSuppleModelStandard = Domain.Models.SpecialNote.ImportantNote.PtSuppleModel;

namespace CommonCheckers.OrderRealtimeChecker.DB
{
    public class RealtimeCheckerFinder : IRealtimeCheckerFinder
    {
        public TenantNoTrackingDataContext NoTrackingDataContext { get; private set; }
        private readonly IMasterDataCacheService _tenMstCacheService;
        public RealtimeCheckerFinder(TenantNoTrackingDataContext noTrackingDataContext, IMasterDataCacheService tenMstCacheService)
        {
            NoTrackingDataContext = noTrackingDataContext;
            _tenMstCacheService = tenMstCacheService;
        }

        public Dictionary<string, string> GetYjCdListByItemCdList(int hpId, List<ItemCodeModel> itemCdList, int sinDate)
        {
            var onlyItemCdList = itemCdList.Select(x => x.ItemCd).Distinct().ToList();
            return _tenMstCacheService.GetTenMstList(onlyItemCdList, sinDate)
                .Select(t => new { t.ItemCd, t.YjCd })
                .ToDictionary(t => t.ItemCd ?? string.Empty, t => t.YjCd ?? string.Empty);
        }

        public List<PtAlrgyFoodModel> GetFoodAllergyByPtId(int hpId, long ptId, int sinDate, List<PtAlrgyFoodModelStandard> ptAlrgyFoodModels, bool isDataOfDb)
        {
            List<PtAlrgyFoodModelStandard> listPtAlrgyFood;
            if (isDataOfDb)
            {
                listPtAlrgyFood =
                    NoTrackingDataContext.PtAlrgyFoods
                    .Where(p => p.HpId == hpId && p.PtId == ptId && p.IsDeleted != 1)
                    .AsEnumerable()
                    .Select(p => new PtAlrgyFoodModelStandard(p.HpId, p.PtId, p.SeqNo, p.SortNo, p.AlrgyKbn ?? string.Empty, p.StartDate, p.EndDate, p.Cmt ?? string.Empty, p.IsDeleted, string.Empty))
                    .ToList();
            }
            else
            {
                listPtAlrgyFood = ptAlrgyFoodModels.Where(p => p.HpId == hpId && p.PtId == ptId && p.IsDeleted != 1).AsEnumerable()
                    .Select(p => new PtAlrgyFoodModelStandard(p.HpId, p.PtId, p.SeqNo, p.SortNo, p.AlrgyKbn ?? string.Empty, p.StartDate, p.EndDate, p.Cmt ?? string.Empty, p.IsDeleted, string.Empty))
                    .ToList();
            }

            var listFilteredBySinData = listPtAlrgyFood
                .Where(p => CIUtil.FullStartDate(p.StartDate) <= sinDate && sinDate <= CIUtil.FullEndDate(p.EndDate))
                .Select(p => new PtAlrgyFoodModel(p.HpId, p.PtId, p.SeqNo, p.SortNo, p.AlrgyKbn ?? string.Empty, p.StartDate, p.EndDate, p.Cmt ?? string.Empty, p.IsDeleted, string.Empty))
                .ToList();
            return listFilteredBySinData;
        }

        public List<PtAlrgyDrugModel> GetDrugAllergyByPtId(int hpId, long ptId, int sinDate, List<PtAlrgyDrugModelStandard> ptAlrgyDrugModels, bool isDataOfDb)
        {
            List<PtAlrgyDrugModelStandard> listPtAlrgyDrug;
            if (isDataOfDb)
            {
                listPtAlrgyDrug = NoTrackingDataContext.PtAlrgyDrugs
                    .Where(p => p.HpId == hpId && p.PtId == ptId && p.IsDeleted != 1)
                    .AsEnumerable()
                    .Select(p => new PtAlrgyDrugModelStandard(p.HpId, p.PtId, p.SeqNo, p.SortNo, p.ItemCd ?? string.Empty, p.DrugName ?? string.Empty, p.StartDate, p.EndDate, p.Cmt ?? string.Empty, p.IsDeleted))
                    .ToList();
            }
            else
            {
                listPtAlrgyDrug = ptAlrgyDrugModels
                    .Where(p => p.HpId == hpId && p.PtId == ptId && p.IsDeleted != 1)
                    .AsEnumerable()
                    .Select(p => new PtAlrgyDrugModelStandard(p.HpId, p.PtId, p.SeqNo, p.SortNo, p.ItemCd ?? string.Empty, p.DrugName ?? string.Empty, p.StartDate, p.EndDate, p.Cmt ?? string.Empty, p.IsDeleted))
                    .ToList();
            }

            var listFilteredBySinData = listPtAlrgyDrug
                .Where(p => CIUtil.FullStartDate(p.StartDate) <= sinDate && sinDate <= CIUtil.FullEndDate(p.EndDate))
                .Select(p => new PtAlrgyDrugModel(p.HpId, p.PtId, p.SeqNo, p.SortNo, p.ItemCd ?? string.Empty, p.DrugName ?? string.Empty, p.StartDate, p.EndDate, p.Cmt ?? string.Empty, p.IsDeleted))
                .ToList();
            return listFilteredBySinData;
        }

        public KensaInfDetail GetBodyInfo(int hpId, long ptId, int sinday, string kensaItemCode)
        {
            return NoTrackingDataContext.KensaInfDetails
                .Where(k => k.HpId == hpId && k.PtId == ptId && k.IraiDate <= sinday && k.KensaItemCd == kensaItemCode && k.ResultVal != null && k.ResultVal != string.Empty)
                .OrderByDescending(k => k.IraiDate).FirstOrDefault() ?? new KensaInfDetail();
        }

        public PhysicalAverage GetCommonBodyInfo(int birthDay, int sinday)
        {
            int ageY = 0;
            int ageM = 0;
            int ageD = 0;
            CIUtil.SDateToDecodeAge(birthDay, sinday, ref ageY, ref ageM, ref ageD);

            int sinYear = sinday / 10000;
            return NoTrackingDataContext.PhysicalAverage
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

        private List<M56ExIngrdtMain> GetDrugTypeInfo(int haigouSetting, List<string> itemCodeList)
        {
            return _tenMstCacheService
                .GetM56ExIngrdtMainList(itemCodeList)
                .Where
                (
                    i => haigouSetting == 0 ||
                         haigouSetting == 1 && (i.HaigouFlg != "1" || i.YuekiFlg != "1") ||
                         haigouSetting == 2 && (i.HaigouFlg != "1" || i.KanpoFlg != "1") ||
                         haigouSetting == 3 && (i.HaigouFlg != "1" || (i.YuekiFlg != "1" && i.KanpoFlg != "1"))
                )
                .ToList();
        }

        #region Check allergy

        public List<DrugAllergyResultModel> CheckDuplicatedComponent(int hpID, long ptID, int sinDate, List<ItemCodeModel> listItemCode, List<string> listComparedItemCode)
        {
            bool IsNoMasterData()
            {
                return NoTrackingDataContext.M56ExEdIngredients.Count() == 0;
            }

            List<string> listDrugAllergyAsPatientCode = listComparedItemCode;
            List<DrugAllergyResultModel> checkedResult = new List<DrugAllergyResultModel>();

            if (!IsNoMasterData())
            {
                (List<TenMst> tenMstList, List<M56ExEdIngredients> m56ExEdIngredientList) getData(List<string> itemCodeList)
                {
                    var tenMstList = _tenMstCacheService.GetTenMstList(itemCodeList, sinDate);
                    var m56ExEdIngredientList = _tenMstCacheService.GetM56ExEdIngredientList(itemCodeList).Where(i => (i.Sbt == 1 || i.Sbt == 2 && i.TenkabutuCheck == "1")).ToList();

                    return new(tenMstList, m56ExEdIngredientList);
                }

                var dataByPatientInf = getData(listDrugAllergyAsPatientCode);
                var listDrugAllergyAsPatientInfo =
                    (from drugMst in dataByPatientInf.tenMstList
                     join componentInfo in dataByPatientInf.m56ExEdIngredientList
                     on drugMst.YjCd equals componentInfo.YjCd
                     select new
                     {
                         drugMst.ItemCd,
                         drugMst.YjCd,
                         componentInfo.SeibunCd
                     }).ToList();

                var data = getData(listItemCode.Select(i => i.ItemCd).ToList());
                var listCheckingDrugInfo =
                    (from drugMst in data.tenMstList
                     join componentInfo in data.m56ExEdIngredientList
                     on drugMst.YjCd equals componentInfo.YjCd
                     join listItemCodes in listItemCode
                     on drugMst.ItemCd equals listItemCodes.ItemCd
                     select new
                     {
                         listItemCodes.Id,
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
                                                                           Id = checkingDrug.Id,
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

                var groupResult = tempResult.GroupBy(p => new { p.YjCd, p.AllergyYjCd, p.Id })
                                            .Select(g => g.ToList())
                                            .ToList();

                groupResult.ForEach((group) =>
                {
                    var generalComponent = group.FirstOrDefault(a => a.SeqNo == "000");
                    if (generalComponent != null)
                    {
                        group = new List<DrugAllergyResultModel>() { generalComponent };
                    }
                    checkedResult.AddRange(group);
                });
            }

            return checkedResult;
        }

        public List<DrugAllergyResultModel> CheckProDrug(int hpID, long ptID, int sinDate, List<ItemCodeModel> listItemCode, List<string> listComparedItemCode)
        {
            List<DrugAllergyResultModel> result = new();
            List<string> listDrugAllergyAsPatientCode = listComparedItemCode;

            var listCheckingDrugInfo =
                (from drugMst in NoTrackingDataContext.TenMsts.Where(i => listItemCode.Select(x => x.ItemCd).Contains(i.ItemCd) && i.StartDate <= sinDate && sinDate <= i.EndDate).AsQueryable()
                 join componentInfo in NoTrackingDataContext.M56ExEdIngredients.Where(i => i.ProdrugCheck != null && i.ProdrugCheck == string.Empty && i.ProdrugCheck != "0")  //Filter ProDrug >= 1
                 on drugMst.YjCd equals componentInfo.YjCd
                 join drugPro in NoTrackingDataContext.M56ProdrugCd
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
                (from drugMst in NoTrackingDataContext.TenMsts.Where(i => listDrugAllergyAsPatientCode.Contains(i.ItemCd) && i.StartDate <= sinDate && sinDate <= i.EndDate).AsQueryable()
                 join componentInfo in NoTrackingDataContext.M56ExEdIngredients.Where(i => i.ProdrugCheck != null && i.ProdrugCheck != string.Empty && i.ProdrugCheck != "0")  //Filter ProDrug >= 1
                 on drugMst.YjCd equals componentInfo.YjCd
                 join drugPro in NoTrackingDataContext.M56ProdrugCd
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
                var id = listItemCode.First(item => item.ItemCd == checkingDrug.ItemCd).Id;
                var checkUsage = listDrugAllergyAsPatientInfo.Where
                                                              (
                                                                    a => a.KasseitaiCd == checkingDrug.KasseitaiCd
                                                              )
                                                              .Select
                                                              (
                                                                    item => new DrugAllergyResultModel()
                                                                    {
                                                                        Id = id,
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

        public List<DrugAllergyResultModel> CheckSameComponent(int hpID, long ptID, int sinDate, List<ItemCodeModel> listItemCode, List<string> listComparedItemCode)
        {
            List<DrugAllergyResultModel> result = new List<DrugAllergyResultModel>();
            List<string> listDrugAllergyAsPatientCode = listComparedItemCode;

            var onlyItemCd = listItemCode.Select(x => x.ItemCd).Distinct().ToList();
            var listCheckingDrugInfo =
                (from drugMst in NoTrackingDataContext.TenMsts.Where(i => onlyItemCd.Contains(i.ItemCd) && i.StartDate <= sinDate && sinDate <= i.EndDate).AsQueryable()
                 join componentInfo in NoTrackingDataContext.M56ExEdIngredients.Where(i => i.AnalogueCheck == "1")
                 on drugMst.YjCd equals componentInfo.YjCd
                 join drugAnalogue in NoTrackingDataContext.M56ExAnalogue
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
                (from drugMst in NoTrackingDataContext.TenMsts.Where(i => listDrugAllergyAsPatientCode.Contains(i.ItemCd) && i.StartDate <= sinDate && sinDate <= i.EndDate)
                 join componentInfo in NoTrackingDataContext.M56ExEdIngredients.Where(i => i.AnalogueCheck == "1")
                 on drugMst.YjCd equals componentInfo.YjCd
                 join drugAnalogue in NoTrackingDataContext.M56ExAnalogue
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
                var id = listItemCode.First(item => item.ItemCd == checkingDrug.ItemCd).Id;
                var checkUsage = listDrugAllergyAsPatientInfo.Where
                                                             (
                                                                   a => a.AnalogueCd == checkingDrug.AnalogueCd
                                                             )
                                                             .Select
                                                             (
                                                                   item => new DrugAllergyResultModel()
                                                                   {
                                                                       Id = id,
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

        public List<DrugAllergyResultModel> CheckDuplicatedClass(int hpID, long ptID, int sinDate, List<ItemCodeModel> listItemCode, List<string> listComparedItemCode)
        {
            List<DrugAllergyResultModel> result;
            List<string> listDrugAllergyAsPatientCode = listComparedItemCode.Distinct().ToList();

            var itemCdCheckingDrugInfoList = listItemCode.Select(x => x.ItemCd).Distinct().ToList();
            var listCheckingDrugInfo =
                 (from drugMst in NoTrackingDataContext.TenMsts.Where(i => itemCdCheckingDrugInfoList.Contains(i.ItemCd) && i.StartDate <= sinDate && sinDate <= i.EndDate).AsQueryable()
                  join componentInfo in NoTrackingDataContext.M56AlrgyDerivatives
                  on drugMst.YjCd equals componentInfo.YjCd
                  join drvalrgyCode in NoTrackingDataContext.M56DrvalrgyCode
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
                 (from drugMst in NoTrackingDataContext.TenMsts.Where(i => listDrugAllergyAsPatientCode.Contains(i.ItemCd) && i.StartDate <= sinDate && sinDate <= i.EndDate).AsQueryable()
                  join componentInfo in NoTrackingDataContext.M56AlrgyDerivatives
                  on drugMst.YjCd equals componentInfo.YjCd
                  join drvalrgyCode in NoTrackingDataContext.M56DrvalrgyCode
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
                join listItemCodes in listItemCode
                on checkingInfo.ItemCd equals listItemCodes.ItemCd
                select new
                {
                    listItemCodes.Id,
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
                .Select(c => c.OrderBy(o => o.RankNo).FirstOrDefault())
                .ToList();

            result =
                (from r in checkedResult
                 select new DrugAllergyResultModel() { Id = r.Id, Level = 4, YjCd = r.YjCd, SeibunCd = r.SeibunCd, AllergyYjCd = r.AllergyYjCd, AllergySeibunCd = r.AllergySeibunCd, Tag = r.DrvalrgyCd, ItemCd = r.ItemCd }
                 ).ToList();

            return result;
        }

        #endregion

        #region Check duplicated component
        public List<DrugAllergyResultModel> CheckDuplicatedComponentForDuplication(int hpID, long ptID, int sinDate, List<ItemCodeModel> itemCodeModelList, List<ItemCodeModel> comparedItemCodeModelList, int haigouSetting)
        {
            List<DrugAllergyResultModel> checkedResult = new();

            (List<TenMst> tenMstList, List<M56ExEdIngredients> componentList, List<M56ExIngrdtMain> drugTypeList) getData(List<string> itemCodeList)
            {
                List<TenMst> tenMstList = _tenMstCacheService.GetTenMstList(itemCodeList, sinDate);
                List<M56ExEdIngredients> componentList = _tenMstCacheService.GetM56ExEdIngredientList(itemCodeList).Where(i => i.Sbt == 1).ToList();
                List<M56ExIngrdtMain> drugTypeList = GetDrugTypeInfo(haigouSetting, itemCodeList);

                return (tenMstList, componentList, drugTypeList);
            }

            var comparedItemCodeList = comparedItemCodeModelList.Select(c => c.ItemCd).Distinct().ToList();
            var comparedData = getData(comparedItemCodeList);
            var listDrugAllergyAsPatientInfo =
                    (from drugMst in comparedData.tenMstList
                     join componentInfo in comparedData.componentList
                     on drugMst.YjCd equals componentInfo.YjCd
                     join drugTypeInfo in comparedData.drugTypeList
                     on drugMst.YjCd equals drugTypeInfo.YjCd
                     join comparedItemCodeModel in comparedItemCodeModelList
                     on drugMst.ItemCd equals comparedItemCodeModel.ItemCd
                     select new
                     {
                         comparedItemCodeModel.Id,
                         drugMst.ItemCd,
                         drugMst.YjCd,
                         componentInfo.SeibunCd,
                         drugTypeInfo.ZensinsayoFlg,
                         drugTypeInfo.YohoCd,
                         drugTypeInfo.HaigouFlg,
                         drugTypeInfo.YuekiFlg,
                         drugTypeInfo.KanpoFlg,
                     }).ToList();

            var itemCodeList = itemCodeModelList.Select(x => x.ItemCd).Distinct().ToList();
            var data = getData(itemCodeList);
            var listCheckingDrugInfo =
                (from drugMst in data.tenMstList
                 join componentInfo in data.componentList
                 on drugMst.YjCd equals componentInfo.YjCd
                 join drugTypeInfo in data.drugTypeList
                 on drugMst.YjCd equals drugTypeInfo.YjCd
                 join listItemCodes in itemCodeModelList
                 on drugMst.ItemCd equals listItemCodes.ItemCd
                 select new
                 {
                     listItemCodes.Id,
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

            List<DrugAllergyResultModel> tempResult = new();
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
                                                                       Id = checkingDrug.Id,
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
                var generalComponent = group.FirstOrDefault(a => a.SeqNo == "000");
                if (generalComponent != null)
                {
                    group = new List<DrugAllergyResultModel>() { generalComponent };
                }
                checkedResult.AddRange(group);
            });

            return checkedResult;
        }

        public List<DrugAllergyResultModel> CheckProDrugForDuplication(int hpID, long ptID, int sinDate, List<ItemCodeModel> itemCodeModelList, List<ItemCodeModel> comparedItemCodeModelList, int haigouSetting)
        {
            List<DrugAllergyResultModel> result = new();

            (List<TenMst> tenMstList, List<M56ExEdIngredients> componentList, List<M56ExIngrdtMain> drugTypeList, List<M56ProdrugCd> drugProList) getData(List<string> itemCodeList)
            {
                List<TenMst> tenMstList = _tenMstCacheService.GetTenMstList(itemCodeList, sinDate);
                List<M56ExEdIngredients> componentList = _tenMstCacheService.GetM56ExEdIngredientList(itemCodeList).Where(i => i.ProdrugCheck != null && i.ProdrugCheck != string.Empty && i.ProdrugCheck != "0").ToList();
                List<M56ExIngrdtMain> drugTypeList = GetDrugTypeInfo(haigouSetting, itemCodeList);
                List<M56ProdrugCd> drugProList = _tenMstCacheService.GetM56ProdrugCdList(itemCodeList);

                return (tenMstList, componentList, drugTypeList, drugProList);
            }

            var itemCodeList = itemCodeModelList.Select(x => x.ItemCd).ToList();
            var data = getData(itemCodeList);
            var listCheckingDrugInfo =
                (from drugMst in data.tenMstList
                 join componentInfo in data.componentList
                 on drugMst.YjCd equals componentInfo.YjCd
                 join drugTypeInfo in data.drugTypeList
                 on drugMst.YjCd equals drugTypeInfo.YjCd
                 join drugPro in data.drugProList
                 on componentInfo.SeibunCd equals drugPro.SeibunCd
                 join listItemCodes in itemCodeModelList
                 on drugMst.ItemCd equals listItemCodes.ItemCd
                 select new
                 {
                     listItemCodes.Id,
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

            var comparedItemCodeList = comparedItemCodeModelList.Select(c => c.ItemCd).Distinct().ToList();
            var comparedData = getData(comparedItemCodeList);
            var listDrugAllergyAsPatientInfo =
                (from drugMst in comparedData.tenMstList
                 join componentInfo in comparedData.componentList
                 on drugMst.YjCd equals componentInfo.YjCd
                 join drugTypeInfo in comparedData.drugTypeList
                 on drugMst.YjCd equals drugTypeInfo.YjCd
                 join drugPro in comparedData.drugProList
                 on componentInfo.SeibunCd equals drugPro.SeibunCd
                 join listComparedItemCodes in comparedItemCodeModelList
                 on drugMst.ItemCd equals listComparedItemCodes.ItemCd
                 select new
                 {
                     listComparedItemCodes.Id,
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
                                                                       Id = checkingDrug.Id,
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

        public List<DrugAllergyResultModel> CheckSameComponentForDuplication(int hpID, long ptID, int sinDate, List<ItemCodeModel> itemCodeModelList, List<ItemCodeModel> comparedItemCodeModelList, int haigouSetting)
        {
            List<DrugAllergyResultModel> result = new();

            (List<TenMst> tenMstList, List<M56ExEdIngredients> componentList, List<M56ExIngrdtMain> drugTypeList, List<M56ExAnalogue> drugAnalogueList) getData(List<string> itemCodeList)
            {
                List<TenMst> tenMstList = _tenMstCacheService.GetTenMstList(itemCodeList, sinDate);
                List<M56ExEdIngredients> componentList = _tenMstCacheService.GetM56ExEdIngredientList(itemCodeList).Where(i => i.AnalogueCheck == "1").ToList();
                List<M56ExIngrdtMain> drugTypeList = GetDrugTypeInfo(haigouSetting, itemCodeList);
                List<M56ExAnalogue> drugAnalogueList = _tenMstCacheService.GetM56ExAnalogueList(itemCodeList);

                return (tenMstList, componentList, drugTypeList, drugAnalogueList);
            }

            var itemCodeList = itemCodeModelList.Select(x => x.ItemCd).Distinct().ToList();
            var data = getData(itemCodeList);
            var listCheckingDrugInfo =
                (from drugMst in data.tenMstList
                 join componentInfo in data.componentList
                 on drugMst.YjCd equals componentInfo.YjCd
                 join drugTypeInfo in data.drugTypeList
                 on drugMst.YjCd equals drugTypeInfo.YjCd
                 join drugAnalogue in data.drugAnalogueList
                 on componentInfo.SeibunCd equals drugAnalogue.SeibunCd
                 join listItemCodes in itemCodeModelList
                 on drugMst.ItemCd equals listItemCodes.ItemCd
                 select new
                 {
                     listItemCodes.Id,
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

            var comparedItemCodeList = comparedItemCodeModelList.Select(c => c.ItemCd).Distinct().ToList();
            var comparedData = getData(comparedItemCodeList);
            var listDrugAllergyAsPatientInfo =
                (from drugMst in comparedData.tenMstList
                 join componentInfo in comparedData.componentList
                 on drugMst.YjCd equals componentInfo.YjCd
                 join drugTypeInfo in comparedData.drugTypeList
                 on drugMst.YjCd equals drugTypeInfo.YjCd
                 join drugAnalogue in comparedData.drugAnalogueList
                 on componentInfo.SeibunCd equals drugAnalogue.SeibunCd
                 join listComparedItemCodes in comparedItemCodeModelList
                 on drugMst.ItemCd equals listComparedItemCodes.ItemCd
                 select new
                 {
                     listComparedItemCodes.Id,
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
                                                                       Id = checkingDrug.Id,
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

        public List<DrugAllergyResultModel> CheckDuplicatedClassForDuplication(int hpID, long ptID, int sinDate, List<ItemCodeModel> itemCodeModelList, List<ItemCodeModel> comparedItemCodeModelList, int haigouSetting)
        {
            List<DrugAllergyResultModel> result = new();

            (List<TenMst> tenMstList, List<M56ExIngrdtMain> drugTypeList, List<M56YjDrugClass> yjDrugList, List<M56DrugClass> drugList) getData(List<string> itemCodeList)
            {
                List<TenMst> tenMstList = _tenMstCacheService.GetTenMstList(itemCodeList, sinDate);
                List<M56ExIngrdtMain> drugTypeList = GetDrugTypeInfo(haigouSetting, itemCodeList);
                List<M56YjDrugClass> yjDrugList = _tenMstCacheService.GetM56YjDrugClassList(itemCodeList);
                List<M56DrugClass> drugList = _tenMstCacheService.GetM56DrugClassList(itemCodeList).Where(d => d.ClassDuplication == "1").ToList();

                return (tenMstList, drugTypeList, yjDrugList, drugList);
            }

            var itemCodeList = itemCodeModelList.Select(x => x.ItemCd).Distinct().ToList();
            var data = getData(itemCodeList);
            var listCheckingDrugInfo =
                (from drugMst in data.tenMstList
                 join drugTypeInfo in data.drugTypeList
                 on drugMst.YjCd equals drugTypeInfo.YjCd
                 join yjDrugClass in data.yjDrugList
                 on drugMst.YjCd equals yjDrugClass.YjCd
                 join drugClass in data.drugList
                 on yjDrugClass.ClassCd equals drugClass.ClassCd
                 join listItemCodes in itemCodeModelList
                 on drugMst.ItemCd equals listItemCodes.ItemCd
                 select new
                 {
                     listItemCodes.Id,
                     drugMst.ItemCd,
                     drugMst.YjCd,
                     drugClass.ClassCd,
                     drugTypeInfo.ZensinsayoFlg,
                     drugTypeInfo.YohoCd,
                     drugTypeInfo.HaigouFlg,
                     drugTypeInfo.YuekiFlg,
                     drugTypeInfo.KanpoFlg,
                 }).ToList();

            var comparedItemCodeList = comparedItemCodeModelList.Select(c => c.ItemCd).Distinct().ToList();
            var compatedData = getData(comparedItemCodeList);
            var listDrugAllergyAsPatientInfo =
                (from drugMst in compatedData.tenMstList
                 join drugTypeInfo in compatedData.drugTypeList
                 on drugMst.YjCd equals drugTypeInfo.YjCd
                 join yjDrugClass in compatedData.yjDrugList
                 on drugMst.YjCd equals yjDrugClass.YjCd
                 join drugClass in compatedData.drugList
                 on yjDrugClass.ClassCd equals drugClass.ClassCd
                 join listComparedItemCodes in comparedItemCodeModelList
                 on drugMst.ItemCd equals listComparedItemCodes.ItemCd
                 select new
                 {
                     listComparedItemCodes.Id,
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
                                                                       Id = checkingDrug.Id,
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

        public List<FoodAllergyResultModel> CheckFoodAllergy(int hpID, long ptID, int sinDate, List<ItemCodeModel> itemCodeModelList, int level, List<PtAlrgyFoodModelStandard> ptAlrgyFoodModels, bool isDataOfDb)
        {
            List<FoodAllergyResultModel> result = new();
            var allergyFoodAsPatient = GetFoodAllergyByPtId(hpID, ptID, sinDate, ptAlrgyFoodModels, isDataOfDb);

            List<string> listAlrgyKbn = allergyFoodAsPatient.Where(a => a.AlrgyKbn != null).Select(a => a.AlrgyKbn).ToList();
            List<string> itemCodeList = itemCodeModelList.Select(x => x.ItemCd).Distinct().ToList();
            var m12FoodAlrgyList = NoTrackingDataContext.M12FoodAlrgy.Where(c => itemCodeList.Contains(c.KikinCd ?? string.Empty) && listAlrgyKbn.Contains(c.FoodKbn)).ToList();

            var checkedResult = (from m12FoodAlrgy in m12FoodAlrgyList
                                 join listItemCodes in itemCodeModelList
                                 on m12FoodAlrgy.KikinCd equals listItemCodes.ItemCd
                                 select new
                                 {
                                     listItemCodes.Id,
                                     m12FoodAlrgy.FoodKbn,
                                     m12FoodAlrgy.KikinCd,
                                     m12FoodAlrgy.YjCd,
                                     m12FoodAlrgy.TenpuLevel,
                                     m12FoodAlrgy.AttentionCmt,
                                     m12FoodAlrgy.WorkingMechanism
                                 }
                                 ).ToList();

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
                        Id = r.Id,
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

        public List<AgeResultModel> CheckAge(int hpID, long ptID, int sinday, int level, int ageTypeCheckSetting, List<ItemCodeModel> listItemCode, List<KensaInfDetailModel> kensaInfDetailModels, bool isDataOfDb)
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
                List<string> listLevel = new();
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

            PtInf patientInfo = _tenMstCacheService.GetPtInf();
            if (patientInfo == null)
            {
                return new List<AgeResultModel>();
            }

            int birthDay = patientInfo.Birthday;
            double age = CIUtil.SDateToAge(birthDay, sinday);
            string sex = patientInfo.Sex.AsString();
            double weight = -1;

            if (isDataOfDb)
            {
                var weightInfo = GetBodyInfo(hpID, ptID, sinday, "V0002");
                if (!(weightInfo.HpId == 0 && weightInfo.PtId == 0 && weightInfo.SeqNo == 0 && weightInfo.IraiCd == 0))
                {
                    weight = weightInfo.ResultVal?.AsDouble() ?? 0;
                }
            }
            else
            {
                var weightInfo = kensaInfDetailModels.Where(k => k.HpId == hpID && k.PtId == ptID && k.IraiDate <= sinday && k.KensaItemCd == "V0002" && !string.IsNullOrEmpty(k.ResultVal))
                .OrderByDescending(k => k.IraiDate).FirstOrDefault();
                if (weightInfo != null)
                {
                    weight = weightInfo.ResultVal?.AsDouble() ?? 0;
                }
            }

            List<string> listSettingLevel = GetLevelRange();
            List<AgeResultModel> checkedResult;
            var itemCdList = listItemCode.Select(x => x.ItemCd).Distinct().ToList();
            if (ageTypeCheckSetting == 0)
            {
                if (weight < 0.0 && age < 0.0)
                {
                    return new List<AgeResultModel>();
                }

                checkedResult =
                (from itemInfo in NoTrackingDataContext.TenMsts.Where(i => itemCdList.Contains(i.ItemCd) && i.StartDate <= sinday && sinday <= i.EndDate).AsQueryable()
                 join ageCheck in NoTrackingDataContext.M14AgeCheck.Where
                 (
                     m =>
                     listSettingLevel.Contains(m.TenpuLevel ?? string.Empty) &&
                     (
                         (m.AgeKbn == "1" && m.AgeMin <= age && age < m.AgeMax && (m.SexKbn == null || m.SexKbn == string.Empty || m.SexKbn == sex)) ||
                         (m.WeightKbn == "1" && m.WeightMin <= weight && weight < m.WeightMax && (m.SexKbn == null || m.SexKbn == string.Empty || m.SexKbn == sex))
                     )
                 )
                 on itemInfo.YjCd equals ageCheck.YjCd
                 select new AgeResultModel()
                 {
                     ItemCd = itemInfo.ItemCd,
                     YjCd = itemInfo.YjCd ?? string.Empty,
                     TenpuLevel = ageCheck.TenpuLevel ?? string.Empty,
                     AttentionCmtCd = ageCheck.AttentionCmtCd,
                     WorkingMechanism = ageCheck.WorkingMechanism ?? string.Empty
                 }).ToList();
            }
            else
            {
                checkedResult =
                (from itemInfo in NoTrackingDataContext.TenMsts.Where(i => itemCdList.Contains(i.ItemCd) && i.StartDate <= sinday && sinday <= i.EndDate).AsQueryable()
                 join ageCheck in NoTrackingDataContext.M14AgeCheck.Where
                 (
                     m =>
                     listSettingLevel.Contains(m.TenpuLevel ?? string.Empty) &&
                     (
                         (m.AgeKbn == null || m.AgeKbn == string.Empty || (m.AgeKbn == "1" && m.AgeMin <= age && age < m.AgeMax && (m.SexKbn == null || m.SexKbn == string.Empty || m.SexKbn == sex))) &&
                         (m.WeightKbn == null || m.WeightKbn == string.Empty || (m.WeightKbn == "1" && weight >= 0.0 && m.WeightMin <= weight && weight < m.WeightMax && (m.SexKbn == null || m.SexKbn == string.Empty || m.SexKbn == sex))) &&
                         ((m.AgeKbn != null && m.AgeKbn != string.Empty) || (m.WeightKbn != null && m.WeightKbn != string.Empty))
                     )
                 )
                 on itemInfo.YjCd equals ageCheck.YjCd
                 select new AgeResultModel()
                 {
                     ItemCd = itemInfo.ItemCd,
                     YjCd = itemInfo.YjCd ?? string.Empty,
                     TenpuLevel = ageCheck.TenpuLevel ?? string.Empty,
                     AttentionCmtCd = ageCheck.AttentionCmtCd,
                     WorkingMechanism = ageCheck.WorkingMechanism ?? string.Empty
                 }).ToList();
            }
            var result = (from itemInfo in checkedResult
                          join itemCode in listItemCode
                          on itemInfo.ItemCd equals itemCode.ItemCd
                          select new AgeResultModel()
                          {
                              Id = itemCode.Id,
                              ItemCd = itemInfo.ItemCd,
                              YjCd = itemInfo.YjCd ?? string.Empty,
                              TenpuLevel = itemInfo.TenpuLevel ?? string.Empty,
                              AttentionCmtCd = itemInfo.AttentionCmtCd,
                              WorkingMechanism = itemInfo.WorkingMechanism ?? string.Empty
                          }
                        ).ToList();

            return result;
        }

        public List<DiseaseResultModel> CheckContraindicationForCurrentDisease(int hpID, long ptID, int level, int sinDate, List<ItemCodeModel> listItemCodeModel, List<PtDiseaseModel> ptDiseaseModels, bool isDataOfDb)
        {
            var listDiseaseCode = new List<string>();
            if (isDataOfDb)
            {
                listDiseaseCode = NoTrackingDataContext.PtByomeis
                    .Where(p => p.HpId == hpID &&
                                p.PtId == ptID &&
                                p.IsDeleted != 1 &&
                                p.StartDate <= sinDate && (p.TenkiKbn == TenkiKbnConst.Continued || sinDate <= p.TenkiDate))
                    .GroupBy(d => d.ByomeiCd)
                    .Select(d => d.Key ?? string.Empty).ToList();
            }
            else
            {
                listDiseaseCode = ptDiseaseModels.Where(p => p.HpId == hpID &&
                                       p.PtId == ptID &&
                                       p.IsDeleted != 1 &&
                                       p.StartDate <= sinDate && (p.TenkiKbn == TenkiKbnConst.Continued || sinDate <= p.TenkiDate))
                           .GroupBy(d => d.ByomeiCd)
                           .Select(d => d.Key)
                           .Distinct()
                           .ToList();
            }

            List<string> listBYCode =
                NoTrackingDataContext.M42ContraindiDisCon
                .Where(m => listDiseaseCode.Contains(m.ReceCd ?? string.Empty))
                .Select(m => m.ByotaiCd)
                .ToList();

            var itemCodeList = listItemCodeModel.Select(i => i.ItemCd).Distinct().ToList();
            var tenMstList = _tenMstCacheService.GetTenMstList(itemCodeList, sinDate)
                .Select(t => new { t.ItemCd, t.YjCd })
                .ToList();
            var yjCodeList = tenMstList.Select(t => t.YjCd).Distinct().ToList();
            var contraindicationList = NoTrackingDataContext.M42ContraindiDrugMainEx
                .Where(c => c.TenpuLevel <= level && listBYCode.Contains(c.ByotaiCd) && (c.KioCd == null || c.KioCd == string.Empty || c.KioCd == "1") && (string.IsNullOrEmpty(c.FamilyCd) || c.FamilyCd == "1") && yjCodeList.Contains(c.YjCd))
                .Select(c => new { c.YjCd, c.ByotaiCd, c.TenpuLevel, c.CmtCd, c.KijyoCd })
                .ToList();

            List<DiseaseResultModel> checkedResult =
                (from itemMst in tenMstList
                 join contraindication in contraindicationList
                 on itemMst.YjCd equals contraindication.YjCd
                 join itemCodeModel in listItemCodeModel
                 on itemMst.ItemCd equals itemCodeModel.ItemCd
                 select new DiseaseResultModel()
                 {
                     Id = itemCodeModel.Id,
                     DiseaseType = 0,
                     ItemCd = itemMst.ItemCd,
                     YjCd = itemMst.YjCd ?? string.Empty,
                     TenpuLevel = contraindication.TenpuLevel,
                     ByotaiCd = contraindication.ByotaiCd,
                     CmtCd = contraindication.CmtCd ?? string.Empty,
                     KijyoCd = contraindication.KijyoCd ?? string.Empty
                 }).ToList();

            return checkedResult;
        }

        public List<DiseaseResultModel> CheckContraindicationForHistoryDisease(int hpID, long ptID, int level, int sinday, List<ItemCodeModel> itemCodeModelList, List<PtKioRekiModelStandard> ptKioRekiModels, bool isDataOfDb)
        {
            List<string> listByomeiCd = new();
            if (isDataOfDb)
            {
                listByomeiCd = NoTrackingDataContext.PtKioRekis
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
            else
            {
                listByomeiCd = ptKioRekiModels
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
            var itemCodeList = itemCodeModelList.Select(i => i.ItemCd).ToList();
            var tenMstList = _tenMstCacheService.GetTenMstList(itemCodeList, sinday)
                .Select(i => new { i.ItemCd, i.YjCd })
                .ToList();
            var yjCodeList = tenMstList.Select(t => t.YjCd).ToList();
            var contraindicationList = NoTrackingDataContext.M42ContraindiDrugMainEx
                .Where(c => c.TenpuLevel <= level && (c.KioCd == "1" || c.KioCd == "2") && yjCodeList.Contains(c.YjCd))
                .Select(c => new { c.YjCd, c.ByotaiCd, c.TenpuLevel, c.CmtCd, c.KijyoCd })
                .ToList();
            var byotaiCdList = contraindicationList.Select(c => c.ByotaiCd).ToList();
            var contraindiDisConList = NoTrackingDataContext.M42ContraindiDisCon
                .Where(c => listByomeiCd.Contains(c.ReceCd!) && byotaiCdList.Contains(c.ByotaiCd))
                .Select(c => new { c.ByotaiCd })
                .ToList();

            List<DiseaseResultModel> checkedResult =
                (from itemMst in tenMstList
                 join contraindication in contraindicationList
                 on itemMst.YjCd equals contraindication.YjCd
                 join contraindiDisCon in contraindiDisConList
                 on contraindication.ByotaiCd equals contraindiDisCon.ByotaiCd
                 join listItemCodes in itemCodeModelList
                 on itemMst.ItemCd equals listItemCodes.ItemCd
                 select new DiseaseResultModel()
                 {
                     Id = listItemCodes.Id,
                     DiseaseType = 1,
                     ItemCd = itemMst.ItemCd,
                     YjCd = itemMst.YjCd ?? string.Empty,
                     TenpuLevel = contraindication.TenpuLevel,
                     ByotaiCd = contraindication.ByotaiCd,
                     CmtCd = contraindication.CmtCd ?? string.Empty,
                     KijyoCd = contraindication.KijyoCd ?? string.Empty
                 }).ToList();

            return checkedResult;
        }

        public List<DiseaseResultModel> CheckContraindicationForFamilyDisease(int hpID, long ptID, int level, int sinday, List<ItemCodeModel> itemCodeModelList, List<FamilyModel> familyModels, bool isDataOfDb)
        {
            var itemCodeList = itemCodeModelList.Select(i => i.ItemCd).Distinct().ToList();
            var tenMstList = _tenMstCacheService.GetTenMstList(itemCodeList, sinday)
                .Select(t => new { t.YjCd, t.ItemCd })
                .ToList();
            var yjCodeList = tenMstList.Select(t => t.YjCd).Distinct().ToList();
            var contraindicationList = NoTrackingDataContext.M42ContraindiDrugMainEx
                .Where(c => c.TenpuLevel <= level && (c.FamilyCd == "1" || c.FamilyCd == "2") && yjCodeList.Contains(c.YjCd))
                .Select(c => new { c.YjCd, c.ByotaiCd, c.TenpuLevel, c.CmtCd, c.KijyoCd })
                .ToList();
            var byotaiCdList = contraindicationList.Select(c => c.ByotaiCd).Distinct().ToList();
            var contraindiDisConList = NoTrackingDataContext.M42ContraindiDisCon
                .Where(c => byotaiCdList.Contains(c.ByotaiCd))
                .Select(p => new { p.ByotaiCd, p.ReceCd })
                .ToList();
            var receCdList = contraindiDisConList.Select(r => r.ReceCd).ToList();

            List<Tuple<long, string>> ptFamilyRekisList;
            List<long> ptFamilyList;
            if (isDataOfDb)
            {
                ptFamilyRekisList = NoTrackingDataContext.PtFamilyRekis
                    .Where(p => p.HpId == hpID && p.IsDeleted == 0 && receCdList.Contains(p.ByomeiCd))
                    .Select(p => new Tuple<long, string>(p.FamilyId, p.ByomeiCd ?? string.Empty))
                    .ToList();
                var familyIdList = ptFamilyRekisList.Select(f => f.Item1).Distinct().ToList();
                ptFamilyList = NoTrackingDataContext.PtFamilys
                    .Where(p => p.HpId == hpID && p.PtId == ptID && p.IsDeleted == 0 && p.ZokugaraCd != "OT" && familyIdList.Contains(p.FamilyId))
                    .Select(p => p.FamilyId)
                    .Distinct()
                    .ToList();
            }
            else
            {
                ptFamilyList = familyModels.Where(f => f.ListPtFamilyRekis.Any(r => receCdList.Contains(r.ByomeiCd)) && f.PtId == ptID && f.ZokugaraCd != "OT").Select(p => p.FamilyId).Distinct().ToList();
                var ptFamilyRekisModelList = new List<(long familyId, PtFamilyRekiModel ptFamilyReki)>();
                foreach (var familyModel in familyModels)
                {
                    var ptFamilyRekis = familyModel.ListPtFamilyRekis.Where(r => receCdList.Contains(r.ByomeiCd));
                    foreach (var ptFamilyReki in ptFamilyRekis)
                    {
                        ptFamilyRekisModelList.Add(new(familyModel.FamilyId, ptFamilyReki));
                    }
                }
                ptFamilyRekisList = ptFamilyRekisModelList.Select(p => new Tuple<long, string>(p.familyId, p.ptFamilyReki.ByomeiCd)).ToList();
            }

            List<DiseaseResultModel> checkedResult =
                (from itemMst in tenMstList
                 join contraindication in contraindicationList
                 on itemMst.YjCd equals contraindication.YjCd
                 join contraindiDisCon in contraindiDisConList
                 on contraindication.ByotaiCd equals contraindiDisCon.ByotaiCd
                 join historyDisease in ptFamilyRekisList
                 on contraindiDisCon.ReceCd equals historyDisease.Item2
                 join familyInfo in ptFamilyList
                 on historyDisease.Item1 equals familyInfo
                 join listItemCodes in itemCodeModelList
                 on itemMst.ItemCd equals listItemCodes.ItemCd
                 select new DiseaseResultModel()
                 {
                     Id = listItemCodes.Id,
                     DiseaseType = 2,
                     ItemCd = itemMst.ItemCd,
                     YjCd = itemMst.YjCd ?? string.Empty,
                     TenpuLevel = contraindication.TenpuLevel,
                     ByotaiCd = contraindication.ByotaiCd,
                     CmtCd = contraindication.CmtCd ?? string.Empty,
                     KijyoCd = contraindication.KijyoCd ?? string.Empty
                 }).ToList();

            return checkedResult;
        }

        public List<KinkiResultModel> CheckKinki(int hpID, int level, int sinday, List<ItemCodeModel> listCurrentOrderCode, List<ItemCodeModel> listAddedOrderCode)
        {
            List<string> itemCodeList = new();
            itemCodeList.AddRange(listCurrentOrderCode.Select(x => x.ItemCd).ToList());
            itemCodeList.AddRange(listAddedOrderCode.Select(x => x.ItemCd).ToList());
            itemCodeList = itemCodeList.Distinct().ToList();

            var tenMstList = _tenMstCacheService.GetTenMstList(itemCodeList, sinday);

            var listCurrentOrderSubYjCode = (from tenMst in tenMstList
                                             join listCurrentOrderCodes in listCurrentOrderCode
                                             on tenMst.ItemCd equals listCurrentOrderCodes.ItemCd
                                             select new
                                             {
                                                 listCurrentOrderCodes.Id,
                                                 tenMst.YjCd,
                                                 tenMst.ItemCd,
                                                 YjCd4 = CIUtil.Substring(tenMst.YjCd ?? string.Empty, 0, 4),
                                                 YjCd7 = CIUtil.Substring(tenMst.YjCd ?? string.Empty, 0, 7),
                                                 YjCd8 = CIUtil.Substring(tenMst.YjCd ?? string.Empty, 0, 8),
                                                 YjCd9 = CIUtil.Substring(tenMst.YjCd ?? string.Empty, 0, 9),
                                                 YjCd12 = CIUtil.Substring(tenMst.YjCd ?? string.Empty, 0, 12)
                                             }
                                             ).ToList();

            var listAddedOrderSubYjCode = (from tenMst in tenMstList
                                           join listAddedOrderCodes in listAddedOrderCode
                                           on tenMst.ItemCd equals listAddedOrderCodes.ItemCd
                                           select new
                                           {
                                               listAddedOrderCodes.Id,
                                               tenMst.YjCd,
                                               tenMst.ItemCd,
                                               YjCd4 = CIUtil.Substring(tenMst.YjCd ?? string.Empty, 0, 4),
                                               YjCd7 = CIUtil.Substring(tenMst.YjCd ?? string.Empty, 0, 7),
                                               YjCd8 = CIUtil.Substring(tenMst.YjCd ?? string.Empty, 0, 8),
                                               YjCd9 = CIUtil.Substring(tenMst.YjCd ?? string.Empty, 0, 9),
                                               YjCd12 = CIUtil.Substring(tenMst.YjCd ?? string.Empty, 0, 12)
                                           }
                                           ).ToList();

            #region filter master data to improve performance
            var listAddedOrderSubYj4Code = listAddedOrderSubYjCode.Select(o => o.YjCd4).Distinct().ToList();
            var listAddedOrderSubYj7Code = listAddedOrderSubYjCode.Select(o => o.YjCd7).Distinct().ToList();
            var listAddedOrderSubYj8Code = listAddedOrderSubYjCode.Select(o => o.YjCd8).Distinct().ToList();
            var listAddedOrderSubYj9Code = listAddedOrderSubYjCode.Select(o => o.YjCd9).Distinct().ToList();
            var listAddedOrderSubYj12Code = listAddedOrderSubYjCode.Select(o => o.YjCd12).Distinct().ToList();
            var listCurrentOrderSubYj4Code = listCurrentOrderSubYjCode.Select(o => o.YjCd4).Distinct().ToList();
            var listCurrentOrderSubYj7Code = listCurrentOrderSubYjCode.Select(o => o.YjCd7).Distinct().ToList();
            var listCurrentOrderSubYj8Code = listCurrentOrderSubYjCode.Select(o => o.YjCd8).Distinct().ToList();
            var listCurrentOrderSubYj9Code = listCurrentOrderSubYjCode.Select(o => o.YjCd9).Distinct().ToList();
            var listCurrentOrderSubYj12Code = listCurrentOrderSubYjCode.Select(o => o.YjCd12).Distinct().ToList();
            var filteredMasterData = NoTrackingDataContext.M01Kinki
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

            List<KinkiResultModel> result = new();

            foreach (var addedOrderItemCode in listAddedOrderCode)
            {
                var addedOrderSubYjCode = listAddedOrderSubYjCode.FirstOrDefault(s => s.ItemCd == addedOrderItemCode.ItemCd);

                if (addedOrderSubYjCode == null)
                {
                    continue;
                }

                foreach (var (currentOrderCode, currentOrderSubYjCode) in from currentOrderCode in listCurrentOrderCode
                                                                          let currentOrderSubYjCode = listCurrentOrderSubYjCode.FirstOrDefault(s => s.ItemCd == currentOrderCode.ItemCd)
                                                                          select (currentOrderCode, currentOrderSubYjCode))
                {
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
                                Id = addedOrderItemCode.Id,
                                AYjCd = addedOrderSubYjCode.YjCd ?? string.Empty,
                                BYjCd = currentOrderSubYjCode.YjCd ?? string.Empty,
                                SubAYjCd = m.ACd,
                                SubBYjCd = m.BCd,
                                CommentCode = m.CmtCd,
                                SayokijyoCode = m.SayokijyoCd,
                                Kyodo = m.Kyodo ?? string.Empty,
                                IsNeedToReplace =
                                (
                                    m.ACd == currentOrderSubYjCode.YjCd7 ||
                                    m.ACd == currentOrderSubYjCode.YjCd8 ||
                                    m.ACd == currentOrderSubYjCode.YjCd9 ||
                                    m.ACd == currentOrderSubYjCode.YjCd12
                                ),
                                ItemCd = addedOrderItemCode.ItemCd ?? string.Empty,
                                KinkiItemCd = currentOrderCode.ItemCd ?? string.Empty,
                            }
                        )
                        .ToList();
                    result.AddRange(checkedResult);
                }
            }

            List<KinkiResultModel> filteredResultAsLevel = new();
            foreach (KinkiResultModel kinkiResultModel in result)
            {
                if (kinkiResultModel.Kyodo.AsInteger() <= level)
                {
                    filteredResultAsLevel.Add(kinkiResultModel);
                }
            }

            return filteredResultAsLevel;
        }

        public List<KinkiResultModel> CheckKinkiUser(int hpID, int level, int sinday, List<ItemCodeModel> listCurrentOrderCode, List<ItemCodeModel> listAddedOrderCode)
        {
            if (level <= 0) return new();

            List<string> itemCodeList = new();

            var listCurrentOrderCodeItemCd = listCurrentOrderCode.Select(x => x.ItemCd).Distinct().ToList();
            var listAddedOrderCodeItemCd = listAddedOrderCode.Select(x => x.ItemCd).Distinct().ToList();

            itemCodeList.AddRange(listCurrentOrderCodeItemCd);
            itemCodeList.AddRange(listAddedOrderCodeItemCd);

            var tenMstList = _tenMstCacheService.GetTenMstList(itemCodeList, sinday);

            var listYjCd = (from tenMst in tenMstList
                            join listAddedOrderCodes in listAddedOrderCode
                            on tenMst.ItemCd equals listAddedOrderCodes.ItemCd
                            select new
                            {
                                listAddedOrderCodes.Id,
                                tenMst.ItemCd,
                                tenMst.YjCd
                            }
                            ).ToList();

            var listChecked = _tenMstCacheService.GetKinkiMstList(itemCodeList).Where(k => 
                                                                         k.BCd != null &&
                                                                         (
                                                                              listCurrentOrderCodeItemCd.Contains(k.ACd) && listAddedOrderCodeItemCd.Contains(k.BCd) ||
                                                                              listCurrentOrderCodeItemCd.Contains(k.BCd) && listAddedOrderCodeItemCd.Contains(k.ACd)
                                                                         )
                                                              ).ToList();

            List<KinkiResultModel> result = new();
            listChecked.ForEach((c) =>
            {
                string addedYjCd = listYjCd.Where(y => y.ItemCd == c.ACd).Select(y => y.YjCd).FirstOrDefault() ?? string.Empty;
                string currentYjCd = listYjCd.Where(y => y.ItemCd == c.BCd).Select(y => y.YjCd).FirstOrDefault() ?? string.Empty;
                string addedItemCd = listYjCd.Where(y => y.ItemCd == c.ACd).Select(y => y.ItemCd).FirstOrDefault() ?? string.Empty;
                string currentItemCd = listYjCd.Where(y => y.ItemCd == c.BCd).Select(y => y.ItemCd).FirstOrDefault() ?? string.Empty;
                string currentId = listYjCd.Where(y => y.ItemCd == c.BCd).Select(y => y.Id).FirstOrDefault() ?? string.Empty;
                result.Add(new KinkiResultModel()
                {
                    Id = currentId,
                    AYjCd = addedYjCd,
                    BYjCd = currentYjCd,
                    IsNeedToReplace = false,
                    ItemCd = addedItemCd,
                    KinkiItemCd = currentItemCd,
                });
            });
            return result;
        }

        public List<KinkiResultModel> CheckKinkiTain(int hpID, long ptId, int sinday, int level, List<ItemCodeModel> addedOrderItemCodeList, List<PtOtherDrugModelStandard> ptOtherDrugModels, bool isDataOfDb)
        {
            List<PtOtherDrugModelStandard> listPtOtherDrugModel;
            if (isDataOfDb)
            {
                listPtOtherDrugModel = NoTrackingDataContext.PtOtherDrug
                                       .Where(o => o.HpId == hpID && o.PtId == ptId && o.IsDeleted == 0)
                                       .AsEnumerable()
                                       .Select(p => new PtOtherDrugModelStandard(p.HpId, p.PtId, p.SeqNo, p.SortNo, p.ItemCd ?? string.Empty, p.DrugName ?? string.Empty, p.StartDate, p.EndDate, p.Cmt ?? string.Empty, p.IsDeleted))
                                       .ToList();
            }
            else
            {

                listPtOtherDrugModel = ptOtherDrugModels
                                       .Where(o => o.HpId == hpID && o.PtId == ptId && o.IsDeleted == 0)
                                       .ToList();
            }

            var listTainCode = listPtOtherDrugModel
                .Where(p => CIUtil.FullStartDate(p.StartDate) <= sinday && sinday <= CIUtil.FullEndDate(p.EndDate))
                .Select(p => p.ItemCd)
                .Distinct()
                .ToList();

            var listCurrentOrderSubYjCode = _tenMstCacheService.GetTenMstList(listTainCode, sinday)
                .Select(m => new
                {
                    m.YjCd,
                    m.ItemCd,
                    YjCd4 = CIUtil.Substring(m.YjCd ?? string.Empty, 0, 4),
                    YjCd7 = CIUtil.Substring(m.YjCd ?? string.Empty, 0, 7),
                    YjCd8 = CIUtil.Substring(m.YjCd ?? string.Empty, 0, 8),
                    YjCd9 = CIUtil.Substring(m.YjCd ?? string.Empty, 0, 9),
                    YjCd12 = CIUtil.Substring(m.YjCd ?? string.Empty, 0, 12)
                })
                .ToList();

            var listAddedOrderSubYjCode = 
                _tenMstCacheService.GetTenMstList(addedOrderItemCodeList.Select(x => x.ItemCd).ToList(), sinday)
                .Select(m => new
                {
                    m.YjCd,
                    m.ItemCd,
                    YjCd4 = CIUtil.Substring(m.YjCd ?? string.Empty, 0, 4),
                    YjCd7 = CIUtil.Substring(m.YjCd ?? string.Empty, 0, 7),
                    YjCd8 = CIUtil.Substring(m.YjCd ?? string.Empty, 0, 8),
                    YjCd9 = CIUtil.Substring(m.YjCd ?? string.Empty, 0, 9),
                    YjCd12 = CIUtil.Substring(m.YjCd ?? string.Empty, 0, 12)
                })
                .ToList();

            List<KinkiResultModel> result = new();

            foreach (var addedOrderItemCode in addedOrderItemCodeList)
            {
                var addedOrderSubYjCode = listAddedOrderSubYjCode.FirstOrDefault(s => s.ItemCd == addedOrderItemCode.ItemCd);
                if (addedOrderSubYjCode == null)
                {
                    continue;
                }

                foreach (var currentOrderCode in listTainCode)
                {
                    var currentOrderSubYjCode = listCurrentOrderSubYjCode.FirstOrDefault(s => s.ItemCd == currentOrderCode);
                    if (currentOrderSubYjCode == null)
                    {
                        continue;
                    }

                    var checkedResult =
                        NoTrackingDataContext.M01Kinki
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
                                Kyodo = m.Kyodo ?? string.Empty,
                                IsNeedToReplace =
                                (
                                    m.ACd == currentOrderSubYjCode.YjCd7 ||
                                    m.ACd == currentOrderSubYjCode.YjCd8 ||
                                    m.ACd == currentOrderSubYjCode.YjCd9 ||
                                    m.ACd == currentOrderSubYjCode.YjCd12
                                ),
                                ItemCd = addedOrderItemCode.ItemCd,
                                Id = addedOrderItemCode.Id
                            }
                        )
                        .ToList();

                    result.AddRange(checkedResult);
                }
            }

            List<KinkiResultModel> filteredResultAsLevel = new();
            foreach (KinkiResultModel kinkiResultModel in result)
            {
                if (kinkiResultModel.Kyodo.AsInteger() <= level)
                {
                    filteredResultAsLevel.Add(kinkiResultModel);
                }
            }

            return filteredResultAsLevel;
        }

        public List<KinkiResultModel> CheckKinkiOTC(int hpID, long ptId, int sinday, int level, List<ItemCodeModel> addedOrderItemCodeList, List<PtOtcDrugModelStandard> ptOtcDrugModels, bool isDataOfDb)
        {
            List<int> listSerialNum = new();
            if (isDataOfDb)
            {
                listSerialNum = NoTrackingDataContext.PtOtcDrug
                                .Where(o => o.HpId == hpID && o.PtId == ptId && o.IsDeleted == 0)
                                .AsEnumerable()
                                .Select(p => new PtOtcDrugModel(p.HpId, p.PtId, p.SeqNo, p.SortNo, p.SerialNum, p.TradeName ?? string.Empty, p.StartDate, p.EndDate, p.Cmt ?? string.Empty, p.IsDeleted))
                                .Where(p => CIUtil.FullStartDate(p.StartDate) <= sinday && sinday <= CIUtil.FullEndDate(p.EndDate))
                                .Select(p => p.SerialNum)
                                .ToList();
            }
            else
            {
                listSerialNum = ptOtcDrugModels
                                 .Where(o => o.HpId == hpID && o.PtId == ptId && o.IsDeleted == 0)
                                 .Where(p => CIUtil.FullStartDate(p.StartDate) <= sinday && sinday <= CIUtil.FullEndDate(p.EndDate))
                                 .Select(p => p.SerialNum)
                                 .ToList();
            }

            var listSubOTCCode = NoTrackingDataContext.M38Ingredients
                .Where(m => listSerialNum.Contains(m.SerialNum))
                .Select(m => new
                {
                    m.SerialNum,
                    m.SeibunCd,
                    m.Sbt,
                    Otc7 = m.SeibunCd.Substring(0, 7),
                })
                .ToList();

            var onlyItemCd = addedOrderItemCodeList.Select(x => x.ItemCd).Distinct().ToList();
            var listAddedOrderSubYjCode = _tenMstCacheService.GetTenMstList(onlyItemCd, sinday)
                .Select(m => new
                {
                    m.YjCd,
                    m.ItemCd,
                    YjCd4 = CIUtil.Substring(m.YjCd ?? string.Empty, 0, 4),
                    YjCd7 = CIUtil.Substring(m.YjCd ?? string.Empty, 0, 7),
                    YjCd8 = CIUtil.Substring(m.YjCd ?? string.Empty, 0, 8),
                    YjCd9 = CIUtil.Substring(m.YjCd ?? string.Empty, 0, 9),
                    YjCd12 = CIUtil.Substring(m.YjCd ?? string.Empty, 0, 12)
                })
                .ToList();

            List<KinkiResultModel> result = new();

            foreach (var addedOrderItemCode in addedOrderItemCodeList)
            {
                var addedOrderSubYjCode = listAddedOrderSubYjCode.FirstOrDefault(s => s.ItemCd == addedOrderItemCode.ItemCd);
                if (addedOrderSubYjCode == null)
                {
                    continue;
                }

                foreach (int oTCSerialNum in listSerialNum)
                {
                    List<string> subOTCCode = listSubOTCCode.Where(s => s.SerialNum == oTCSerialNum).Select(s => s.Otc7).ToList();
                    var checkedResult =
                        NoTrackingDataContext.M01Kinki
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
                                Id = addedOrderItemCode.Id,
                                ItemCd = addedOrderItemCode.ItemCd,
                                AYjCd = addedOrderSubYjCode.YjCd ?? string.Empty,
                                BYjCd = oTCSerialNum.ToString(),
                                SubAYjCd = m.ACd,
                                SubBYjCd = m.BCd,
                                CommentCode = m.CmtCd,
                                SayokijyoCode = m.SayokijyoCd,
                                Kyodo = m.Kyodo ?? string.Empty,
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

            List<KinkiResultModel> filteredResultAsLevel = new();
            foreach (KinkiResultModel kinkiResultModel in result)
            {
                if (kinkiResultModel.Kyodo.AsInteger() <= level)
                {
                    filteredResultAsLevel.Add(kinkiResultModel);
                }
            }

            return filteredResultAsLevel;
        }

        public List<KinkiResultModel> CheckKinkiSupple(int hpID, long ptId, int sinday, int level, List<ItemCodeModel> addedOrderItemCodeList, List<PtSuppleModelStandard> ptSuppleModels, bool isDataOfDb)
        {
            List<string> listIndexWord = new();
            if (isDataOfDb)
            {
                listIndexWord = NoTrackingDataContext.PtSupples
                                .Where(o => o.HpId == hpID && o.PtId == ptId && o.IsDeleted == 0)
                                .AsEnumerable()
                                .Select(p => new PtSuppleModel(p.HpId, p.PtId, p.SeqNo, p.SortNo, p.IndexCd ?? string.Empty, p.IndexWord ?? string.Empty, p.StartDate, p.EndDate, p.Cmt ?? string.Empty, p.IsDeleted))
                                .Where(p => p.StartDate <= sinday && sinday <= p.EndDate)
                                .Select(p => p.IndexWord)
                                .ToList();
            }
            else
            {
                listIndexWord = ptSuppleModels
                                .Where(o => o.HpId == hpID
                                            && o.PtId == ptId
                                            && o.IsDeleted == 0
                                            && o.StartDate <= sinday
                                            && sinday <= o.EndDate)
                                .Select(p => p.IndexWord)
                                .ToList();
            }

            List<SeibunInfo> listSeibunInfo =
                    (
                        from indexdef in NoTrackingDataContext.M41SuppleIndexdefs.Where(s => listIndexWord.Contains(s.IndexWord ?? string.Empty))
                        join indexCode in NoTrackingDataContext.M41SuppleIndexcodes
                        on indexdef.SeibunCd equals indexCode.IndexCd
                        select new SeibunInfo
                        {
                            IndexWord = indexdef.IndexWord ?? string.Empty,
                            SeibunCd = indexCode.SeibunCd,
                            IndexCd = indexCode.IndexCd
                        }
                    ).ToList();

            var addedOrderItemCodeListDistinct = addedOrderItemCodeList.Select(x => x.ItemCd).Distinct().ToList();
            var listAddedOrderSubYjCode = 
                _tenMstCacheService.GetTenMstList(addedOrderItemCodeListDistinct, sinday)
                .Select(m => new
                {
                    m.YjCd,
                    m.ItemCd,
                    YjCd4 = CIUtil.Substring(m.YjCd ?? string.Empty, 0, 4),
                    YjCd7 = CIUtil.Substring(m.YjCd ?? string.Empty, 0, 7),
                    YjCd8 = CIUtil.Substring(m.YjCd ?? string.Empty, 0, 8),
                    YjCd9 = CIUtil.Substring(m.YjCd ?? string.Empty, 0, 9),
                    YjCd12 = CIUtil.Substring(m.YjCd ?? string.Empty, 0, 12)
                })
                .ToList();

            List<KinkiResultModel> result = new();

            foreach (var addedOrderItemCode in addedOrderItemCodeList)
            {
                var addedOrderSubYjCode = listAddedOrderSubYjCode.FirstOrDefault(s => s.ItemCd == addedOrderItemCode.ItemCd);
                if (addedOrderSubYjCode == null)
                {
                    continue;
                }

                foreach (var seibunInfo in listSeibunInfo)
                {
                    string seibunCd = seibunInfo.SeibunCd;
                    var checkedResult =
                        NoTrackingDataContext.M01Kinki
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
                                Id = addedOrderItemCode.Id,
                                AYjCd = addedOrderSubYjCode.YjCd ?? string.Empty,
                                BYjCd = seibunInfo.IndexCd,
                                SubAYjCd = m.ACd,
                                SubBYjCd = m.BCd,
                                CommentCode = m.CmtCd,
                                SayokijyoCode = m.SayokijyoCd,
                                Kyodo = m.Kyodo ?? string.Empty,
                                IsNeedToReplace = true,
                                IndexWord = seibunInfo.IndexWord,
                                SeibunCd = seibunInfo.SeibunCd,
                                ItemCd = addedOrderItemCode.ItemCd
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

        public (double weight, double height) GetPtBodyInfo(int hpId, long ptId, int sinday, double currentHeight, double currentWeight, List<KensaInfDetailModel> kensaInfDetailModels, bool isDataOfDb)
        {
            PtInf patientInfo = _tenMstCacheService.GetPtInf();
            if (patientInfo == null)
            {
                return new(0, 0);
            }
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
                weight = GetPatientWeight(hpId, ptId, patientInfo.Birthday, sinday, sex, kensaInfDetailModels, isDataOfDb);
            }
            else
            {
                weight = currentWeight;
            }

            double height = 0;
            if (currentHeight <= -1)
            {
                // Get new data from SpecialNote but have no HeightInfo
                height = GetCommonHeight(patientInfo.Birthday, sinday, sex);
            }
            else if (currentHeight == 0)
            {
                // Can't get newData from SpecialNote
                height = GetPatientHeight(hpId, ptId, patientInfo.Birthday, sinday, sex, kensaInfDetailModels);
            }
            else
            {
                height = currentHeight;
            }

            return new(weight, height);
        }

        public List<DosageResultModel> CheckDosage(int hpId, long ptId, int sinday, List<DrugInfo> listItem, bool minCheck, double ratioSetting, double height, double weight, List<KensaInfDetailModel> kensaInfDetailModels, bool isDataOfDb)
        {
            PtInf patientInfo = _tenMstCacheService.GetPtInf();
            if (patientInfo == null)
            {
                return new List<DosageResultModel>();
            }

            List<DosageResultModel> checkedResult = new List<DosageResultModel>();

            double age = CIUtil.SDateToAge(patientInfo.Birthday, sinday);
            double ratioAsAge = GetRatio(patientInfo.Birthday, sinday);
            double bodySize = GetBodySize(weight, height, age);

            List<ItemCodeModel> listDrugCode = listItem.Select(x => new ItemCodeModel(x.ItemCD, x.Id)).ToList();

            #region Check by UserData
            var itemCodeList = listDrugCode.Select(x => x.ItemCd).ToList();

            var listDosageInfoByUser =
            (
                    from tenMst in _tenMstCacheService.GetTenMstList(itemCodeList, sinday)
                    join dosageDrug in _tenMstCacheService.GetDosageDrugList(itemCodeList)
                    on tenMst.YjCd equals dosageDrug.YjCd
                    join dosageDMst in _tenMstCacheService.GetDosageMstList(itemCodeList)
                    on tenMst.ItemCd equals dosageDMst.ItemCd
                    join listDrugCodes in listDrugCode
                    on tenMst.ItemCd equals listDrugCodes.ItemCd
                    select new
                    {
                        listDrugCodes.Id,
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

            List<ItemCodeModel> listCheckedCode = listDosageInfoByUser.Select(x => new ItemCodeModel(x.ItemCd, x.Id)).ToList();
            List<ItemCodeModel> listRestCode = listDrugCode.Where(d => !listCheckedCode.Contains(d)).ToList();
            itemCodeList = listRestCode.Select(x => x.ItemCd).ToList();

            var listDosageInfo =
                (from tenMst in _tenMstCacheService.GetTenMstList(itemCodeList, sinday)
                 join dosageDrug in _tenMstCacheService.GetDosageDrugList(itemCodeList)
                 on tenMst.YjCd equals dosageDrug.YjCd
                 join dosageDosage in _tenMstCacheService.GetDosageDosageList(itemCodeList).Where(d => string.IsNullOrEmpty(d.KyugenCd)
                                                                                                     && d.DosageCheckFlg == "1"
                                                                                                     && (string.IsNullOrEmpty(d.AgeCd) || (d.AgeOver <= age && d.AgeUnder > age) || (d.AgeOver == 0 && d.AgeUnder == 0))
                                                                                                     && ((d.WeightOver <= weight && d.WeightUnder > weight) || (d.WeightOver == 0 && d.WeightUnder == 0))
                                                                                                     && ((d.BodyOver <= bodySize && d.BodyUnder > bodySize) || (d.BodyOver == 0 && d.BodyUnder == 0))
                                                                                                )
                 on dosageDrug.DoeiCd equals dosageDosage.DoeiCd
                 join listRestCodes in listRestCode
                 on tenMst.ItemCd equals listRestCodes.ItemCd
                 select new
                 {
                     listRestCodes.Id,
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

                        if (tempLimitByTerm != -1 && upperLimitPeriod != -1 && itemInfo.UsageQuantity > dosageInfo.DosageLimitTerm * upperLimitPeriod)
                        {
                            tempLimitByTerm = tempLimitByTerm / (dosageInfo.DosageLimitTerm * upperLimitPeriod) * itemInfo.UsageQuantity;
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
                    dosageResult.Id = firstDosageInfo.Id;
                    dosageResult.ItemCd = firstDosageInfo.ItemCd;
                    dosageResult.YjCd = firstDosageInfo.YjCd;
                    dosageResult.CurrentValue = itemInfo.Suryo;
                    dosageResult.UnitName = itemInfo.UnitName;
                    dosageResult.ItemName = itemInfo.ItemName;
                }
                // Execute checking
                DosageResultModel? dosageResultModel = null;

                //Follow this document: https://wiki.sotatek.com/pages/viewpage.action?pageId=29130796
                bool isOnceLimitFoundIntoAllOfRecord = !listFilteredDosageInfo.Any(f => string.IsNullOrEmpty(f.OnceLimitUnit));
                bool isNotOnceLimitFoundIntoAnyRecord = !listFilteredDosageInfo.Any(f => !string.IsNullOrEmpty(f.OnceLimitUnit));
                bool isDayLimitFoundIntoAllOfRecord = !listFilteredDosageInfo.Any(f => string.IsNullOrEmpty(f.DayLimitUnit));
                bool isNotDayLimitFoundIntoAnyRecord = !listFilteredDosageInfo.Any(f => !string.IsNullOrEmpty(f.DayLimitUnit));

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

        public List<DayLimitResultModel> CheckDayLimit(int hpID, int sinday, List<ItemCodeModel> listAddedOrderCodes, double usingDay)
        {
            var listAddedOrderCodeItemCd = listAddedOrderCodes.Select(item => item.ItemCd).Distinct().ToList();
            var dayLimitInfoByUser =
               (
                   from drugMst in NoTrackingDataContext.TenMsts.Where(m => listAddedOrderCodeItemCd.Contains(m.ItemCd) && m.StartDate <= sinday && sinday <= m.EndDate).AsQueryable()
                   join dayLimit in NoTrackingDataContext.DrugDayLimits.Where(d => 0 < d.LimitDay &&
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

            var listItemCodeByUserSetting = dayLimitInfoByUser.Select(d => d.ItemCd).Distinct().ToList();
            var listRestedItemCode = listAddedOrderCodes.Where(c => !listItemCodeByUserSetting.Contains(c.ItemCd)).ToList();

            var listRestedItemCd = listRestedItemCode.Select(x => x.ItemCd).Distinct().ToList();
            var dayLimitInfo =
                (
                from drugMst in NoTrackingDataContext.TenMsts.Where(m => listRestedItemCd.Contains(m.ItemCd) && m.StartDate <= sinday && sinday <= m.EndDate).AsQueryable()
                join dayLimit in NoTrackingDataContext.M10DayLimit.Where(d => 0 < d.LimitDay && d.LimitDay < 999)
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

            List<DayLimitResultModel> result = new();

            foreach (var item in dayLimitInfoByUser)
            {
                if (usingDay <= item.LimitDay)
                {
                    continue;
                }
                var id = listAddedOrderCodes.First(id => id.ItemCd == item.ItemCd).Id;
                result.Add(new DayLimitResultModel()
                {
                    Id = id,
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
                var id = listAddedOrderCodes.First(id => id.ItemCd == item.ItemCd).Id;
                result.Add(new DayLimitResultModel()
                {
                    Id = id,
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
            double result;
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

        private double GetCommonHeight(int birdthDay, int sinday, int sex)
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

        private double GetPatientWeight(int hpId, long ptID, int birdthDay, int sinday, int sex, List<KensaInfDetailModel> kensaInfDetailModels, bool isDataOfDb)
        {
            if (isDataOfDb)
            {
                //Get data in db
                KensaInfDetail weightInfo = GetBodyInfo(hpId, ptID, sinday, "V0002");

                if (weightInfo != null && CIUtil.IsDigitsOnly(weightInfo?.ResultVal ?? string.Empty))
                {
                    return weightInfo?.ResultVal?.AsDouble() ?? 0;
                }
            }
            else
            {
                var weightInfo = kensaInfDetailModels
                          .Where(k => k.HpId == hpId && k.PtId == ptID && k.IraiDate <= sinday && k.KensaItemCd == "V002" && !string.IsNullOrEmpty(k.ResultVal))
                          .OrderByDescending(k => k.IraiDate).FirstOrDefault();

                if (weightInfo != null && CIUtil.IsDigitsOnly(weightInfo?.ResultVal ?? string.Empty))
                {
                    return weightInfo?.ResultVal?.AsDouble() ?? 0;
                }
            }

            return GetCommonWeight(hpId, ptID, birdthDay, sinday, sex);
        }

        private double GetPatientHeight(int hpId, long ptID, int birdthDay, int sinday, int sex, List<KensaInfDetailModel> kensaInfDetailModels)
        {
            var heightInfoModel = kensaInfDetailModels.Where(k => k.HpId == hpId && k.PtId == ptID && k.IraiDate <= sinday && k.KensaItemCd == "V0001" && !string.IsNullOrEmpty(k.ResultVal))
            .OrderByDescending(k => k.IraiDate).FirstOrDefault();

            if (heightInfoModel != null && CIUtil.IsDigitsOnly(heightInfoModel.ResultVal ?? string.Empty))
            {
                var value = heightInfoModel.ResultVal ?? string.Empty;
                return value.AsDouble();
            }

            //KensaInfDetail heightInfo = GetBodyInfo(hpId, ptID, sinday, "V0001");

            //if (heightInfo != null && heightInfo.HpId != 0 && heightInfo.PtId != 0 && heightInfo.SeqNo != 0 && heightInfo.IraiCd != 0 && CIUtil.IsDigitsOnly(heightInfo.ResultVal ?? string.Empty))
            //{
            //    var value = heightInfo.ResultVal ?? string.Empty;
            //    return value.AsDouble();
            //}

            return GetCommonHeight(birdthDay, sinday, sex);
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
