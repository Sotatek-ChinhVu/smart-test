using Domain.Models.SetMst;
using Domain.Models.User;
using UseCase.SetMst.SaveSetMst;

namespace Interactor.SetMst;

public class SaveSetMstInteractor : ISaveSetMstInputPort
{
    private readonly ISetMstRepository _setMstRepository;
    private readonly IUserRepository _userRepository;

    public SaveSetMstInteractor(ISetMstRepository setMstRepository, IUserRepository userRepository)
    {
        _setMstRepository = setMstRepository;
        _userRepository = userRepository;
    }

    public SaveSetMstOutputData Handle(SaveSetMstInputData inputData)
    {
        var notAllowSave = _userRepository.NotAllowSaveMedicalExamination(inputData.HpId, inputData.PtId, inputData.RaiinNo, inputData.SinDate, inputData.UserId);
        if (notAllowSave)
        {
            return new SaveSetMstOutputData(new(), SaveSetMstStatus.MedicalScreenLocked);
        }
        if (inputData.SinDate <= 15000101 && inputData.SinDate > 30000000)
        {
            return new SaveSetMstOutputData(new(), SaveSetMstStatus.InvalidSindate);
        }
        else if (inputData.SetCd < 0)
        {
            return new SaveSetMstOutputData(new(), SaveSetMstStatus.InvalidSetCd);
        }
        else if (inputData.SetKbn < 1 && inputData.SetKbn > 10)
        {
            return new SaveSetMstOutputData(new(), SaveSetMstStatus.InvalidSetKbn);
        }
        else if (inputData.SetKbnEdaNo < 1 && inputData.SetKbnEdaNo > 6)
        {
            return new SaveSetMstOutputData(new(), SaveSetMstStatus.InvalidSetKbnEdaNo);
        }
        else if (inputData.SetName.Length > 60)
        {
            return new SaveSetMstOutputData(new(), SaveSetMstStatus.InvalidSetName);
        }
        else if (inputData.WeightKbn < 0)
        {
            return new SaveSetMstOutputData(new(), SaveSetMstStatus.InvalidWeightKbn);
        }
        else if (inputData.Color < 0)
        {
            return new SaveSetMstOutputData(new(), SaveSetMstStatus.InvalidColor);
        }
        else if (inputData.IsDeleted < 0 && inputData.IsDeleted > 1)
        {
            return new SaveSetMstOutputData(new(), SaveSetMstStatus.InvalidIsDeleted);
        }
        try
        {
            var setMstModel = new SetMstModel(
                                inputData.HpId,
                                inputData.SetCd,
                                inputData.SetKbn,
                                inputData.SetKbnEdaNo,
                                0,
                                0,
                                0,
                                0,
                                inputData.SetName,
                                inputData.WeightKbn,
                                inputData.Color,
                                inputData.IsDeleted,
                                inputData.IsGroup ? 1 : 0,
                                inputData.IsAddNew
                             );
            var resultData = _setMstRepository.SaveSetMstModel(inputData.UserId, inputData.SinDate, setMstModel);
            if (resultData != null)
            {
                return new SaveSetMstOutputData(resultData, SaveSetMstStatus.Successed);
            }
            return new SaveSetMstOutputData(new(), SaveSetMstStatus.Failed);
        }
        finally
        {
            _setMstRepository.ReleaseResource();
        }
    }
}
