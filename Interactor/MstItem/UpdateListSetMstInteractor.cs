using Domain.Models.ListSetMst;
using UseCase.ListSetMst.UpdateListSetMst;

namespace Interactor.ListSetMst
{
    public class UpdateListSetMstInteractor : IUpdateListSetMstInputPort
    {
        private readonly IListSetMstRepository _listSetMstRepository;
        public UpdateListSetMstInteractor(IListSetMstRepository listSetMstRepository)
        {
            _listSetMstRepository = listSetMstRepository;
        }
        public UpdateListSetMstOutputData Handle(UpdateListSetMstInputData inputData)
        {

            if (inputData.HpId < 0)
            {
                return new UpdateListSetMstOutputData(false, UpdateListSetMstStatus.InValidHpId);
            }

            if (inputData.UserId < 0)
            {
                return new UpdateListSetMstOutputData(false, UpdateListSetMstStatus.InValidUserId);
            }

            if (inputData.ListSetMsts.Count <= 0)
            {
                return new UpdateListSetMstOutputData(false, UpdateListSetMstStatus.InvalidDataUpdate);
            }
            try
            {
                var data = _listSetMstRepository.UpdateTreeListSetMst(inputData.UserId, inputData.HpId, inputData.ListSetMsts);
                return new UpdateListSetMstOutputData(data, UpdateListSetMstStatus.Successed);
            }
            finally
            {
                _listSetMstRepository.ReleaseResource();
            }
        }
    }
}
