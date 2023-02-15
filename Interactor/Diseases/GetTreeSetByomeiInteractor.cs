using Domain.Models.Diseases;
using UseCase.Diseases.GetTreeSetByomei;

namespace Interactor.Diseases
{
    public class GetTreeSetByomeiInteractor : IGetTreeSetByomeiInputPort
    {
        private readonly IPtDiseaseRepository _diseaseRepository;
        public GetTreeSetByomeiInteractor(IPtDiseaseRepository diseaseRepository)
        {
            _diseaseRepository = diseaseRepository;
        }

        public GetTreeSetByomeiOutputData Handle(GetTreeSetByomeiInputData inputData)
        {
            try
            {
                if (inputData.HpId < 0)
                    return new GetTreeSetByomeiOutputData(new List<ByomeiSetMstModel>(), GetTreeSetByomeiStatus.InvalidHpId);

                if(inputData.SinDate < 0)
                    return new GetTreeSetByomeiOutputData(new List<ByomeiSetMstModel>(), GetTreeSetByomeiStatus.InvalidSinDate);

                var datas = _diseaseRepository.GetDataTreeSetByomei(inputData.HpId, inputData.SinDate);
                if (!datas.Any())
                    return new GetTreeSetByomeiOutputData(new List<ByomeiSetMstModel>(), GetTreeSetByomeiStatus.NoData);
                else
                {
                    var rootNodes = datas.FindAll(p => p.Level == 1).OrderBy(p => p.Level1).ToList();
                    foreach (var node1 in rootNodes)
                    {
                        node1.Childrens = datas.FindAll(p => p.Level == 2 && p.Level1 == node1.Level1).OrderBy(p => p.Level2).ToList();
                        foreach (var node2 in node1.Childrens)
                        {
                            node2.Childrens = datas.FindAll(p => p.Level == 3 && p.Level1 == node1.Level1 && p.Level2 == node2.Level2).OrderBy(p => p.Level3).ToList();
                            foreach (var node3 in node2.Childrens)
                            {
                                node3.Childrens = datas.FindAll(p => p.Level == 4 && p.Level1 == node1.Level1 && p.Level2 == node2.Level2 && p.Level3 == node3.Level3).OrderBy(p => p.Level4).ToList();
                                foreach (var node4 in node3.Childrens)
                                {
                                    node4.Childrens = datas.FindAll(p => p.Level == 5 && p.Level1 == node1.Level1 && p.Level2 == node2.Level2 && p.Level3 == node3.Level3 && p.Level4 == node4.Level4).OrderBy(p => p.Level5).ToList();
                                }
                            }
                        }
                    }
                    return new GetTreeSetByomeiOutputData(rootNodes, GetTreeSetByomeiStatus.Successful);
                }
            }
            finally
            {
                _diseaseRepository.ReleaseResource();
            }
        }
    }
}
