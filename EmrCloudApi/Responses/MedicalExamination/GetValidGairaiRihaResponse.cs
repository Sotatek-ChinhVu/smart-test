using UseCase.MedicalExamination.GetValidGairaiRiha;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class GetValidGairaiRihaResponse
    {
        public GetValidGairaiRihaResponse(int type, string itemName, int lastDaySanteiRiha, string rihaItemName)
        {
            Type = type;
            ItemName = itemName;
            LastDaySanteiRiha = lastDaySanteiRiha;
            RihaItemName = rihaItemName;
        }

        public int Type { get; private set; }
        public string ItemName { get; private set; }
        public int LastDaySanteiRiha { get; private set; }
        public string RihaItemName { get; private set; }
    }
}
