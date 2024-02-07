using Domain.Models.User;
using UseCase.User.GetListFunctionPermission;

namespace Interactor.User
{
    public class GetListFunctionPermissionInteractor : IGetListFunctionPermissionInputPort
    {
        private readonly IUserRepository _userRepository;

        public GetListFunctionPermissionInteractor(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public GetListFunctionPermissionOutputData Handle(GetListFunctionPermissionInputData input)
        {
            try
            {
                var data = _userRepository.GetListFunctionPermission(input.HpId);
                if (data.Any())
                    return new GetListFunctionPermissionOutputData(GetListFunctionPermissionStatus.Successful, data);
                else
                    return new GetListFunctionPermissionOutputData(GetListFunctionPermissionStatus.NoData, data);
            }
            finally
            {
                _userRepository.ReleaseResource();
            }
        }
    }
}
