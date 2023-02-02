using Domain.Models.PtGroupMst;
using UseCase.PtGroupMst.GetGroupNameMst;

namespace Interactor.PtGroupMst
{
    public class GetGroupNameMstInteractor : IGetGroupNameMstInputPort
    {
        private readonly IGroupNameMstRepository _groupNameMstRepository;

        public GetGroupNameMstInteractor(IGroupNameMstRepository groupNameMstRepository)
        {
            _groupNameMstRepository = groupNameMstRepository;
        }

        public GetGroupNameMstOutputData Handle(GetGroupNameMstInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                    return new GetGroupNameMstOutputData(GetGroupNameMstStatus.InvalidHpId, new List<GroupNameMstModel>());

                var result = _groupNameMstRepository.GetListGroupNameMst(inputData.HpId);
                if (result.Any())
                    return new GetGroupNameMstOutputData(GetGroupNameMstStatus.Successful, result);
                else
                    return new GetGroupNameMstOutputData(GetGroupNameMstStatus.DataNotFound, result);
            }
            catch
            {
                return new GetGroupNameMstOutputData(GetGroupNameMstStatus.Exception, new List<GroupNameMstModel>());
            }
        }
    }
}
