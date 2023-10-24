namespace EmrCloudApi.Responses.Insurance;

public class FindHokenInfByPtIdResponse
{
    public FindHokenInfByPtIdResponse(List<HokenInfDto> hokenInfList)
    {
        HokenInfList = hokenInfList;
    }

    public List<HokenInfDto> HokenInfList { get; private set; }
}
