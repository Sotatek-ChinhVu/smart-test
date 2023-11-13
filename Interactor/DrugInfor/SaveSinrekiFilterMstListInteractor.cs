using Domain.Models.DrugInfor;
using Domain.Models.MstItem;
using UseCase.DrugInfor.SaveSinrekiFilterMstList;

namespace Interactor.DrugInfor;

public class SaveSinrekiFilterMstListInteractor : ISaveSinrekiFilterMstListInputPort
{
    private readonly IDrugInforRepository _drugInforRepository;
    private readonly IMstItemRepository _mstItemRepository;

    public SaveSinrekiFilterMstListInteractor(IDrugInforRepository drugInforRepository, IMstItemRepository mstItemRepository)
    {
        _drugInforRepository = drugInforRepository;
        _mstItemRepository = mstItemRepository;
    }

    public SaveSinrekiFilterMstListOutputData Handle(SaveSinrekiFilterMstListInputData inputData)
    {
        try
        {
            var validateResult = ValidateData(inputData);
            if (validateResult != SaveSinrekiFilterMstListStatus.ValidateSuccess)
            {
                return new SaveSinrekiFilterMstListOutputData(validateResult);
            }
            if (_drugInforRepository.SaveSinrekiFilterMstList(inputData.HpId, inputData.UserId, inputData.SinrekiFilterMstList))
            {
                return new SaveSinrekiFilterMstListOutputData(SaveSinrekiFilterMstListStatus.Successed);
            }
            return new SaveSinrekiFilterMstListOutputData(SaveSinrekiFilterMstListStatus.Failed);
        }
        finally
        {
            _drugInforRepository.ReleaseResource();
            _mstItemRepository.ReleaseResource();
        }
    }

    private SaveSinrekiFilterMstListStatus ValidateData(SaveSinrekiFilterMstListInputData inputData)
    {
        var allSinrekiFilterMst = _drugInforRepository.GetSinrekiFilterMstList(inputData.HpId);
        var grpCdList = inputData.SinrekiFilterMstList.Where(item => item.GrpCd != 0).Select(item => item.GrpCd).Distinct().ToList();
        if (!_drugInforRepository.CheckExistGrpCd(inputData.HpId, grpCdList))
        {
            return SaveSinrekiFilterMstListStatus.InvalidSinrekiFilterMstGrpCd;
        }
        else if (inputData.SinrekiFilterMstList.Exists(item => item.Name.Length > 100))
        {
            return SaveSinrekiFilterMstListStatus.InvalidSinrekiFilterMstName;
        }
        List<int> kouiKbnIdList = new();
        List<long> kouiSeqNoList = new();
        List<long> idDetailList = new();
        List<string> itemCdList = new();
        foreach (var mstModel in inputData.SinrekiFilterMstList)
        {
            foreach (var kouiModel in mstModel.SinrekiFilterMstKouiList)
            {
                if (mstModel.SinrekiFilterMstKouiList.Count(item => item.KouiKbnId == kouiModel.KouiKbnId) > 1)
                {
                    return SaveSinrekiFilterMstListStatus.InvalidSinrekiFilterMstKouiKbnId;
                }
                kouiKbnIdList.Add(kouiModel.KouiKbnId);
                if (kouiModel.SeqNo != 0)
                {
                    kouiSeqNoList.Add(kouiModel.SeqNo);
                }
            }

            var itemCdDbList = allSinrekiFilterMst.FirstOrDefault(item => item.GrpCd == mstModel.GrpCd)?.SinrekiFilterMstDetailList?.Select(item => item.ItemCd).ToList() ?? new();
            if (itemCdDbList.Any() && mstModel.SinrekiFilterMstDetailList.Where(item => !item.IsDeleted).Any(item => itemCdDbList.Contains(item.ItemCd) && item.Id == 0))
            {
                return SaveSinrekiFilterMstListStatus.InvalidSinrekiFilterMstDetaiDuplicateItemCd;
            }
            foreach (var detailModel in mstModel.SinrekiFilterMstDetailList)
            {
                itemCdList.Add(detailModel.ItemCd);
                if (detailModel.Id != 0)
                {
                    idDetailList.Add(detailModel.Id);
                }
            }
        }
        kouiKbnIdList = kouiKbnIdList.Distinct().ToList();
        kouiSeqNoList = kouiSeqNoList.Distinct().ToList();
        itemCdList = itemCdList.Distinct().ToList();
        idDetailList = idDetailList.Distinct().ToList();
        if (_mstItemRepository.GetCheckItemCds(itemCdList).Count != itemCdList.Count)
        {
            return SaveSinrekiFilterMstListStatus.InvalidItemCd;
        }
        else if (!_drugInforRepository.CheckExistKouiKbn(inputData.HpId, kouiKbnIdList))
        {
            return SaveSinrekiFilterMstListStatus.InvalidSinrekiFilterMstKouiKbnId;
        }
        else if (!_drugInforRepository.CheckExistSinrekiFilterMstKoui(inputData.HpId, kouiSeqNoList))
        {
            return SaveSinrekiFilterMstListStatus.InvalidSinrekiFilterMstKouiSeqNo;
        }
        else if (!_drugInforRepository.CheckExistSinrekiFilterMstDetail(inputData.HpId, idDetailList))
        {
            return SaveSinrekiFilterMstListStatus.InvalidSinrekiFilterMstDetailId;
        }
        return SaveSinrekiFilterMstListStatus.ValidateSuccess;
    }
}
