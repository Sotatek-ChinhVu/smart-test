using UseCase.Core.Sync.Core;

namespace UseCase.UserConf.UpdateAdoptedByomeiConfig
{
    public class UpdateAdoptedByomeiConfigInputData : IInputData<UpdateAdoptedByomeiConfigOutputData>
    {
        public UpdateAdoptedByomeiConfigInputData(int adoptedValue, int hpId, int userId)
        {
            AdoptedValue = adoptedValue;
            HpId = hpId;
            UserId = userId;
        }

        public int AdoptedValue { get; private set; }

        public int HpId { get; private set; }

        public int UserId { get; private set; }
    }
}
