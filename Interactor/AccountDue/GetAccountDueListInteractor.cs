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
            else if (!_patientInforRepository.CheckListId(new List<long>() { inputData.PtId }))
            {
                return new GetAccountDueListOutputData(GetAccountDueListStatus.InvalidPtId);
            }
            else if (inputData.SinDate.ToString().Length != 8)
            {
                return new GetAccountDueListOutputData(GetAccountDueListStatus.InvalidSindate);
            }
            else if (inputData.PageIndex < 1)
            {
                return new GetAccountDueListOutputData(GetAccountDueListStatus.InvalidpageIndex);
            }
            else if (inputData.PageSize < 1)
            {
                return new GetAccountDueListOutputData(GetAccountDueListStatus.InvalidpageSize);
            }
            var uketsukeSbt = _accountDueRepository.GetUketsukeSbt(inputData.HpId);
            var paymentMethod = _accountDueRepository.GetPaymentMethod(inputData.HpId);
            var listAccountDues = _accountDueRepository.GetAccountDueList(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.IsUnpaidChecked, inputData.PageIndex, inputData.PageSize);
            var hokenPatternList = _receptionRepository.GetList(inputData.HpId, inputData.SinDate, -1, inputData.PtId);

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
            AccountDueListModel tempModel = new();
            foreach (AccountDueListModel model in listAccountDues)
            {
                if (hokenPatternDict.ContainsKey(model.HokenPid))
                {
                    model.HokenPatternName = hokenPatternDict[model.HokenPid];
                }
                if (model.NyukinKbn == 2 || model.NyukinKbn == 0)
                {
                    tempModel = model;
                    continue;
                }
                if (tempModel.RaiinNo == model.RaiinNo)
                {
                    model.UnPaid = tempModel.UnPaid - model.NyukinGaku - model.AdjustFutan;
                    model.IsSeikyuRow = false;
                }
                else
                {
                    model.UnPaid = model.SeikyuGaku - model.NyukinGaku - model.AdjustFutan;
                }
                tempModel = model;
            }


            var result = new AccountDueModel(listAccountDues, paymentMethod, uketsukeSbt);
            return new GetAccountDueListOutputData(result, GetAccountDueListStatus.Successed);
        }
        catch
        {
            return new GetAccountDueListOutputData(GetAccountDueListStatus.Failed);
        }
    }
}
