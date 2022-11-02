using CommonChecker.Types;
using CommonCheckers.OrderRealtimeChecker.Models;

namespace CommonCheckers.OrderRealtimeChecker.Services
{
    public class KinkiChecker<TOdrInf, TOdrDetail> : UnitChecker<TOdrInf, TOdrDetail>
        where TOdrInf : class, IOdrInfModel<TOdrDetail>
        where TOdrDetail : class, IOdrInfDetailModel
    {
        private readonly SystemConfig? _systemConf;
        public KinkiChecker(SystemConfig systemConf)
        {
            _systemConf = systemConf;
        }
        public KinkiChecker()
        {
        }

        public List<TOdrInf> CurrentListOrder = new();

        public override UnitCheckerResult<TOdrInf, TOdrDetail> HandleCheckOrder(UnitCheckerResult<TOdrInf, TOdrDetail> unitCheckerResult)
        {
            // Read setting from SystemConfig
            int settingLevel = GetSettingLevel();
            if (settingLevel <= 0 || settingLevel >= 5)
            {
                return unitCheckerResult;
            }

            // Get listItemCode
            TOdrInf checkingOrder = unitCheckerResult.CheckingData;
            List<string> listItemCode = GetAllOdrDetailCodeByOrder(checkingOrder);
            List<KinkiResultModel> checkedResult = Finder.CheckKinki(HpID, settingLevel, Sinday, listItemCode, listItemCode);
            RemoveDuplicate(ref checkedResult);

            List<string> listDrugItemCode = GetAllOdrDetailCodeByOrderList(CurrentListOrder);
            checkedResult.AddRange(Finder.CheckKinki(HpID, settingLevel, Sinday, listDrugItemCode, listItemCode));

            return unitCheckerResult;
        }

        private void RemoveDuplicate(ref List<KinkiResultModel> checkedResult)
        {
            checkedResult = checkedResult.Where(c => c.AYjCd != c.BYjCd).ToList();
            for (int i = 0; i < checkedResult.Count; i++)
            {
                var item = checkedResult[i];
                var listDuplicate = checkedResult.Where(c => item.AYjCd == c.BYjCd && item.AYjCd == c.BYjCd).ToList();
                for (int j = 0; j < listDuplicate.Count(); j++)
                {
                    checkedResult.Remove(listDuplicate[j]);
                }
            }
        }

        private int GetSettingLevel()
        {
            return _systemConf != null ? _systemConf.KinkiLevelSetting : 0;
        }

        public override UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> HandleCheckOrderList(UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> unitCheckerForOrderListResult)
        {
            throw new NotImplementedException();
        }
    }
}
