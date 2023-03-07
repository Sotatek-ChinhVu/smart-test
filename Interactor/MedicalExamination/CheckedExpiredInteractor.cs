using Domain.Models.MstItem;
using UseCase.MedicalExamination.CheckedExpired;

namespace Interactor.MedicalExamination
{
    public class CheckedExpiredInteractor : ICheckedExpiredInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;

        public CheckedExpiredInteractor(IMstItemRepository mstItemRepository)
        {
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
                var itemCds = inputData.CheckedExpiredItems.Select(i => i.ItemCd).Distinct().ToList();
                var tenMstItemList = _mstItemRepository.FindTenMst(inputData.HpId, itemCds) ?? new();
                List<(string, string)> expiredItems = new();

                foreach (var detail in inputData.CheckedExpiredItems)
                {
                    
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

                    var tenMstByItemCdList = tenMstItemList.Where(t => t.ItemCd == detail.ItemCd).ToList();
                    if (tenMstByItemCdList.Count == 0)
                    {
                        continue;
                    }

                    int minStartDate = tenMstByItemCdList.Min(item => item.StartDate);
                    int maxEndDate = tenMstByItemCdList.Max(item => item.EndDate);


                    if (minStartDate > inputData.SinDate || maxEndDate < inputData.SinDate)
                    {
                        expiredItems.Add(new(detail.ItemCd, detail.ItemName));
                    }
                }

                var convertItems = _mstItemRepository.GetConversionItem(expiredItems, inputData.HpId, inputData.SinDate);

                List<CheckedExpiredOutputItem> result = new();

                foreach (var convertItem in convertItems)
                {
                    result.Add(new CheckedExpiredOutputItem(convertItem.Key, convertItem.Value.Item1, convertItem.Value.Item2));
                }

                return new CheckedExpiredOutputData(CheckedExpiredStatus.Successed, result);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
