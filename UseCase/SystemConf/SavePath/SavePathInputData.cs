using Domain.Models.SystemConf;
using UseCase.Core.Sync.Core;

namespace UseCase.SystemConf.SavePath
{
    public class SavePathInputData : IInputData<SavePathOutputData>
    {
        public SavePathInputData(int hpId, int userId, List<SystemConfListXmlPathModel> systemConfListXmlPathModels)
        {
            HpId = hpId;
            UserId = userId;
            SystemConfListXmlPathModels = systemConfListXmlPathModels;
        }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public List<SystemConfListXmlPathModel> SystemConfListXmlPathModels { get; private set; }
    }
}
