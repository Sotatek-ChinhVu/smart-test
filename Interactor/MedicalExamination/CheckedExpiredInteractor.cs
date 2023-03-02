using Domain.Models.MstItem;
using Domain.Models.TodayOdr;
using Helper.Common;
using Helper.Constants;
using UseCase.MedicalExamination.CheckedExpired;

namespace Interactor.MedicalExamination
{
    public class CheckedExpiredInteractor : ICheckedExpiredInputPort
    {
        private readonly ITodayOdrRepository _todayOdrRepository;
        private readonly IMstItemRepository _mstItemRepository;

        public CheckedExpiredInteractor(ITodayOdrRepository todayOdrRepository, IMstItemRepository mstItemRepository)
        {
            _todayOdrRepository = todayOdrRepository;
            _mstItemRepository = mstItemRepository;
        }

        public CheckedExpiredOutputData Handle(CheckedExpiredInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new CheckedExpiredOutputData(CheckedExpiredStatus.InValidHpId, new());
                }
                if (inputData.SinDate <= 0)
                {
                    return new CheckedExpiredOutputData(CheckedExpiredStatus.InValidSinDate, new());
                }
                if (inputData.CheckedExpiredItems.Count == 0)
                {
                    return new CheckedExpiredOutputData(CheckedExpiredStatus.InputNotData, new());
                }
                var checkedItems = new List<string>();
                var itemCds = inputData.CheckedExpiredItems.Select(i => i.ItemCd).ToList();
                var tenMstItemList = _mstItemRepository.FindTenMst(inputData.HpId, itemCds, inputData.SinDate, inputData.SinDate) ?? new();
                foreach (var detail in inputData.CheckedExpiredItems)
                {
                    var tenMsts = tenMstItemList.Where(t => t.ItemCd == detail.ItemCd);
                    if (checkedItems.Contains(detail.ItemCd))
                    {
                        continue;
                    }
                    if (string.IsNullOrEmpty(detail.ItemCd))
                    {
                        continue;
                    }
                    if (tenMstItemList.Count == 0)
                    {
                        continue;
                    }
                    int minStartDate = tenMstItemList.Min(item => item.StartDate);

                    if (minStartDate > inputData.SinDate)
                    {
                        checkedItems.Add(FormatDisplayMessage(DisplayItemName(detail.ItemCd, detail.ItemName, detail.BunkatuKoui, detail.Bunkatu), minStartDate, true));
                    }

                    int maxEndDate = tenMstItemList.Max(item => item.EndDate);

                    if (maxEndDate < inputData.SinDate)
                    {
                        checkedItems.Add(FormatDisplayMessage(DisplayItemName(detail.ItemCd, detail.ItemName, detail.BunkatuKoui, detail.Bunkatu), maxEndDate, false));
                    }
                }
                return new CheckedExpiredOutputData(CheckedExpiredStatus.Successed, checkedItems);
            }
            finally
            {
                _todayOdrRepository.ReleaseResource();
                _mstItemRepository.ReleaseResource();
            }
        }

        private string DisplayItemName(string itemCd, string itemName, int bunkatuKoui, string bunkatu)
        {
            if (itemCd == ItemCdConst.Con_TouyakuOrSiBunkatu)
            {
                return itemName + TenUtils.GetBunkatu(bunkatuKoui, bunkatu);
            }
            return itemName;
        }

        private string FormatDisplayMessage(string itemName, int dateCheck, bool isCheckStartDate)
        {
            string dateString = CIUtil.SDateToShowSDate(dateCheck);

            if (isCheckStartDate)
            {
                return $"\"{itemName}\"は{dateString}から使用可能です。";
            }
            else
            {
                return $"\"{itemName}\"は{dateString}まで使用可能です。";
            }
        }
    }
}
