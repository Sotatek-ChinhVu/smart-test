namespace EmrCloudApi.Responses.SetKbnMst;

public class GetSetKbnMstListByGenerationIdResponse
{
    public GetSetKbnMstListByGenerationIdResponse(List<SetKbnMstDto> setKbnMstList)
    {
        SetKbnMstList = setKbnMstList;
    }

    public List<SetKbnMstDto> SetKbnMstList { get; private set; }
}
