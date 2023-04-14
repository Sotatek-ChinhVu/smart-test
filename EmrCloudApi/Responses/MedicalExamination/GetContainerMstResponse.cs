using UseCase.MedicalExamination.GetContainerMst;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class GetContainerMstResponse
    {
        public GetContainerMstResponse(List<KensaPrinterItem> kensaPrinterItems)
        {
            KensaPrinterItems = kensaPrinterItems;
        }

        public List<KensaPrinterItem> KensaPrinterItems { get; private set; }
    }
}
