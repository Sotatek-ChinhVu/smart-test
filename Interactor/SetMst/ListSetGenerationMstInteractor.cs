using Domain.Models.SetGenerationMst;
using UseCase.MstItem.GetListSetGenerationMst;

namespace Interactor.SetMst
{
    public class ListSetGenerationMstInteractor : IGetListSetGenerationMstInputPort
    {
        private ISetGenerationMstRepository _setGenerationMstRepository { get; set; }
        public ListSetGenerationMstInteractor(ISetGenerationMstRepository setGenerationMstRepository)
        {
            _setGenerationMstRepository = setGenerationMstRepository;
        }

        public GetListSetGenerationMstOutputData Handle(GetListSetGenerationMstInputData inputData)
        {
            try
            {
                var data = _setGenerationMstRepository.GetAll(inputData.HpId);
                if (!data.Any())
                {
                    return new GetListSetGenerationMstOutputData(new(), GetListSetGenerationMstStatus.NoData);
                }
                return new GetListSetGenerationMstOutputData(data, GetListSetGenerationMstStatus.Successed);
            }
            finally
            {
                _setGenerationMstRepository.ReleaseResource();
            }
        }
    }
}
