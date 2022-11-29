using Domain.Models.UserConf;
using UseCase.UserConf.UpdateAdoptedByomeiConfig;

namespace Interactor.UserConf;

public class UpdateAdoptedByomeiConfigInteractor : IUpdateAdoptedByomeiConfigInputPort
{
    private readonly IUserConfRepository _userConfRepository;

    public UpdateAdoptedByomeiConfigInteractor(IUserConfRepository userConfRepository)
    {
        _userConfRepository = userConfRepository;
    }

    public UpdateAdoptedByomeiConfigOutputData Handle(UpdateAdoptedByomeiConfigInputData inputData)
    {
        try
        {
            if (inputData.HpId <= 0)
            {
                return new UpdateAdoptedByomeiConfigOutputData(UpdateAdoptedByomeiConfigStatus.InvalidHpId);
            }
            if (inputData.UserId <= 0)
            {
                return new UpdateAdoptedByomeiConfigOutputData(UpdateAdoptedByomeiConfigStatus.InvalidUserId);
            }
            if (inputData.AdoptedValue != 0 && inputData.AdoptedValue != 1)
            {
                return new UpdateAdoptedByomeiConfigOutputData(UpdateAdoptedByomeiConfigStatus.InvalidAdoptedValue);

            }
            _userConfRepository.UpdateAdoptedByomeiConfig(inputData.HpId, inputData.UserId, inputData.AdoptedValue);

            return new UpdateAdoptedByomeiConfigOutputData(UpdateAdoptedByomeiConfigStatus.Successed);
        }
        catch
        {
            return new UpdateAdoptedByomeiConfigOutputData(UpdateAdoptedByomeiConfigStatus.Failed);
        }
    }
}
