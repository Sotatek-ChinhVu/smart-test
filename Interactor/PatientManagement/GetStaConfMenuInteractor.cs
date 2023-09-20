using Domain.Models.MainMenu;
using Domain.Models.MstItem;
using UseCase.PatientManagement.GetStaConf;

namespace Interactor.PatientManagement
{
    public class GetStaConfMenuInteractor : IGetStaConfMenuInputPort
    {
        private readonly IStatisticRepository _statisticRepository;
        private readonly IMstItemRepository _mstItemRepository;

        public GetStaConfMenuInteractor(IStatisticRepository statisticRepository, IMstItemRepository mstItemRepository)
        {
            _statisticRepository = statisticRepository;
            _mstItemRepository = mstItemRepository;
        }

        public GetStaConfMenuOutputData Handle(GetStaConfMenuInputData inputData)
        {
            try
            {
                var staMenu = _statisticRepository.GetStatisticMenuModels(inputData.HpId);

                var tenMstItems = new Dictionary<string, string>();

                foreach (var item in staMenu)
                {
                    var itemDatas = GetItemName(inputData.HpId, item.PatientManagement.ItemCds, item.PatientManagement.ItemCmts);

                    foreach (var kvp in itemDatas)
                    {
                        if (!tenMstItems.ContainsKey(kvp.Key))
                        {
                            tenMstItems.Add(kvp.Key, kvp.Value);
                        }
                    }
                }

                return new GetStaConfMenuOutputData(staMenu, tenMstItems, staMenu.Any() ? GetStaConfMenuStatus.Successed : GetStaConfMenuStatus.NoData);
            }
            finally
            {
                _statisticRepository.ReleaseResource();
                _mstItemRepository.ReleaseResource();
            }
        }

        private Dictionary<string, string> GetItemName(int hpId, List<string> itemCds, List<string> itemCmts)
        {
            var result = new Dictionary<string, string>();
            int index = 0;
            for (int i = 0; i < itemCds.Count; i = i + 3)
            {
                if (string.IsNullOrEmpty(itemCds[i]))
                {
                    if (index < itemCmts.Count)
                    {
                        if (!result.ContainsKey(itemCds[i]))
                        {
                            result.Add(itemCds[i], itemCmts[index]);
                        }
                        index++;
                    }
                }
                else
                {
                    if (!result.ContainsKey(itemCds[i]))
                    {
                        result.Add(itemCds[i], _mstItemRepository.GetNameByItemCd(hpId, itemCds[i]));
                    }
                }
            }

            return result;
        }
    }
}
