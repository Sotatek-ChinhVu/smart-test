using CommonCheckers.OrderRealtimeChecker.Models;
using Domain.Types;

namespace CommonCheckers.OrderRealtimeChecker.Services
{
    public class KinkiUserChecker<TOdrInf, TOdrDetail> : UnitChecker<TOdrInf, TOdrDetail>
        where TOdrInf : class, IOdrInfModel<TOdrDetail>
        where TOdrDetail : class, IOdrInfDetailModel
    {
        private readonly SystemConfig _systemConf;
        public KinkiUserChecker(SystemConfig systemConf)
        {
            _systemConf = systemConf;
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
            List<KinkiResultModel> checkedResult = Finder.CheckKinkiUser(HpID, settingLevel, Sinday, listItemCode, listItemCode);
            RemoveDuplicate(ref checkedResult);

            List<string> listDrugItemCode = GetAllOdrDetailCodeByOrderList(CurrentListOrder);
            checkedResult.AddRange(Finder.CheckKinkiUser(HpID, settingLevel, Sinday, listDrugItemCode, listItemCode));

            return unitCheckerResult;
        }

        private void RemoveDuplicate(ref List<KinkiResultModel> checkedResult)
        {
            checkedResult = checkedResult.Where(c => c.AYjCd != c.BYjCd).ToList();
            for (int i = 0; i < checkedResult.Count; i++)
            {
                var item = checkedResult[i];
                var listDuplicate = checkedResult
                    .Where(c => (item.AYjCd == c.BYjCd && item.AYjCd == c.BYjCd) ||
                                (item != c && item.AYjCd == c.AYjCd && item.BYjCd == c.BYjCd))
                    .ToList();
                for (int j = 0; j < listDuplicate.Count; j++)
                {
                    checkedResult.Remove(listDuplicate[j]);
                }
            }
        }

        private int GetSettingLevel()
        {
            return _systemConf.KinkiLevelSetting;
        }

        public override UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> HandleCheckOrderList(UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> unitCheckerForOrderListResult)
        {
            throw new NotImplementedException();
        }
    }
}
