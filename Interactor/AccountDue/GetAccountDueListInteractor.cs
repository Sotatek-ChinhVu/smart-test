using Domain.Models.AccountDue;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using UseCase.AccountDue.GetAccountDueList;

namespace Interactor.AccountDue;

public class GetAccountDueListInteractor : IGetAccountDueListInputPort
{
    private readonly IAccountDueRepository _accountDueRepository;
    private readonly IReceptionRepository _receptionRepository;
    private readonly IPatientInforRepository _patientInforRepository;

    public GetAccountDueListInteractor(IAccountDueRepository accountDueRepository, IReceptionRepository receptionRepository, IPatientInforRepository patientInforRepository)
    {
        _accountDueRepository = accountDueRepository;
        _receptionRepository = receptionRepository;
        _patientInforRepository = patientInforRepository;
    }

    public GetAccountDueListOutputData Handle(GetAccountDueListInputData inputData)
    {
        try
        {
            if (inputData.HpId <= 0)
            {
                return new GetAccountDueListOutputData(GetAccountDueListStatus.InvalidHpId);
            }
            else if (!_patientInforRepository.CheckExistListId(new List<long>() { inputData.PtId }))
            {
                return new GetAccountDueListOutputData(GetAccountDueListStatus.InvalidPtId);
            }
            else if (inputData.SinDate.ToString().Length != 8)
            {
                return new GetAccountDueListOutputData(GetAccountDueListStatus.InvalidSindate);
            }
            var uketsukeSbt = _accountDueRepository.GetUketsukeSbt(inputData.HpId);
            var paymentMethod = _accountDueRepository.GetPaymentMethod(inputData.HpId);
            var listAccountDues = _accountDueRepository.GetAccountDueList(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.IsUnpaidChecked);
            var hokenPatternList = _receptionRepository.GetList(inputData.HpId, inputData.SinDate, -1, inputData.PtId, true);

            // Get HokenPattern List
            Dictionary<int, string> hokenPatternDict = new Dictionary<int, string>();
            foreach (var model in hokenPatternList)
            {
                if (!hokenPatternDict.ContainsKey(model.HokenPid))
                {
                    hokenPatternDict.Add(model.HokenPid, model.HokenPatternName);
                }
            }

            // Calculate Unpaid
            AccountDueModel tempModel = new();
            foreach (var model in listAccountDues)
            {
                var hokenPatternName = string.Empty;
                var unPaid = 0;
                var isSeikyuRow = true;
                if (hokenPatternDict.ContainsKey(model.HokenPid))
                {
                    hokenPatternName = hokenPatternDict[model.HokenPid];
                }
                if (model.NyukinKbn == 2 || model.NyukinKbn == 0)
                {
                    tempModel = model;
                    model.UpdateAccountDueListModel(unPaid, hokenPatternName, isSeikyuRow);
                    continue;
                }
                if (tempModel.RaiinNo == model.RaiinNo)
                {
                    unPaid = tempModel.UnPaid - model.NyukinGaku - model.AdjustFutan;
                    isSeikyuRow = false;
                }
                else
                {
                    unPaid = model.SeikyuGaku - model.NyukinGaku - model.AdjustFutan;
                }
                model.UpdateAccountDueListModel(unPaid, hokenPatternName, isSeikyuRow);
                tempModel = model;
            }

            var result = new AccountDueListModel(listAccountDues, paymentMethod, uketsukeSbt);
            return new GetAccountDueListOutputData(result, GetAccountDueListStatus.Successed);
        }
        finally
        {
            _accountDueRepository.ReleaseResource();
            _receptionRepository.ReleaseResource();
            _patientInforRepository.ReleaseResource();
        }
    }
}
