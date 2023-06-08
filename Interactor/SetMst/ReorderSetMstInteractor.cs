using Domain.Models.SetMst;
using Interactor.SetMst.CommonSuperSet;
using Domain.Models.User;
using UseCase.SetMst.ReorderSetMst;

namespace Interactor.SetMst;

public class ReorderSetMstInteractor : IReorderSetMstInputPort
{
    private readonly ISetMstRepository _setMstRepository;
    private readonly ICommonSuperSet _commonSuperSet;
    private readonly IUserRepository _userRepository;

    public ReorderSetMstInteractor(ISetMstRepository setMstRepository, ICommonSuperSet commonSuperSet, IUserRepository userRepository)
    {
        _setMstRepository = setMstRepository;
        _commonSuperSet = commonSuperSet;
        _userRepository = userRepository;
    }

    public ReorderSetMstOutputData Handle(ReorderSetMstInputData inputData)
    {
        var checkLockMedical = _userRepository.CheckLockMedicalExamination(inputData.HpId, inputData.PtId, inputData.RaiinNo, inputData.SinDate, inputData.UserId);
        if (checkLockMedical)
        {
            return new ReorderSetMstOutputData(ReorderSetMstStatus.MedicalScreenLocked);
        }
        else if (inputData.HpId <= 0)
        {
            return new ReorderSetMstOutputData(ReorderSetMstStatus.InvalidHpId);
        }
        else if (inputData.DragSetCd <= 0)
        {
            return new ReorderSetMstOutputData(ReorderSetMstStatus.InvalidDragSetCd);
        }
        else if (inputData.DropSetCd < 0)
        {
            return new ReorderSetMstOutputData(ReorderSetMstStatus.InvalidDropSetCd);
        }
        try
        {
            var result = _setMstRepository.ReorderSetMst(inputData.UserId, inputData.HpId, inputData.DragSetCd, inputData.DropSetCd);
            if (result.status)
            {
                return new ReorderSetMstOutputData(_commonSuperSet.BuildTreeSetKbn(result.setMstModels), ReorderSetMstStatus.Successed);
            }
            return new ReorderSetMstOutputData(ReorderSetMstStatus.InvalidLevel);
        }
        catch
        {
            return new ReorderSetMstOutputData(ReorderSetMstStatus.Failed);
        }
        finally
        {
            _setMstRepository.ReleaseResource();
        }
    }
}
