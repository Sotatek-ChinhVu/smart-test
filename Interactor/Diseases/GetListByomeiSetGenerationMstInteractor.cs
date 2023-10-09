using Domain.Models.ByomeiSetGenerationMst;
using UseCase.MstItem.GetListByomeiSetGenerationMst;

namespace Interactor.Diseases
{
    public class GetListByomeiSetGenerationMstInteractor: IGetListByomeiSetGenerationMstInputPort
    {
        private IByomeiSetGenerationMstRepository _byomeiSetGenerationMstRepository;
        public GetListByomeiSetGenerationMstInteractor(IByomeiSetGenerationMstRepository byomeiSetGenerationMstRepository)
        {
            _byomeiSetGenerationMstRepository = byomeiSetGenerationMstRepository;
        }
        public GetListByomeiSetGenerationMstOutputData Handle(GetListByomeiSetGenerationMstInputData inputData)
        {
            try
            {
                if (inputData.HpId < 0)
                {
                    return new GetListByomeiSetGenerationMstOutputData(new List<ByomeiSetGenerationMstModel>(), GetListByomeiSetGenerationMstStatus.InvalidHpId);
                }
                var data = _byomeiSetGenerationMstRepository.GetAll(inputData.HpId);
                if (!data.Any())
                {
                    new GetListByomeiSetGenerationMstOutputData(new List<ByomeiSetGenerationMstModel>(), GetListByomeiSetGenerationMstStatus.NoData);
                }
                return new GetListByomeiSetGenerationMstOutputData(data, GetListByomeiSetGenerationMstStatus.Successed);
            }
            finally
            {
                _byomeiSetGenerationMstRepository.ReleaseResource();
            }
        }
    }
}
