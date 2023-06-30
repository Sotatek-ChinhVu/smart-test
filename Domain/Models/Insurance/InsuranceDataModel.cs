using Domain.Models.InsuranceInfor;

namespace Domain.Models.Insurance
{
    public class InsuranceDataModel
    {
        public InsuranceDataModel(List<InsuranceModel> listInsurance, List<HokenInfModel> listHokenInf, List<KohiInfModel> listKohi, int maxIdHokenInf, int maxIdHokenKohi, int maxPidHokenPattern)
        {
            ListInsurance = listInsurance;
            ListHokenInf = listHokenInf;
            ListKohi = listKohi;
            MaxIdHokenInf = maxIdHokenInf;
            MaxIdHokenKohi = maxIdHokenKohi;
            MaxPidHokenPattern = maxPidHokenPattern;
        }
        
        public InsuranceDataModel()
        {
            ListInsurance = new List<InsuranceModel>();
            ListHokenInf = new List<HokenInfModel>();
            ListKohi = new List<KohiInfModel>();
        }

        public List<InsuranceModel> ListInsurance { get; private set; }

        public List<HokenInfModel> ListHokenInf { get; private set; }

        public List<KohiInfModel> ListKohi { get; private set; }

        public int MaxIdHokenInf { get; private set; }

        public int MaxIdHokenKohi { get; private set; }

        public int MaxPidHokenPattern { get; private set; }
    }
}
