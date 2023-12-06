using UseCase.Core.Sync.Core;

namespace UseCase.SetSendaiGeneration.Delete
{
    public class DeleteSendaiGenerationInputData : IInputData<DeleteSendaiGenerationOutputData>
    {
        public DeleteSendaiGenerationInputData(int hpId, int generationId, int rowIndex, int userId, int startDate)
        {
            GenerationId = generationId;
            RowIndex = rowIndex;
            UserId = userId;
            HpId = hpId;
            StartDate = startDate;
        }

        public int HpId { get; private set; }
        public int GenerationId { get; private set; }
        public int RowIndex { get; private set; }
        public int UserId { get; private set; }
        public int StartDate { get; private set; }
    }
}
