
using Domain.Models.SetGenerationMst;
using Domain.Models.SetMst;
using Interactor.SetMst.CommonSuperSet;
using UseCase.SetMst.GetList;

namespace Interactor.SetMst
{
    public class GetSetMstListInteractor : IGetSetMstListInputPort
    {
        private readonly ISetMstRepository _setRepository;
        private readonly ISetGenerationMstRepository _setGenerationRepository;
        private readonly ICommonSuperSet _commonSuperSet;
        public GetSetMstListInteractor(ISetMstRepository setRepository, ISetGenerationMstRepository setGenerationRepository, ICommonSuperSet commonSuperSet)
        {
            _setRepository = setRepository;
            _setGenerationRepository = setGenerationRepository;
            _commonSuperSet = commonSuperSet;
        }

        public GetSetMstListOutputData Handle(GetSetMstListInputData inputData)
        {
            try
            {
                if (inputData.HpId < 0)
                {
                    return new GetSetMstListOutputData(null, GetSetMstListStatus.InvalidHpId);
                }
                if (inputData.SetKbn < 0)
                {
                    return new GetSetMstListOutputData(null, GetSetMstListStatus.InvalidSetKbn);
                }
                if (inputData.SetKbnEdaNo < 0)
                {
                    return new GetSetMstListOutputData(null, GetSetMstListStatus.InvalidSetKbnEdaNo);
                }
                if (inputData.SinDate < 0)
                {
                    return new GetSetMstListOutputData(null, GetSetMstListStatus.InvalidSinDate);
                }
                var generationId = _setGenerationRepository.GetGenerationId(inputData.HpId, inputData.SinDate);
                var sets = _setRepository.GetList(inputData.HpId, inputData.SetKbn, inputData.SetKbnEdaNo, generationId, inputData.TextSearch);

                var output = _commonSuperSet.BuildTreeSetKbn(sets);

                return (output?.Count > 0) ? new GetSetMstListOutputData(output, GetSetMstListStatus.Successed) : new GetSetMstListOutputData(null, GetSetMstListStatus.NoData);
            }
            finally
            {
                _setGenerationRepository.ReleaseResource();
                _setRepository.ReleaseResource();
            }
        }
    }
}