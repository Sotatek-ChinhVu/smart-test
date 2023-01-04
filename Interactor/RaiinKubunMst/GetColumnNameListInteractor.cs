using Domain.Models.RaiinKubunMst;
using UseCase.RaiinKubunMst.GetListColumnName;

namespace Interactor.RaiinKubunMst
{
    public class GetColumnNameListInteractor : IGetColumnNameListInputPort
    {
        private readonly IRaiinKubunMstRepository _raiinKubunMstRepository;
        public GetColumnNameListInteractor(IRaiinKubunMstRepository raiinKubunMstRepository)
        {
            _raiinKubunMstRepository = raiinKubunMstRepository;
        }

        public GetColumnNameListOutputData Handle(GetColumnNameListInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new GetColumnNameListOutputData(GetColumnNameListStatus.InvalidHpId, new());
                }

                var columnNames = _raiinKubunMstRepository.GetListColumnName(inputData.HpId);

                if (!columnNames.Any())
                {
                    return new GetColumnNameListOutputData(GetColumnNameListStatus.NoData, new());
                }

                return new GetColumnNameListOutputData(GetColumnNameListStatus.Successed, columnNames);
            }
            catch
            {
                return new GetColumnNameListOutputData(GetColumnNameListStatus.Failed, new());
            }
            finally
            {
                _raiinKubunMstRepository.ReleaseResource();
            }
        }
    }
}
