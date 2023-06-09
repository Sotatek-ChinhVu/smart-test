using Domain.Models.SetMst;
using Interactor.SetMst.CommonSuperSet;
using UseCase.SetMst.CopyPasteSetMst;

namespace Interactor.SetMst;

public class CopyPasteSetMstInteractor : ICopyPasteSetMstInputPort
{
    private readonly ISetMstRepository _setMstRepository;
    private readonly ICommonSuperSet _commonSuperSet;

    public CopyPasteSetMstInteractor(ISetMstRepository setMstRepository, ICommonSuperSet commonSuperSet)
    {
        _setMstRepository = setMstRepository;
        _commonSuperSet = commonSuperSet;
    }
    public CopyPasteSetMstOutputData Handle(CopyPasteSetMstInputData inputData)
    {
        if (inputData.HpId <= 0)
        {
            return new CopyPasteSetMstOutputData(CopyPasteSetMstStatus.InvalidHpId);
        }
        else if (inputData.UserId <= 0)
        {
            return new CopyPasteSetMstOutputData(CopyPasteSetMstStatus.InvalidUserId);
        }
        else if (inputData.CopySetCd < 0 || (inputData.CopySetCd == 0 && !inputData.PasteToOtherGroup))
        {
            return new CopyPasteSetMstOutputData(CopyPasteSetMstStatus.InvalidCopySetCd);
        }
        else if (inputData.PasteSetCd < 0)
        {
            return new CopyPasteSetMstOutputData(CopyPasteSetMstStatus.InvalidPasteSetCd);
        }
        else if (inputData.PasteSetKbn < 1 || inputData.PasteSetKbn > 10)
        {
            return new CopyPasteSetMstOutputData(CopyPasteSetMstStatus.InvalidPasteSetCd);
        }
        else if (inputData.PasteSetKbnEdaNo < 1 || inputData.PasteSetKbnEdaNo > 6)
        {
            return new CopyPasteSetMstOutputData(CopyPasteSetMstStatus.InvalidPasteSetCd);
        }
        else if (inputData.PasteToOtherGroup && (inputData.CopySetKbn < 1 || inputData.CopySetKbn > 10))
        {
            return new CopyPasteSetMstOutputData(CopyPasteSetMstStatus.InvalidCopySetCd);
        }
        else if (inputData.PasteToOtherGroup && (inputData.CopySetKbnEdaNo < 1 || inputData.CopySetKbnEdaNo > 6))
        {
            return new CopyPasteSetMstOutputData(CopyPasteSetMstStatus.InvalidCopySetCd);
        }
        try
        {
            var setMsts = _setMstRepository.PasteSetMst(inputData.HpId, inputData.UserId, inputData.GenerationId, inputData.CopySetCd, inputData.PasteSetCd, inputData.PasteToOtherGroup, inputData.CopySetKbnEdaNo, inputData.CopySetKbn, inputData.PasteSetKbnEdaNo, inputData.PasteSetKbn);
            if (setMsts.Count > 0)
            {
                return new CopyPasteSetMstOutputData(_commonSuperSet.BuildTreeSetKbn(setMsts), CopyPasteSetMstStatus.Successed);
            }
            return new CopyPasteSetMstOutputData(CopyPasteSetMstStatus.InvalidLevel);
        }
        finally
        {
            _setMstRepository.ReleaseResource();
        }
    }
}
