using Domain.Models.UserConf;
using UseCase.User.Sagaku;

namespace Interactor.UserConf;

public class SagakuInteractor : ISagakuInputPort
{
    private readonly IUserConfRepository _userConfRepository;

    public SagakuInteractor(IUserConfRepository userConfRepository)
    {
        _userConfRepository = userConfRepository;
    }

    public SagakuOutputData Handle(SagakuInputData inputData)
    {
        try
        {
            if (inputData.HpId <= 0)
            {
                return new SagakuOutputData(SagakuStatus.InvalidHpId, 0);
            }
            if (inputData.UserId <= 0)
            {
                return new SagakuOutputData(SagakuStatus.InvalidUserId, 0);
            }

            var result = _userConfRepository.Sagaku(inputData.FromRece);
            return new SagakuOutputData(SagakuStatus.Successed, result);
        }
        catch
        {
            return new SagakuOutputData(SagakuStatus.Failed, 0);
        }
        finally
        {
            _userConfRepository.ReleaseResource();
        }
    }
}
