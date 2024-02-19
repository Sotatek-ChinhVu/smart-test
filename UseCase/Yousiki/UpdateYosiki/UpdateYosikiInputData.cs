using Domain.Models.Yousiki;
using UseCase.Core.Sync.Core;

namespace UseCase.Yousiki.UpdateYosiki
{
    public class UpdateYosikiInputData : IInputData<UpdateYosikiOutputData>
    {
        public UpdateYosikiInputData(int hpId, int userId, Yousiki1InfModel yousiki1Inf, List<Yousiki1InfDetailModel> yousiki1InfDetails, List<CategoryModel> categoryModels, bool isTemporarySave)
        {
            HpId = hpId;
            UserId = userId;
            Yousiki1Inf = yousiki1Inf;
            Yousiki1InfDetails = yousiki1InfDetails;
            CategoryModels = categoryModels;
            IsTemporarySave = isTemporarySave;
        }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public Yousiki1InfModel Yousiki1Inf { get; private set; }

        public List<Yousiki1InfDetailModel> Yousiki1InfDetails { get; private set; }

        public List<CategoryModel> CategoryModels { get; private set; }

        public bool IsTemporarySave { get; private set; }
    }
}
