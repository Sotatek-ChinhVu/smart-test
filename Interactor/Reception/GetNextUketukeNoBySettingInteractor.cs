using Domain.Models.Reception;
using UseCase.Reception.GetNextUketukeNoBySetting;

namespace Interactor.Reception;

public class GetNextUketukeNoBySettingInteractor : IGetNextUketukeNoBySettingInputPort
{
    private readonly IReceptionRepository _receptionRepository;

    public GetNextUketukeNoBySettingInteractor(IReceptionRepository receptionRepository)
    {
        _receptionRepository = receptionRepository;
    }

    public GetNextUketukeNoBySettingOutputData Handle(GetNextUketukeNoBySettingInputData inputData)
    {
        try
        {
            var result = _receptionRepository.GetNextUketukeNoBySetting(inputData.HpId, inputData.Sindate, inputData.InfKbn, inputData.KaId, inputData.UketukeMode, inputData.DefaultUkeNo);
            return new GetNextUketukeNoBySettingOutputData(GetNextUketukeNoBySettingStatus.Successed, result);
        }
        finally
        {
            _receptionRepository.ReleaseResource();
        }
    }
}
