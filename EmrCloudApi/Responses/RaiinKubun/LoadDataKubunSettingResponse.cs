using Domain.Models.RaiinKubunMst;

namespace EmrCloudApi.Responses.RaiinKubun;

public class LoadDataKubunSettingResponse
{
    public int MaxGrpId { get; private set; }

    public List<RaiinKubunMstModel> RaiinKubunMstList { get; private set; }

    public LoadDataKubunSettingResponse(int maxGrpId, List<RaiinKubunMstModel> raiinKubunMstList)
    {
        RaiinKubunMstList = raiinKubunMstList;
        MaxGrpId = maxGrpId;
    }
}
