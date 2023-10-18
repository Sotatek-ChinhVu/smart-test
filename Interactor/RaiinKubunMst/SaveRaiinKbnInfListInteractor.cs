using Domain.Models.RaiinKubunMst;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.RaiinKubunMst.SaveRaiinKbnInfList;

namespace Interactor.RaiinKubunMst
{
    public class SaveRaiinKbnInfListInteractor : ISaveRaiinKbnInfListInputPort
    {
        private readonly IRaiinKubunMstRepository _raiinKubunMstRepository;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;

        public SaveRaiinKbnInfListInteractor(ITenantProvider tenantProvider, IRaiinKubunMstRepository raiinKubunMstRepository)
        {
            _raiinKubunMstRepository = raiinKubunMstRepository;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public SaveRaiinKbnInfListOutputData Handle(SaveRaiinKbnInfListInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new SaveRaiinKbnInfListOutputData(SaveRaiinKbnInfListStatus.InvalidHpId);
                }
                if (inputData.PtId <= 0)
                {
                    return new SaveRaiinKbnInfListOutputData(SaveRaiinKbnInfListStatus.InvalidPtId);
                }
                if (inputData.SinDate <= 0)
                {
                    return new SaveRaiinKbnInfListOutputData(SaveRaiinKbnInfListStatus.InvalidSinDate);
                }
                if (inputData.RaiinNo <= 0)
                {
                    return new SaveRaiinKbnInfListOutputData(SaveRaiinKbnInfListStatus.InvalidRaiinNo);
                }
                if (inputData.UserId <= 0)
                {
                    return new SaveRaiinKbnInfListOutputData(SaveRaiinKbnInfListStatus.InvalidUserId);
                }
                if (inputData.KbnInfDtos.Count <= 0)
                {
                    return new SaveRaiinKbnInfListOutputData(SaveRaiinKbnInfListStatus.InvalidKbnInf);
                }
                var data = _raiinKubunMstRepository.SaveRaiinKbnInfs(inputData.HpId,
                    inputData.PtId, inputData.SinDate, inputData.RaiinNo, inputData.UserId, inputData.KbnInfDtos);
                if (data)
                {
                    return new SaveRaiinKbnInfListOutputData(SaveRaiinKbnInfListStatus.Successed);
                }
                else
                {
                    return new SaveRaiinKbnInfListOutputData(SaveRaiinKbnInfListStatus.Failed);
                }
            }
            catch (Exception ex)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
            finally
            {
                _raiinKubunMstRepository.ReleaseResource();
                _loggingHandler.Dispose();
            }
        }
    }
}
