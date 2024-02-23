using Domain.Models.MstItem;
using Domain.Models.SetMst;
using Domain.Models.SuperSetDetail;
using Interactor.SetMst.CommonSuperSet;
using UseCase.MainMenu.SaveOdrSet;

namespace Interactor.SuperSetDetail;

public class SaveOdrSetInteractor : ISaveOdrSetInputPort
{
    private readonly ISuperSetDetailRepository _superSetDetailRepository;
    private readonly IMstItemRepository _mstItemRepository;
    private readonly ISetMstRepository _setMstRepository;
    private readonly ICommonSuperSet _commonSuperSet;

    public SaveOdrSetInteractor(ISuperSetDetailRepository superSetDetailRepository, IMstItemRepository mstItemRepository, ISetMstRepository setMstRepository, ICommonSuperSet commonSuperSet)
    {
        _superSetDetailRepository = superSetDetailRepository;
        _mstItemRepository = mstItemRepository;
        _setMstRepository = setMstRepository;
        _commonSuperSet = commonSuperSet;
    }

    public SaveOdrSetOutputData Handle(SaveOdrSetInputData inputData)
    {
        try
        {
            var resultValidate = ValidateData(inputData.HpId, inputData.SetNameModelList, inputData.UpdateSetNameList);
            if (resultValidate != SaveOdrSetStatus.ValidateSuccessd)
            {
                return new SaveOdrSetOutputData(resultValidate);
            }
            var result = _superSetDetailRepository.SaveOdrSet(inputData.HpId, inputData.UserId, inputData.SinDate, inputData.SetNameModelList, inputData.UpdateSetNameList);
            if (result.SaveSuccess)
            {
                if (result.SetMstUpdateList.Any())
                {
                    return new SaveOdrSetOutputData(SaveOdrSetStatus.Successed, _commonSuperSet.BuildTreeSetKbn(result.SetMstUpdateList));
                }
                return new SaveOdrSetOutputData(SaveOdrSetStatus.Successed);
            }
            return new SaveOdrSetOutputData(SaveOdrSetStatus.Failed);
        }
        finally
        {
            _superSetDetailRepository.ReleaseResource();
            _mstItemRepository.ReleaseResource();
        }
    }

    private SaveOdrSetStatus ValidateData(int hpId, List<OdrSetNameModel> setNameModelList, List<OdrSetNameModel> updateSetNameList)
    {
        var itemCdList = setNameModelList.Select(item => item.ItemCd).Distinct().ToList();
        var setCdList = setNameModelList.Select(item => item.SetCd).ToList();
        setCdList.AddRange(updateSetNameList.Select(item => item.SetCd));
        setCdList = setCdList.Distinct().ToList();

        if (_mstItemRepository.GetCheckItemCds(hpId, itemCdList).Count != itemCdList.Count)
        {
            return SaveOdrSetStatus.InvalidItemCd;
        }
        else if (setNameModelList.Any(item => item.Quantity < 0))
        {
            return SaveOdrSetStatus.InvalidQuanlity;
        }
        else if (!_setMstRepository.CheckExistSetMstBySetCd(hpId, setCdList))
        {
            return SaveOdrSetStatus.InvalidSetCd;
        }
        return SaveOdrSetStatus.ValidateSuccessd;
    }
}
