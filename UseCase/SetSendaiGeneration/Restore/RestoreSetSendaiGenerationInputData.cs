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
        public RestoreSetSendaiGenerationInputData(int restoreGenerationId, int hpId, int userId)
        {
            RestoreGenerationId = restoreGenerationId;
            HpId = hpId;
            UserId = userId;
        }

        public int RestoreGenerationId { get; set; }
        public int HpId { get; set; }
        public int UserId { get; set; }
    }
}
