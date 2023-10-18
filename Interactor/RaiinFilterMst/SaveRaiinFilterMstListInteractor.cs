using Domain.Models.RaiinFilterMst;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.RaiinFilterMst.SaveList;

namespace Interactor.RaiinFilterMst;

public class SaveRaiinFilterMstListInteractor : ISaveRaiinFilterMstListInputPort
{
    private readonly IRaiinFilterMstRepository _raiinFilterMstRepository;
    private readonly ILoggingHandler _loggingHandler;
    private readonly ITenantProvider _tenantProvider;

    public SaveRaiinFilterMstListInteractor(ITenantProvider tenantProvider, IRaiinFilterMstRepository raiinFilterMstRepository)
    {
        _raiinFilterMstRepository = raiinFilterMstRepository;
        _tenantProvider = tenantProvider;
        _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
    }

    public SaveRaiinFilterMstListOutputData Handle(SaveRaiinFilterMstListInputData input)
    {
        try
        {
            _raiinFilterMstRepository.SaveList(input.FilterMsts, input.HpId, input.UserId);
            return new SaveRaiinFilterMstListOutputData(SaveRaiinFilterMstListStatus.Success);
        }
        catch (Exception ex)
        {
            _loggingHandler.WriteLogExceptionAsync(ex);
            throw;
        }
        finally
        {
            _raiinFilterMstRepository.ReleaseResource();
            _loggingHandler.Dispose();
        }
    }
}
