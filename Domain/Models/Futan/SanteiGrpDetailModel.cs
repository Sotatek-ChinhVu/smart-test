using Entity.Tenant;

namespace Domain.Models.Futan
{
    public class SanteiGrpDetailModel
    {
        public SanteiGrpDetail SanteiGrpDetail { get; }

        public SanteiGrpDetailModel(SanteiGrpDetail santeiGrpDetail)
        {
            SanteiGrpDetail = santeiGrpDetail;
        }

        public int SanteiGroupCd => SanteiGrpDetail.SanteiGrpCd;
    }
}
