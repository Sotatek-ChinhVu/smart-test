using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Async.Core;
using UseCase.Core.Sync.Core;

namespace UseCase.SetSendaiGeneration.Delete
{
    public class DeleteSendaiGenerationInputData: IInputData<DeleteSendaiGenerationOutputData>
    {
        public DeleteSendaiGenerationInputData(int generationId, int rowIndex, int userId)
        {
            GenerationId = generationId;
            RowIndex = rowIndex;
            UserId = userId;
        }

        public int GenerationId { get; private set; }
        public int RowIndex { get; private set; }
        public int UserId { get; private set; }
    }
}
