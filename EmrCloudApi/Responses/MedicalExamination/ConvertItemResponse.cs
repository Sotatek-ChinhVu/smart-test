using OdrInfItemOfTodayOrder = UseCase.OrdInfs.GetListTrees.OdrInfItem;

namespace EmrCloudApi.Responses.MedicalExamination
{
    public class ConvertItemResponse
    {
        public ConvertItemResponse(List<OdrInfItemOfTodayOrder> result)
        {
            Result = result;
        }

        public List<OdrInfItemOfTodayOrder> Result { get; private set; }
    }
}
