namespace EmrCloudApi.Responses.Family;

public class GetFamilyListResponse
{
    public GetFamilyListResponse(List<FamilyDto> familyList)
    {
        FamilyList = familyList;
    }

    public List<FamilyDto> FamilyList { get; private set; }
}
