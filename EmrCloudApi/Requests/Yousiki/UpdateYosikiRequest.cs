using EmrCloudApi.Requests.Yousiki.RequestItem;
using UseCase.Yousiki.UpdateYosiki;

namespace EmrCloudApi.Requests.Yousiki
{
    public class UpdateYosikiRequest
    {
        public UpdateYosikiInfRequestItem Yousiki1InfModel { get; set; } = new();

        public List<UpdateYosiki1InfDetailRequestItem> Yousiki1InfDetailModels { get; set; } = new();

        public Dictionary<int, int> DataTypes { get; set; } = new();

        public bool IsTemporarySave { get; set; }
    }
}
