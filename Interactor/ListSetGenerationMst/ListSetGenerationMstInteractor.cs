using Domain.Models.ListSetGenerationMst;
using UseCase.ListSetGenerationMst.GetListSetGenerationMst;

namespace Interactor.ListSetGenerationMst
{
    public class ListSetGenerationMstInteractor : IGetListSetGenerationMstInputPort
    {
        private IListSetGenerationMstRepository _listSetGenerationMstRepository { get; set; }
        public ListSetGenerationMstInteractor(IListSetGenerationMstRepository listSetGenerationMstRepository)
        {
            _listSetGenerationMstRepository = listSetGenerationMstRepository;
        }
        public GetListSetGenerationMstOutputData Handle(GetListSetGenerationMstInputData inputData)
        {
            try
            {
                if (inputData.hpId < 0)
                {
                    return new GetListSetGenerationMstOutputData(new List<ListSetGenerationMstModel>(), GetListSetGenerationMstStatus.InvalidHpId);
                }
                var data = _listSetGenerationMstRepository.GetAll(inputData.hpId);
                if (!data.Any())
                {
                    new GetListSetGenerationMstOutputData(new List<ListSetGenerationMstModel>(), GetListSetGenerationMstStatus.NoData);
                }
                return new GetListSetGenerationMstOutputData(data, GetListSetGenerationMstStatus.Successed);
            }
            finally
            {
                _listSetGenerationMstRepository.ReleaseResource();
            }
        }
    }
}
