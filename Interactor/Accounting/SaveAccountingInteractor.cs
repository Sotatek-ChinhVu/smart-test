using Domain.Models.Accounting;
using Domain.Models.HpInf;
using Domain.Models.PatientInfor;
using Domain.Models.SystemConf;
using Domain.Models.User;
using UseCase.Accounting.SaveAccounting;

namespace Interactor.Accounting
{
    public class SaveAccountingInteractor : ISaveAccountingInputPort
    {
        private readonly IAccountingRepository _accountingRepository;
        private readonly ISystemConfRepository _systemConfRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHpInfRepository _hpInfRepository;
        private readonly IPatientInforRepository _patientInforRepository;

        public SaveAccountingInteractor(IAccountingRepository accountingRepository, ISystemConfRepository systemConfRepository, IUserRepository userRepository, IHpInfRepository hpInfRepository, IPatientInforRepository patientInforRepository)
        {
            _accountingRepository = accountingRepository;
            _systemConfRepository = systemConfRepository;
            _userRepository = userRepository;
            _hpInfRepository = hpInfRepository;
            _patientInforRepository = patientInforRepository;
        }

        public SaveAccountingOutputData Handle(SaveAccountingInputData inputData)
        {
            try
            {
                var validateResult = ValidateInputData(inputData);
                if (validateResult != SaveAccountingStatus.ValidateSuccess) return new SaveAccountingOutputData(validateResult);

                var raiinInfList = _accountingRepository.GetListRaiinInf(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.RaiinNo);

                var raiinNoList = raiinInfList.Select(r => r.RaiinNo).ToList();

                var listSyunoSeikyu = _accountingRepository.GetListSyunoSeikyu(inputData.HpId, inputData.PtId, inputData.SinDate, raiinNoList);

                var syunoSeikyu = listSyunoSeikyu.FirstOrDefault(x => x.RaiinNo == inputData.RaiinNo);

                if (syunoSeikyu == null)
                {
                    return new SaveAccountingOutputData(SaveAccountingStatus.InputDataNull);
                }
                else if (syunoSeikyu.NyukinKbn == 0)
                {
                    listSyunoSeikyu = listSyunoSeikyu.Where(item => item.NyukinKbn == 0).ToList();
                }
                else
                {
                    listSyunoSeikyu = listSyunoSeikyu.Where(item => item.NyukinKbn != 0).ToList();
                }

                var listAllSyunoSeikyu = _accountingRepository.GetListSyunoSeikyu(inputData.HpId, inputData.PtId, inputData.SinDate, raiinNoList, true);

                var debitBalance = listAllSyunoSeikyu.Sum(item => item.SeikyuGaku -
                                                  item.SyunoNyukinModels.Sum(itemNyukin =>
                                                      itemNyukin.NyukinGaku + itemNyukin.AdjustFutan));
                var accDue = (int)_systemConfRepository.GetSettingValue(3020, 0, 0);

                if (accDue == 0)
                {
                    accDue = debitBalance;
                }

                var save = _accountingRepository.SaveAccounting(listAllSyunoSeikyu, listSyunoSeikyu, inputData.HpId, inputData.PtId, inputData.UserId, accDue, inputData.SumAdjust, inputData.ThisWari, inputData.Credit,
                                                                inputData.PayType, inputData.Comment, inputData.IsDisCharged);
                if (save)
                {
                    return new SaveAccountingOutputData(SaveAccountingStatus.Success);
                }
                else
                {
                    return new SaveAccountingOutputData(SaveAccountingStatus.Failed);
                }
            }
            finally
            {
                _accountingRepository.ReleaseResource();
                _systemConfRepository.ReleaseResource();
                _userRepository.ReleaseResource();
                _hpInfRepository.ReleaseResource();
                _patientInforRepository.ReleaseResource();
            }
        }

        public SaveAccountingStatus ValidateInputData(SaveAccountingInputData inputData)
        {
            if (inputData.HpId <= 0 || !_hpInfRepository.CheckHpId(inputData.HpId))
            {
                return SaveAccountingStatus.InvalidHpId;
            }
            else if (inputData.UserId <= 0 || !_userRepository.CheckExistedUserId(inputData.UserId))
            {
                return SaveAccountingStatus.InvalidUserId;
            }
            else if (inputData.PtId <= 0 || !_patientInforRepository.CheckExistIdList(new List<long> { inputData.PtId }))
            {
                return SaveAccountingStatus.InvalidPtId;
            }
            else if (inputData.SumAdjust < 0)
            {
                return SaveAccountingStatus.InvalidSumAdjust;
            }
            else if (inputData.ThisWari < 0)
            {
                return SaveAccountingStatus.InvalidThisWari;
            }
            else if (inputData.Credit < 0)
            {
                return SaveAccountingStatus.InvalidCredit;
            }
            else if (inputData.PayType < 0)
            {
                return SaveAccountingStatus.InvalidPayType;
            }
            else if (inputData.Comment.Length > 100)
            {
                return SaveAccountingStatus.InvalidComment;
            }
            else if (inputData.SinDate.ToString().Length != 8)
            {
                return SaveAccountingStatus.InvalidSindate;
            }
            else if (inputData.RaiinNo <= 0)
            {
                return SaveAccountingStatus.InvalidRaiinNo;
            }

            return SaveAccountingStatus.ValidateSuccess;
        }
    }
}
