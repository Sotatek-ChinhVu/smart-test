using Domain.Models.MstItem;
using Helper.Extension;
using UseCase.MstItem.GetSetNameMnt;

namespace Interactor.MstItem
{
    public class GetSetNameMntInteractor : IGetSetNameMntInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;
        public GetSetNameMntInteractor(IMstItemRepository tenMstMaintenanceRepository)
        {
            _mstItemRepository = tenMstMaintenanceRepository;
        }

        public GetSetNameMntOutPutData Handle(GetSetNameMntInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                    return new GetSetNameMntOutPutData(new List<SetNameMntModel>(), GetSetNameMntStatus.InvalidHpId);
                var setCheckBoxStatus = new SetCheckBoxStatusModel(inputData.SetKbnChecked1, inputData.SetKbnChecked2, inputData.SetKbnChecked3, inputData.SetKbnChecked4, inputData.SetKbnChecked5, inputData.SetKbnChecked6, inputData.SetKbnChecked7,
                    inputData.SetKbnChecked8, inputData.SetKbnChecked9, inputData.SetKbnChecked10, inputData.JihiChecked, inputData.KihonChecked, inputData.TokuChecked, inputData.YohoChecked, inputData.DiffChecked);
                var generationId = _mstItemRepository.GetGenerationId(inputData.HpId);
                var data = _mstItemRepository.GetSetNameMnt(setCheckBoxStatus, generationId, inputData.HpId);
                if (!data.Any())
                    return new GetSetNameMntOutPutData(new List<SetNameMntModel>(), GetSetNameMntStatus.NoData);
                EditSetNameMnt(data, inputData.HpId);
                return new GetSetNameMntOutPutData(data, GetSetNameMntStatus.Successful);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
        private void EditSetNameMnt(List<SetNameMntModel> listSetNameMnt, int hpId)
        {
            if (listSetNameMnt.Count <= 0)
                return;

            SetNameMntModel setNameMntAbove;
            SetNameMntModel setNameMnt = listSetNameMnt[0];

            setNameMnt.SetCategory = GetNameCategory(setNameMnt.SetKbn, hpId);
            setNameMnt.SetKbnEdaNoBinding = (setNameMnt.SetKbnEdaNo + 1).AsString();
            setNameMnt.Level1Binding = setNameMnt.Level1.AsString();
            setNameMnt.Level2Binding = setNameMnt.Level2.AsString();
            setNameMnt.Level3Binding = setNameMnt.Level3.AsString();
            setNameMnt.IsSetString = "○";
            setNameMnt.ItemNameBinding = setNameMnt.SetName;
            setNameMnt.IsSet = true;

            for (int i = 1; i < listSetNameMnt.Count; i++)
            {
                setNameMntAbove = listSetNameMnt[i - 1];
                setNameMnt = listSetNameMnt[i];

                if (setNameMnt.SetKbn != setNameMntAbove.SetKbn)
                {
                    setNameMnt.SetCategory = GetNameCategory(setNameMnt.SetKbn, hpId);
                }
                if (setNameMnt.SetKbnEdaNo != setNameMntAbove.SetKbnEdaNo ||
                    setNameMnt.SetKbn != setNameMntAbove.SetKbn)
                {
                    setNameMnt.SetKbnEdaNoBinding = (setNameMnt.SetKbnEdaNo + 1).AsString();
                }
                if (setNameMnt.Level1 != setNameMntAbove.Level1)
                {
                    setNameMnt.Level1Binding = setNameMnt.Level1.AsString();
                }
                if (setNameMnt.Level2 != setNameMntAbove.Level2 ||
                    setNameMnt.Level1 != setNameMntAbove.Level1)
                {
                    setNameMnt.Level2Binding = setNameMnt.Level2.AsString();
                }
                if (setNameMnt.Level3 != setNameMntAbove.Level3 ||
                    setNameMnt.Level1 != setNameMntAbove.Level1 ||
                    setNameMnt.Level2 != setNameMntAbove.Level2)
                {
                    setNameMnt.Level3Binding = setNameMnt.Level3.AsString();
                }
                if (!setNameMnt.IsSetOdrInfDetail)
                {
                    setNameMnt.IsSetString = "○";
                    if (setNameMnt.Level3 > 0)
                    {
                        setNameMnt.ItemNameBinding = "　　" + setNameMnt.SetName;
                    }
                    else if (setNameMnt.Level2 > 0)
                    {
                        setNameMnt.ItemNameBinding = "　" + setNameMnt.SetName;
                    }
                    else
                    {
                        setNameMnt.ItemNameBinding = setNameMnt.SetName;
                    }

                    setNameMnt.IsSet = true;
                }
                else
                {
                    setNameMnt.ItemNameBinding = "　　　" + setNameMnt.ItemName;
                    setNameMnt.ItemNameTenMstBinding = setNameMnt.ItemNameTenMst;
                    if (CheckItemNameAndTenMstName(GetItemName(setNameMnt), setNameMnt.ItemNameTenMst))
                    {
                        setNameMnt.SetFlag = "ー";
                    }
                }
            }
        }
        private string GetNameCategory(int setKbn, int hpId)
        {
            var generationId = _mstItemRepository.GetGenerationId(hpId);
            var listSetKbnMst = _mstItemRepository.GetListSetKbnMst(generationId, hpId);
            var setKbnMst = listSetKbnMst.FirstOrDefault(item => item.SetKbn == setKbn);
            return setKbnMst?.SetKbnName ?? "";
        }

        private bool CheckItemNameAndTenMstName(string itemName, string tenMstName)
        {
            if (itemName.Replace("　", "").Replace(" ", "") != tenMstName.Replace("　", "").Replace(" ", ""))
                return false;

            return true;
        }

        private string GetItemName(SetNameMntModel setNameMnt)
        {
            return setNameMnt.IsCommentMaster
                ? setNameMnt.CmtName
                : setNameMnt.ItemName;
        }
    }
}
