using Domain.Models.Accounting;

namespace EmrCloudApi.Responses.Accounting
{
    public class GetPtByoMeiResponse
    {
        public GetPtByoMeiResponse(List<PtByomeiModel> ptByomeiModels)
        {
            PtByomeiModels = ptByomeiModels;
        }

        public List<PtByomeiModel> PtByomeiModels { get; private set; }
    }
}
