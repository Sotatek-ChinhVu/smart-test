using Domain.Models.ReleasenoteRead;
using UseCase.ReleasenoteRead;

namespace Interactor.ReleasenoteRead;

public class GetListReleasenoteReadInteractor : IGetListReleasenoteReadInputPort
{
    private readonly IReleasenoteReadRepository _releasenoteReadRepository;

    public GetListReleasenoteReadInteractor(IReleasenoteReadRepository releasenoteReadRepository)
    {
        _releasenoteReadRepository = releasenoteReadRepository;
    }

    public GetListReleasenoteReadOutputData Handle(GetListReleasenoteReadInputData inputData)
    {
        try
        {
            List<string> versions = _releasenoteReadRepository.GetListReleasenote(inputData.HpId, inputData.UserId);
            return new GetListReleasenoteReadOutputData(GetListReleasenoteReadStatus.Successed, versions);
        }
        finally 
        {
            _releasenoteReadRepository.ReleaseResource();
        }
    }
}
