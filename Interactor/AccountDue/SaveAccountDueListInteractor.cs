using Domain.Models.AccountDue;
using Domain.Models.HpInf;
using Domain.Models.PatientInfor;
using Domain.Models.User;
using EventProcessor.Interfaces;
using EventProcessor.Model;
using Helper.Constants;
using UseCase.AccountDue.SaveAccountDueList;

namespace Interactor.AccountDue;

public class SaveAccountDueListInteractor : ISaveAccountDueListInputPort
{
    private readonly IAccountDueRepository _accountDueRepository;
    private readonly IUserRepository _userRepository;
    private readonly IHpInfRepository _hpInfRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IEventProcessorService _eventProcessorService;

    public SaveAccountDueListInteractor(IAccountDueRepository accountDueRepository, IUserRepository userRepository, IHpInfRepository hpInfRepository, IPatientInforRepository patientInforRepository, IEventProcessorService eventProcessorService)
    {
        _accountDueRepository = accountDueRepository;
        _userRepository = userRepository;
        _hpInfRepository = hpInfRepository;
        _patientInforRepository = patientInforRepository;
        _eventProcessorService = eventProcessorService;
    }

    public SaveAccountDueListOutputData Handle(SaveAccountDueListInputData inputData)
    {
        try
        {
            var validateResult = ValidateInputData(inputData);
            if (validateResult != SaveAccountDueListStatus.ValidateSuccess)
            {
                return new SaveAccountDueListOutputData(validateResult);
            }
            var listAccountDueModel = ConvertToListAccountDueModel(inputData.SyunoNyukinInputItems.Where(item => item.IsUpdated).ToList());
            var listRaiinNo = listAccountDueModel.Select(item => item.RaiinNo).ToList();
            var listSyunoSeikyuDB = _accountDueRepository.GetListSyunoSeikyuModel(listRaiinNo);
            var listSyunoNyukinDB = _accountDueRepository.GetListSyunoNyukinModel(listRaiinNo);
            List<ArgumentModel> listTraiLogModels = new();

            if (!listAccountDueModel.Any())
            {
                return new SaveAccountDueListOutputData(SaveAccountDueListStatus.NoItemChange);
            }
            // validate PaymentMethodCd
            var listSeqNos = listAccountDueModel.Select(item => item.SeqNo).ToList();
            foreach (var accountDue in listAccountDueModel)
            {
                var validateInvalidNyukinKbnResult = ValidateInvalidNyukinKbn(accountDue, listSeqNos, listSyunoSeikyuDB, listSyunoNyukinDB, listAccountDueModel);
                if (validateInvalidNyukinKbnResult != SaveAccountDueListStatus.ValidateSuccess)
                {
                    return new SaveAccountDueListOutputData(validateInvalidNyukinKbnResult);
                }
                listTraiLogModels = CreateListAuditTrailLogModel(inputData, accountDue, listSyunoSeikyuDB, listTraiLogModels);
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
                _eventProcessorService.DoEvent(listTraiLogModels);
                return new SaveAccountDueListOutputData(SaveAccountDueListStatus.Successed);
            }
        }
        finally
        {
            _accountDueRepository.ReleaseResource();
            _userRepository.ReleaseResource();
            _patientInforRepository.ReleaseResource();
            _hpInfRepository.ReleaseResource();
        }
        return new SaveAccountDueListOutputData(SaveAccountDueListStatus.Failed);
    }

