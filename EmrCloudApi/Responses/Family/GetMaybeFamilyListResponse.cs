namespace EmrCloudApi.Responses.Family;

public class GetMaybeFamilyListResponse
{
    public GetMaybeFamilyListResponse(List<FamilyDto> familyList)
    {
        FamilyList = familyList;
    }

    public List<FamilyDto> FamilyList { get; private set; }
}
