using Domain.Models.UserConf;
using Helper.Constants;
using UseCase.User.UpsertUserConfList;

namespace Interactor.UserConf;

public class UpsertUserConfListInteractor : IUpsertUserConfListInputPort
{
    private readonly IUserConfRepository _userConfRepository;

    public UpsertUserConfListInteractor(IUserConfRepository userConfRepository)
    {
        _userConfRepository = userConfRepository;
    }

    public UpsertUserConfListOutputData Handle(UpsertUserConfListInputData inputData)
    {
        try
        {
            if (inputData.HpId <= 0)
            {
                return new UpsertUserConfListOutputData(UpsertUserConfListStatus.InvalidHpId, new());
            }
            if (inputData.UserId <= 0)
            {
                return new UpsertUserConfListOutputData(UpsertUserConfListStatus.InvalidUserId, new());
            }
            if (inputData.UserConfs.Count < 0)
            {
                return new UpsertUserConfListOutputData(UpsertUserConfListStatus.InvalidUserConfs, new());
            }
            var count = 0;
            List<UserConfItemValidation> userConfItemValidations = new();
            foreach (var userConf in inputData.UserConfs)
            {
                var validation = userConf.Validation();
                if (validation != UserConfConst.UserConfStatus.Valid)
                {
                    userConfItemValidations.Add(new UserConfItemValidation(count, validation));
                }
                count++;
            }
            if (inputData.UserConfs.GroupBy(u => new { u.GrpCd, u.GrpItemCd, u.GrpItemEdaNo }).Count() != inputData.UserConfs.Count())
            {
                return new UpsertUserConfListOutputData(UpsertUserConfListStatus.DuplicateUserConf, new());
            }
            if (userConfItemValidations.Count > 0)
            {
                return new UpsertUserConfListOutputData(UpsertUserConfListStatus.Failed, userConfItemValidations);
            }
            var check = _userConfRepository.UpsertUserConfs(inputData.HpId, inputData.UserId, inputData.UserConfs);
            if (!check)
            {
                return new UpsertUserConfListOutputData(UpsertUserConfListStatus.Failed, new());
            }
            return new UpsertUserConfListOutputData(UpsertUserConfListStatus.Successed, new());
        }
        finally
        {
            _userConfRepository.ReleaseResource();
        }
    }
}
