using Domain.Models.Accounting;
using Domain.Models.TodayOdr;
using UseCase.MedicalExamination.CheckOpenTrialAccounting;
using UseCase.MedicalExamination.GetValidGairaiRiha;

namespace Interactor.MedicalExamination
{
    public class CheckOpenTrialAccountingInteractor : ICheckOpenTrialAccountingInputPort
    {
        private readonly IAccountingRepository _accountingRepository;
        private readonly ITodayOdrRepository _todayOdrRepository;

        public CheckOpenTrialAccountingInteractor(IAccountingRepository accountingRepository, ITodayOdrRepository todayOdrRepository)
        {
            _accountingRepository = accountingRepository;
            _todayOdrRepository = todayOdrRepository;
        }

        public CheckOpenTrialAccountingOutputData Handle(CheckOpenTrialAccountingInputData inputData)
        {
            try
            {
                var isHokenPtSelect = CheckHokenPatternSelect(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.OdrInfHokenPid);
                var checkJihiYobo = GetValidJihiYobo(inputData.HpId, inputData.SinDate, inputData.SyosaiKbn, inputData.AllOdrInfItem);
                var checkGaiRaiRiha = GetValidGairaiRiha(inputData.HpId, inputData.PtId, inputData.RaiinNo, inputData.SinDate, inputData.SyosaiKbn, inputData.AllOdrInfItem);
                return new CheckOpenTrialAccountingOutputData(isHokenPtSelect, checkGaiRaiRiha.Select(c => new GairaiRihaItem(c.type, c.itemName, c.lastDaySanteiRiha, c.rihaItemName)).ToList(), checkJihiYobo.systemSetting,
                                                              checkJihiYobo.isExistYoboItemOnly, CheckOpenTrialAccountingStatus.Successed);
            }
            finally
            {
                _accountingRepository.ReleaseResource();
                _todayOdrRepository.ReleaseResource();
            }
        }

        private bool CheckHokenPatternSelect(int hpId, long ptId, int sinDate, List<int> odrInfHokenPid)
        {
            var listHokenPattern = _accountingRepository.FindPtHokenPatternList(hpId, ptId, sinDate);
            var listHokenPids = listHokenPattern.Select(u => u.HokenPid).ToList();
            var hokenPidInValid = odrInfHokenPid.FirstOrDefault(i => !listHokenPids.Any(h => h == i));
            if (!listHokenPids.Any() || hokenPidInValid > 0) return false;

            return true;
        }

        private List<(int type, string itemName, int lastDaySanteiRiha, string rihaItemName)> GetValidGairaiRiha(int hpId, long ptId, long raiinNo, int sinDate, int syosaiKbn, List<Tuple<string, string>> allOdrInfItem)
        {
            var check = _todayOdrRepository.GetValidGairaiRiha(hpId, ptId, raiinNo, sinDate, syosaiKbn, allOdrInfItem);
            return check;
        }

        private (double systemSetting, bool isExistYoboItemOnly) GetValidJihiYobo(int hpId, int sinDate, int syosaiKbn, List<Tuple<string, string>> allOdrInfItem)
        {
            var check = _todayOdrRepository.GetValidJihiYobo(hpId, sinDate, syosaiKbn, allOdrInfItem.Select(x => x.Item1).Distinct().ToList());

            return (check.systemSetting, check.isExistYoboItemOnly);
        }
    }
}
