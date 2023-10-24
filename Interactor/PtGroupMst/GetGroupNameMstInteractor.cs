using Domain.Models.PtGroupMst;
using UseCase.PtGroupMst.GetGroupNameMst;

namespace Interactor.PtGroupMst;

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
            {
                return new GetGroupNameMstOutputData(GetGroupNameMstStatus.InvalidHpId, new List<GroupNameMstModel>());
            }

            var result = _groupNameMstRepository.GetListGroupNameMst(inputData.HpId);
            if (result.Any())
            {
                if (!inputData.IsGetAll)
                {
                    result = result.Where(item => item.IsDeleted == 0)
                                   .OrderBy(item => item.SortNo)
                                   .ToList();
                }
                return new GetGroupNameMstOutputData(GetGroupNameMstStatus.Successful, result);
            }
            return new GetGroupNameMstOutputData(GetGroupNameMstStatus.DataNotFound, result);
        }
        finally
        {
            _groupNameMstRepository.ReleaseResource();
        }
    }
}
