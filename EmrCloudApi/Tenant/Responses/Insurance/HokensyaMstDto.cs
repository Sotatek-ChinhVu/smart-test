using Domain.Models.Insurance;

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
