using Domain.Models.CalculateModel;

namespace UseCase.MedicalExamination.GetCheckedOrder
{
    public class RunTraialCalculateResponse
    {

        public RunTraialCalculateResponse(List<SinMeiDataModel> sinMeiList, List<KaikeiInfDataModel> kaikeiInfList)
        {
            SinMeiList = sinMeiList;
            KaikeiInfList = kaikeiInfList;
        }

        public List<SinMeiDataModel> SinMeiList { get; private set; }

        public List<KaikeiInfDataModel> KaikeiInfList { get; private set; }
    }
}
