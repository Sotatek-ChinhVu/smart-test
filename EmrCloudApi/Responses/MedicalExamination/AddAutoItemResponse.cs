using UseCase.OrdInfs.GetListTrees;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class AddAutoItemResponse
    {
        public AddAutoItemResponse(List<OdrInfItem> odrInfItemInputDatas)
        {
            OdrInfItemInputDatas = odrInfItemInputDatas;
        }

        public List<OdrInfItem> OdrInfItemInputDatas { get; private set; }
    }
}
