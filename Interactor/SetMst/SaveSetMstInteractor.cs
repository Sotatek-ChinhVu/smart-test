using Domain.Models.SetMst;
using Helper.Constants;
using UseCase.SetMst.SaveSetMst;

namespace Interactor.SetMst;

public class SaveSetMstInteractor : ISaveSetMstInputPort
{
    private readonly int _hpId = TempIdentity.HpId;
    private readonly int _userId = TempIdentity.UserId;
    private readonly ISetMstRepository _setMstRepository;
    public SaveSetMstInteractor(ISetMstRepository setMstRepository)
    {
        _setMstRepository = setMstRepository;
    }

    public SaveSetMstOutputData Handle(SaveSetMstInputData inputData)
    {
        try
        {
            var setMstModel = new SetMstModel(
                                _hpId,
                                inputData.SetCd,
                                inputData.SetKbn,
                                inputData.SetKbnEdaNo,
                                inputData.GenerationId,
                                inputData.Level1,
                                inputData.Level2,
                                inputData.Level3,
                                inputData.SetName,
                                inputData.WeightKbn,
                                inputData.Color,
                                inputData.IsDeleted,
                                inputData.IsGroup ? 1 : 0,
                                null
                             );
            var resultData = _setMstRepository.SaveSetMstModel(_userId, inputData.SinDate, setMstModel);
            if (resultData != null)
            {
                return new SaveSetMstOutputData(resultData, SaveSetMstStatus.Successed);
            }
            return new SaveSetMstOutputData(null, SaveSetMstStatus.Failed);
        }
        catch (Exception)
        {
            return new SaveSetMstOutputData(null, SaveSetMstStatus.Failed);
        }
    }
}
