using Domain.Models.Accounting;
using Domain.Models.AuditLog;
using Domain.Models.HpInf;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using Domain.Models.SystemConf;
using Domain.Models.User;
using Helper.Constants;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.Accounting.SaveAccounting;
using static Helper.Constants.UserConst;

namespace Interactor.Accounting
{
    public class SaveAccountingInteractor : ISaveAccountingInputPort
    {
        private readonly IAccountingRepository _accountingRepository;
        private readonly ISystemConfRepository _systemConfRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHpInfRepository _hpInfRepository;
        private readonly IPatientInforRepository _patientInforRepository;
        private readonly IReceptionRepository _receptionRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;

        public SaveAccountingInteractor(ITenantProvider tenantProvider, IAccountingRepository accountingRepository, ISystemConfRepository systemConfRepository, IUserRepository userRepository, IHpInfRepository hpInfRepository, IPatientInforRepository patientInforRepository, IReceptionRepository receptionRepository, IAuditLogRepository auditLogRepository)
        {
            _accountingRepository = accountingRepository;
            _systemConfRepository = systemConfRepository;
            _userRepository = userRepository;
            _hpInfRepository = hpInfRepository;
            _patientInforRepository = patientInforRepository;
            _receptionRepository = receptionRepository;
            _auditLogRepository = auditLogRepository;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public SaveAccountingOutputData Handle(SaveAccountingInputData inputData)
        {
            try
            {
                var validateResult = ValidateInputData(inputData);
                if (validateResult != SaveAccountingStatus.ValidateSuccess) return new SaveAccountingOutputData(validateResult, new(), new());

                var raiinInfList = _accountingRepository.GetListRaiinInf(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.RaiinNo);

                var raiinNoList = raiinInfList.Select(r => r.RaiinNo).ToList();

                var listSyunoSeikyu = _accountingRepository.GetListSyunoSeikyu(inputData.HpId, inputData.PtId, inputData.SinDate, raiinNoList);

                var syunoSeikyu = listSyunoSeikyu.FirstOrDefault(x => x.RaiinNo == inputData.RaiinNo);

                if (syunoSeikyu == null)
                {
                    return new SaveAccountingOutputData(SaveAccountingStatus.InputDataNull, new(), new());
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
                var accDue = (int)_systemConfRepository.GetSettingValue(3020, 0, inputData.HpId);

                if (accDue == 0)
                {
                    accDue = debitBalance;
                }

                var save = _accountingRepository.SaveAccounting(listAllSyunoSeikyu, listSyunoSeikyu, inputData.HpId, inputData.PtId, inputData.UserId, accDue, inputData.SumAdjust, inputData.ThisWari, inputData.Credit,
                                                                inputData.PayType, inputData.Comment, inputData.IsDisCharged, inputData.KaikeiTime);
                if (save)
                {
                    AddAuditTrailLog(inputData.HpId, inputData.UserId, inputData.PtId, inputData.SinDate, inputData.RaiinNo, accDue, inputData.SinDate, inputData.Credit, inputData.IsDisCharged);

                    var receptionInfos = _receptionRepository.GetList(inputData.HpId, inputData.SinDate, CommonConstants.InvalidId, inputData.PtId, isDeleted: 0);
                    var sameVisitList = _receptionRepository.GetListSameVisit(inputData.HpId, inputData.PtId, inputData.SinDate);
                    return new SaveAccountingOutputData(SaveAccountingStatus.Success, receptionInfos, sameVisitList);
                }
                return new SaveAccountingOutputData(SaveAccountingStatus.Failed, new(), new());

            }
            catch (Exception ex)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
            finally
            {
                _accountingRepository.ReleaseResource();
                _systemConfRepository.ReleaseResource();
                _userRepository.ReleaseResource();
                _hpInfRepository.ReleaseResource();
                _patientInforRepository.ReleaseResource();
                _receptionRepository.ReleaseResource();
                _auditLogRepository.ReleaseResource();
                _tenantProvider.DisposeDataContext();
                _loggingHandler.Dispose();
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
            else if (_userRepository.GetPermissionByScreenCode(inputData.HpId, inputData.UserId, FunctionCode.Accounting) != PermissionType.Unlimited)
            {
                return SaveAccountingStatus.NoPermission;
            }
            return SaveAccountingStatus.ValidateSuccess;
        }

        #region AddAuditTrailLog
        private void AddAuditTrailLog(int hpId, int userId, long ptId, int sinDate, long raiinNo, int misyu, int nyukinDate, int nyukin, bool isDisCharged)
        {
            if (isDisCharged)
            {
                var arg = new ArgumentModel(
                                EventCode.DisCharged,
                                ptId,
                                sinDate,
                                raiinNo,
                                misyu,
                                nyukinDate,
                                0,
                                0,
                                string.Empty
                );

                _auditLogRepository.AddAuditTrailLog(hpId, userId, arg);
            }
            else
            {
                var arg = new ArgumentModel(
                                EventCode.AccountingExecute,
                                ptId,
                                sinDate,
                                raiinNo,
                                misyu,
                                nyukinDate,
                                nyukin,
                                1,
                                string.Empty
                );

                _auditLogRepository.AddAuditTrailLog(hpId, userId, arg);
            }
        }
        #endregion
    }
}
