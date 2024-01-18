using Helper.Common;
using Helper.Extension;

namespace Domain.Models.Yousiki
{
    public class OutpatientConsultationInfModel
    {
        public OutpatientConsultationInfModel(Yousiki1InfDetailModel yousiki1InfDetailModel, int consultationDateValue)
        {
            Yousiki1InfDetailModel = yousiki1InfDetailModel;
        }

        public Yousiki1InfDetailModel Yousiki1InfDetailModel { get; private set; }

        public int ConsultationDateValue
        {
            get => Yousiki1InfDetailModel.Value.AsInteger(); 
        }

        public string ConsultationDateDisplay
        {
            get => CIUtil.SDateToShowSDate(ConsultationDateValue);
        }
    }
}
