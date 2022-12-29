using Domain.Models.SetMst;
using UseCase.SetMst.CopyPasteSetMst;

namespace Interactor.SetMst;

public class CopyPasteSetMstInteractor : ICopyPasteSetMstInputPort
{
    private readonly ISetMstRepository _setMstRepository;

    public CopyPasteSetMstInteractor(ISetMstRepository setMstRepository)
    {
        _setMstRepository = setMstRepository;
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
        else if (inputData.CopySetCd <= 0)
        {
            return new CopyPasteSetMstOutputData(CopyPasteSetMstStatus.InvalidCopySetCd);
        }
        else if (inputData.PasteSetCd < 0)
        {
            return new CopyPasteSetMstOutputData(CopyPasteSetMstStatus.InvalidPasteSetCd);
        }
        try
        {
            int newSetCd = _setMstRepository.PasteSetMst(inputData.UserId, inputData.HpId, inputData.CopySetCd, inputData.PasteSetCd);
            if (newSetCd > 0)
            {
                return new CopyPasteSetMstOutputData(newSetCd, CopyPasteSetMstStatus.Successed);
            }
            return new CopyPasteSetMstOutputData(CopyPasteSetMstStatus.InvalidLevel);
        }
        catch
        {
            return new CopyPasteSetMstOutputData(CopyPasteSetMstStatus.Failed);
        }
        finally
        {
            _setMstRepository.ReleaseResource();
        }
    }
}
