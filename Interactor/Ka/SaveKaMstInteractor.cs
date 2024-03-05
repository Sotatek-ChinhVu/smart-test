using Domain.Models.Ka;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.Ka.SaveList;

namespace Interactor.Ka;

public class SaveKaMstInteractor : ISaveKaMstInputPort
{
    private readonly IKaRepository _kaMstRepository;
    private readonly ILoggingHandler _loggingHandler;
    private readonly ITenantProvider _tenantProvider;

    public SaveKaMstInteractor(ITenantProvider tenantProvider, IKaRepository kaMstRepository)
    {
        _kaMstRepository = kaMstRepository;
        _tenantProvider = tenantProvider;
        _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
    }

    public SaveKaMstOutputData Handle(SaveKaMstInputData inputData)
    {
        if (inputData == null)
        {
            return new SaveKaMstOutputData(SaveKaMstStatus.Failed);
        }
        else if (inputData.HpId <= 0)
        {
            return new SaveKaMstOutputData(SaveKaMstStatus.InvalidHpId);
        }
        else if (inputData.UserId <= 0)
        {
            return new SaveKaMstOutputData(SaveKaMstStatus.InvalidUserId);
        }
        else if (inputData.KaMstModels.Count == 0)
        {
            return new SaveKaMstOutputData(SaveKaMstStatus.InputNotFound);
        }

        var listKaCode = _kaMstRepository.GetListKacode(inputData.HpId);
        foreach (var input in inputData.KaMstModels)
        {
            if (input.KaId <= 0)
            {
                return new SaveKaMstOutputData(SaveKaMstStatus.InvalidKaId);
            }
            else if (input.KaSname.Length > 20)
            {
                return new SaveKaMstOutputData(SaveKaMstStatus.KaSnameMaxLength20);
            }
            else if (input.KaName.Length > 40)
            {
                return new SaveKaMstOutputData(SaveKaMstStatus.KaNameMaxLength40);
            }
            else if (!listKaCode.Any(code => code.ReceKaCd == input.ReceKaCd))
            {
                return new SaveKaMstOutputData(SaveKaMstStatus.ReceKaCdNotFound);
            }
            else if (inputData.KaMstModels.Count(x => x.KaId == input.KaId) > 1)
            {
                return new SaveKaMstOutputData(SaveKaMstStatus.CanNotDuplicateKaId);
            }
        }
        try
        {
            var kaMstModels = inputData.KaMstModels.Select(input => new KaMstModel(
                                                                    input.Id,
                                                                    input.KaId,
                                                                    0,
                                                                    input.ReceKaCd,
                                                                    input.KaSname,
                                                                    input.KaName,
                                                                    input.YousikiKaCd
                                                    )).ToList();
            if (_kaMstRepository.SaveKaMst(inputData.HpId, inputData.UserId, kaMstModels))
            {
                return new SaveKaMstOutputData(SaveKaMstStatus.Successed);
            }
            return new SaveKaMstOutputData(SaveKaMstStatus.Failed);
        }
        catch (Exception ex)
        {
            _loggingHandler.WriteLogExceptionAsync(ex);
            throw;
        }
        finally
        {
            _kaMstRepository.ReleaseResource();
            _loggingHandler.Dispose();
        }
    }
}
