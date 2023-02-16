using Domain.Models.Family;
using UseCase.Family;
using UseCase.Family.GetFamilyReverserList;

namespace Interactor.Family;

public class GetFamilyReverserListInteractor : IGetFamilyReverserListInputPort
{
    private readonly IFamilyRepository _familyRepository;
    private readonly Dictionary<string, string> RelationshipReverserMDic = new Dictionary<string, string>()
                                                                                                            {
                                                                                                                {"BR","BR"},
                                                                                                                {"MA","MA"},
                                                                                                                { "FA","SO"},
                                                                                                                { "MO","SO"},
                                                                                                                { "GF1","GC"},
                                                                                                                { "GM1","GC"},
                                                                                                                { "GF2","GC"},
                                                                                                                { "GM2","GC"},
                                                                                                                { "SO","FA"},
                                                                                                                { "DA","FA"},
                                                                                                                { "BB","LB"},
                                                                                                                { "LB","BB"},
                                                                                                                { "BS","LB"},
                                                                                                                { "LS","BB"},
                                                                                                                { "GC","GF1"},
                                                                                                                { "OT","OT"},
                                                                                                            };
    private readonly Dictionary<string, string> RelationshipReverserFDic = new Dictionary<string, string>()
                                                                                                            {
                                                                                                                {"BR","BR"},
                                                                                                                {"MA","MA"},
                                                                                                                { "FA","DA"},
                                                                                                                { "MO","DA"},
                                                                                                                { "GF1","GC"},
                                                                                                                { "GM1","GC"},
                                                                                                                { "GF2","GC"},
                                                                                                                { "GM2","GC"},
                                                                                                                { "SO","MO"},
                                                                                                                { "DA","MO"},
                                                                                                                { "BB","LS"},
                                                                                                                { "LB","BS"},
                                                                                                                { "BS","LS"},
                                                                                                                { "LS","BS"},
                                                                                                                { "GC","GM1"},
                                                                                                                { "OT","OT"},
                                                                                                            };

    public GetFamilyReverserListInteractor(IFamilyRepository familyRepository)
    {
        _familyRepository = familyRepository;
    }

    public GetFamilyReverserListOutputData Handle(GetFamilyReverserListInputData inputData)
    {
        try
        {
            if (inputData.FamilyPtId <= 0)
            {
                return new GetFamilyReverserListOutputData(GetFamilyReverserListStatus.InvalidPtId);
            }
            return new GetFamilyReverserListOutputData(GetDataFamilyReverser(inputData), GetFamilyReverserListStatus.Successed);
        }
        finally
        {
            _familyRepository.ReleaseResource();
        }
    }

    private List<FamilyReverserItem> GetDataFamilyReverser(GetFamilyReverserListInputData inputData)
    {
        List<FamilyReverserItem> result = new();
        var ptInfList = _familyRepository.GetFamilyReverserList(inputData.HpId, inputData.FamilyPtId, inputData.ListPtInf.Keys.ToList());
        foreach (var item in ptInfList)
        {
            var zokugaraCd = inputData.ListPtInf[item.PtId];
            if (zokugaraCd == null)
            {
                continue;
            }
            if (item.Sex == 1)
            {
                zokugaraCd = RelationshipReverserMDic[zokugaraCd];
            }
            else if (item.Sex == 2)
            {
                zokugaraCd = RelationshipReverserFDic[zokugaraCd];
            }
            result.Add(new FamilyReverserItem(item, zokugaraCd));
        }
        return result;
    }
}
