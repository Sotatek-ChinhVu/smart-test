using Amazon.S3;
using AWSSDK.Constants;
using Domain.Models.ReleasenoteRead;
using Helper.Constants;
using UseCase.Releasenote.LoadListVersion;
using UseCase.Releasenote.UpdateListReleasenote;

namespace Interactor.ReleasenoteRead;

public class UpdateListReleasenoteInteractor : IUpdateListReleasenoteInputPort
{
    public readonly IReleasenoteReadRepository _releasenoteReadRepository;

    public UpdateListReleasenoteInteractor(IReleasenoteReadRepository releasenoteReadRepository)
    {
        _releasenoteReadRepository = releasenoteReadRepository;
    }
    public UpdateListReleasenoteOutputData Handle(UpdateListReleasenoteInputData inputData)
    {
        try
        {
            var result = _releasenoteReadRepository.UpdateListReleasenote(inputData.HpId, inputData.UserId, inputData.Versions);

            if (result)
            {
                return new UpdateListReleasenoteOutputData(UpdateListReleasenoteStatus.Successed);
            }
            else
            {
                return new UpdateListReleasenoteOutputData(UpdateListReleasenoteStatus.Failed);
            }
        }
        finally
        {
            _releasenoteReadRepository.ReleaseResource();
        }
    }
}
