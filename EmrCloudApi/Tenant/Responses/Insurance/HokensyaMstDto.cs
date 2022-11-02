using Domain.Models.Insurance;
using Domain.Models.InsuranceMst;

namespace EmrCloudApi.Tenant.Responses.Insurance
{
    public class HokensyaMstDto
    {
        public HokensyaMstDto(HokensyaMstModel hokensyaMstModel)
        {
            IsKigoNa = hokensyaMstModel.IsKigoNa;
        }

        public int IsKigoNa { get; private set; }
    }
}
