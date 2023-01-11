using Domain.Models.InsuranceMst;

namespace EmrCloudApi.Responses.InsuranceMst
{
    public class InsuranceMasterDto
    {
        public InsuranceMasterDto(HokenMstModel insuranceMst, IEnumerable<InsuranceDetailDto> details)
        {
            HokenNo = insuranceMst.HokenNo;
            HokenEdaNo = insuranceMst.HokenEdaNo;
            DisplayHokenNo = insuranceMst.DisplayHokenNo;
            HokenNameCd = insuranceMst.HokenNameCd;
            HokenSbtKbn = insuranceMst.HokenSbtKbn;
            HokenSName = insuranceMst.HokenSName;
            Details = details;
        }

        public int HokenNo { get; private set; }

        public int HokenEdaNo { get; private set; }

        public string DisplayHokenNo { get; private set; }
        
        public string HokenNameCd { get; private set; }
       
        public int HokenSbtKbn { get; private set; }

        public string HokenSName { get; private set; }

        public IEnumerable<InsuranceDetailDto> Details { get; private set; }
    }
}
