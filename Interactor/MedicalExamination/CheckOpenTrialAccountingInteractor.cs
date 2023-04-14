using Domain.Models.Accounting;
using Domain.Models.TodayOdr;
using UseCase.MedicalExamination.CheckOpenTrialAccounting;

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
                var checkJihiYobo = GetValidJihiYobo(inputData.HpId, inputData.SinDate, inputData.SyosaiKbn, inputData.AllOdrInfItem);
                var checkHokenPt = CheckHokenPatternSelect(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.OdrInfHokenPid);
                if (checkHokenPt != CheckOpenTrialAccountingStatus.Successed)
                {
                    return new CheckOpenTrialAccountingOutputData(0, string.Empty, 0, string.Empty, checkJihiYobo.systemSetting, checkJihiYobo.isExistYoboItemOnly, CheckOpenTrialAccountingStatus.InvalidHokenPattern);
                }

                var checkGaiRaiRiha = GetValidGairaiRiha(inputData.HpId, inputData.PtId, inputData.RaiinNo, inputData.SinDate, inputData.SyosaiKbn, inputData.AllOdrInfItem);
                if (checkGaiRaiRiha.status != CheckOpenTrialAccountingStatus.Successed)
                {
                    return new CheckOpenTrialAccountingOutputData(checkGaiRaiRiha.type, checkGaiRaiRiha.itemName, checkGaiRaiRiha.lastDaySanteiRiha, checkGaiRaiRiha.rihaItemName, checkJihiYobo.systemSetting, checkJihiYobo.isExistYoboItemOnly, CheckOpenTrialAccountingStatus.InvalidGaiRaiRiha);
                }

                return new CheckOpenTrialAccountingOutputData(0, string.Empty, 0, string.Empty, checkJihiYobo.systemSetting, checkJihiYobo.isExistYoboItemOnly, CheckOpenTrialAccountingStatus.Successed);
            }
            finally
            {
                _accountingRepository.ReleaseResource();
                _todayOdrRepository.ReleaseResource();
            }
        }

        private CheckOpenTrialAccountingStatus CheckHokenPatternSelect(int hpId, int ptId, int sinDate, List<int> odrInfHokenPid)
        {
            var listHokenPattern = _accountingRepository.FindPtHokenPatternList(hpId, ptId, sinDate);
            var listHokenPids = listHokenPattern.Select(u => u.HokenPid).ToList();
            var hokenPidInValid = odrInfHokenPid.FirstOrDefault(i => !listHokenPids.Any(h => h == i));
            if (!listHokenPids.Any() || hokenPidInValid > 0)
            {
                return CheckOpenTrialAccountingStatus.InvalidHokenPattern;
            }

            return CheckOpenTrialAccountingStatus.Successed;
        }

        private (CheckOpenTrialAccountingStatus status, int type, string itemName, int lastDaySanteiRiha, string rihaItemName) GetValidGairaiRiha(int hpId, int ptId, long raiinNo, int sinDate, int syosaiKbn, List<Tuple<string, string>> allOdrInfItem)
        {
            var check = _todayOdrRepository.GetValidGairaiRiha(hpId, ptId, raiinNo, sinDate, syosaiKbn, allOdrInfItem);
            if (check.type == 0)
            {
                return (CheckOpenTrialAccountingStatus.Successed, check.type, check.itemName, check.lastDaySanteiRiha, check.rihaItemName);
            }
            return (CheckOpenTrialAccountingStatus.InvalidGaiRaiRiha, check.type, check.itemName, check.lastDaySanteiRiha, check.rihaItemName);
        }

        private (double systemSetting, bool isExistYoboItemOnly) GetValidJihiYobo(int hpId, int sinDate, int syosaiKbn, List<Tuple<string, string>> allOdrInfItem)
        {
            var check = _todayOdrRepository.GetValidJihiYobo(hpId, sinDate, syosaiKbn, allOdrInfItem.Select(x => x.Item1).Distinct().ToList());

            return (check.systemSetting, check.isExistYoboItemOnly);
        }
    }
}
