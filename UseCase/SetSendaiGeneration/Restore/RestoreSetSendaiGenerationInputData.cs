using Helper.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.SetSendaiGeneration.Restore
{
    public class RestoreSetSendaiGenerationInputData: IInputData<RestoreSetSendaiGenerationOutputData>
    {
        public RestoreSetSendaiGenerationInputData(int restoreGenerationId, int hpId, int userId, IMessenger messenger)
        {
            RestoreGenerationId = restoreGenerationId;
            HpId = hpId;
            UserId = userId;
            Messenger = messenger;
        }

        public int RestoreGenerationId { get; private set; }
        public int HpId { get; private set; }
        public int UserId { get; private set; }
        public IMessenger Messenger { get; private set; }
    }
}
