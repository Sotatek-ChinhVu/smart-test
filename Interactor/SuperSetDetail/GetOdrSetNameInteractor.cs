using Domain.Models.KensaIrai;
using Domain.Models.SuperSetDetail;
using UseCase.MainMenu.GetOdrSetName;

namespace Interactor.SuperSetDetail;

public class GetOdrSetNameInteractor : IGetOdrSetNameInputPort
{
    private readonly ISuperSetDetailRepository _superSetDetailRepository;

    public GetOdrSetNameInteractor(ISuperSetDetailRepository superSetDetailRepository)
    {
        _superSetDetailRepository = superSetDetailRepository;
    }

    public GetOdrSetNameOutputData Handle(GetOdrSetNameInputData inputData)
    {
        try
        {
            var result = _superSetDetailRepository.GetOdrSetName(inputData.HpId, inputData.CheckBoxStatus, inputData.GenerationId, inputData.TimeExpired, inputData.ItemName);
            return new GetOdrSetNameOutputData(result, GetOdrSetNameStatus.Successed);
        }
        finally
        {
            _superSetDetailRepository.ReleaseResource();
        }
    }
}
