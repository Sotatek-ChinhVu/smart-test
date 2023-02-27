using Domain.Models.PtGroupMst;
using UseCase.PtGroupMst.CheckAllowDelete;

namespace Interactor.PtGroupMst
{
    public class CheckAllowDeleteGroupMstInteractor : ICheckAllowDeleteGroupMstInputPort
    {
        private readonly IGroupNameMstRepository _groupNameMstRepository;

        public CheckAllowDeleteGroupMstInteractor(IGroupNameMstRepository groupNameMstRepository)
        {
            _groupNameMstRepository = groupNameMstRepository;
        }

        public CheckAllowDeleteGroupMstOutputData Handle(CheckAllowDeleteGroupMstInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                    return new CheckAllowDeleteGroupMstOutputData(CheckAllowDeleteGroupMstStatus.InvalidHpId);

                if(inputData.CheckAllowDeleteGroupName)
                {
                    if(_groupNameMstRepository.IsInUseGroupName(inputData.GroupId, inputData.GroupCode))
                    {
                        return new CheckAllowDeleteGroupMstOutputData(CheckAllowDeleteGroupMstStatus.NotAllowDelete);
                    }
                }
                else
                {
                    if (_groupNameMstRepository.IsInUseGroupItem(inputData.GroupId, inputData.GroupCode))
                    {
                        return new CheckAllowDeleteGroupMstOutputData(CheckAllowDeleteGroupMstStatus.NotAllowDelete);
                    }
                }

                return new CheckAllowDeleteGroupMstOutputData(CheckAllowDeleteGroupMstStatus.AllowDelete);
            }
            finally
            {
                _groupNameMstRepository.ReleaseResource();
            }
        }
    }
}
