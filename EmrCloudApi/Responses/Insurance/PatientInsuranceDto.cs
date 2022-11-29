using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;

namespace EmrCloudApi.Responses.Insurance
{
    public class PatientInsuranceDto
    {
        public PatientInsuranceDto(List<InsuranceModel> listInsurance, List<HokenInfModel> listHokenInf, List<KohiInfModel> listKohi)
        {
            ListInsurance = listInsurance.Select(i => new PatternDto(i)).ToList();
            ListHokenInf = listHokenInf.Select(h => new HokenInfDto(h)).ToList();
            ListKohi = listKohi.Select(k => new KohiInfDto(k)).ToList();
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
    }
}
