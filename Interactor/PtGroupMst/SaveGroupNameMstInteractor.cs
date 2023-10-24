using Domain.Models.PtGroupMst;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.PtGroupMst.SaveGroupNameMst;

namespace Interactor.PtGroupMst
{
    public class SaveGroupNameMstInteractor : ISaveGroupNameMstInputPort
    {
        private readonly IGroupNameMstRepository _groupNameMstRepository;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;

        public SaveGroupNameMstInteractor(ITenantProvider tenantProvider, IGroupNameMstRepository groupNameMstRepository)
        {
            _groupNameMstRepository = groupNameMstRepository;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public SaveGroupNameMstOutputData Handle(SaveGroupNameMstInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                    return new SaveGroupNameMstOutputData(SaveGroupNameMstStatus.InvalidHpId, string.Empty);

                if (inputData.GroupNameMst.Any(x => x.GrpId <= 0 || x.SortNo <= 0 || string.IsNullOrEmpty(x.GrpName)
                            || x.GroupItems.Any(u => string.IsNullOrEmpty(u.GrpCode) || string.IsNullOrEmpty(u.GrpCodeName))))
                    return new SaveGroupNameMstOutputData(SaveGroupNameMstStatus.InvalidInputGroupMst, string.Empty);

                bool result = _groupNameMstRepository.SaveGroupNameMst(inputData.GroupNameMst, inputData.HpId, inputData.UserId);
                if (result)
                    return new SaveGroupNameMstOutputData(SaveGroupNameMstStatus.Successful, string.Empty);
                else
                    return new SaveGroupNameMstOutputData(SaveGroupNameMstStatus.Failed, string.Empty);
            }
            catch (Exception ex)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
            finally
            {
                _groupNameMstRepository.ReleaseResource();
                _loggingHandler.Dispose();
            }
        }
    }
}
