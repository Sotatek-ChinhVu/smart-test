using Domain.Models.RaiinKubunMst;
using UseCase.RaiinKbn.GetPatientRaiinKubunList;

namespace Interactor.RaiinKubunMst
{
    public class GetPatientRaiinKubunListInteractor : IGetPatientRaiinKubunListInputPort
    {
        private readonly IRaiinKubunMstRepository _raiinKbnReponsitory;
        public GetPatientRaiinKubunListInteractor(IRaiinKubunMstRepository raiinKbnReponsitory)
        {
            _raiinKbnReponsitory = raiinKbnReponsitory;
        }

        public GetPatientRaiinKubunListOutputData Handle(GetPatientRaiinKubunListInputData inputData)
        {
            try
            {
                if (inputData.HpId < 0)
                {
                    return new GetPatientRaiinKubunListOutputData(new(), GetPatientRaiinKubunListStatus.InvalidPtId);
                }

                if (inputData.PtId < 0)
                {
                    return new GetPatientRaiinKubunListOutputData(new(), GetPatientRaiinKubunListStatus.InvalidPtId);
                }

                if (inputData.RaiinNo < 0)
                {
                    return new GetPatientRaiinKubunListOutputData(new(), GetPatientRaiinKubunListStatus.InvalidRaiinNo);
                }

                if (inputData.SinDate < 0)
                {
                    return new GetPatientRaiinKubunListOutputData(new(), GetPatientRaiinKubunListStatus.InvalidSinDate);
                }

                var data = _raiinKbnReponsitory.GetPatientRaiinKubuns(inputData.HpId, inputData.PtId, inputData.RaiinNo, inputData.SinDate);

                return new GetPatientRaiinKubunListOutputData(data.Select(d => new GetPatientRaiinKubunDto(d.RaiinKbnInfModel.HpId, d.RaiinKbnInfModel.GrpId, d.RaiinKbnInfModel.KbnCd, d.SortNo)).ToList(), GetPatientRaiinKubunListStatus.Successed);
            }
            finally
            {
                _raiinKbnReponsitory.ReleaseResource();
            }
        }
    }
}
