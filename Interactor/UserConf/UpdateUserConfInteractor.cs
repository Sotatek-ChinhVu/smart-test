using Domain.Models.UserConf;
using UseCase.User.UpdateUserConf;

namespace Interactor.UserConf;

public class UpdateUserConfInteractor : IUpdateUserConfInputPort
{
    private readonly IUserConfRepository _userConfRepository;

    public UpdateUserConfInteractor(IUserConfRepository userConfRepository)
    {
        _userConfRepository = userConfRepository;
    }

    public UpdateUserConfOutputData Handle(UpdateUserConfInputData inputData)
    {
        try
        {
            if (inputData.HpId <= 0)
            {
                return new UpdateUserConfOutputData(UpdateUserConfStatus.InvalidHpId);
            }
            if (inputData.UserId <= 0)
            {
                return new UpdateUserConfOutputData(UpdateUserConfStatus.InvalidUserId);
            }
            if (inputData.GrpCd <= 0)
            {
                return new UpdateUserConfOutputData(UpdateUserConfStatus.InvalidGrpCd);
            }
            if (inputData.Value < 0)
            {
                return new UpdateUserConfOutputData(UpdateUserConfStatus.InvalidUserId);
            }

            _userConfRepository.UpdateUserConf(inputData.HpId, inputData.UserId, inputData.GrpCd, inputData.Value);
            return new UpdateUserConfOutputData(UpdateUserConfStatus.Successed);
        }
        catch
        {
            return new UpdateUserConfOutputData(UpdateUserConfStatus.Failed);
        }
        finally
        {
            _userConfRepository.ReleaseResource();
        }
    }
}
