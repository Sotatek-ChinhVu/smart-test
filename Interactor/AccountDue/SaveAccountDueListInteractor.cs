using Domain.Models.AccountDue;
using Domain.Models.AuditTrailLog;
using Domain.Models.HpMst;
using Domain.Models.PatientInfor;
using Domain.Models.User;
using Helper.Constants;
using UseCase.AccountDue.SaveAccountDueList;

namespace Interactor.AccountDue;

public class SaveAccountDueListInteractor : ISaveAccountDueListInputPort
{
    private readonly IAccountDueRepository _accountDueRepository;
    private readonly IUserRepository _userRepository;
    private readonly IHpInfRepository _hpInfRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IAuditTrailLogRepository _auditTrailLogRepositoty;

    public SaveAccountDueListInteractor(IAccountDueRepository accountDueRepository, IUserRepository userRepository, IHpInfRepository hpInfRepository, IPatientInforRepository patientInforRepository, IAuditTrailLogRepository auditTrailLogRepository)
    {
        _accountDueRepository = accountDueRepository;
        _userRepository = userRepository;
        _hpInfRepository = hpInfRepository;
        _patientInforRepository = patientInforRepository;
        _auditTrailLogRepositoty = auditTrailLogRepository;
    }

    public SaveAccountDueListOutputData Handle(SaveAccountDueListInputData inputData)
    {
        var validateResult = ValidateInputData(inputData);
        if (validateResult != SaveAccountDueListStatus.ValidateSuccess)
        {
            return new SaveAccountDueListOutputData(validateResult);
        }
        var listAccountDueModel = ConvertToListAccountDueModel(inputData.SyunoNyukinInputItems.Where(item => item.IsUpdated).ToList());
        var listRaiinNo = listAccountDueModel.Select(item => item.RaiinNo).ToList();
        var listSyunoNyukinDB = _accountDueRepository.GetListSyunoNyukinViewModel(listRaiinNo);
        List<AuditTraiLogModel> listTraiLogModels = new();

        if (!listAccountDueModel.Any())
        {
            return new SaveAccountDueListOutputData(SaveAccountDueListStatus.NoItemChange);
        }
        // validate PaymentMethodCd
        foreach (var accountDue in listAccountDueModel)
        {
            var accountDueByRaiins = listAccountDueModel.Where(item => item.RaiinNo == accountDue.RaiinNo).ToList();
            var seikyuGaku = accountDueByRaiins.Sum(item => (item.SeikyuGaku + item.AdjustFutan));
            var seikyuAdjustFutan = accountDueByRaiins.Sum(item => item.SeikyuAdjustFutan);
            var unPaid = seikyuGaku - seikyuAdjustFutan - accountDue.NyukinGaku - accountDue.AdjustFutan;
            if (unPaid == 0 && (accountDue.PaymentMethodCd != 2 || accountDue.PaymentMethodCd != 3))
            {

            }

            if (listSyunoNyukinDB.Any(item => accountDue.RaiinNo == item.RaiinNo && accountDue.PaymentMethodCd != item.PaymentMethodCd))
            {
                int tempStatus = accountDue.NyukinKbn == 0 ? RaiinState.Waiting : RaiinState.Settled;
                var eventCd = string.Empty;
                var hosoku = string.Empty;

                if (tempStatus != accountDue.RaiinInfStatus)
                {
                    if (tempStatus == RaiinState.Waiting)
                    {
                        eventCd = EventCode.UpdateToWaiting;
                    }
                    else
                    {
                        eventCd = EventCode.UpdateToSettled;
                    }
                }

                listTraiLogModels.Add(new AuditTraiLogModel(
                        inputData.HpId,
                        inputData.UserId,
                        eventCd,
                        accountDue.PtId,
                        inputData.SinDate,
                        accountDue.RaiinNo,
                        hosoku
                    ));
            }
        }
        var result = _accountDueRepository.SaveAccountDueList(
                                                inputData.HpId,
                                                inputData.PtId,
                                                inputData.UserId,
                                                inputData.SinDate,
                                                listAccountDueModel
                                            );
        if (result)
        {
            _auditTrailLogRepositoty.AddListAuditTrailLog(listTraiLogModels);
            return new SaveAccountDueListOutputData(SaveAccountDueListStatus.Successed);
        }
        return new SaveAccountDueListOutputData(SaveAccountDueListStatus.Failed);
    }

