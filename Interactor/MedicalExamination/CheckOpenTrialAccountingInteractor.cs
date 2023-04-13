using Domain.Models.Accounting;
using UseCase.MedicalExamination.CheckOpenTrialAccounting;

namespace Interactor.MedicalExamination
{
    public class CheckOpenTrialAccountingInteractor : ICheckOpenTrialAccountingInputPort
    {
        private readonly IAccountingRepository _accountingRepository;

        public CheckOpenTrialAccountingInteractor(IAccountingRepository accountingRepository)
        {
            _accountingRepository = accountingRepository;
        }

        public CheckOpenTrialAccountingOutputData Handle(CheckOpenTrialAccountingInputData inputData)
        {
            try
            {

            }
            finally
            {
                _accountingRepository.ReleaseResource();
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

        private CheckOpenTrialAccountingStatus IsValidCheckDetail()
        {

        }
    }
}
