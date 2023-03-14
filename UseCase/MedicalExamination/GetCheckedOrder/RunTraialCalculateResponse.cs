using Domain.Models.CalculateModel;
using Domain.Models.Futan;

namespace UseCase.MedicalExamination.GetCheckedOrder
{
    public class RunTraialCalculateResponse
    {

        public RunTraialCalculateResponse(List<SinMeiDataModel> sinMeiList, List<KaikeiInfModel> kaikeiInfList)
        {
            SinMeiList = sinMeiList;
            KaikeiInfList = kaikeiInfList;
        }

        public List<SinMeiDataModel> SinMeiList { get; private set; }

        public List<KaikeiInfModel> KaikeiInfList { get; private set; }
    }
}
