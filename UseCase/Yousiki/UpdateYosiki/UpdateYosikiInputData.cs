using Domain.Models.Yousiki;
using Entity.Tenant;
using UseCase.Core.Sync.Core;

namespace UseCase.Yousiki.UpdateYosiki
{
    public class UpdateYosikiInputData : IInputData<UpdateYosikiOutputData>
    {
        public UpdateYosikiInputData(int hpId, int userId, Yousiki1InfModel yousiki1Inf, List<Yousiki1InfDetailModel> yousiki1InfDetails, Dictionary<int, int> dataTypes, bool isTemporarySave) 
        {
            HpId = hpId;
            UserId = userId;
            Yousiki1Inf = yousiki1Inf;
            Yousiki1InfDetails = yousiki1InfDetails;
            DataTypes = dataTypes;
            IsTemporarySave = isTemporarySave;
        }

        public int HpId {  get; private set; }

        public int UserId { get; private set; }

        public Yousiki1InfModel Yousiki1Inf { get; private set; }

        public List<Yousiki1InfDetailModel> Yousiki1InfDetails { get; private set; }

        public Dictionary<int, int> DataTypes { get; private set; }

        public bool IsTemporarySave { get; private set; }
    }
}
