using Domain.Models.MonshinInf;

namespace EmrCloudApi.Responses.MonshinInfor
{
    public class GetMonshinInforListResponse
    {
        public GetMonshinInforListResponse(List<MonshinInforModel> monshinInfors)
        {
            MonshinInfors = monshinInfors;
        }

        public List<MonshinInforModel> MonshinInfors { get; private set; }
    }
}
