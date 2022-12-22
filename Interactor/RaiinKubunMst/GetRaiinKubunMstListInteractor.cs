using Domain.Models.RaiinKubunMst;
using UseCase.RaiinKubunMst.GetList;

namespace Interactor.RaiinKubunMst
{
    public class GetRaiinKubunMstListInteractor : IGetRaiinKubunMstListInputPort
    {
        private readonly IRaiinKubunMstRepository _raiinKubunMstRepository;
        public GetRaiinKubunMstListInteractor(IRaiinKubunMstRepository raiinKubunMstRepository)
        {
            _raiinKubunMstRepository = raiinKubunMstRepository;
        }

        public GetRaiinKubunMstListOutputData Handle(GetRaiinKubunMstListInputData inputData)
        {
            try
            {
                List<RaiinKubunMstModel> raiinKubunList = _raiinKubunMstRepository.GetList(inputData.IsDeleted);

                return new GetRaiinKubunMstListOutputData(raiinKubunList);
            }
            finally
            {
                _raiinKubunMstRepository.ReleaseResource();
            }
        }
    }
}
