using Domain.Models.UketukeSbtMst;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.UketukeSbtMst.Upsert;
using static Helper.Constants.UketukeSbtMstConstant;

namespace Interactor.UketukeSbtMst;

public class UpsertUketukeSbtMstInteractor : IUpsertUketukeSbtMstInputPort
{
    private readonly IUketukeSbtMstRepository _uketukeSbtMstRepository;
    private readonly ILoggingHandler _loggingHandler;
    private readonly ITenantProvider _tenantProvider;

    public UpsertUketukeSbtMstInteractor(ITenantProvider tenantProvider, IUketukeSbtMstRepository uketukeSbtMstRepository)
    {
        _uketukeSbtMstRepository = uketukeSbtMstRepository;
        _tenantProvider = tenantProvider;
        _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
    }

    public UpsertUketukeSbtMstOutputData Handle(UpsertUketukeSbtMstInputData inputdata)
    {
        try
        {
            if (inputdata.ToList() == null || inputdata.ToList().Count == 0)
            {
                return new UpsertUketukeSbtMstOutputData(UpsertUketukeSbtMstStatus.InputNoData);
            }

            foreach (var data in inputdata.ToList())
            {
                var status = data.Validation();
                if (status != ValidationStatus.Valid)
                {
                    return new UpsertUketukeSbtMstOutputData(ConvertStatus(status));
                }
            }

            var checkInputKbnId = inputdata.UketukeSbtMsts.Where(x => x.KbnId >= 0).Select(x => x.KbnId);
            if (checkInputKbnId.Count() != checkInputKbnId.Distinct().Count())
            {
                return new UpsertUketukeSbtMstOutputData(UpsertUketukeSbtMstStatus.InputDataDuplicateKbnId);
            }

            _uketukeSbtMstRepository.Upsert(inputdata.ToList(), inputdata.UserId, inputdata.HpId);

            return new UpsertUketukeSbtMstOutputData(UpsertUketukeSbtMstStatus.Success);
        }
        catch (Exception ex)
        {
            _loggingHandler.WriteLogExceptionAsync(ex);
            throw;
        }
        finally
        {
            _uketukeSbtMstRepository.ReleaseResource();
            _loggingHandler.Dispose();
        }
    }
    private static UpsertUketukeSbtMstStatus ConvertStatus(ValidationStatus status)
    {
        if (status == ValidationStatus.InvalidKbnId)
            return UpsertUketukeSbtMstStatus.InvalidKbnId;
        if (status == ValidationStatus.InvalidKbnName)
            return UpsertUketukeSbtMstStatus.InvalidKbnName;
        if (status == ValidationStatus.InvalidSortNo)
            return UpsertUketukeSbtMstStatus.InvalidSortNo;
        if (status == ValidationStatus.InvalidIsDeleted)
            return UpsertUketukeSbtMstStatus.InvalidIsDeleted;
        if (status == ValidationStatus.InputNoData)
            return UpsertUketukeSbtMstStatus.InputNoData;
        if (status == ValidationStatus.InputDataDuplicateKbnId)
            return UpsertUketukeSbtMstStatus.InputDataDuplicateKbnId;

        return UpsertUketukeSbtMstStatus.Success;
    }
}