    private SaveAccountDueListStatus ValidateInputData(SaveAccountDueListInputData inputData)
    {
        if (inputData.HpId <= 0 || !_hpInfRepository.CheckHpId(inputData.HpId))
        {
            return SaveAccountDueListStatus.InvalidHpId;
        }
        else if (inputData.UserId <= 0 || !_userRepository.CheckExistedUserId(inputData.UserId))
        {
            return SaveAccountDueListStatus.InvalidUserId;
        }
        else if (inputData.PtId <= 0 || !_patientInforRepository.CheckListId(new List<long> { inputData.PtId }))
        {
            return SaveAccountDueListStatus.InvalidUserId;
        }
        else if (inputData.SinDate.ToString().Length != 8)
        {
            return SaveAccountDueListStatus.InvalidSindate;
        }
        else if (inputData.SyunoNyukinInputItems.Any(item => item.NyukinKbn < 0))
        {
            return SaveAccountDueListStatus.InvalidNyukinKbn;
        }
        else if (inputData.SyunoNyukinInputItems.Any(item => item.RaiinNo <= 0))
        {
            return SaveAccountDueListStatus.InvalidRaiinNo;
        }
        else if (inputData.SyunoNyukinInputItems.Any(item => item.SortNo < 0))
        {
            return SaveAccountDueListStatus.InvalidSortNo;
        }
        else if (inputData.SyunoNyukinInputItems.Any(item => item.AdjustFutan < 0))
        {
            return SaveAccountDueListStatus.InvalidAdjustFutan;
        }
        else if (inputData.SyunoNyukinInputItems.Any(item => item.NyukinGaku < 0))
        {
            return SaveAccountDueListStatus.InvalidNyukinGaku;
        }
        else if (inputData.SyunoNyukinInputItems.Any(item => item.PaymentMethodCd < 0))
        {
            return SaveAccountDueListStatus.InvalidPaymentMethodCd;
        }
        else if (inputData.SyunoNyukinInputItems.Any(item => item.NyukinDate < 0))
        {
            return SaveAccountDueListStatus.InvalidNyukinDate;
        }
        else if (inputData.SyunoNyukinInputItems.Any(item => item.UketukeSbt < 0))
        {
            return SaveAccountDueListStatus.InvalidUketukeSbt;
        }
        else if (inputData.SyunoNyukinInputItems.Any(item => item.NyukinCmt.Length > 100))
        {
            return SaveAccountDueListStatus.NyukinCmtMaxLength100;
        }
        else if (inputData.SyunoNyukinInputItems.Any(item => item.SeikyuGaku < 0))
        {
            return SaveAccountDueListStatus.InvalidSeikyuGaku;
        }
        else if (inputData.SyunoNyukinInputItems.Any(item => item.SeikyuTensu < 0))
        {
            return SaveAccountDueListStatus.InvalidSeikyuTensu;
        }
        else if (inputData.SyunoNyukinInputItems.Any(item => item.SeqNo <= 0))
        {
            return SaveAccountDueListStatus.InvalidSeqNo;
        }
        return SaveAccountDueListStatus.ValidateSuccess;
    }

    private List<AccountDueModel> ConvertToListAccountDueModel(List<SyunoNyukinInputItem> syunoNyukinInputItems)
    {
        var accountDueModels = syunoNyukinInputItems
                                        .Select(item => new AccountDueModel(
                                            item.NyukinKbn,
                                            item.SortNo,
                                            item.RaiinNo,
                                            item.AdjustFutan,
                                            item.NyukinGaku,
                                            item.PaymentMethodCd,
                                            item.NyukinDate,
                                            item.UketukeSbt,
                                            item.NyukinCmt,
                                            item.SeikyuGaku,
                                            item.SeikyuTensu,
                                            item.SeikyuDetail,
                                            item.SeqNo,
                                            item.RaiinInfStatus
                                        )).ToList();
        return accountDueModels;
    }
}
