namespace EmrCloudApi.Responses.Family;

public class GetFamilyReverserListResponse
{
    public GetFamilyReverserListResponse(List<FamilyReverserDto> familyReverserList)
    {
        FamilyReverserList = familyReverserList;
    }

    public List<FamilyReverserDto> FamilyReverserList { get; private set; }
}
