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
        if (inputData.SinDate <= 15000101 && inputData.SinDate > 30000000)
        {
            return new SaveSetMstOutputData(null, SaveSetMstStatus.InvalidSindate);
        }
        else if (inputData.SetCd < 0)
        {
            return new SaveSetMstOutputData(null, SaveSetMstStatus.InvalidSetCd);
        }
        else if (inputData.SetKbn < 1 && inputData.SetKbn > 10)
        {
            return new SaveSetMstOutputData(null, SaveSetMstStatus.InvalidSetKbn);
        }
        else if (inputData.SetKbnEdaNo < 1 && inputData.SetKbnEdaNo > 6)
        {
            return new SaveSetMstOutputData(null, SaveSetMstStatus.InvalidSetKbnEdaNo);
        }
        else if (inputData.GenerationId < 0)
        {
            return new SaveSetMstOutputData(null, SaveSetMstStatus.InvalidGenarationId);
        }
        else if (inputData.Level1 <= 0)
        {
            return new SaveSetMstOutputData(null, SaveSetMstStatus.InvalidLevel1);
        }
        else if (inputData.Level2 < 0)
        {
            return new SaveSetMstOutputData(null, SaveSetMstStatus.InvalidLevel2);
        }
        else if (inputData.Level3 < 0)
        {
            return new SaveSetMstOutputData(null, SaveSetMstStatus.InvalidLevel3);
        }
        else if (inputData.SetName.Length > 60)
        {
            return new SaveSetMstOutputData(null, SaveSetMstStatus.InvalidSetName);
        }
        else if (inputData.WeightKbn < 0)
        {
            return new SaveSetMstOutputData(null, SaveSetMstStatus.InvalidWeightKbn);
        }
        else if (inputData.Color < 0)
        {
            return new SaveSetMstOutputData(null, SaveSetMstStatus.InvalidColor);
        }
        else if (inputData.IsDeleted < 0 && inputData.IsDeleted > 1)
        {
            return new SaveSetMstOutputData(null, SaveSetMstStatus.InvalidIsDeleted);
        }
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
                                inputData.IsGroup ? 1 : 0
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
