namespace EmrCloudApi.Responses.Family;

public class GetListFamilyResponse
{
    public GetListFamilyResponse(List<FamilyDto> listFamily)
    {
        ListFamily = listFamily;
    }

    public List<FamilyDto> ListFamily { get; private set; }
}
