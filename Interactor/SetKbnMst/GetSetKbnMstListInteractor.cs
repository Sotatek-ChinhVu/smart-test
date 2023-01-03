using Domain.Models.SetGenerationMst;
using Domain.Models.SetKbnMst;
using UseCase.SetKbnMst.GetList;

namespace Interactor.SetKbnMst
{
    public class GetSetKbnMstListInteractor : IGetSetKbnMstListInputPort
    {
        private readonly ISetKbnMstRepository _setKbnMstRepository;
        private readonly ISetGenerationMstRepository _setGenerationRepository;
        public GetSetKbnMstListInteractor(ISetKbnMstRepository setKbnMstRepository, ISetGenerationMstRepository setGenerationRepository)
        {
            _setKbnMstRepository = setKbnMstRepository;
            _setGenerationRepository = setGenerationRepository;
        }

        public GetSetKbnMstListOutputData Handle(GetSetKbnMstListInputData inputData)
        {

            if (inputData.HpId < 0)
            {
                return new GetSetKbnMstListOutputData(null, GetSetKbnMstListStatus.InvalidHpId);
            }
            if (inputData.SetKbnFrom < 0)
            {
                return new GetSetKbnMstListOutputData(null, GetSetKbnMstListStatus.InvalidSetKbnFrom);
            }
            if (inputData.SetKbnTo < 0)
            {
                return new GetSetKbnMstListOutputData(null, GetSetKbnMstListStatus.InvalidSetKbnTo);
            }
            if (inputData.SinDate < 0)
            {
                return new GetSetKbnMstListOutputData(null, GetSetKbnMstListStatus.InvalidSinDate);
            }
            if (inputData.SetKbnFrom > inputData.SetKbnTo)
            {
                return new GetSetKbnMstListOutputData(null, GetSetKbnMstListStatus.InvalidSetKbn);
            }
            try
            {
                var lowerSetGenarationMsts = _setGenerationRepository.GetList(inputData.HpId, inputData.SinDate);
                var setGenarationMst = lowerSetGenarationMsts.FirstOrDefault();
                if (setGenarationMst == null)
                {
                    return new GetSetKbnMstListOutputData(null, GetSetKbnMstListStatus.NoData);
                }
                var setKbnMsts = _setKbnMstRepository.GetList(inputData.HpId, inputData.SetKbnFrom, inputData.SetKbnTo);

                var result = setKbnMsts.Where(r => r.GenerationId == setGenarationMst.GenerationId).OrderBy(r => r.SetKbn).ToList();

                return result?.Count > 0 ? new GetSetKbnMstListOutputData(result.Select(r => new GetSetKbnMstListOutputItem(r)).ToList(), GetSetKbnMstListStatus.Successed) : new GetSetKbnMstListOutputData(null, GetSetKbnMstListStatus.NoData);

            }
            finally
            {
                _setGenerationRepository.ReleaseResource();
                _setKbnMstRepository.ReleaseResource();
            }
        }
    }
}