using Domain.Models.Ka;
using UseCase.Ka.SaveList;

namespace Interactor.Ka;

public class SaveKaMstInteractor : ISaveKaMstInputPort
{
    private readonly IKaRepository _kaMstRepository;

    public SaveKaMstInteractor(IKaRepository kaMstRepository)
    {
        _kaMstRepository = kaMstRepository;
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

        var listKaCode = _kaMstRepository.GetListKacode();
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
                                                                    input.KaName
                                                    )).ToList();
            if (_kaMstRepository.SaveKaMst(inputData.HpId, inputData.UserId, kaMstModels))
            {
                return new SaveKaMstOutputData(SaveKaMstStatus.Successed);
            }
            return new SaveKaMstOutputData(SaveKaMstStatus.Failed);
        }
        catch (Exception)
        {
            return new SaveKaMstOutputData(SaveKaMstStatus.Failed);
        }
        finally
        {
            _kaMstRepository.ReleaseResource();
        }
    }
}
