using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetValidGairaiRiha
{
    public class GetValidGairaiRihaOutputData : IOutputData
    {
        public GetValidGairaiRihaOutputData(int type, string itemName, int lastDaySanteiRiha, string rihaItemName, GetValidGairaiRihaStatus status)
        {
            Type = type;
            ItemName = itemName;
            LastDaySanteiRiha = lastDaySanteiRiha;
            RihaItemName = rihaItemName;
            Status = status;
        }

        public int Type { get; private set; }
        public string ItemName { get; private set; }
        public int LastDaySanteiRiha { get; private set; }
        public string RihaItemName { get; private set; }
        public GetValidGairaiRihaStatus Status {get; private set; }
    }
}
