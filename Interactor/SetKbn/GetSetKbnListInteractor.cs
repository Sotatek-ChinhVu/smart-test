using Domain.Models.SetGeneration;
using Domain.Models.SetKbn;
using UseCase.SetKbn.GetList;

namespace Interactor.SetKbn
{
    public class GetSetKbnListInteractor : IGetSetKbnListInputPort
    {
        private readonly ISetKbnMstRepository _setKbnMstRepository;
        private readonly ISetGenerationRepository _setGenerationRepository;
        public GetSetKbnListInteractor(ISetKbnMstRepository setKbnMstRepository, ISetGenerationRepository setGenerationRepository)
        {
            _setKbnMstRepository = setKbnMstRepository;
            _setGenerationRepository = setGenerationRepository;
        }

        public GetSetKbnListOutputData Handle(GetSetKbnListInputData inputData)
        {

            if (inputData.HpId < 0)
            {
                return new GetSetKbnListOutputData(null, GetSetKbnListStatus.InvalidHpId);
            }
            if (inputData.SetKbnFrom < 0)
            {
                return new GetSetKbnListOutputData(null, GetSetKbnListStatus.InvalidSetKbnFrom);
            }
            if (inputData.SetKbnTo < 0)
            {
                return new GetSetKbnListOutputData(null, GetSetKbnListStatus.InvalidSetKbnTo);
            }
            if (inputData.SinDate < 0)
            {
                return new GetSetKbnListOutputData(null, GetSetKbnListStatus.InvalidSinDate);
            }
            if (inputData.SetKbnFrom > inputData.SetKbnTo)
            {
                return new GetSetKbnListOutputData(null, GetSetKbnListStatus.InvalidSetKbn);
            }

            var lowerSetGenarationMsts = _setGenerationRepository.GetList(inputData.HpId, inputData.SinDate);
            var setGenarationMst = lowerSetGenarationMsts.FirstOrDefault();
            if (setGenarationMst == null)
            {
                return new GetSetKbnListOutputData(null, GetSetKbnListStatus.NoData);
            }
            var setKbnMsts = _setKbnMstRepository.GetList(inputData.HpId, inputData.SetKbnFrom, inputData.SetKbnTo);

            var result = setKbnMsts.Where(r => r.GenerationId == setGenarationMst.GenerationId).OrderBy(r => r.SetKbn).ToList();

            return result?.Count > 0 ? new GetSetKbnListOutputData(result.Select(r => new GetSetKbnListOutputItem(r)).ToList(), GetSetKbnListStatus.Successed) : new GetSetKbnListOutputData(null, GetSetKbnListStatus.NoData);
        }
    }
}