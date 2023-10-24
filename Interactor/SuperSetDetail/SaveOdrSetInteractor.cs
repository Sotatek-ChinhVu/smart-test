using Domain.Models.MstItem;
using Domain.Models.SetMst;
using Domain.Models.SuperSetDetail;
using UseCase.MainMenu.SaveOdrSet;

namespace Interactor.SuperSetDetail;

public class SaveOdrSetInteractor : ISaveOdrSetInputPort
{
    private readonly ISuperSetDetailRepository _superSetDetailRepository;
    private readonly IMstItemRepository _mstItemRepository;
    private readonly ISetMstRepository _setMstRepository;

    public SaveOdrSetInteractor(ISuperSetDetailRepository superSetDetailRepository, IMstItemRepository mstItemRepository, ISetMstRepository setMstRepository)
    {
        _superSetDetailRepository = superSetDetailRepository;
        _mstItemRepository = mstItemRepository;
        _setMstRepository = setMstRepository;
    }

    public SaveOdrSetOutputData Handle(SaveOdrSetInputData inputData)
    {
        try
        {
            var resultValidate = ValidateData(inputData.HpId, inputData.SetNameModelList);
            if (resultValidate != SaveOdrSetStatus.ValidateSuccessd)
            {
                return new SaveOdrSetOutputData(resultValidate);
            }
            if (_superSetDetailRepository.SaveOdrSet(inputData.HpId, inputData.UserId, inputData.SinDate, inputData.SetNameModelList))
            {
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

    private SaveOdrSetStatus ValidateData(int hpId, List<OdrSetNameModel> setNameModelList)
    {
        var itemCdList = setNameModelList.Select(item => item.ItemCd).Distinct().ToList();
        var setCdList = setNameModelList.Select(item => item.SetCd).Distinct().ToList();
        if (_mstItemRepository.GetCheckItemCds(itemCdList).Count != itemCdList.Count)
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
