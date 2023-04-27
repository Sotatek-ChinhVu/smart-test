﻿using CommonChecker.Models;
using CommonChecker.Types;
using CommonCheckers.OrderRealtimeChecker.Models;

namespace CommonCheckers.OrderRealtimeChecker.Services
{
    public class DiseaseChecker<TOdrInf, TOdrDetail> : UnitChecker<TOdrInf, TOdrDetail>
        where TOdrInf : class, IOdrInfoModel<TOdrDetail>
        where TOdrDetail : class, IOdrInfoDetailModel
    {
        public override UnitCheckerResult<TOdrInf, TOdrDetail> HandleCheckOrder(UnitCheckerResult<TOdrInf, TOdrDetail> unitCheckerResult)
        {
            throw new NotImplementedException();
        }

        private int GetSettingLevel()
        {
            return SystemConfig!.DiseaseLevelSetting;
        }

        public override UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> HandleCheckOrderList(UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> unitCheckerForOrderListResult)
        {
            // Read setting from SystemConfig
            int settingLevel = GetSettingLevel();
            if (settingLevel <= 0 || settingLevel >= 4)
            {
                return unitCheckerForOrderListResult;
            }

            // Get listItemCode
            List<TOdrInf> checkingOrderList = unitCheckerForOrderListResult.CheckingOrderList;
            List<ItemCodeModel> listItemCode = GetAllOdrDetailCodeByOrderList(checkingOrderList);

            List<DiseaseResultModel> checkedResult = new List<DiseaseResultModel>();

            List<DiseaseResultModel> checkedResultForCurrentDisease = Finder!.CheckContraindicationForCurrentDisease(HpID, PtID, settingLevel, Sinday, listItemCode, unitCheckerForOrderListResult.PtDiseaseModels, unitCheckerForOrderListResult.IsDataOfDb);
            if (checkedResultForCurrentDisease != null)
            {
                checkedResult.AddRange(checkedResultForCurrentDisease);
            }

            List<DiseaseResultModel> checkedResultForHistoryDisease = Finder.CheckContraindicationForHistoryDisease(HpID, PtID, settingLevel, Sinday, listItemCode, unitCheckerForOrderListResult.SpecialNoteModel.ImportantNoteModel.KioRekiItems, unitCheckerForOrderListResult.IsDataOfDb);
            if (checkedResultForHistoryDisease != null)
            {
                checkedResult.AddRange(checkedResultForHistoryDisease);
            }

            List<DiseaseResultModel> checkedResultForFamilyDisease = Finder.CheckContraindicationForFamilyDisease(HpID, PtID, settingLevel, Sinday, listItemCode, unitCheckerForOrderListResult.FamilyModels, unitCheckerForOrderListResult.IsDataOfDb);
            if (checkedResultForFamilyDisease != null)
            {
                checkedResult.AddRange(checkedResultForFamilyDisease);
            }

            if (checkedResult.Count > 0)
            {
                unitCheckerForOrderListResult.ErrorInfo = checkedResult;
                unitCheckerForOrderListResult.ErrorOrderList = GetErrorOrderList(checkingOrderList, checkedResult);
            }

            return unitCheckerForOrderListResult;
        }

        private List<TOdrInf> GetErrorOrderList(List<TOdrInf> checkingOrderList, List<DiseaseResultModel> checkedResultList)
        {
            List<string> listErrorItemCode = checkedResultList.Select(r => r.ItemCd).ToList();

            List<TOdrInf> resultList = new List<TOdrInf>();
            foreach (var checkingOrder in checkingOrderList)
            {
                var existed = checkingOrder.OdrInfDetailModelsIgnoreEmpty.Any(o => listErrorItemCode.Contains(o.ItemCd));
                if (existed)
                {
                    resultList.Add(checkingOrder);
                }
            }

            return resultList;
        }
    }
}
