using Domain.Models.AuditLog;
using Domain.Models.SetMst;
using Domain.Models.User;
using Helper.Constants;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using Interactor.SetMst.CommonSuperSet;
using UseCase.SetMst.SaveSetMst;

namespace Interactor.SetMst;

public class SaveSetMstInteractor : ISaveSetMstInputPort
{
    private readonly ISetMstRepository _setMstRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICommonSuperSet _commonSuperSet;
    private readonly IAuditLogRepository _auditLogRepository;
    private readonly ILoggingHandler _loggingHandler;
    private readonly ITenantProvider _tenantProvider;

    public SaveSetMstInteractor(ITenantProvider tenantProvider, ISetMstRepository setMstRepository, IUserRepository userRepository, ICommonSuperSet commonSuperSet, IAuditLogRepository auditLogRepository)
    {
        _setMstRepository = setMstRepository;
        _userRepository = userRepository;
        _commonSuperSet = commonSuperSet;
        _auditLogRepository = auditLogRepository;
        _tenantProvider = tenantProvider;
        _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
    }

    public SaveSetMstOutputData Handle(SaveSetMstInputData inputData)
    {
        var notAllowSave = _userRepository.NotAllowSaveMedicalExamination(inputData.HpId, inputData.PtId, inputData.RaiinNo, inputData.SinDate, inputData.UserId);
        if (notAllowSave)
        {
            return new SaveSetMstOutputData(new(), SaveSetMstStatus.MedicalScreenLocked);
        }
        if (inputData.SinDate <= 15000101 && inputData.SinDate > 30000000)
        {
            return new SaveSetMstOutputData(new(), SaveSetMstStatus.InvalidSindate);
        }
        else if (inputData.SetCd < 0)
        {
            return new SaveSetMstOutputData(new(), SaveSetMstStatus.InvalidSetCd);
        }
        else if (inputData.SetKbn < 1 && inputData.SetKbn > 10)
        {
            return new SaveSetMstOutputData(new(), SaveSetMstStatus.InvalidSetKbn);
        }
        else if (inputData.SetKbnEdaNo < 1 && inputData.SetKbnEdaNo > 6)
        {
            return new SaveSetMstOutputData(new(), SaveSetMstStatus.InvalidSetKbnEdaNo);
        }
        else if (inputData.SetName.Length > 60)
        {
            return new SaveSetMstOutputData(new(), SaveSetMstStatus.InvalidSetName);
        }
        else if (inputData.WeightKbn < 0)
        {
            return new SaveSetMstOutputData(new(), SaveSetMstStatus.InvalidWeightKbn);
        }
        else if (inputData.Color < 0)
        {
            return new SaveSetMstOutputData(new(), SaveSetMstStatus.InvalidColor);
        }
        else if (inputData.IsDeleted < 0 && inputData.IsDeleted > 1)
        {
            return new SaveSetMstOutputData(new(), SaveSetMstStatus.InvalidIsDeleted);
        }
        try
        {
            var setMstModel = new SetMstModel(
                                inputData.HpId,
                                inputData.SetCd,
                                inputData.SetKbn,
                                inputData.SetKbnEdaNo,
                                0,
                                0,
                                0,
                                0,
                                inputData.SetName,
                                inputData.WeightKbn,
                                inputData.Color,
                                inputData.IsDeleted,
                                inputData.IsGroup ? 1 : 0,
                                inputData.IsAddNew
                             );
            var resultData = _setMstRepository.SaveSetMstModel(inputData.UserId, inputData.SinDate, setMstModel);
            AddAuditTrailLog(inputData.HpId, inputData.UserId, setMstModel.GenerationId, setMstModel.SetKbn, setMstModel.SetKbnEdaNo);
            if (resultData != null)
            {
                var data = _commonSuperSet.BuildTreeSetKbn(resultData);
                return new SaveSetMstOutputData(data, SaveSetMstStatus.Successed);
            }
            return new SaveSetMstOutputData(new(), SaveSetMstStatus.Failed);
        }
        catch (Exception ex)
        {
            _loggingHandler.WriteLogExceptionAsync(ex);
            throw;
        }
        finally
        {
            _setMstRepository.ReleaseResource();
            _auditLogRepository.ReleaseResource();
            _userRepository.ReleaseResource();
            _loggingHandler.Dispose();
        }
    }

    private void AddAuditTrailLog(int hpId, int userId, int generationId, int setKbn, int setKbnEdaNo)
    {
        string hosoku = $"{generationId}-{setKbn}-{setKbnEdaNo}";
        var arg = new ArgumentModel(
            EventCode.SupetSetDetailChanged,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
        hosoku
        );

        _auditLogRepository.AddAuditTrailLog(hpId, userId, arg);
    }
}
