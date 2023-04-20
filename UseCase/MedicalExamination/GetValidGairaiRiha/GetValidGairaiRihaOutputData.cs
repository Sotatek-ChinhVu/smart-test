using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.GetValidGairaiRiha
{
    public class GetValidGairaiRihaOutputData : IOutputData
    {
        public GetValidGairaiRihaOutputData(List<GairaiRihaItem> gairaiRihaItems, GetValidGairaiRihaStatus status)
        {
            GairaiRihaItems = gairaiRihaItems;
            Status = status;
        }

        public List<GairaiRihaItem> GairaiRihaItems { get; private set; }
        public GetValidGairaiRihaStatus Status {get; private set; }
    }
}
