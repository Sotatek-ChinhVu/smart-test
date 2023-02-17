﻿using Domain.Models.Diseases;
using UseCase.Diseases.GetSetByomeiTree;

namespace Interactor.Diseases
{
    public class GetSetByomeiTreeInteractor : IGetSetByomeiTreeInputPort
    {
        private readonly IPtDiseaseRepository _diseaseRepository;
        public GetSetByomeiTreeInteractor(IPtDiseaseRepository diseaseRepository)
        {
            _diseaseRepository = diseaseRepository;
        }

        public GetSetByomeiTreeOutputData Handle(GetSetByomeiTreeInputData inputData)
        {
            try
            {
                if (inputData.HpId < 0)
                    return new GetSetByomeiTreeOutputData(new List<ByomeiSetMstModel>(), GetSetByomeiTreeStatus.InvalidHpId);

                if(inputData.SinDate < 0)
                    return new GetSetByomeiTreeOutputData(new List<ByomeiSetMstModel>(), GetSetByomeiTreeStatus.InvalidSinDate);

                var datas = _diseaseRepository.GetDataTreeSetByomei(inputData.HpId, inputData.SinDate);
                if (!datas.Any())
                    return new GetSetByomeiTreeOutputData(new List<ByomeiSetMstModel>(), GetSetByomeiTreeStatus.NoData);
                else
                {
                    var rootNodes = datas.FindAll(p => p.Level == 1).OrderBy(p => p.Level1).ToList();
                    foreach (var levelNode1 in rootNodes)
                    {
                        levelNode1.Childrens = datas.FindAll(p => p.Level == 2 && p.Level1 == levelNode1.Level1).OrderBy(p => p.Level2).ToList();
                        foreach (var levelNode2 in levelNode1.Childrens)
                        {
                            levelNode2.Childrens = datas.FindAll(p => p.Level == 3 && p.Level1 == levelNode1.Level1 && p.Level2 == levelNode2.Level2).OrderBy(p => p.Level3).ToList();
                            foreach (var levelNode3 in levelNode2.Childrens)
                            {
                                levelNode3.Childrens = datas.FindAll(p => p.Level == 4 && p.Level1 == levelNode1.Level1 && p.Level2 == levelNode2.Level2 && p.Level3 == levelNode3.Level3).OrderBy(p => p.Level4).ToList();
                                foreach (var levelNode4 in levelNode3.Childrens)
                                {
                                    levelNode4.Childrens = datas.FindAll(p => p.Level == 5 && p.Level1 == levelNode1.Level1 && p.Level2 == levelNode2.Level2 && p.Level3 == levelNode3.Level3 && p.Level4 == levelNode4.Level4).OrderBy(p => p.Level5).ToList();
                                }
                            }
                        }
                    }
                    return new GetSetByomeiTreeOutputData(rootNodes, GetSetByomeiTreeStatus.Successful);
                }
            }
            finally
            {
                _diseaseRepository.ReleaseResource();
            }
        }
    }
}
