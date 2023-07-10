using Domain.Models.User;
using UseCase.User.GetListJobMst;

namespace Interactor.User
{
    public class GetListJobMstInteractor : IGetListJobMstInputPort
    {
        private readonly IUserRepository _userRepository;

        public GetListJobMstInteractor(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public GetListJobMstOutputData Handle(GetListJobMstInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                    return new GetListJobMstOutputData(GetListJobMstStatus.InvalidHpId, new List<JobMstModel>());

                var data = _userRepository.GetListJob(inputData.HpId);
                if (data.Any())
                    return new GetListJobMstOutputData(GetListJobMstStatus.Successful, data);
                else
                    return new GetListJobMstOutputData(GetListJobMstStatus.NoData, data);
            }
            finally
            {
                _userRepository.ReleaseResource();
            }
        }
    }
}
