using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;

namespace EmrCloudApi.Responses.Insurance
{
    public class PatientInsuranceDto
    {
        public PatientInsuranceDto(List<InsuranceModel> listInsurance, List<HokenInfModel> listHokenInf, List<KohiInfModel> listKohi, int maxIdHokenInf, int maxIdHokenKohi, int maxPidHokenPattern)
        {
            ListInsurance = listInsurance.Select(i => new PatternDto(i)).ToList();
            ListHokenInf = listHokenInf.Select(h => new HokenInfDto(h)).ToList();
            ListKohi = listKohi.Select(k => new KohiInfDto(k)).ToList();
            MaxIdHokenInf = maxIdHokenInf;
            MaxIdHokenKohi = maxIdHokenKohi;
            MaxPidHokenPattern = maxPidHokenPattern;
        }

        public PatientInsuranceDto(List<PatternDto> listInsurance, List<HokenInfDto> listHokenInf, List<KohiInfModel> listKohi, int maxIdHokenInf, int maxIdHokenKohi, int maxPidHokenPattern)
        {
            ListInsurance = listInsurance;
            ListHokenInf = listHokenInf;
            ListKohi = listKohi.Select(k => new KohiInfDto(k)).ToList();
            MaxIdHokenInf = maxIdHokenInf;
            MaxIdHokenKohi = maxIdHokenKohi;
            MaxPidHokenPattern = maxPidHokenPattern;
        }


        public PatientInsuranceDto()
        {
            ListInsurance = new List<PatternDto>();
            ListHokenInf = new List<HokenInfDto>();
            ListKohi = new List<KohiInfDto>();
        }

        public List<PatternDto> ListInsurance { get; private set; }

        public List<HokenInfDto> ListHokenInf { get; private set; }

        public List<KohiInfDto> ListKohi { get; private set; }

        public int MaxIdHokenInf { get; private set; }

        public int MaxIdHokenKohi { get; private set; }

        public int MaxPidHokenPattern { get; private set; }
    }
}
