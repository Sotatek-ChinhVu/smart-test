using Domain.Models.Family;
using UseCase.Family.GetListFamilyReverser;

namespace Interactor.Family;

public class GetListFamilyReverserInteractor : IGetListFamilyReverserInputPort
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

    public GetListFamilyReverserInteractor(IFamilyRepository familyRepository)
    {
        _familyRepository = familyRepository;
    }

    public GetListFamilyReverserOutputData Handle(GetListFamilyReverserInputData inputData)
    {
        try
        {
            if (inputData.FamilyPtId <= 0)
            {
                return new GetListFamilyReverserOutputData(GetListFamilyReverserStatus.InvalidPtId);
            }
            return new GetListFamilyReverserOutputData(GetDataFamilyReverser(inputData), GetListFamilyReverserStatus.Successed);
        }
        finally
        {
            _familyRepository.ReleaseResource();
        }
    }

    private List<FamilyReverserOutputItem> GetDataFamilyReverser(GetListFamilyReverserInputData inputData)
    {
        List<FamilyReverserOutputItem> result = new();
        var listPtInf = _familyRepository.GetListFamilyReverser(inputData.HpId, inputData.FamilyPtId, inputData.ListPtInf.Keys.ToList());
        foreach (var item in listPtInf)
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
            result.Add(new FamilyReverserOutputItem(item, zokugaraCd));
        }
        return result;
    }
}
