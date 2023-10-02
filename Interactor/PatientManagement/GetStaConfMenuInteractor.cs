using Domain.Models.Diseases;
using Domain.Models.MainMenu;
using Domain.Models.MstItem;
using Helper.Constants;
using UseCase.PatientManagement.GetStaConf;

namespace Interactor.PatientManagement
{
    public class GetStaConfMenuInteractor : IGetStaConfMenuInputPort
    {
        private readonly IStatisticRepository _statisticRepository;
        private readonly IMstItemRepository _mstItemRepository;
        private readonly IPtDiseaseRepository _ptDiseaseRepository;

        public GetStaConfMenuInteractor(IStatisticRepository statisticRepository, IMstItemRepository mstItemRepository, IPtDiseaseRepository ptDiseaseRepository)
        {
            _statisticRepository = statisticRepository;
            _mstItemRepository = mstItemRepository;
            _ptDiseaseRepository = ptDiseaseRepository;
        }

        public GetStaConfMenuOutputData Handle(GetStaConfMenuInputData inputData)
        {
            try
            {
                var staMenu = _statisticRepository.GetStatisticMenuModels(inputData.HpId);

                var tenMstItems = new Dictionary<string, string>();
                var byomeis = new Dictionary<string, string>();

                foreach (var item in staMenu)
                {
                    var itemDatas = GetItemName(inputData.HpId, item.PatientManagement.ItemCds, item.PatientManagement.ItemCmts);
                    var byomeiDatas = GetByomeiName(inputData.HpId, item.PatientManagement.ByomeiCds);
                    foreach (var kvp in itemDatas)
                    {
                        if (!tenMstItems.ContainsKey(kvp.Key))
                        {
                            tenMstItems.Add(kvp.Key, kvp.Value);
                        }
                    }

                    foreach (var kvp in byomeiDatas)
                    {
                        if (!byomeis.ContainsKey(kvp.Key))
                        {
                            byomeis.Add(kvp.Key, kvp.Value);
                        }
                    }
                }

                return new GetStaConfMenuOutputData(staMenu, tenMstItems, byomeis, staMenu.Any() ? GetStaConfMenuStatus.Successed : GetStaConfMenuStatus.NoData);
            }
            finally
            {
                _statisticRepository.ReleaseResource();
                _mstItemRepository.ReleaseResource();
                _ptDiseaseRepository.ReleaseResource();
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

        private Dictionary<string, string> GetByomeiName(int hpId, List<string> byomeiCds)
        {
            var result = new Dictionary<string, string>();

            byomeiCds = byomeiCds.Where(x => x != ByomeiConstant.FreeWordCode).ToList();
            var byomeis = _ptDiseaseRepository.GetByomeiMst(hpId, byomeiCds);

            foreach (var item in byomeis)
            {
                if (!result.ContainsKey(item.Key))
                {
                    result.Add(item.Key, item.Value);
                }
            }

            return result;
        }
    }
}
