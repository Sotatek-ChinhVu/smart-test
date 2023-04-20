using UseCase.MedicalExamination.GetValidGairaiRiha;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class GetValidGairaiRihaResponse
    {
        public GetValidGairaiRihaResponse(List<GairaiRihaItem> gairaiRihaItems)
        {
            GairaiRihaItems = gairaiRihaItems;
        }

        public List<GairaiRihaItem> GairaiRihaItems { get; private set; }
    }
}
