using Domain.Models.PtGroupMst;
using UseCase.PtGroupMst.SaveGroupNameMst;

namespace Interactor.PtGroupMst
{
    public class SaveGroupNameMstInteractor : ISaveGroupNameMstInputPort
    {
        private readonly IGroupNameMstRepository _groupNameMstRepository;

        public SaveGroupNameMstInteractor(IGroupNameMstRepository groupNameMstRepository)
        {
            _groupNameMstRepository = groupNameMstRepository;
        }

        public SaveGroupNameMstOutputData Handle(SaveGroupNameMstInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                    return new SaveGroupNameMstOutputData(SaveGroupNameMstStatus.InvalidHpId, string.Empty);

                if(inputData.GroupNameMst.Any(x=>x.GrpId <=0 || x.SortNo <= 0 || string.IsNullOrEmpty(x.GrpName)
                            || x.GroupItems.Any(u => string.IsNullOrEmpty(u.GrpCode) || string.IsNullOrEmpty(u.GrpCodeName))))
                    return new SaveGroupNameMstOutputData(SaveGroupNameMstStatus.InvalidSortNoGrpIdGroupMst, string.Empty);

                bool result = _groupNameMstRepository.SaveGroupNameMst(inputData.GroupNameMst, inputData.HpId, inputData.UserId);
                if (result)
                    return new SaveGroupNameMstOutputData(SaveGroupNameMstStatus.Successful, string.Empty);
                else
                    return new SaveGroupNameMstOutputData(SaveGroupNameMstStatus.Failed, string.Empty);
            }
            catch(Exception ex)
            {
                return new SaveGroupNameMstOutputData(SaveGroupNameMstStatus.Exception, ex.Message);
            }
        }
    }
}
