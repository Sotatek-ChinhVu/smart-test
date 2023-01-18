namespace EmrCloudApi.Responses.Family;

public class GetListFamilyReverserResponse
{
    public GetListFamilyReverserResponse(List<FamilyReverserDto> listFamilyReverser)
    {
        ListFamilyReverser = listFamilyReverser;
    }

    public List<FamilyReverserDto> ListFamilyReverser { get; private set; }
}