    private SaveAccountDueListStatus ValidateInvalidNyukinKbn(AccountDueModel accountDue, List<long> listSeqNos, List<SyunoSeikyuModel> listSyunoSeikyuDB, List<SyunoNyukinModel> listSyunoNyukinDB, List<AccountDueModel> listAccountDueModel)
    {
        var accountDueByRaiinNo = listAccountDueModel.Where(item => item.RaiinNo == accountDue.RaiinNo && !item.IsDelete);
        var seikyuAdjustFutanListInput = accountDueByRaiinNo.Select(item => item.SeikyuAdjustFutan).Distinct().ToList();
        var seikyuGakuListInput = accountDueByRaiinNo.Select(item => item.SeikyuGaku).Distinct().ToList();
        if (seikyuAdjustFutanListInput.Count != 1 && !accountDue.IsDelete)
        {
            return SaveAccountDueListStatus.InvalidSeikyuAdjustFutan;
        }
        if (seikyuGakuListInput.Count != 1 && !accountDue.IsDelete)
        {
            return SaveAccountDueListStatus.InvalidSeikyuGaku;
        }

        int sumNyukinGakuInput = accountDueByRaiinNo.Sum(item => item.NyukinGaku);
        int sumAdjustFutanInput = accountDueByRaiinNo.Sum(item => item.AdjustFutan);
        var syunoSeikyuRaiins = listSyunoSeikyuDB.Where(item => item.RaiinNo == accountDue.RaiinNo).ToList();
        int nyukinGakuDB = listSyunoNyukinDB.Where(item => !listSeqNos.Contains(item.SeqNo) && item.RaiinNo == accountDue.RaiinNo).Sum(item => item.NyukinGaku);
        int adjustFutanDB = listSyunoNyukinDB.Where(item => !listSeqNos.Contains(item.SeqNo) && item.RaiinNo == accountDue.RaiinNo).Sum(item => item.AdjustFutan);
        int seikyuGakuDB = syunoSeikyuRaiins.FirstOrDefault()?.SeikyuGaku ?? 0;
        int newSeikyuGakuDB = syunoSeikyuRaiins.FirstOrDefault()?.NewSeikyuGaku ?? 0;
        int seikyuGakuInput = seikyuGakuListInput.FirstOrDefault();

        if (seikyuGakuInput != seikyuGakuDB && newSeikyuGakuDB == seikyuGakuInput)
        {
            seikyuGakuDB = seikyuGakuInput;
        }

        int unPaid = seikyuGakuDB - nyukinGakuDB - adjustFutanDB - sumAdjustFutanInput - sumNyukinGakuInput;
        if (accountDue.NyukinKbn == 1 && unPaid == 0)
        {
            return SaveAccountDueListStatus.InvalidNyukinKbn;
        }
        else if (accountDue.NyukinKbn == 2
                && (unPaid != (seikyuGakuDB - nyukinGakuDB - adjustFutanDB)
                    || listSyunoNyukinDB.Count(item => item.RaiinNo == accountDue.RaiinNo) - accountDueByRaiinNo.Count() > 1
                    || sumNyukinGakuInput != 0
                    || sumAdjustFutanInput != 0))
        {
            return SaveAccountDueListStatus.InvalidNyukinKbn;
        }
        else if (accountDue.NyukinKbn == 3 && (unPaid != 0))
        {
            return SaveAccountDueListStatus.InvalidNyukinKbn;
        }
        return SaveAccountDueListStatus.ValidateSuccess;
    }

    private List<ArgumentModel> CreateListAuditTrailLogModel(SaveAccountDueListInputData inputData, AccountDueModel accountDue, List<SyunoSeikyuModel> listSyunoNyukinDB, List<ArgumentModel> listTraiLogModels)
    {
        if (listSyunoNyukinDB.Any(item => accountDue.RaiinNo == item.RaiinNo && accountDue.NyukinKbn != item.NyukinKbn))
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

                listTraiLogModels.Add(new ArgumentModel(
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

        return listTraiLogModels;
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
        else if (inputData.PtId <= 0 || !_patientInforRepository.CheckExistIdList(new List<long> { inputData.PtId }))
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
        else if (inputData.SyunoNyukinInputItems.Any(item => item.PaymentMethodCd < 0))
        {
            return SaveAccountDueListStatus.InvalidPaymentMethodCd;
        }
        else if (inputData.SyunoNyukinInputItems.Any(item => item.NyukinDate < 0))
        {
            return SaveAccountDueListStatus.InvalidNyukinDate;
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
        else if (inputData.SyunoNyukinInputItems.Any(item => item.SeqNo < 0))
        {
            return SaveAccountDueListStatus.InvalidSeqNo;
        }
        // validate same value
        var updatedItem = inputData.SyunoNyukinInputItems;
        var countNyukinKbn = updatedItem.Select(item => new
        {
            item.RaiinNo,
            item.NyukinKbn
        }).Distinct().Count();
        var countRaiinNo = updatedItem.Select(item => item.RaiinNo).Distinct().Count();
        if (countRaiinNo != countNyukinKbn)
        {
            return SaveAccountDueListStatus.InvalidNyukinKbn;
        }
        var countSeikyuGaku = updatedItem.Select(item => new
        {
            item.RaiinNo,
            item.SeikyuGaku
        }).Distinct().Count();
        if (countRaiinNo != countSeikyuGaku)
        {
            return SaveAccountDueListStatus.InvalidSeikyuGaku;
        }
        var countSeikyuAdjustFutan = updatedItem.Select(item => new
        {
            item.RaiinNo,
            item.SeikyuAdjustFutan
        }).Distinct().Count();
        if (countRaiinNo != countSeikyuAdjustFutan)
        {
            return SaveAccountDueListStatus.InvalidSeikyuAdjustFutan;
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
                                            item.RaiinInfStatus,
                                            item.SeikyuAdjustFutan,
                                            item.SeikyuSinDate,
                                            item.IsDelete
                                        )).ToList();
        return accountDueModels;
    }
}
