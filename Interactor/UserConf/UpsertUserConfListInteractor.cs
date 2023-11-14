using Domain.Models.UserConf;
using Helper.Constants;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.User.UpsertUserConfList;

namespace Interactor.UserConf;

public class UpsertUserConfListInteractor : IUpsertUserConfListInputPort
{
    private readonly IUserConfRepository _userConfRepository;
    private readonly ILoggingHandler _loggingHandler;

    public UpsertUserConfListInteractor(ITenantProvider tenantProvider, IUserConfRepository userConfRepository)
    {
        _userConfRepository = userConfRepository;
        _loggingHandler = new LoggingHandler(tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
    }

    public UpsertUserConfListOutputData Handle(UpsertUserConfListInputData inputData)
    {
        try
        {
            if (inputData.HpId <= 0)
            {
                return new UpsertUserConfListOutputData(UpsertUserConfListStatus.InvalidHpId, new());
            }
            if (inputData.UserId <= 0)
            {
                return new UpsertUserConfListOutputData(UpsertUserConfListStatus.InvalidUserId, new());
            }
            if (!inputData.UserConfs.Any())
            {
                return new UpsertUserConfListOutputData(UpsertUserConfListStatus.InvalidUserConfs, new());
            }
            var count = 0;
            List<UserConfItemValidation> userConfItemValidations = new();
            foreach (var userConf in inputData.UserConfs)
            {
                var validation = userConf.Validation();
                if (validation != UserConfConst.UserConfStatus.Valid)
                {
                    userConfItemValidations.Add(new UserConfItemValidation(count, validation));
                }
                count++;
            }
            if (inputData.UserConfs.GroupBy(u => new { u.GrpCd, u.GrpItemCd, u.GrpItemEdaNo }).Count() != inputData.UserConfs.Count)
            {
                return new UpsertUserConfListOutputData(UpsertUserConfListStatus.DuplicateUserConf, new());
            }
            if (userConfItemValidations.Count > 0)
            {
                return new UpsertUserConfListOutputData(UpsertUserConfListStatus.Failed, userConfItemValidations);
            }
            var check = _userConfRepository.UpsertUserConfs(inputData.HpId, inputData.UserId, inputData.UserConfs);
            if (!check)
            {
                return new UpsertUserConfListOutputData(UpsertUserConfListStatus.Failed, new());
            }
            return new UpsertUserConfListOutputData(UpsertUserConfListStatus.Successed, new());
        }
        catch (Exception ex)
        {
            _loggingHandler.WriteLogExceptionAsync(ex);
            throw;
        }
        finally
        {
            _userConfRepository.ReleaseResource();
            _loggingHandler.Dispose();
        }
    }
}
